# AssureCloud Backend

A comprehensive assurance management platform built with .NET 9, Clean Architecture, and Modular Monolith patterns.

## Architecture

```
backend/
├── src/
│   ├── AssureCloud.Domain              # Domain layer (no dependencies)
│   │   ├── Entities/                   # Domain entities (aggregates, entities, value objects)
│   │   ├── ValueObjects/               # Immutable value objects
│   │   ├── Enumerations/               # Type-safe enumerations
│   │   ├── Events/                     # Domain events
│   │   ├── Specifications/             # Query specifications
│   │   └── Aggregates/                 # Aggregate roots
│   ├── AssureCloud.Application         # Application layer (depends on Domain)
│   │   ├── Abstractions/               # Interfaces (repositories, services)
│   │   ├── Commands/                   # CQRS Commands
│   │   ├── Queries/                    # CQRS Queries
│   │   ├── DTOs/                       # Data Transfer Objects
│   │   ├── Validators/                 # FluentValidation validators
│   │   ├── Services/                   # Command/Query handlers
│   │   └── Mappings/                   # AutoMapper profiles
│   ├── AssureCloud.Infrastructure      # Infrastructure layer (implements Application abstractions)
│   │   ├── Persistence/                # EF Core implementations
│   │   │   ├── Context/                # DbContext
│   │   │   ├── Configurations/         # Entity configurations
│   │   │   └── Repositories/           # Repository implementations
│   │   └── Services/                   # Infrastructure services
│   ├── AssureCloud.Api                 # REST API (ASP.NET Core)
│   │   ├── Controllers/                # API Controllers
│   │   ├── Middleware/                 # Custom middleware
│   │   └── Program.cs                  # Entry point
│   ├── AssureCloud.Mvc                 # MVC Web UI (Razor Pages)
│   │   ├── Controllers/                # MVC Controllers
│   │   ├── Services/                   # API Client
│   │   ├── Models/                     # View Models
│   │   └── Program.cs                  # Entry point
│   ├── AssureCloud.Messaging           # Distributed Messaging (Azure Service Bus)
│   │   ├── Abstractions/               # Message publisher/consumer interfaces
│   │   ├── Messages/                   # Message types
│   │   └── Services/                   # Service Bus implementations
│   ├── AssureCloud.Workers             # Background Workers
│   │   └── Workers/                    # Background services
│   ├── AssureCloud.AppHost             # .NET Aspire Orchestration
│   └── AssureCloud.ServiceDefaults     # Shared service configuration
├── tests/
│   ├── AssureCloud.Domain.Tests        # Domain unit tests
│   ├── AssureCloud.Application.Tests   # Application unit tests
│   └── AssureCloud.Integration.Tests   # Integration tests
├── AssureCloud.sln                     # Solution file
├── Directory.Build.props               # Common build properties
├── Directory.Packages.props            # Central package management
├── global.json                         # .NET SDK version
├── docker-compose.yml                  # Local development stack
└── .editorconfig                       # Code style configuration
```

## Getting Started

### Prerequisites

- .NET 9 SDK
- Docker Desktop (for local dependencies)
- SQL Server (or use Docker)
- Azure Service Bus Emulator (included in docker-compose)

### Running with Docker Compose

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f api

# Stop services
docker-compose down
```

### Running Locally

1. Start dependencies:
```bash
docker-compose up -d sqlserver redis servicebus identityserver
```

2. Run database migrations:
```bash
dotnet ef database update --project src/AssureCloud.Infrastructure --startup-project src/AssureCloud.Api
```

3. Run the API:
```bash
dotnet run --project src/AssureCloud.Api
```

4. Run the MVC app:
```bash
dotnet run --project src/AssureCloud.Mvc
```

### Using .NET Aspire

```bash
dotnet run --project src/AssureCloud.AppHost
```

## Domain Model

### Core Aggregates

- **Organization** - Multi-tenant organizations with locations, suppliers, users
- **Program** - Assurance programs with standards, requirements, criteria, controls
- **Assessment** - Self-assessments with responses, evidence, findings
- **Audit** - External audits with findings, corrective actions
- **Certification** - Certifications with decisions, scopes
- **Report** - Generated reports

### Key Patterns

- **Domain Events** - For cross-aggregate communication
- **Enumeration Classes** - Type-safe enums with behavior
- **Value Objects** - Immutable value types (Email, PhoneNumber, Address, Money)
- **Specifications** - Encapsulated query logic
- **Repository Pattern** - Data access abstraction

## Application Layer

### CQRS with MediatR

- Commands for write operations
- Queries for read operations
- Validators using FluentValidation
- Handlers implementing business logic

### DTOs

- Input DTOs for commands
- Output DTOs for queries
- Pagination support

## Infrastructure

### Entity Framework Core

- SQL Server provider
- Configuration via IEntityTypeConfiguration
- Unit of Work pattern
- Soft delete support
- Audit timestamps

### Messaging

- Azure Service Bus integration
- Topic-based pub/sub
- Dead letter handling
- Message serialization

## Testing

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/AssureCloud.Domain.Tests
dotnet test tests/AssureCloud.Application.Tests
```

## API Endpoints

### Organizations
- `GET /api/v1/organizations` - List organizations (paginated)
- `GET /api/v1/organizations/{id}` - Get organization
- `POST /api/v1/organizations` - Create organization
- `PUT /api/v1/organizations/{id}` - Update organization
- `DELETE /api/v1/organizations/{id}` - Delete organization
- `POST /api/v1/organizations/{id}/locations` - Add location
- `POST /api/v1/organizations/{id}/suppliers` - Add supplier

### Programs
- `GET /api/v1/programs` - List programs
- `GET /api/v1/programs/{id}` - Get program
- `POST /api/v1/programs` - Create program
- `POST /api/v1/programs/{id}/standards` - Add standard
- `POST /api/v1/programs/standards/{id}/requirements` - Add requirement

### Assessments
- `GET /api/v1/assessments` - List assessments
- `GET /api/v1/assessments/{id}` - Get assessment
- `POST /api/v1/assessments` - Create assessment
- `POST /api/v1/assessments/{id}/start` - Start assessment
- `POST /api/v1/assessments/{id}/complete` - Complete assessment
- `POST /api/v1/assessments/{id}/responses` - Submit response
- `POST /api/v1/assessments/{id}/evidence` - Upload evidence
- `POST /api/v1/assessments/{id}/findings` - Create finding

### Audits
- `GET /api/v1/audits` - List audits
- `GET /api/v1/audits/{id}` - Get audit
- `POST /api/v1/audits` - Create audit
- `POST /api/v1/audits/{id}/start` - Start audit
- `POST /api/v1/audits/{id}/complete` - Complete audit
- `POST /api/v1/audits/{id}/findings` - Create finding
- `POST /api/v1/audits/{id}/corrective-actions` - Create corrective action

### Certifications
- `GET /api/v1/certifications` - List certifications
- `GET /api/v1/certifications/{id}` - Get certification
- `POST /api/v1/certifications` - Create certification
- `POST /api/v1/certifications/{id}/issue` - Issue certification
- `POST /api/v1/certifications/{id}/suspend` - Suspend certification
- `POST /api/v1/certifications/{id}/revoke` - Revoke certification
- `POST /api/v1/certifications/{id}/renew` - Renew certification

### Reports
- `GET /api/v1/reports` - List reports
- `GET /api/v1/reports/{id}` - Get report
- `POST /api/v1/reports` - Create report
- `POST /api/v1/reports/{id}/generate` - Generate report
- `GET /api/v1/reports/{id}/download` - Download report

## Configuration

Key configuration sections:

- `ConnectionStrings:DefaultConnection` - SQL Server connection
- `Messaging:ConnectionString` - Azure Service Bus connection
- `IdentityServer:Authority` - IdentityServer URL
- `IdentityServer:Audience` - API audience
- `Cors:AllowedOrigins` - Allowed CORS origins
- `FileStorage:BasePath` - File storage path
- `FileStorage:BaseUrl` - File storage URL

## License

Proprietary - AssureCloud Team