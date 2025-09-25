output "dynamodb_table_arn" {
  description = "ARN da tabela DynamoDB"
  value       = module.dynamodb_table.dynamodb_table_arn
}

output "dynamodb_table_id" {
  description = "ID da tabela DynamoDB"
  value       = module.dynamodb_table.dynamodb_table_id
}