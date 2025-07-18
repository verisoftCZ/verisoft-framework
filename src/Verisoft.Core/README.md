# Verisoft.Core

Core framework libraries providing common functionality and utilities for building enterprise applications with .NET 8.0.

## 📦 Components

This framework consists of several modular components:

### Authentication & Security
- **Verisoft.Core.Authentication** - Base authentication functionality
- **Verisoft.Core.Authentication.MicrosoftIdentity** - Microsoft Identity integration
- **Verisoft.Core.Authentication.Permissions** - Permission-based authorization
- **Verisoft.Core.Security** - Security utilities and helpers

### Data & Persistence
- **Verisoft.Core.Data** - Data access abstractions
- **Verisoft.Core.Data.EF** - Entity Framework Core integration
- **Verisoft.Core.Contracts** - Domain contracts and interfaces

### Web & API
- **Verisoft.Core.AspNet** - ASP.NET Core extensions
- **Verisoft.Core.AspNet.Concurrency** - Concurrency handling for web applications

### Infrastructure
- **Verisoft.Core.Common** - Common utilities and helpers
- **Verisoft.Core.Configuration** - Configuration management
- **Verisoft.Core.BlobStorage** - Blob storage abstractions
- **Verisoft.Core.MassTransit** - Message bus integration

### Document Processing
- **Verisoft.Core.Excel** - Excel file processing
- **Verisoft.Core.Pdf** - PDF document generation
- **Verisoft.Core.Template** - Template processing
- **Verisoft.Core.ExportStrategies** - Data export strategies

### Utilities
- **Verisoft.Core.TypeMapper** - Object mapping utilities
- **Verisoft.Core.Validation** - Validation frameworks
- **Verisoft.Core.Aspire.Hosting** - .NET Aspire hosting extensions

## 🚀 Installation

### As NuGet Package References

Add references to the specific components you need in your .csproj file:

```xml
<PackageReference Include="Verisoft.Core.Common" Version="1.0.0" />
<PackageReference Include="Verisoft.Core.Data.EF" Version="1.0.0" />
<PackageReference Include="Verisoft.Core.Authentication" Version="1.0.0" />
```

### As Project References

When developing within the same solution:

```xml
<ProjectReference Include="..\Verisoft.Core\Verisoft.Core.Common\Verisoft.Core.Common.csproj" />
<ProjectReference Include="..\Verisoft.Core\Verisoft.Core.Data.EF\Verisoft.Core.Data.EntityFramework.csproj" />
```

## 🛠️ Development

### Building the Framework

```bash
# Build all core components
dotnet build Verisoft.Core.sln

# Build specific component
dotnet build Verisoft.Core.Common/Verisoft.Core.Common.csproj
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test Verisoft.Core.Common.Test/Verisoft.Core.Common.Test.csproj
```

## 📋 Requirements

- .NET 8.0 or later
- Dependencies vary by component (see individual .csproj files)

## 🔧 Configuration

Most components use standard .NET configuration patterns. Example configuration in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyApp;Trusted_Connection=true;"
  },
  "Authentication": {
    "Authority": "https://login.microsoftonline.com/tenant-id",
    "ClientId": "your-client-id"
  }
}
```

## 📖 Usage Examples

### Using Common Utilities

```csharp
using Verisoft.Core.Common;

// Example usage of common utilities
var result = await SomeCommonUtility.ProcessAsync(data);
```

### Using Data Access

```csharp
using Verisoft.Core.Data.EF;

// Example Entity Framework integration
services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));
```

### Using Authentication

```csharp
using Verisoft.Core.Authentication;

// Example authentication setup
services.AddVerisoftAuthentication(configuration);
```

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](../../LICENSE) file for details.

## 🤝 Contributing

Please follow the coding standards defined in the `.ruleset` file and ensure all tests pass before submitting changes.

---

**Part of the Verisoft Framework - Copyright © 2024 Verisoft s.r.o**