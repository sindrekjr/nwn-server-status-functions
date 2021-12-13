# NwnServerStatus.Functions

Exploring Azure Durable Functions by polling for game servers with Beamdog's [NWMasterJsonApi](https://api.nwn.beamdog.net/v1/).

## Requirements
* [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=v4%2Cwindows%2Ccsharp%2Cportal%2Cbash%2Ckeda)
* One of:
  * [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio-code)
  * Azure Storage Account
 
## Setup
Clone the repository and follow these steps.

#### 1. Add `local.settings.json`
Required to run Azure Functions locally. You may simply copy-paste the following for development runtime.
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  }
}
```
If you have an Azure Storage Account available and wish to use it, add its connection string as the value of `AzureWebJobsStorage`. Please note that using a live storage account for development purposes may incur unwanted costs, as well as unpredictable behaviour when you run varying versions of your functions.

#### 2. Run Azurite OR use Azure Storage Account
Azurite may be run via the VS Code extension or the command line. I recommend the latter, and I recommend running the following command from the root of your project. That way you will easily be able to delete the generated storage files if you need to "reset".
```
azurite --blobHost
```

If you have an Azure Storage Account available and wish to use it, add its connection string as the value of `AzureWebJobsStorage` in `local.settings.json` from the previous step. Please note that using a live storage account for development purposes may incur unwanted costs, as well as unpredictable behaviour when you run varying versions of your functions.

## Usage
```
dotnet restore
dotnet build
func start
```

Once the project is running you can start triggering its functions via its HTTP triggers. Use cURL, a browser, Postman, whichever. All current HTTP triggers respond to both GET and POST.

For example, to start polling the server at `46.4.59.55:5123`, run your functions with `func start`, then:
```
curl http://localhost:7071/api/poll/46.4.59.55/5123
```

You should receive a response which includes `statusQueryGetUri`. This address is used to fetch the polling orchestrations current status once it has been triggered.

## Docs
- [Durable Functions Overview | Microsoft Docs](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview?tabs=csharp)
- [Create your first durable function in C# | Microsoft Docs](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-create-first-csharp?pivots=code-editor-vscode)
- [Use Azurite emulator for local Azure Storage development | Microsoft Docs](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio-code)
- [NWMaster JSON API](https://api.nwn.beamdog.net/v1/)
