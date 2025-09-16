terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "6.13.0"
    }
  }

  # Configure the backend for Terraform state
  # backend "s3" {
  #   bucket         = aws_s3_bucket.terraform_state.bucket
  #   key            = "terraform.tfstate"
  #   region         = "us-east-1"
  #   dynamodb_table = aws_dynamodb_table.terraform_state_lock.name
  #   encrypt        = true
  # }
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