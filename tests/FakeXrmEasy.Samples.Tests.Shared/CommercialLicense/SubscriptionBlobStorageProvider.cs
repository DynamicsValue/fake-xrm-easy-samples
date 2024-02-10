using System;
using System.IO;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FakeXrmEasy.Abstractions.CommercialLicense;
using FakeXrmEasy.Extensions;
using Newtonsoft.Json;

namespace FakeXrmEasy.Samples.Tests.Shared.CommercialLicense
{
    public class SubscriptionBlobStorageProvider: ISubscriptionStorageProvider
    {
        private const string FakeXrmEasySettingsFileName = "fxe.json";
        private const string ContainerName = "fxe";
        private static FakeXrmEasySettings _settings;
        private static readonly object _settingsLock = new object();
        
        /// <summary>
        /// https://learn.microsoft.com/en-us/azure/storage/blobs/concurrency-manage
        /// </summary>
        private ETag _eTag;
        
        public string GetLicenseKey()
        {
            ReadSettings();
            return _settings.LicenseKey;
        }

        private BlobContainerClient GetBlobContainerClient()
        {
            var uriBuilder = new BlobUriBuilder(new Uri(_settings.BlobStorageUri));
            if (uriBuilder.Sas == null)
            {
                throw new Exception("An Uri with a SAS token is expected");
            }

            var uriBuilderWithoutSas = uriBuilder.Copy();
            uriBuilderWithoutSas.Sas = null;

            var baseUri = uriBuilderWithoutSas.ToUri();
            var sas = uriBuilder.Sas.ToString();
            var blobServiceClient = new BlobServiceClient(
                baseUri,
                new AzureSasCredential(sas));
            
            //The blob service already contains a connection to the "fxe" container
            var blobContainerClient = blobServiceClient.GetBlobContainerClient("");
            
            return blobContainerClient;
        }
        
        public ISubscriptionUsage Read()
        {
            ReadSettings();

            var blobContainerClient = GetBlobContainerClient();
            var blobClient = blobContainerClient.GetBlobClient(FakeXrmEasySettingsFileName);
            if (!blobClient.Exists())
            {
                return null;
            }
            var response = blobClient.DownloadContent();
            var rawResponse = response.GetRawResponse();
            var jsonString = rawResponse.Content.ToString();
            _eTag = rawResponse.Headers.ETag.Value;
            return JsonConvert.DeserializeObject<ISubscriptionUsage>(jsonString);
        }

        public void Write(ISubscriptionUsage currentUsage)
        {
            ReadSettings();
            
            var blobContainerClient = GetBlobContainerClient();
            var blobClient = blobContainerClient.GetBlobClient(FakeXrmEasySettingsFileName);
            var jsonString = JsonConvert.SerializeObject(currentUsage);

            var blobUploadOptions = new BlobUploadOptions()
            {
                Conditions = new BlobRequestConditions()
                {
                    IfMatch = _eTag,
                }
            };
            
            blobClient.Upload(BinaryData.FromString(jsonString), blobUploadOptions);
        }

        private void ReadSettings()
        {
            lock (_settingsLock)
            {
                if (_settings == null)
                {
                    string key = Environment.GetEnvironmentVariable("FXE_LICENSE_KEY");
                    string blobUri = Environment.GetEnvironmentVariable("FXE_BLOB_STORAGE_URI");
                    _settings = new FakeXrmEasySettings()
                    {
                        LicenseKey = key,
                        BlobStorageUri = blobUri
                    };
                }
            }
        }

        public class FakeXrmEasySettings
        {
            public string LicenseKey { get; set; }
            public string BlobStorageUri { get; set; }
        }
    }
}