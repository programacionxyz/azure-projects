

variable "business" {
  description = "Business Division in the large organization this Infrastructure belongs"
  type        = string
  default     = "operations"
}

variable "cost_center" {
  description = "Cost center in the large organization this Infrastructure belongs"
  type        = string
  default     = "it"    
}

variable "environment" {
  description = "Environment variable"
  type        = string
  default     = "dev"
}

variable "resource_group_name" {
  description = "Azure Resource Group Name"
  type        = string
  default     = "rg"  
}

variable "resource_group_location" {
  description = "Region in which Azure Resources to be created"
  type        = string
  default     = "eastus2"  
} 

variable "vnet_name" {
  description = "Virtual Network name"
  type        = string
  default     = "vnet"
}

variable "vnet_address_space" {
  description = "Virtual Network Address Space"
  type        = list(string)
  default     = ["10.0.0.0/16"]
}

variable "vm_subnet_name" {
  description = "Virtual Network VM Subnet Name"
  type        = string 
  default     = "vmsubnet"
}

variable "vm_subnet_address" {
  description = "Virtual Network VM Subnet Address Spaces"
  type = list(string)
  default = ["10.0.1.0/24"]
}



