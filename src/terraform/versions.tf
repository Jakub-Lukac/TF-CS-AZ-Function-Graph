terraform {
  required_providers {
    source  = "hashicorp/azurem"
    version = "~> 3.89.0"
  }

  backend "azurerm" {}
}

provider "azurerm" {
  skip_provider_registration = "true"
  features {}

  client_id       = var.env_client_id
  client_secret   = var.env_client_secret
  tenant_id       = var.tenant_id
  subscription_id = var.subscription_id
}

provider "azuread" {
  client_id     = var.env_client_id
  client_secret = var.env_client_secret
  tenant_id     = var.tenant_id
}
