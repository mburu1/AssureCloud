# AssureCloud

**Enterprise Sustainability, Compliance & Assurance Platform**

AssureCloud is a production-oriented digital assurance platform designed to manage sustainability programs, organizations, assessments, evidence, audits, compliance workflows, reporting, and certification activities.

The project demonstrates how to design, build, secure, test, containerize, and deploy a high-traffic enterprise application using **C#/.NET 10, ASP.NET Core REST APIs, ASP.NET Core MVC, Angular, Scalar, JWT, SQL, messaging, Docker, .NET Aspire, Azure Container Apps, Azure DevOps, and Terraform**.

The architecture is designed around clean boundaries, scalability, security, observability, automated testing, and cloud-native deployment.

---

## 1. Project Goals

AssureCloud focuses on the engineering problems involved in large-scale enterprise digital products:

* High-performance .NET 10 backend services
* RESTful API design
* Secure JWT-based authentication and authorization
* ASP.NET Core MVC for server-rendered administrative functionality
* Angular SPA for the primary user portal
* SQL-based transactional persistence
* Asynchronous messaging
* Docker containerization
* .NET Aspire orchestration
* Azure Container Apps deployment
* Azure DevOps CI/CD
* Terraform Infrastructure as Code
* Automated unit, integration, API, and architecture testing
* Application observability
* Secure configuration and secret management
* Horizontal scalability
* Distributed-team-friendly architecture and development practices

---

# 2. Business Problem

Large sustainability and assurance organizations must manage large amounts of:

* Organizations
* Suppliers
* Farms
* Products
* Certification programs
* Sustainability requirements
* Assessments
* Audit activities
* Evidence
* Non-conformities
* Corrective actions
* Certification decisions
* Documents
* Notifications
* Compliance reports

Traditional workflows often rely on disconnected systems, spreadsheets, emails, and manually maintained documents.

AssureCloud provides a unified digital platform for managing the full assurance lifecycle.

---

# 3. Core Functional Areas

## Organization Management

* Organization registration
* Organization profiles
* Supplier management
* Locations
* Contact management
* Organization hierarchy
* Status management

## Program Management

* Sustainability programs
* Standards
* Requirements
* Criteria
* Controls
* Applicable jurisdictions
* Program versions

## Assessment Management

* Assessment creation
* Assessment scheduling
* Assessment assignments
* Assessment workflows
* Evidence collection
* Assessment scoring
* Findings

## Audit Management

* Audit planning
* Auditor assignment
* Audit execution
* Findings
* Non-conformities
* Corrective actions
* Follow-up audits

## Certification Management

* Certification applications
* Certification decisions
* Certificate lifecycle
* Expiration tracking
* Suspension
* Renewal
* Revocation

## Evidence Management

* Document uploads
* Evidence metadata
* Evidence verification
* Versioning
* Audit trails

## Reporting

* Compliance dashboards
* Assessment metrics
* Certification statistics
* Audit findings
* Corrective action status
* Exportable reports

## Notifications

* Email notifications
* Workflow notifications
* Assessment reminders
* Certification expiration alerts
* Corrective action deadlines

---

# 4. Technology Stack

## Backend

| Technology             | Purpose                                     |
| ---------------------- | ------------------------------------------- |
| C#                     | Primary programming language                |
| .NET 10                | Backend platform                            |
| ASP.NET Core           | REST API                                    |
| ASP.NET Core MVC       | Server-rendered administrative portal       |
| Entity Framework Core  | Data access                                 |
| SQL Server / Azure SQL | Relational database                         |
| Scalar                 | OpenAPI API documentation                   |
| JWT Bearer             | Authentication                              |
| ASP.NET Core Identity  | Identity and authorization                  |
| FluentValidation       | Request validation                          |
| MediatR                | Application request/command handling        |
| Serilog                | Structured logging                          |
| OpenTelemetry          | Observability                               |
| Azure Service Bus      | Messaging                                   |
| .NET Aspire            | Local distributed application orchestration |

## Frontend

| Technology         | Purpose                  |
| ------------------ | ------------------------ |
| Angular            | Main SPA                 |
| TypeScript         | Frontend language        |
| Angular Router     | Application navigation   |
| Angular HttpClient | REST API communication   |
| RxJS               | Reactive programming     |
| Angular Material   | Enterprise UI components |
| Reactive Forms     | Form management          |

## Cloud / DevOps

| Technology                 | Purpose                |
| -------------------------- | ---------------------- |
| Azure Container Apps       | Container hosting      |
| Azure Container Registry   | Container images       |
| Azure SQL                  | Cloud database         |
| Azure Service Bus          | Messaging              |
| Azure Key Vault            | Secrets                |
| Azure Application Insights | Monitoring             |
| Azure DevOps               | CI/CD                  |
| Terraform                  | Infrastructure as Code |
| Docker                     | Containerization       |
| .NET Aspire                | Local orchestration    |

## Testing

| Technology                         | Purpose             |
| ---------------------------------- | ------------------- |
| xUnit                              | Unit testing        |
| FluentAssertions                   | Test assertions     |
| Moq                                | Mocking             |
| Testcontainers                     | Integration testing |
| ASP.NET Core WebApplicationFactory | API testing         |
| Playwright                         | End-to-end testing  |
| Angular Testing                    | Frontend testing    |

---

# 5. Architecture

AssureCloud follows a **Clean Architecture + Modular Monolith + Distributed Messaging** approach.

```text
                       ┌─────────────────────────┐
                       │       Angular SPA       │
                       │   TypeScript + RxJS     │
                       └────────────┬────────────┘
                                    │ HTTPS
                                    ▼
                       ┌─────────────────────────┐
                       │     ASP.NET Core API    │
                       │     REST + Scalar       │
                       │       JWT Security      │
                       └────────────┬────────────┘
                                    │
                     ┌──────────────┼──────────────┐
                     │              │              │
                     ▼              ▼              ▼
              Application        Domain       Infrastructure
                     │              │              │
                     └──────────────┼──────────────┘
                                    │
                     ┌──────────────┼─────────────┐
                     ▼              ▼             ▼
                 Azure SQL     Azure Service   External APIs
                                  Bus
                                    │
                                    ▼
                           Background Workers
                                    │
                                    ▼
                               Notifications
```

Administrative functionality can additionally be exposed through:

```text
ASP.NET Core MVC
      │
      ├── Razor Views
      ├── Administrative workflows
      ├── Reporting
      └── Operational dashboards
```

---

# 6. Repository Structure

```text
AssureCloud/
│
├── backend/
│   │
│   ├── src/
│   │   │
│   │   ├── AssureCloud.Api/
│   │   │   ├── Controllers/
│   │   │   ├── Middleware/
│   │   │   ├── Filters/
│   │   │   ├── Extensions/
│   │   │   ├── Configuration/
│   │   │   ├── Authentication/
│   │   │   ├── Authorization/
│   │   │   └── Program.cs
│   │   │
│   │   ├── AssureCloud.Mvc/
│   │   │   ├── Controllers/
│   │   │   ├── Views/
│   │   │   ├── ViewModels/
│   │   │   ├── Areas/
│   │   │   ├── Middleware/
│   │   │   └── Program.cs
│   │   │
│   │   ├── AssureCloud.Application/
│   │   │   ├── Abstractions/
│   │   │   ├── Behaviors/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   ├── DTOs/
│   │   │   ├── Validators/
│   │   │   ├── Services/
│   │   │   └── Mappings/
│   │   │
│   │   ├── AssureCloud.Domain/
│   │   │   ├── Entities/
│   │   │   ├── ValueObjects/
│   │   │   ├── Aggregates/
│   │   │   ├── Events/
│   │   │   ├── Enums/
│   │   │   ├── Specifications/
│   │   │   └── Exceptions/
│   │   │
│   │   ├── AssureCloud.Infrastructure/
│   │   │   ├── Persistence/
│   │   │   │   ├── DbContext/
│   │   │   │   ├── Configurations/
│   │   │   │   ├── Migrations/
│   │   │   │   └── Repositories/
│   │   │   ├── Identity/
│   │   │   ├── Messaging/
│   │   │   ├── Storage/
│   │   │   ├── Email/
│   │   │   ├── ExternalServices/
│   │   │   └── Observability/
│   │   │
│   │   ├── AssureCloud.Messaging/
│   │   │   ├── Contracts/
│   │   │   ├── Consumers/
│   │   │   ├── Producers/
│   │   │   └── Handlers/
│   │   │
│   │   ├── AssureCloud.Workers/
│   │   │   ├── Assessment/
│   │   │   ├── Notifications/
│   │   │   ├── Reports/
│   │   │   └── Certification/
│   │   │
│   │   └── AssureCloud.AppHost/
│   │       └── Program.cs
│   │
│   └── tests/
│       ├── AssureCloud.UnitTests/
│       ├── AssureCloud.ApplicationTests/
│       ├── AssureCloud.IntegrationTests/
│       ├── AssureCloud.ApiTests/
│       ├── AssureCloud.ArchitectureTests/
│       └── AssureCloud.EndToEndTests/
│
├── frontend/
│   │
│   ├── assurecloud-web/
│   │   ├── src/
│   │   │   ├── app/
│   │   │   │   ├── core/
│   │   │   │   ├── shared/
│   │   │   │   ├── features/
│   │   │   │   │   ├── dashboard/
│   │   │   │   │   ├── organizations/
│   │   │   │   │   ├── programs/
│   │   │   │   │   ├── assessments/
│   │   │   │   │   ├── audits/
│   │   │   │   │   ├── certifications/
│   │   │   │   │   ├── evidence/
│   │   │   │   │   ├── reports/
│   │   │   │   │   └── administration/
│   │   │   │   ├── guards/
│   │   │   │   ├── interceptors/
│   │   │   │   ├── services/
│   │   │   │   ├── models/
│   │   │   │   └── app.routes.ts
│   │   │   │
│   │   │   ├── assets/
│   │   │   ├── environments/
│   │   │   ├── styles/
│   │   │   └── main.ts
│   │   │
│   │   ├── angular.json
│   │   ├── package.json
│   │   ├── tsconfig.json
│   │   └── Dockerfile
│
├── infrastructure/
│   │
│   ├── terraform/
│   │   ├── modules/
│   │   │   ├── resource-group/
│   │   │   ├── container-app/
│   │   │   ├── container-registry/
│   │   │   ├── sql/
│   │   │   ├── service-bus/
│   │   │   ├── key-vault/
│   │   │   └── monitoring/
│   │   │
│   │   ├── environments/
│   │   │   ├── dev/
│   │   │   ├── staging/
│   │   │   └── production/
│   │   │
│   │   ├── main.tf
│   │   ├── variables.tf
│   │   ├── outputs.tf
│   │   └── providers.tf
│   │
│   └── docker/
│       ├── api/
│       ├── mvc/
│       ├── worker/
│       └── angular/
│
├── deploy/
│   │
│   ├── azure/
│   ├── container-apps/
│   └── scripts/
│
├── pipelines/
│   ├── azure-pipelines.yml
│   ├── ci.yml
│   ├── cd.yml
│   ├── security-scan.yml
│   └── infrastructure.yml
│
├── docs/
│   ├── architecture/
│   ├── ooad/
│   ├── uml/
│   ├── database/
│   ├── api/
│   ├── security/
│   ├── messaging/
│   ├── deployment/
│   ├── adr/
│   └── operations/
│
├── .dockerignore
├── .gitignore
├── Directory.Build.props
├── Directory.Build.targets
├── global.json
├── docker-compose.yml
├── README.md
└── AssureCloud.slnx
```

---

# 7. Backend Architecture

The backend uses dependency inversion:

```text
Api
 │
 ▼
Application
 │
 ▼
Domain

Infrastructure
 │
 ├── Application
 └── Domain
```

The Domain layer has no dependency on Infrastructure.

The Application layer defines use cases and contracts.

Infrastructure implements persistence, messaging, external integrations, authentication infrastructure, storage, and observability.

The API is responsible for HTTP concerns and translating incoming requests into application commands and queries.

---

# 8. REST API

The REST API follows resource-oriented design.

Example endpoints:

```http
GET    /api/v1/organizations
GET    /api/v1/organizations/{id}
POST   /api/v1/organizations
PUT    /api/v1/organizations/{id}
DELETE /api/v1/organizations/{id}

GET    /api/v1/assessments
POST   /api/v1/assessments
GET    /api/v1/assessments/{id}
POST   /api/v1/assessments/{id}/submit

GET    /api/v1/audits
POST   /api/v1/audits

GET    /api/v1/certifications
POST   /api/v1/certifications

GET    /api/v1/reports/compliance
GET    /api/v1/reports/audits
```

API characteristics:

* RESTful resource design
* API versioning
* Pagination
* Filtering
* Sorting
* Validation
* Idempotency
* Consistent error responses
* Correlation IDs
* Structured logging
* Authorization policies
* Rate limiting
* Health checks

---

# 9. Scalar API Documentation

Scalar is used as the interactive API documentation interface.

```text
ASP.NET Core API
       │
       ▼
OpenAPI
       │
       ▼
Scalar
```

Developers can inspect:

* Endpoints
* Schemas
* Request models
* Response models
* Authentication
* HTTP methods
* API contracts

---

# 10. Authentication & Authorization

Authentication uses JWT bearer tokens.

```text
User
 │
 ▼
Authentication
 │
 ▼
JWT
 │
 ▼
Angular
 │
 ▼
Authorization Header
 │
 ▼
ASP.NET Core API
```

Example:

```http
Authorization: Bearer <access-token>
```

Authorization is implemented through:

* Roles
* Claims
* Policies
* Resource-based authorization
* Permission checks

Example roles:

```text
Administrator
AssuranceManager
Auditor
Reviewer
CertificationOfficer
OrganizationUser
ReadOnly
```

Security practices include:

* HTTPS
* JWT validation
* Short-lived access tokens
* Secure refresh strategy
* Password hashing
* Role-based access control
* Policy-based authorization
* Input validation
* Output encoding
* CSRF protection where applicable
* Rate limiting
* Security headers
* Secret isolation
* Audit logging

---

# 11. SQL Database

Primary transactional storage:

```text
SQL Server
       │
       └── Azure SQL in production
```

Major aggregates include:

```text
Organizations
OrganizationLocations
Programs
Standards
Requirements
Assessments
AssessmentResponses
Audits
AuditFindings
CorrectiveActions
Certifications
Evidence
Documents
Users
Roles
Notifications
AuditLogs
```

Entity Framework Core is used for:

* Migrations
* Database configuration
* Relationships
* Querying
* Transactions
* Optimistic concurrency

Performance considerations:

* Indexing
* Query projection
* Pagination
* Compiled queries where necessary
* Avoiding N+1 queries
* Connection pooling
* Read/write optimization
* Database monitoring

---

# 12. Messaging

The platform uses asynchronous messaging for long-running and decoupled processes.

Example:

```text
AssessmentSubmitted
        │
        ▼
Azure Service Bus
        │
        ├── Notification Worker
        ├── Reporting Worker
        ├── Audit Worker
        └── Analytics Worker
```

Example events:

```text
OrganizationCreated
AssessmentCreated
AssessmentSubmitted
AssessmentApproved
AssessmentRejected
AuditCompleted
FindingCreated
CorrectiveActionCreated
CorrectiveActionCompleted
CertificationIssued
CertificationExpired
```

Messaging principles:

* Async processing
* Retry policies
* Dead-letter queues
* Idempotent consumers
* Correlation IDs
* Event contracts
* Failure isolation

---

# 13. .NET Aspire

.NET Aspire is used for local development and distributed application orchestration.

Development topology:

```text
                    Aspire AppHost
                         │
          ┌──────────────┼──────────────┐
          │              │              │
          ▼              ▼              ▼
        API             MVC          Worker
          │              │              │
          └──────────────┼──────────────┘
                         │
              ┌──────────┴──────────┐
              ▼                     ▼
           SQL Server          Service Bus
```

Aspire provides:

* Local orchestration
* Service discovery
* Health checks
* Dashboard
* Telemetry
* Environment configuration
* Distributed application development

---

# 14. Docker

Every deployable service is containerized.

```text
Docker
 │
 ├── API Container
 ├── MVC Container
 ├── Worker Container
 └── Angular Container
```

Example production flow:

```text
Source Code
    │
    ▼
Docker Build
    │
    ▼
Container Image
    │
    ▼
Azure Container Registry
    │
    ▼
Azure Container Apps
```

Containers are designed to be:

* Stateless
* Reproducible
* Configurable through environment variables
* Independently scalable
* Health-check aware

---

# 15. Azure Container Apps

Production services are hosted using Azure Container Apps.

Example deployment:

```text
Azure Container Apps Environment
│
├── assurecloud-api
├── assurecloud-mvc
├── assurecloud-worker
└── assurecloud-web
```

Supporting Azure services:

```text
Azure Container Registry
Azure SQL
Azure Service Bus
Azure Key Vault
Application Insights
Log Analytics
```

Scaling considerations:

* Horizontal scaling
* CPU-based scaling
* Memory-based scaling
* HTTP concurrency
* Queue-driven scaling
* Stateless services

---

# 16. Azure DevOps CI/CD

Azure DevOps manages CI/CD.

Pipeline:

```text
Git Push
   │
   ▼
Build
   │
   ▼
Unit Tests
   │
   ▼
Integration Tests
   │
   ▼
Security Scanning
   │
   ▼
Docker Build
   │
   ▼
Push to ACR
   │
   ▼
Terraform Validation
   │
   ▼
Deploy Infrastructure
   │
   ▼
Deploy Container Apps
   │
   ▼
Smoke Tests
```

Pipeline responsibilities:

* Restore dependencies
* Build solution
* Run tests
* Generate coverage
* Perform security checks
* Build Docker images
* Push images
* Validate Terraform
* Provision infrastructure
* Deploy services
* Execute smoke tests

---

# 17. Terraform

Infrastructure as Code is managed through Terraform.

Example resources:

```text
Resource Group
Container Apps Environment
Container Apps
Container Registry
Azure SQL
Service Bus
Key Vault
Application Insights
Log Analytics
Networking
Managed Identity
```

Environment model:

```text
dev
staging
production
```

Terraform workflow:

```text
terraform fmt
terraform validate
terraform plan
terraform apply
```

Infrastructure changes should be reviewed through pull requests before production deployment.

---

# 18. Automated Testing

Testing is implemented at multiple levels.

## Unit Tests

Test:

* Domain logic
* Application services
* Validators
* Business rules

## Integration Tests

Test:

* SQL persistence
* Messaging
* Infrastructure integrations
* Repository behavior

## API Tests

Test:

* HTTP endpoints
* Authentication
* Authorization
* Validation
* Error handling
* Serialization

## Architecture Tests

Verify dependency rules such as:

```text
Domain
  ↓
must not depend on Infrastructure
```

## End-to-End Tests

Playwright validates:

```text
Angular
  ↓
API
  ↓
Database
```

Critical workflows:

```text
Login
Organization registration
Assessment submission
Audit workflow
Corrective action
Certification
Reporting
```

---

# 19. Observability

OpenTelemetry is used for:

* Logs
* Metrics
* Distributed traces

Important telemetry:

```text
Request duration
HTTP error rate
Database latency
Message processing time
Queue depth
Container health
Authentication failures
Business transaction failures
```

Correlation identifiers allow a transaction to be traced across:

```text
Angular
   ↓
API
   ↓
Application
   ↓
Database
   ↓
Service Bus
   ↓
Worker
```

---

# 20. Performance & Scalability

The system is designed for high-traffic environments.

Strategies include:

* Async programming
* Stateless APIs
* Horizontal container scaling
* Database indexing
* Efficient EF Core queries
* Pagination
* Response compression
* Caching
* Background processing
* Asynchronous messaging
* Connection pooling
* Distributed tracing
* Load testing

Potential caching:

```text
Memory Cache
Distributed Redis Cache
HTTP caching
Application-level caching
```

---

# 21. Security Architecture

Security follows defense-in-depth principles.

```text
Internet
   │
   ▼
HTTPS
   │
   ▼
Azure Container Apps
   │
   ▼
JWT Authentication
   │
   ▼
Policy Authorization
   │
   ▼
Application Validation
   │
   ▼
SQL / Messaging
```

Security controls include:

* HTTPS everywhere
* JWT validation
* Role and policy authorization
* Secure secret storage
* Azure Key Vault
* Managed identities
* Input validation
* SQL parameterization
* Security headers
* Rate limiting
* Audit logs
* Dependency scanning
* Container vulnerability scanning
* Infrastructure security controls

Security principles:

```text
Least Privilege
Zero Trust
Defense in Depth
Secure by Default
Fail Securely
```

---

# 22. API Error Contract

The API uses a consistent error structure.

Example:

```json
{
  "type": "https://api.assurecloud.com/errors/validation",
  "title": "Validation failed",
  "status": 400,
  "instance": "/api/v1/assessments",
  "traceId": "00-abc123",
  "errors": {
    "organizationId": [
      "Organization is required."
    ]
  }
}
```

---

# 23. Health Checks

Health endpoints:

```http
GET /health
GET /health/live
GET /health/ready
```

Readiness verifies critical dependencies such as:

```text
SQL Database
Service Bus
External integrations
```

---

# 24. Development Workflow

```text
Feature Branch
      │
      ▼
Implementation
      │
      ▼
Unit Tests
      │
      ▼
Integration Tests
      │
      ▼
Pull Request
      │
      ▼
Code Review
      │
      ▼
Azure DevOps CI
      │
      ▼
Staging
      │
      ▼
Smoke Tests
      │
      ▼
Production
```

Recommended branch structure:

```text
main
develop
feature/*
bugfix/*
hotfix/*
release/*
```

---

# 25. Local Development

Required tooling:

```text
.NET 10 SDK
Visual Studio / Rider / VS Code
Node.js
Angular CLI
Docker Desktop
Azure CLI
Terraform
Git
SQL Server
```

Optional but recommended:

```text
Azure Storage Explorer
Azure Service Bus Explorer
Postman
```

---

# 26. Running the Backend

```powershell
cd backend
dotnet restore
dotnet build
dotnet test
```

Run the API:

```powershell
dotnet run --project src/AssureCloud.Api
```

Run MVC:

```powershell
dotnet run --project src/AssureCloud.Mvc
```

---

# 27. Running Angular

```powershell
cd frontend/assurecloud-web
npm install
npm start
```

---

# 28. Running with Aspire

```powershell
cd backend/src/AssureCloud.AppHost
dotnet run
```

Aspire orchestrates the local application topology and provides the development dashboard.

---

# 29. Docker

Build:

```powershell
docker build -t assurecloud-api ./backend
```

Run:

```powershell
docker run -p 8080:8080 assurecloud-api
```

For the complete environment:

```powershell
docker compose up --build
```

---

# 30. Database

Create migrations:

```powershell
dotnet ef migrations add InitialCreate `
  --project src/AssureCloud.Infrastructure `
  --startup-project src/AssureCloud.Api
```

Apply migrations:

```powershell
dotnet ef database update `
  --project src/AssureCloud.Infrastructure `
  --startup-project src/AssureCloud.Api
```

---

# 31. API Documentation

Once the API is running, Scalar provides the interactive API documentation.

```text
OpenAPI
   ↓
Scalar
   ↓
Explore / Test API
```

The API contract documents:

* Endpoints
* Request models
* Response models
* Authentication
* Validation
* HTTP status codes

---

# 32. Domain Model

Core domain:

```text
Organization
    │
    ├── OrganizationLocation
    ├── Supplier
    └── User

Program
    │
    ├── Standard
    ├── Requirement
    └── Criterion

Assessment
    │
    ├── AssessmentResponse
    ├── Evidence
    └── Finding

Audit
    │
    ├── AuditFinding
    └── CorrectiveAction

Certification
    │
    └── CertificationDecision
```

---

# 33. Event-Driven Workflow Example

Assessment submission:

```text
User
 │
 ▼
Angular
 │
 ▼
POST /api/v1/assessments/{id}/submit
 │
 ▼
Application Layer
 │
 ▼
AssessmentSubmitted
 │
 ▼
Azure Service Bus
 │
 ├────► Notification Worker
 │
 ├────► Reporting Worker
 │
 └────► Audit Worker
```

This allows the request path to remain responsive while non-critical work happens asynchronously.

---

# 34. Architecture Decisions

Important architectural decisions are documented under:

```text
docs/adr/
```

Example ADRs:

```text
ADR-001 Clean Architecture
ADR-002 Modular Monolith
ADR-003 Angular SPA
ADR-004 ASP.NET Core MVC
ADR-005 Azure Service Bus
ADR-006 Azure Container Apps
ADR-007 Terraform
ADR-008 JWT Authentication
ADR-009 SQL Server / Azure SQL
ADR-010 .NET Aspire
```

---

# 35. Engineering Principles

The project follows:

```text
SOLID
DRY
KISS
Clean Architecture
Domain-Driven Design concepts
Dependency Inversion
Separation of Concerns
API-first design
Secure-by-default
Observability-first
Infrastructure as Code
Automated Testing
Continuous Delivery
```

---

# 36. What This Project Demonstrates

This repository demonstrates practical experience with:

* C#
* .NET 10
* ASP.NET Core
* ASP.NET Core Web API
* ASP.NET Core MVC
* Angular
* TypeScript
* REST APIs
* Scalar
* OpenAPI
* JWT
* SQL
* Entity Framework Core
* Messaging
* Azure Service Bus
* Docker
* .NET Aspire
* Azure Container Apps
* Azure Container Registry
* Azure SQL
* Azure Key Vault
* Azure DevOps
* Terraform
* CI/CD
* Automated testing
* OpenTelemetry
* Cloud architecture
* Application security
* Distributed systems
* High-traffic application design

---

# 37. Target Architecture

```text
                         ┌───────────────────────┐
                         │       Angular         │
                         │       TypeScript      │
                         └──────────┬────────────┘
                                    │
                                    ▼
                         ┌───────────────────────┐
                         │    ASP.NET Core API   │
                         │ REST + Scalar + JWT   │
                         └──────────┬────────────┘
                                    │
                     ┌──────────────┼──────────────┐
                     │              │              │
                     ▼              ▼              ▼
                  Domain      Application   Infrastructure
                                                    │
                         ┌──────────────────────────┤
                         │                          │
                         ▼                          ▼
                     Azure SQL               Azure Service Bus
                         │                          │
                         │                          ▼
                         │                    Background Workers
                         │
                         ▼
                  Azure Container Apps
                         │
              ┌──────────┼───────────┐
              ▼          ▼           ▼
             API        MVC        Workers

                  Infrastructure
                         │
                         ▼
                     Terraform

                  CI/CD
                         │
                         ▼
                   Azure DevOps
```

---

# 38. Why This Repository Exists

The project is intentionally designed as a realistic enterprise system rather than a simple CRUD demonstration.

It demonstrates the complete software lifecycle:

```text
Requirements
     ↓
Domain Modeling
     ↓
Architecture
     ↓
API Design
     ↓
Implementation
     ↓
Testing
     ↓
Containerization
     ↓
Infrastructure as Code
     ↓
CI/CD
     ↓
Cloud Deployment
     ↓
Observability
     ↓
Operations
```

The goal is to demonstrate not only the ability to write .NET code, but also the ability to participate in **system design, architecture, cloud engineering, security, DevOps, testing, and product development**.

---

# 39. Future Enhancements

Potential extensions include:

* AI-assisted evidence classification
* Automated compliance analysis
* Document intelligence
* Risk scoring
* Predictive non-conformity detection
* Advanced analytics
* GIS-based organization/farm visualization
* Offline assessment workflows
* Multi-region deployment
* Event sourcing for selected aggregates
* Distributed caching
* Data warehouse integration
* Power BI integration

---

# 40. License

This project is intended as a portfolio and engineering demonstration project.

```text
MIT License
```

---

## Portfolio Positioning

**AssureCloud** demonstrates an end-to-end enterprise technology stack centered on **C#/.NET 10 and Angular**, including RESTful APIs, JWT security, SQL, messaging, Docker, .NET Aspire, Azure Container Apps, Azure DevOps CI/CD, Terraform Infrastructure as Code, automated testing, observability, and cloud-native architecture.
