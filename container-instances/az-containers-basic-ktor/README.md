# Deploying a Linux Virtual Machine in Azure with Terraform

- Welcome to my Azure Projects repository! This repository serves as an example of using Azure CLI scripts to provision a Ktor API into an Azure Container Instances service.

## Overview

This guide will walk you through deploying the Container Instances using the scripts. Additionally, this repository is part of an initiative where I am compiling a series of projects and proof of concepts. These resources are intended to serve as practical examples and references for anyone interested in learning Azure.

To explore other projects and concepts within this initiative, please visit the repository index:
[https://github.com/HugoGomezArenas/azure-projects](https://github.com/HugoGomezArenas/azure-projects)

## Prerequisites

Before you begin, make sure you have the following installed:

- Azure CLI or Azure PowerShell
- Active Azure subscription
- Docker installed
- gitbash (for Windows users only)

## Steps to Deploy

## Step 1: Create the .env file

The .env file contains environment-specific variables that are used during the deployment process, ensuring the configuration is correctly set up for your Azure environment. Inside the folder cli, create the .env file. Use as reference the example.env file:

| Variable              | Description                                                 |
| --------------------- | ----------------------------------------------------------- |
| AZ_RESOURCE_GROUP     | The resource group where you want to allocate the resources |
| AZ_REGION             | The Azure Region where you want to allocate the resources   |
| AZ_CONTAINER_REGISTRY | The name of the Azure Container Registry                    |
| AZ_CONTAINER_NAME     | The azure container's name                                  |
| AZ_APP_NAME           | The application's name                                      |

All the resources will be created by the script if they do not already exist.

### Step 2: Execute Deployment script

Run the /cli/azure-cli-deployment.sh script from the command line, ensuring you are in the project directory.

### Step 3: Verify the service is running

Once the script execution is done, the service URL will be displayed in the console.
After opening the URL, you should see a confirmation page or the default API endpoint indicating that the Ktor service is successfully running.
