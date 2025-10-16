# RentifyX - Users Registration Service

A core service of the RentifyX property rental platform, responsible for robust and scalable customer (tenant) registration. Built with modern .NET and cloud-native principles.

## 🚀 Key Features

- **Users Onboarding:** Secure registration flow with data validation, email verification, and initial profile setup.
- **Email Integration:** Sends welcome and account verification emails via Amazon SES upon successful registration.
- **Data Persistence:** Stores customer data in a highly available and scalable DynamoDB table.
- **Document Storage:** Utilizes Amazon S3 for secure storage of customer-uploaded documents (e.g., identity verification).
- **Idempotent Operations:** Ensures safe retries of registration requests without creating duplicate customers.

## 🛠️ Technology Stack

- **Framework:** .NET 9 with Minimal APIs
- **Architecture:** Clean Architecture
- **Design:** Domain-Driven Design (DDD) with .NET Aspire
- **Testing:** Test-Driven Development (TDD)
- **Cloud & IaC:**
  - **Infrastructure as Code (IaC):** Terraform
  - **Database:** Amazon DynamoDB
  - **Storage:** Amazon S3
  - **Email:** Amazon Simple Email Service (SES)
- **Dependency Management:** Centralized versioning via `Directory.Packages.props`

## 📋 API Endpoints (Clients)

| Method     | Endpoint              | Description                        | Auth     |
| :--------- | :-------------------- | :--------------------------------- | :------- |
| **POST**   | `v1/api/users`      | Registers a new user.          | None     |
| **GET**    | `v1/api/users`      | Gets a list of user's profile. | Required |
| **GET**    | `v1/api/users/{id}` | Gets a user's profile.         | Required |
| **UPDATE** | `v1/api/users/{id}` | Updates a user profile.      | Required |
| **DELETE** | `v1/api/users/{id}` | Deletes a user profile.      | Required |
