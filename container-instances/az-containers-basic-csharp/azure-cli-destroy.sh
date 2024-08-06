ENV_FILE=".env"

if [ -f "$ENV_FILE" ]; then
    source "$ENV_FILE"
else
    echo ".env file does not exist."
    exit
fi


az login
az group delete --name $AZ_RESOURCE_GROUP