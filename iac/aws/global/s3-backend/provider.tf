terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "6.13.0"
    }
  }

  backend "s3" {
    bucket         = "rentifyx-clients-tf-state-dev"
    key            = "terraform.tfstate"
    region         = "us-east-1"
    dynamodb_table = "rentifyx-clients-tf-lock-dev"
    encrypt        = true
    profile        = "terraform-user"
  }
}

provider "aws" {
  region  = "us-east-1"
  profile = "terraform-user"

  default_tags {
    tags = {
      Project     = var.project_name
      Terraform   = "true"
      Environment = var.environment
      Component   = "backend"
    }
  }
}