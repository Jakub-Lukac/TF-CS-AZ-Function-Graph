output "resource_group_name" {
  description = "Resource group name which is passed into the cd pipeline for automate deployment."
  value       = azurerm_resource_group.main.name
}

output "function_name" {
  description = "Function name which is passed into the cd pipeline for automate deployment."
  value       = azurerm_windows_function_app.fn.name
}