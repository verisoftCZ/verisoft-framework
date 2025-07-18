# Verisoft.DemoApi

A demonstration API service showcasing the capabilities of the Verisoft Framework, built with clean architecture and modern .NET 8.0 practices.

## 🏗️ Architecture

The project follows clean architecture patterns with the following layers:

- **Verisoft.DemoApi.Host** - Web API host, startup configuration, and controllers
- **Verisoft.DemoApi.Application** - Application business logic, use cases, and services
- **Verisoft.DemoApi.Common** - Shared utilities and common functionality
- **Verisoft.DemoApi.Contracts** - API contracts, DTOs, and request/response models
- **Verisoft.DemoApi.Data.EF** - Entity Framework data access layer and repositories
- **Verisoft.DemoApi.Client** - Generated client library for API consumption

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (local or remote)
- RabbitMQ (for message queuing)
- [Optional] Docker for containerized deployment

### Running Locally

1. **Clone and navigate to the project:**
   ```bash
   cd src/Verisoft.DemoApi
   ```

2. **Update configuration in appsettings.json:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=DemoApi;Trusted_Connection=true;"
     },
     "RabbitMQ": {
       "Host": "localhost",
       "Username": "guest",
       "Password": "guest"
     }
   }
   ```

3. **Run database migrations:**
   ```bash
   dotnet ef database update --project Verisoft.DemoApi.Data.EF
   ```

4. **Build and run the application:**
   ```bash
   dotnet build Verisoft.DemoApi.sln
   dotnet run --project Verisoft.DemoApi.Host
   ```

5. **Access the API:**
   - API: `https://localhost:5001`
   - Swagger UI: `https://localhost:5001/swagger`

### Using Docker

```bash
# Build and run with Docker Compose from root directory
docker compose up --build
```

This will start:
- Demo API service on port 80
- SQL Server database on port 1433
- RabbitMQ on ports 5672 (AMQP) and 15672 (Management UI)

## 🔧 Development

### Building the Solution

```bash
# Build all projects
dotnet build Verisoft.DemoApi.sln

# Build specific project
dotnet build Verisoft.DemoApi.Host/Verisoft.DemoApi.Host.csproj
```

### Running Tests

```bash
# Run all tests
dotnet test Verisoft.DemoApi.sln

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Database Migrations

```bash
# Add new migration
dotnet ef migrations add MigrationName --project Verisoft.DemoApi.Data.EF

# Update database
dotnet ef database update --project Verisoft.DemoApi.Data.EF

# Generate SQL script
dotnet ef migrations script --project Verisoft.DemoApi.Data.EF
```

## 📋 API Features

The Demo API demonstrates the following framework capabilities:

### Core Features
- RESTful API design with OpenAPI/Swagger documentation
- Clean architecture with dependency injection
- Entity Framework Core with SQL Server
- Message queuing with RabbitMQ and MassTransit
- Authentication and authorization
- Logging and error handling

### Example Endpoints
- `GET /api/demo/health` - Health check endpoint
- `GET /api/demo/items` - Get demo items
- `POST /api/demo/items` - Create new demo item
- `PUT /api/demo/items/{id}` - Update demo item
- `DELETE /api/demo/items/{id}` - Delete demo item

## 🔧 Configuration

### Environment Variables

```bash
# Database
ConnectionStrings__DefaultConnection="Server=db;Database=VerisoftFramework;User Id=sa;Password=your.password!;TrustServerCertificate=True;"

# RabbitMQ
RabbitMQ__Host="rabbitmq"
RabbitMQ__Username="user"
RabbitMQ__Password="password"

# Environment
ASPNETCORE_ENVIRONMENT="Development"
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DemoApi;Trusted_Connection=true;"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## 📦 Dependencies

### Core Dependencies
- .NET 8.0
- ASP.NET Core
- Entity Framework Core
- MassTransit (RabbitMQ)
- Verisoft.Core framework components

### Infrastructure
- SQL Server
- RabbitMQ
- Docker (for containerized deployment)

## 🧪 Testing

The project includes comprehensive tests demonstrating testing patterns:

```bash
# Run unit tests
dotnet test --filter Category=Unit

# Run integration tests
dotnet test --filter Category=Integration

# Run all tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📊 Monitoring

The API includes health checks and monitoring capabilities:

- Health check endpoint: `/health`
- Metrics and telemetry integration
- Structured logging with Serilog
- Application Insights support

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](../../LICENSE) file for details.

## 🤝 Contributing

This is a demonstration project showcasing the Verisoft Framework capabilities. Please follow the project's coding standards and ensure all tests pass before submitting changes.

---

**Part of the Verisoft Framework - Copyright © 2024 Verisoft s.r.o**