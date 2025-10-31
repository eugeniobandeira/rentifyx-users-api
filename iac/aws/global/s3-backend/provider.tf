terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "6.13.0"
    }
  }

  # backend "s3" {
  #   bucket         = "rentifyx-users-tf-state"
  #   key            = "terraform.tfstate"
  #   region         = "us-east-1"
  #   dynamodb_table = "rentifyx-users-tf-lock"
  #   encrypt        = true
  #   profile        = "terraform-user"
  # }
}

provider "aws" {
  region  = "sa-east-1"
  profile = "terraform-user"

  default_tags {
    tags = {
      Project     = var.project_name
      Terraform   = "true"
      Component   = "backend"
      IAC         = "true"
      Terraform   = "true"
    }
  }
}