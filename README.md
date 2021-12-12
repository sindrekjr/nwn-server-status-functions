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

#### 2. Run Azurite
May be run via the VS Code extension or the command line. I recommend the latter.
```
azurite --blobHost
```

#### 3. Run Azure Functions
May be run via the VS Code extension or the command line. I recommend the latter.
```
func start
```

## Usage
Once the project is running you can start triggering its functions via its HTTP triggers. 
