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


