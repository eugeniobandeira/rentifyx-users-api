module "dynamodb_table" {
  source   = "terraform-aws-modules/dynamodb-table/aws"

  name     = "${var.dynamodb_users_table_name}-${var.environment}"
  hash_key = "Document"
  billing_mode = "PAY_PER_REQUEST"

  attributes = [
    {
      name = "Document"
      type = "S"
    }
  ]

  tags = {
    Terraform   = "true"
    Environment = "dev"
  }
}