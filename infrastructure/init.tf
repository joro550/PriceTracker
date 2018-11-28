# Configure the Azure Provider
provider "azurerm" {

}

# Create a resource group
resource "azurerm_resource_group" "test" {
  name     = "azure-functions-test-rg"
  location = "ukwest"
}

resource "azurerm_storage_account" "test" {
  name                     = "funcprices"
  resource_group_name      = "${azurerm_resource_group.test.name}"
  location                 = "${azurerm_resource_group.test.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

/* Function */

resource "azurerm_app_service_plan" "test" {
  name                = "src-pln-prices"
  location            = "${azurerm_resource_group.test.location}"
  resource_group_name = "${azurerm_resource_group.test.name}"
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "test" {
  name                      = "mark-prices-func"
  version = "~2"
  location                  = "${azurerm_resource_group.test.location}"
  resource_group_name       = "${azurerm_resource_group.test.name}"
  app_service_plan_id       = "${azurerm_app_service_plan.test.id}"
  storage_connection_string = "${azurerm_storage_account.test.primary_connection_string}"

  app_settings {
    "QueueConnectionString" = "${azurerm_storage_account.test.primary_connection_string}"
    "TableConnectionString" = "${azurerm_storage_account.test.primary_connection_string}"
  }
}


/* Website */

resource "azurerm_app_service_plan" "web" {
  name                = "markprices"
  location            = "${azurerm_resource_group.test.location}"
  resource_group_name = "${azurerm_resource_group.test.name}"

  sku {
    tier = "Free"
    size = "F1"
  }
}

resource "azurerm_app_service" "test" {
  name                = "markprices"
  location            = "${azurerm_resource_group.test.location}"
  resource_group_name = "${azurerm_resource_group.test.name}"
  app_service_plan_id = "${azurerm_app_service_plan.web.id}"

  site_config {
    dotnet_framework_version = "v4.0"
    scm_type                 = "LocalGit"
  }

  app_settings {
    "StorageConnectionString" = "${azurerm_storage_account.test.primary_connection_string}"
  }
}