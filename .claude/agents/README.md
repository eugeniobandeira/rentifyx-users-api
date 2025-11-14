# Rentifyx Users API - Specialized Agents

This directory contains specialized Claude Code agents tailored for the Rentifyx.Users .NET API project. Each agent has deep expertise in specific domains to enhance development efficiency and code quality.

## Prerequisites

Before using the agents, ensure you have the following installed:

| Tool | Required For | Installation |
|------|--------------|--------------|
| **.NET 8 SDK** | All development | [Download](https://dotnet.microsoft.com/download) |
| **Docker Desktop** | Integration tests | [Download](https://www.docker.com/products/docker-desktop) |
| **Git** | Version control | [Download](https://git-scm.com/) |
| **AWS CLI** (optional) | AWS operations | [Download](https://aws.amazon.com/cli/) |

**Important Notes:**
- 🐳 **Docker is REQUIRED** for running integration tests (uses Testcontainers)
- ✅ Unit tests (Validators, Handlers) work **WITHOUT Docker**
- ⚡ Quick test feedback: `dotnet test --filter "FullyQualifiedName!~Integration"` (skips Docker)

### Quick Setup Verification

```bash
# Verify .NET installation
dotnet --version

# Verify Docker installation (required for integration tests)
docker --version
docker ps  # Should NOT error - start Docker Desktop if it does

# Run unit tests (no Docker needed)
dotnet test --filter "FullyQualifiedName!~Integration"

# Run integration tests (Docker required)
dotnet test tests/Integration/Integration.Test/
```

**Common Issue:**
```
❌ Error: "Docker daemon is not running"
✅ Solution: Start Docker Desktop and run `docker ps` to verify
```

---

## Available Agents

### 1. **code-reviewer** 🔍
Expert in reviewing .NET code for Clean Architecture, SOLID principles, and best practices.

**Use when:**
- After implementing new features
- Before creating pull requests
- Reviewing security-critical code changes
- Ensuring architectural compliance

**Specialties:**
- Clean Architecture & DDD validation
- SOLID principles enforcement
- C# best practices
- Security vulnerability detection
- Performance optimization

---

### 2. **test-runner** 🧪
Specialized in executing tests, generating coverage reports, and maintaining test quality.

**Use when:**
- Running unit or integration tests
- Generating code coverage reports
- Debugging test failures
- Optimizing test performance
- Setting up CI/CD test pipelines

**Specialties:**
- xUnit test execution
- **Testcontainers & Docker integration** (for integration tests)
- Code coverage analysis (Coverlet)
- Test result reporting
- Integration test setup with DynamoDB containers
- Test optimization

**⚠️ Requirements:**
- **Docker Desktop required** for integration tests (uses Testcontainers)
- Unit tests (Validators, Handlers) run without Docker
- Integration tests spin up real DynamoDB containers

---

### 3. **dotnet-analyzer** 🔧
Expert in .NET build analysis, package management, and code quality tools.

**Use when:**
- Resolving build warnings
- Managing NuGet packages
- Checking for security vulnerabilities
- Optimizing project configuration
- Enforcing code style rules

**Specialties:**
- MSBuild & .csproj configuration
- NuGet Central Package Management
- Roslyn analyzers
- Code style enforcement (.editorconfig)
- Compiler warnings (CA*, CS*, IDE*)

---

### 4. **api-security-checker** 🔒
Security expert focused on OWASP Top 10 and .NET API security.

**Use when:**
- Performing security audits
- Reviewing authentication/authorization
- Checking for vulnerabilities
- Implementing security best practices
- Preparing for security compliance

**Specialties:**
- OWASP Top 10 threat analysis
- Authentication & authorization (JWT, Identity)
- Input validation & injection prevention
- Data protection & encryption
- Security headers & CORS
- Dependency vulnerability scanning

---

### 5. **migration-helper** 🗄️
Database migration expert for Entity Framework Core and AWS RDS.

**Use when:**
- Creating database migrations
- Deploying schema changes
- Managing migration rollbacks
- Optimizing database performance
- Handling data migrations

**Specialties:**
- Entity Framework Core migrations
- Database design & indexing
- Safe deployment strategies
- AWS RDS management
- Data integrity & backup

---

### 6. **github-actions-expert** ⚙️
CI/CD specialist for GitHub Actions workflows.

**Use when:**
- Creating or optimizing CI/CD pipelines
- Setting up automated testing
- Configuring Docker builds
- Deploying to AWS
- Troubleshooting workflow failures

**Specialties:**
- GitHub Actions workflows
- .NET CI/CD pipelines
- Docker multi-stage builds
- AWS deployment (ECR, ECS)
- Pipeline optimization & caching
- Security scanning automation

---

### 7. **aws-developer** ☁️
AWS cloud architecture and development expert.

**Use when:**
- Architecting AWS infrastructure
- Deploying to ECS/Fargate
- Managing RDS databases
- Setting up monitoring & logging
- Optimizing AWS costs
- Implementing Infrastructure as Code

**Specialties:**
- ECS (Fargate/EC2) deployment
- RDS database management
- AWS CDK (.NET) & CloudFormation
- Secrets Manager & Parameter Store
- CloudWatch monitoring
- Cost optimization
- IAM & security best practices

---

### 8. **semantic-commit** 📝
Semantic commit message expert following Conventional Commits specification.

**Use when:**
- Creating meaningful commit messages
- Validating commit message format
- Setting up git commit templates
- Generating changelogs
- Preparing for releases
- Reviewing commit history

**Specialties:**
- Conventional Commits (feat, fix, chore, etc.)
- Project-specific scopes (api, domain, infrastructure)
- Commit message validation
- Breaking change documentation
- Changelog generation
- Git hooks integration
- Semantic versioning alignment

---

## How to Use Agents

### Using the Task Tool
```typescript
// In Claude Code, invoke agents using the Task tool
Task({
  subagent_type: "code-reviewer",
  prompt: "Review the authentication implementation in UserController.cs for security issues and SOLID violations"
})
```

### Common Workflows

**Feature Development Workflow:**
1. **aws-developer**: Architect AWS infrastructure
2. **dotnet-analyzer**: Ensure clean build
3. **code-reviewer**: Review implementation
4. **test-runner**: Run tests with coverage (🐳 ensure Docker is running for integration tests)
5. **api-security-checker**: Security audit
6. **github-actions-expert**: Set up CI/CD

**Database Change Workflow:**
1. **migration-helper**: Create and test migration
2. **test-runner**: Run integration tests (🐳 Docker required)
3. **code-reviewer**: Review migration code
4. **github-actions-expert**: Automate deployment
5. **aws-developer**: Apply to DynamoDB/RDS

**Security Audit Workflow:**
1. **api-security-checker**: Perform OWASP audit
2. **dotnet-analyzer**: Check for vulnerable packages
3. **code-reviewer**: Review security fixes
4. **test-runner**: Verify security tests pass

**Deployment Workflow:**
1. **test-runner**: Run full test suite (🐳 Docker required for integration tests)
2. **dotnet-analyzer**: Verify no warnings
3. **github-actions-expert**: Build and push to ECR
4. **aws-developer**: Deploy to ECS
5. **migration-helper**: Apply database changes (if needed)

**Quick Development Loop (No Docker):**
1. Make code changes
2. **test-runner**: Run unit tests only (`--filter "FullyQualifiedName!~Integration"`)
3. **code-reviewer**: Quick review
4. Before commit: **test-runner**: Full test suite with Docker
5. **semantic-commit**: Create meaningful commit message

**Release Workflow:**
1. **semantic-commit**: Review commit history for changelog
2. **dotnet-analyzer**: Verify build and packages
3. **test-runner**: Full test suite with coverage
4. **api-security-checker**: Security audit
5. **semantic-commit**: Generate changelog from commits
6. **github-actions-expert**: Tag and release via CI/CD

---

## Best Practices

1. **Use the Right Agent**: Select the agent that best matches your current task
2. **Chain Agents**: Use multiple agents for complex workflows
3. **Be Specific**: Provide clear context and requirements
4. **Review Output**: Always review agent recommendations before applying
5. **Learn from Agents**: Study the detailed explanations to improve your skills

---

## Project Context

**Project**: Rentifyx.Users API
**Architecture**: Clean Architecture / DDD
**Technology**: .NET 8, Entity Framework Core, ASP.NET Core
**Database**: AWS DynamoDB (NoSQL)
**Cloud**: AWS (ECS Fargate, RDS, ECR, DynamoDB)
**CI/CD**: GitHub Actions
**Testing**: xUnit, Coverlet, Testcontainers
**Containerization**: Docker (for integration tests & deployment)

**Layer Structure:**
```
src/
├── 01 - Api/              # API presentation layer
├── 02 - Application/      # Application logic & handlers
├── 03 - Domain/           # Domain entities & business logic
├── 04 - Exception/        # Custom exceptions
├── 05 - IoC/              # Dependency injection
└── 06 - Infrastructure/   # Data access & external services (DynamoDB)

tests/
├── Validators.Test/       # Unit tests (no Docker)
├── Handlers.Test/         # Unit tests (no Docker)
├── Integration.Test/      # Integration tests (requires Docker)
└── CommomTestUtilities/   # Shared test helpers
```

**Integration Test Architecture:**
- Uses **Testcontainers** to spin up real DynamoDB containers
- **WebApplicationFactory** for in-memory API testing
- **xUnit Collection Fixture** for container reuse (performance optimization)
- Requires **Docker Desktop** running locally
- CI/CD (GitHub Actions) includes Docker by default

---

## Feedback & Improvements

These agents are living documents. If you find areas for improvement or need additional specializations, feel free to:
- Update agent prompts with project-specific patterns
- Add new agents for emerging needs
- Share successful workflows with the team

---

**Created**: 2024-11-14
**Last Updated**: 2024-11-14 (Added Docker/Testcontainers documentation)
**Maintainer**: Development Team

---

## Changelog

### 2025-11-14 (Update 2)
- ✨ **NEW AGENT**: semantic-commit - Conventional Commits expert
- 📝 Added commit message standards and validation
- 🔄 Added Release Workflow using semantic-commit
- 📋 Updated Quick Development Loop to include commit step

### 2025-11-14 (Update 1)
- ✨ Added Prerequisites section with Docker requirements
- 🐳 Updated test-runner agent description to highlight Testcontainers
- 📚 Enhanced Project Context with integration test architecture
- ⚡ Added Quick Development Loop workflow (without Docker)
- 🔧 Added Quick Setup Verification commands
