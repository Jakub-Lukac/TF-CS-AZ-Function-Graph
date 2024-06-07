data "azurerm_client_config" "current" {}

resource "azurerm_resource_group" "main" {
  name = "${var.application_name}-${var.environment_name}-${var.resource_group_suffix}"
  location = var.resource_group_location

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

resource "azurerm_storage_account" "fn_st" {
  name                     = "${var.application_name}-${var.environment_name}-${var.function_storage_acc_suffix}"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

