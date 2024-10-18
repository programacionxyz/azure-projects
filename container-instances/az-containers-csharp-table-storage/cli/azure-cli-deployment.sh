# This script creates all the necessary resources to deploy the Web Api Project to the Azure Container Instances Service, including Azure Table Storage.

# Requirements: 
# 1) An Azure Account 
# 2) Azure Command Line installer in your computer 
# 3) Docket installed
# 4) Create your own .env file and set the variable values. Use the file example.env as reference

ENV_FILE=".env"

if [ -f "$ENV_FILE" ]; then
    source "$ENV_FILE"
else
    echo ".env file does not exist."
    exit
fi

SCRIPT_DIR=$(dirname "$0")

echo "Variable values "
echo ""
echo "AZ_RESOURCE_GROUP     : ${AZ_RESOURCE_GROUP}"
echo "AZ_REGION             : ${AZ_REGION}"
echo "AZ_CONTAINER_REGISTRY : ${AZ_CONTAINER_REGISTRY}"
echo "AZ_CONTAINER_NAME     : ${AZ_CONTAINER_NAME}"
echo "AZ_STORAGE_ACCOUNT    : ${AZ_STORAGE_ACCOUNT}"
echo "AZ_TABLE_NAME         : ${AZ_TABLE_NAME}"
echo ""
read -p "Press enter to continue"

az login 

# Creates the Resource Group
az group create --name $AZ_RESOURCE_GROUP --location $AZ_REGION

# Creates the Azure Container Registry 
az acr create --resource-group $AZ_RESOURCE_GROUP --name $AZ_CONTAINER_REGISTRY --sku Basic
az acr update -n $AZ_CONTAINER_REGISTRY --admin-enabled true

# Creates the Storage Account
az storage account create --name $AZ_STORAGE_ACCOUNT --resource-group $AZ_RESOURCE_GROUP --location $AZ_REGION --sku Standard_LRS

# Creates the Table Storage
az storage table create --name $AZ_TABLE_NAME --account-name $AZ_STORAGE_ACCOUNT

# Get the Storage Account connection string
AZURE_STORAGE_CONNECTION_STRING=$(az storage account show-connection-string --name $AZ_STORAGE_ACCOUNT --resource-group $AZ_RESOURCE_GROUP --query connectionString --output tsv)

# Create the image
cd "$SCRIPT_DIR/../Azure.Containers.WebApi.HelloWorld"
dotnet publish
docker buildx build --no-cache --platform linux/amd64 -t "${AZ_CONTAINER_REGISTRY}.azurecr.io/${AZ_CONTAINER_NAME}" .

# Login and push the image to the Azure Container Registry
az acr login --name "${AZ_CONTAINER_REGISTRY}.azurecr.io/${AZ_CONTAINER_NAME}"
docker push "${AZ_CONTAINER_REGISTRY}.azurecr.io/${AZ_CONTAINER_NAME}"

# Get the Container Registry credentials
ACR_USERNAME=$(az acr credential show --name "${AZ_CONTAINER_REGISTRY}.azurecr.io/${AZ_CONTAINER_NAME}" --query "username" -o tsv)
ACR_PASSWORD=$(az acr credential show --name "${AZ_CONTAINER_REGISTRY}.azurecr.io/${AZ_CONTAINER_NAME}" --query "passwords[0].value" -o tsv)

# Create Azure Container Instance 
az container create --resource-group $AZ_RESOURCE_GROUP --name $AZ_APP_NAME --image "${AZ_CONTAINER_REGISTRY}.azurecr.io/${AZ_CONTAINER_NAME}" --ports 8080 --cpu 1 --memory 1 --ip-address public --registry-username $ACR_USERNAME --registry-password $ACR_PASSWORD \
  --environment-variables "AZURE_STORAGE_CONNECTION_STRING=$AZURE_STORAGE_CONNECTION_STRING"

# Get the public IP of the container
ACR_IP=$(az container show --name $AZ_APP_NAME --resource-group $AZ_RESOURCE_GROUP --query ipAddress.ip --output tsv)
echo "Done! http://${ACR_IP}:8080/swagger/"
