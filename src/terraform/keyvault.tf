resource "azurerm_key_vault" "fnkv" {
  tenant_id           = data.azurerm_client_config.current.tenant_id
  sku_name            = "standard"
  name                = "${var.application_name}-${var.environment_name}-${var.keyvault_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  # enable_rbac_authorization = true
}

resource "azurerm_key_vault_secret" "app_conf_app_id" {
  name         = "conf-app-id"
  value        = azuread_application.app.application_id
  key_vault_id = azurerm_key_vault.fnkv.id
}

resource "azurerm_key_vault_secret" "app_conf_app_secret" {
  name         = "conf-app-secret"
  value        = azuread_application_password.app_secret.value
  key_vault_id = azurerm_key_vault.fnkv.id
}

resource "azurerm_key_vault_secret" "app_conf_tenant_id" {
  name         = "conf-tenant-id"
  value        = data.azurerm_client_config.current.tenant_id
  key_vault_id = azurerm_key_vault.fnkv.id
}

resource "azurerm_key_vault_access_policy" "fnkv_policy" {
  key_vault_id = azurerm_key_vault.fnkv.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id

  secret_permissions = [
    "Set",
    "Get",
    "List",
    "Delete",
    "Purge"
  ]
}

resource "azurerm_key_vault_access_policy" "akv_fn_policy" {
  key_vault_id = azurerm_key_vault.fnkv.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_windows_function_app.fn.identity[0].principal_id

  secret_permissions = [
    "Get",
    "List"
  ]
}
