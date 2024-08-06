


terraform {
  required_version = ">= 1.9.3"
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = ">= 3.113.0"
    }
    random = {
      source = "hashicorp/random"
      version = "3.6.2"
    }
  }
}

provider "azurerm" {
 features {}          
}

