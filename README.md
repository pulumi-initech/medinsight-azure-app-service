# MedInsight Azure App Service

A Pulumi project that deploys a containerized web application to Azure App Service using C# and the Azure Native provider.

## Overview

This project demonstrates:

- **Provider**: `pulumi-azure-native`
- **Language**: C# (.NET 9.0)
- **Architecture**: Component resource pattern for reusable infrastructure
- **Resources**:
  - `azure-native:resources:ResourceGroup` — resource group for all resources
  - `azure-native:web:AppServicePlan` — Linux-based App Service Plan (Basic tier)
  - `azure-native:web:WebApp` — containerized web application
- **Component**: `ContainerApp` — reusable component encapsulating App Service Plan and Web App
- **Container Image**: `nginxdemos/hello:latest` from Docker Hub
- **Outputs**:
  - `resourceGroupName` — name of the resource group
  - `appServicePlanName` — name of the App Service Plan
  - `appName` — name of the web app
  - `appUrl` — HTTPS URL of the deployed application

## Prerequisites

- An Azure subscription with sufficient permissions
- Azure CLI installed and authenticated (`az login`)
- .NET 9.0 SDK or later
- Pulumi CLI installed

## Project Structure

```plaintext
.
├── Program.cs                        # Main Pulumi program
├── ContainerApp.cs                   # Reusable component resource
├── MedInsightAzureAppService.csproj  # .NET project file
├── Pulumi.yaml                       # Project settings
├── Pulumi.dev.yaml                   # Stack configuration (dev environment)
└── .gitignore                        # Git ignore rules for .NET
```

## Configuration

Configure the following settings:

```bash
# Required: Azure region
pulumi config set azure-native:location westus2

# Optional: Application name (defaults to "medinsight")
pulumi config set appName myapp

# Optional: Container image (defaults to "nginxdemos/hello:latest")
pulumi config set image nginx:alpine
```

## Usage

### 1. Restore Dependencies

```bash
dotnet restore MedInsightAzureAppService.csproj
```

### 2. Preview Deployment

```bash
pulumi preview
```

### 3. Deploy the Stack

```bash
pulumi up
```

### 4. Access the Application

After deployment, get the application URL:

```bash
pulumi stack output appUrl
# Example output: https://medinsight-app.azurewebsites.net
```

### 5. Clean Up

To destroy all resources:

```bash
pulumi destroy
```

## Component Architecture

The `ContainerApp` component encapsulates:

- **App Service Plan** (Linux, Basic B1 tier)
  - Reserved for Linux containers
  - Configurable SKU
- **Web App** (Container-based)
  - Docker container deployment
  - HTTPS-only enforcement
  - Production environment configuration
  - Health monitoring

### Component Usage

```csharp
var containerApp = new ContainerApp("container-app", new ContainerAppArgs
{
    ResourceGroupName = resourceGroup.Name,
    Location = resourceGroup.Location,
    AppName = "myapp",
    Sku = "B1",
    Image = "nginxdemos/hello:latest",
});
```

## Container Configuration

The application is configured to:
- Pull images from Docker Hub (`https://index.docker.io`)
- Disable App Service storage for stateless containers
- Run in HTTPS-only mode
- Set environment variable `ENVIRONMENT=production`

## Outputs

Access stack outputs:

```bash
# Get all outputs
pulumi stack output

# Get specific output
pulumi stack output appUrl
pulumi stack output resourceGroupName
```

## Future Enhancements

Potential improvements:
- Add deployment slots for staging/production
- Integrate with Azure Key Vault for secrets
- Add Application Insights for monitoring
- Configure custom domains and SSL certificates
- Add auto-scaling rules
- Implement virtual network integration

## Getting Help

- [Pulumi Documentation](https://www.pulumi.com/docs/)
- [Azure Native Provider Reference](https://www.pulumi.com/registry/packages/azure-native/)
- [Pulumi Community Slack](https://pulumi.com/community/)