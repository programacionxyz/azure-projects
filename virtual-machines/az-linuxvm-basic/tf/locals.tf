

locals {
  
  owners               = var.business
  environment          = var.environment
  cost_center          = var.cost_center
  resource_name_prefix = "${var.business}-${var.environment}"

  common_tags = {
    cost_center =  local.cost_center
    owners      = local.owners
    environment = local.environment
  }
} 