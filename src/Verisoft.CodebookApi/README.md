# Verisoft.CodebookApi

A codebook API service built with clean architecture principles, providing CRUD operations for managing application codebooks and reference data.

## 🏗️ Architecture

The project follows clean architecture patterns with the following layers:

- **Verisoft.CodebookApi.Host** - Web API host and startup configuration
- **Verisoft.CodebookApi.Application** - Application business logic and use cases
- **Verisoft.CodebookApi.Core** - Domain models and core business rules
- **Verisoft.CodebookApi.Contracts** - API contracts and DTOs
- **Verisoft.CodebookApi.Database** - Data access layer and Entity Framework context
- **Verisoft.CodebookApi.Client** - Generated client library for API consumption

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (local or remote)
- [Optional] Docker for containerized deployment

### Running Locally

1. **Clone and navigate to the project:**
   ```bash
   cd src/Verisoft.CodebookApi
   ```

2. **Update connection string in appsettings.json:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=CodebookApi;Trusted_Connection=true;"
     }
   }
   ```

3. **Run database migrations:**
   ```bash
   dotnet ef database update --project Verisoft.CodebookApi.Database
   ```

4. **Build and run the application:**
   ```bash
   dotnet build Verisoft.CodebookApi.sln
   dotnet run --project Verisoft.CodebookApi.Host
   ```

5. **Access the API:**
   - API: `https://localhost:5001`
   - Swagger UI: `https://localhost:5001/swagger`

### Using Docker

```bash
# Build and run with Docker Compose from root directory
docker compose up --build
```

## 🔧 Development

### Building the Solution

```bash
# Build all projects
dotnet build Verisoft.CodebookApi.sln

# Build specific project
dotnet build Verisoft.CodebookApi.Host/Verisoft.CodebookApi.Host.csproj
```

### Running Tests

```bash
# Run all tests
dotnet test Verisoft.CodebookApi.sln

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Database Migrations

```bash
# Add new migration
dotnet ef migrations add MigrationName --project Verisoft.CodebookApi.Database

# Update database
dotnet ef database update --project Verisoft.CodebookApi.Database

# Generate SQL script
dotnet ef migrations script --project Verisoft.CodebookApi.Database
```

## 📋 API Endpoints

The API provides the following main endpoints:

### Codebooks
- `GET /api/codebooks` - Get all codebooks
- `GET /api/codebooks/{id}` - Get specific codebook
- `POST /api/codebooks` - Create new codebook
- `PUT /api/codebooks/{id}` - Update codebook
- `DELETE /api/codebooks/{id}` - Delete codebook

### Codebook Items
- `GET /api/codebooks/{id}/items` - Get codebook items
- `POST /api/codebooks/{id}/items` - Add item to codebook
- `PUT /api/codebooks/{id}/items/{itemId}` - Update codebook item
- `DELETE /api/codebooks/{id}/items/{itemId}` - Delete codebook item

## 🔧 Configuration

### Environment Variables

```bash
# Database
ConnectionStrings__DefaultConnection="Server=localhost;Database=CodebookApi;Trusted_Connection=true;"

# Logging
Logging__LogLevel__Default="Information"
Logging__LogLevel__Microsoft.AspNetCore="Warning"

# CORS
CORS__AllowedOrigins="https://localhost:4200"
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CodebookApi;Trusted_Connection=true;"
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
- Entity Framework Core
- ASP.NET Core
- Verisoft.Core framework components

### Database
- SQL Server
- Entity Framework Core SQL Server provider

## 🧪 Testing

The project includes comprehensive tests:

```bash
# Run unit tests
dotnet test --filter Category=Unit

# Run integration tests
dotnet test --filter Category=Integration

# Run all tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](../../LICENSE) file for details.

## 🤝 Contributing

Please follow the project's coding standards and ensure all tests pass before submitting changes.

---

**Part of the Verisoft Framework - Copyright © 2024 Verisoft s.r.o**