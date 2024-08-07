


resource "azurerm_virtual_network" "vnet" {
  name                = "${local.resource_name_prefix}-${var.vnet_name}"
  address_space       = var.vnet_address_space
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tags                = local.common_tags   
}


resource "azurerm_subnet" "vm_subnet" {
  name                 = "${azurerm_virtual_network.vnet.name}-${var.vm_subnet_name}"
  resource_group_name  = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = var.vm_subnet_address
}



resource "azurerm_public_ip" "linux_vm_public_ip" {
  name                = "${local.resource_name_prefix}-linux-vm-public-ip"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  allocation_method   = "Static"
  sku                 = "Standard"
  domain_name_label   = "app1-vm-${random_string.vm_random.id}"
}