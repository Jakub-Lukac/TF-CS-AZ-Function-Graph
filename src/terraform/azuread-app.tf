data "azuread_application_published_app_ids" "well_known" {}

resource "azuread_service_principal" "msgraph" {
  client_id    = data.azuread_application_published_app_ids.well_known.result["MicrosoftGraph"]
  use_existing = true
}

resource "azuread_application" "app" {
  display_name     = "${var.application_name}-${var.environment_name}"
  sign_in_audience = "AzureADMyOrg"

  required_resource_access {
    resource_app_id = data.azuread_application_published_app_ids.well_known.result["MicrosoftGraph"]

    resource_access {
      id   = "6918b873-d17a-4dc1-b314-35f528134491" # Contacts.ReadWrite
      type = "Role"
    }

    resource_access {
      id   = "5b567255-7703-4780-807c-7be8301ae99b" # Group.Read.All
      type = "Role"
    }

    resource_access {
      id   = "98830695-27a2-44f7-8c18-0c3ebc9698f6" # GroupMember.Read.All
      type = "Role"
    }

    resource_access {
      id   = "e1a88a34-94c4-4418-be12-c87b00e26bea" # OrgContact.Read.All
      type = "Role"
    }

    resource_access {
      id   = "df021288-bdef-4463-88db-98f22de89214" # User.Read.All
      type = "Role"
    }
  }
}

resource "azuread_service_principal" "app_principal" {
  client_id = azuread_application.app.client_id
}

resource "azuread_application_password" "example" {
  application_id = azuread_application.app.object_id
  display_name   = "Secret for ${azuread_application.app.display_name} App"
}

