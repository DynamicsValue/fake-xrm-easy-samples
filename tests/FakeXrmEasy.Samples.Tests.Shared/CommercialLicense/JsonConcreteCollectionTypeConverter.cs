using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FakeXrmEasy.Samples.Tests.Shared.CommercialLicense
{
    public class JsonConcreteCollectionTypeConverter<TAbstractItem, TConcreteItem> : JsonConverter where TConcreteItem : class
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var deserialisedList = serializer.Deserialize<List<TConcreteItem>>(reader);

            return deserialisedList.Cast<TAbstractItem>().ToList();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}