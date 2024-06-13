# Deployment of C# Azure function using Terraform

## Project Overview 
This project leverages Terraform as Infrastructure as Code (IaC) to deploy Azure resources, including a Windows Function App autonomously. A GitHub Actions pipeline is used to manage the CI/CD process. The Azure Function consists of multiple functions with different triggers (HTTP, Blob, Timer). The Graph API is implemented as a core component of these functions.
