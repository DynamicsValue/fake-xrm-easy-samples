# FakeXrmEasy Samples

Sample repo you can use to experiment with the different versions of FakeXrmEasy

This is the supporting code for a the following scenario :

## Scenario : Azure function + Dataverse + Plugin

[You'll find an overview of why / how this project is structured in the docs here](https://dynamicsvalue.github.io/fake-xrm-easy-docs/quickstart/azure-functions/)

- 1) We receive some contactdata into an Azure Function
- 2) The Azure function creates a Contact using DataverseClient nuget package in .net core
- 3) A plugin that fires on create of that contact record and creates a follow up Task.

In the demo we approached unit testing such scenario as follows:

- 1) A unit test to validate that the contact data is sent to Dataverse correcly (the contact is created)
- 2) A unit test to verify the plugin creates a follow up task given a Create message with an OutputParameter (the resulting from the Create operation) was received
- 3) A pipeline simulation scenario that wires **everything together In-Memory**, to check that both the contact and the task are created (this is experimental stuff, because really, the target versions of the client (.net core 3.1) and the server (net462) are different in production, but eventually....)

# Building and testing

The Azure function project targets net3.1, but the plugins project targets net462 so bear in mind you'll need a Windows machine to build the whole solution

From the command line:

    dotnet restore
    dotnet build
    dotnet test

Alternatively from VS and Test Explorer

## Scenario : Automatic Registration of Plugin Steps with Spkl

You'll find the sample code for automatic registration with Spkl under the /spkl directory.

Also please make sure to [check out the relevant documentation](https://dynamicsvalue.github.io/fake-xrm-easy-docs/quickstart/plugins/pipeline/automatic-registration/) 


## Custom Apis

The FakeXrmEasy.Samples.sln also has a running example of a custom api.

[Please check the docs for high level [overview of Messages](https://dynamicsvalue.github.io/fake-xrm-easy-docs/quickstart/messages/), [Custom Actions](https://dynamicsvalue.github.io/fake-xrm-easy-docs/quickstart/messages/custom-actions/), and [Custom Apis](https://dynamicsvalue.github.io/fake-xrm-easy-docs/quickstart/messages/custom-apis/).


# Release notes

Check out the latest release notes of FakeXrmEasy you don't miss any impotant updates!

- https://dynamicsvalue.github.io/fake-xrm-easy-docs/releases/2x/
- https://dynamicsvalue.github.io/fake-xrm-easy-docs/releases/3x/


# License

This repo is available under the [FakeXrmEasy's LICENSE](https://dynamicsvalue.github.io/fake-xrm-easy-docs/licensing/license/). 

Basically, you can use FakeXrmEasy free of charge: 

- In Open Source Projects and/or 
- For a Non-Commercial use and/or
- As part of a Non-Commercial Organization

**You'll need a commercial license for propietary and commercial use**.

Please, have a look at our [licensing FAQ for more details](https://dynamicsvalue.github.io/fake-xrm-easy-docs/licensing/faq/).

Pricing: https://dynamicsvalue.com/pricing


