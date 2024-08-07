

resource "azurerm_network_interface" "linux_vm_nic" {
  name                            = "${local.resource_name_prefix}-linux-vm-nic"
  location                        = azurerm_resource_group.rg.location
  resource_group_name             = azurerm_resource_group.rg.name

  ip_configuration {
    name                          = "linux-vm-ip-1"
    subnet_id                     = azurerm_subnet.vm_subnet.id
    private_ip_address_allocation = "Dynamic"
    public_ip_address_id          =  azurerm_public_ip.linux_vm_public_ip.id
  }
}

