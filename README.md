# Deployment of C# Azure function using Terraform

## Project Overview 
This project leverages Terraform as Infrastructure as Code (IaC) to deploy Azure resources, including a Windows Function App autonomously. A GitHub Actions pipeline is used to manage the CI/CD process. The Azure Function consists of multiple functions with different triggers (HTTP, Blob, Timer). The Graph API is implemented as a core component of these functions.

## Terraform service principal Setup
To run this code, you need to get an Azure Subscription Id. Run the az login command to get a list of available subscriptions.

To create a Terraform service principal, go to portal.azure.com open up the cloud power shell, and run the following command:
```text
az ad sp create-for-rbac -n GraphApp-Test-Terraform --role="Contributor" --scopes="/subscriptions/{your-subcription-id}"
```

From the command output, note the `appId` value and `password` and store them in var.tf file for **the time being.** Note them as `env_` variables together with *subscription_id* and *tenant_id*.

```text
{
  "appId": "b194bcf7-****-****-****-5a8fed8448ff",
  "displayName": "GraphApp-Test-Terraform",
  "password": "**************************",
  "tenant": "ab22e0f4-****-****-****-9be459c33fb2"
}
```

```text
variable "env_client_id" {
  type    = string
  default = ""
}

variable "env_client_secret" {
  type    = string
  default = ""
}

variable "tenant_id" {
  type    = string
  default = ""
}

variable "subscription_id" {
  type    = string
  default = ""
}
```

## Before run Terraform
Terraform uses Azure AD Application to run commands. For creating new resources, **GraphApp-Test-Terraform** Application needs API Permissions as follows:

```text
Microsoft Graph

  Application.ReadWrite.All                 Application
  AppRoleAssignment.ReadWrite.All           Application
  Directory.ReadWrite.All                   Application
```

After adding permissions use the **Grant admin consent** button to commit permissions.

## Run Terraform

Prepare backend.conf file with the following attributes for storing your terraform.tfstate

```terraform
resource_group_name  = "Your-RG-Name"
storage_account_name = "yourstorageaccount"
container_name       = "your-container-name"
key                  = "terraform.tfstate"
access_key           = "your-access-key"
```

**Later on this file won't be needed, and will be deleted/ ignored**
**It's purpose is only for initial run**

When you're ready, run following commands:

```text
terraform init -backend-config=backend.conf
terraform validate
```

Create environment for `customer`

```text
terraform workspace new customer
terraform workspace select customer
terraform plan -out="plan_customer.out" 
```

If there is no error reported, run `apply` command to deploy solution for customer.

```text
terraform apply "plan_customer.out"
```
