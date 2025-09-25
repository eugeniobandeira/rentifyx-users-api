variable "dynamodb_clients_table_name" {
  description = "DynamoDB clients table name"
  type        = string
  default     = "rentifyx-clients"
}

variable "environment" {
  description = "The environment"
  type        = string
  default     = "dev"
}