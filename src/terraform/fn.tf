resource "azurerm_user_assigned_identity" "functions" {
  location            = azurerm_resource_group.main.location
  name                = "mi-${var.application_name}-${var.environment_name}-${var.fn_suffix}"
  resource_group_name = azurerm_resource_group.main.name
}

resource "azurerm_service_plan" "fn_sp" {
  name                = "${var.application_name}-${var.environment_name}-${var.service_plan_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  os_type             = "Windows"
  sku_name            = "Y1"
}

resource "azurerm_windows_function_app" "fn" {
  name                = "${var.application_name}-${var.environment_name}-${var.fn_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location

  storage_account_name       = azurerm_storage_account.fn_st.name
  storage_account_access_key = azurerm_storage_account.fn_st.primary_access_key
  service_plan_id            = azurerm_service_plan.fn_sp.id

  # so it wont be rebuilded when some data will change for istance the connection string, etc.
  lifecycle {
    ignore_changes = [
      tags
    ]
  }

  # managed identity set to system assigned
  identity {
    type         = "SystemAssigned, UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.functions.id]
  }

  site_config {
    application_stack {
      dotnet_version              = "v8.0"
      use_dotnet_isolated_runtime = true
    }
    cors {
      allowed_origins = [
        "https://portal.azure.com",
        "https://${var.application_name}-${var.environment_name}-${var.service_plan_suffix}.azurewebsites.net"
      ]
      support_credentials = true
    }
  }

  /* auth_settings_v2 {
    auth_enabled           = true
    require_authentication = true
    default_provider       = "AzureActiveDirectory"
    unauthenticated_action = "RedirectToLoginPage"

    # our default_provider:
    active_directory_v2 {
      tenant_auth_endpoint       = "https://login.microsoftonline.com/${data.azurerm_client_config.current.tenant_id}/v2.0"
      client_secret_setting_name = "CONF_APP_SECRET"
      client_id                  = azuread_application.app.application_id
      # allowed_groups              = var.app_allowed_groups
    }

    # use a store for tokens (az blob storage backed)
    login {
      token_store_enabled = true
    }
  } */

  app_settings = {
    "WEBSITE_RUN_FROM_PACKAGE"       = 1
    "CONF_APP_ID"                    = "@Microsoft.KeyVault(VaultName=${azurerm_key_vault.fnkv.name};SecretName=${azurerm_key_vault_secret.app_conf_app_id.name})",
    "CONF_APP_SECRET"                = "@Microsoft.KeyVault(VaultName=${azurerm_key_vault.fnkv.name};SecretName=${azurerm_key_vault_secret.app_conf_app_secret.name})",
    "CONF_TENANT_ID"                 = "@Microsoft.KeyVault(VaultName=${azurerm_key_vault.fnkv.name};SecretName=${azurerm_key_vault_secret.app_conf_tenant_id.name})",
    "APPINSIGHTS_INSTRUMENTATIONKEY" = "${azurerm_application_insights.application_insights.instrumentation_key}"
  }
}
