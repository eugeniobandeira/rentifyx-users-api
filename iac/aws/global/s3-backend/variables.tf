variable "project_name" {
  description = "The project name"
  type        = string
  default     = "rentifyx"
}

variable "service_name" {
  description = "The service name"
  type        = string
  default     = "users"
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

variable "dynamodb_users_table_name" {
  description = "DynamoDB users table name"
  type        = string
  default     = "rentifyx-users"
}