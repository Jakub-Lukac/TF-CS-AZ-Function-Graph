# Create Function App Insignts plan
resource "azurerm_application_insights" "application_insights" {
  name                = "${var.application_name}-${var.environment_name}-${var.app-insights-suffix}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  application_type    = "web"

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

resource "azurerm_application_insights_smart_detection_rule" "smart_detection" {
  name                    = "Slow server response time"
  application_insights_id = azurerm_application_insights.application_insights.id
  enabled                 = false
}