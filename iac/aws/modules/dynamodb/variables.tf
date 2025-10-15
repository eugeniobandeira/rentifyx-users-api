variable "dynamodb_users_table_name" {
  description = "DynamoDB users table name"
  type        = string
  default     = "rentifyx-users"
}

variable "environment" {
  description = "The environment"
  type        = string
  default     = "dev"
}