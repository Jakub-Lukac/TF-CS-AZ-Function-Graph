# all the env variables will be populated through the ci/cd pipeline using github actions variables and secrets

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

#TF_VAR_application_name
variable "application_name" {
  type = string
}

#TF_VAR_environment_name
variable "environment_name" {
  type = string
}

variable "resource_group_location" {
  type = string
  default = "West Europe"
}

variable "resource_group_suffix" {
  type = string
  default = "RG"
}

variable "function_storage_acc_name" {
  type = string
  default = "graphapptestfnst"
}

