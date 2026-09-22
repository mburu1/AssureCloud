terraform {
  required_version = ">= 1.9.0"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0"
    }
    azapi = {
      source  = "azure/azapi"
      version = "~> 2.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.6"
    }
  }

  backend "azurerm" {
    resource_group_name   = "rg-assurecloud-tfstate"
    storage_account_name  = "stassurecloudtfstate"
    container_name        = "tfstate"
    key                   = "assurecloud.terraform.tfstate"
  }
}

provider "azurerm" {
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
    key_vault {
      purge_soft_delete_on_destroy           = false
      recover_soft_deleted_key_vault         = true
      purge_soft_deleted_secret_on_destroy   = false
      recover_soft_deleted_secret            = true
    }
  }
}

provider "azapi" {}

provider "random" {}
