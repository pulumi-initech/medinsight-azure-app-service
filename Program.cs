using Pulumi;
using Pulumi.AzureNative.Resources;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MedInsightAzureAppService
{
    class Program
    {
        static Task<int> Main() => Pulumi.Deployment.RunAsync(() =>
        {
            var config = new Config();
            var azureConfig = new Config("azure-native");
            var location = azureConfig.Require("location");
            var appName = config.Get("appName") ?? "medinsight";
            var image = config.Get("image") ?? "nginxdemos/hello:latest";

            // Resource Group
            var resourceGroup = new ResourceGroup("rg", new ResourceGroupArgs
            {
                ResourceGroupName = $"{appName}-rg",
                Location = location,
            });

            // Container App (encapsulates App Service Plan and Web App)
            var containerApp = new ContainerApp("container-app", new ContainerAppArgs
            {
                ResourceGroupName = resourceGroup.Name,
                Location = resourceGroup.Location,
                AppName = appName!,
                Image = image!,
            });

            // Exports
            return new Dictionary<string, object?>
            {
                ["resourceGroupName"] = resourceGroup.Name,
                ["appServicePlanName"] = containerApp.AppServicePlan.Name,
                ["appName"] = containerApp.App.Name,
                ["appUrl"] = containerApp.App.DefaultHostName.Apply(h => $"https://{h}"),
            };
        });
    }
}
