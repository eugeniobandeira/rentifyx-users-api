variable "aws_account_id" {
  description = "The AWS Account ID"
  type        = string
  default     = "480831398199"
}

variable "project_name" {
  description = "The project name"
  type        = string
  default     = "rentifyx"
}

variable "service_name" {
  description = "The service name"
  type        = string
  default     = "clients"
}

variable "environment" {
  description = "The environment"
  type        = string
  default     = "dev"
}

variable "dynamodb_lock_table_name" {
  description = "DynamoDB lock table name"
  type        = string
  default     = "tf-lock"
}

variable "dynamodb_clients_table_name" {
  description = "DynamoDB clients table name"
  type        = string
  default     = "rentifyx-clients"
}