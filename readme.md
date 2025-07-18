# Verisoft Framework

A comprehensive enterprise framework built with .NET 8.0 and Angular 18, providing a robust foundation for building scalable web applications and APIs.

## 🏗️ Architecture

The framework consists of several key components:

- **Verisoft.Core** - Core framework libraries with common functionality
- **Verisoft.CodebookApi** - Codebook API service with clean architecture
- **Verisoft.DemoApi** - Demo API service showcasing framework capabilities
- **Verisoft.Frontend** - Angular 18 frontend application with NX monorepo

## 🚀 Quick Start

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Running with Docker Compose

To run the entire project with Docker Compose:

```bash
docker compose up --build
```

This will start:
- Demo API service
- SQL Server database
- RabbitMQ message broker
- Nginx router

### Development Setup

#### Backend (.NET)

1. Navigate to the source directory:
   ```bash
   cd src
   ```

2. Build the Core framework:
   ```bash
   dotnet build Verisoft.Core.sln
   ```

3. Run the Demo API:
   ```bash
   dotnet run --project Verisoft.DemoApi/Verisoft.DemoApi.Host
   ```

#### Frontend (Angular)

1. Navigate to the frontend directory:
   ```bash
   cd src/Verisoft.Frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

## 📁 Project Structure

```
├── build/              # Build scripts and configuration
├── deploy/             # Deployment configurations
├── docs/               # Project documentation
├── src/                # Source code
│   ├── Verisoft.Core/            # Core framework components
│   ├── Verisoft.CodebookApi/     # Codebook API service
│   ├── Verisoft.DemoApi/         # Demo API service
│   └── Verisoft.Frontend/        # Angular frontend application
├── docker-compose.yml  # Docker Compose configuration
└── README.md          # This file
```

## 🔧 Technologies

### Backend
- .NET 8.0
- Entity Framework Core
- MassTransit (RabbitMQ)
- SQL Server
- Docker

### Frontend
- Angular 18
- TypeScript 5.5
- NX 20.0
- Bootstrap 5.3
- PrimeNG 17.18
- RxJS 7.8

## 🛠️ Development

### Building the Solution

```bash
# Build all .NET projects
dotnet build

# Build specific solution
dotnet build src/Verisoft.Core.sln
dotnet build src/Verisoft.DemoApi.sln
dotnet build src/Verisoft.CodebookApi.sln
```

### Running Tests

```bash
# Run .NET tests
dotnet test

# Run frontend tests
cd src/Verisoft.Frontend
npm test
```

### Linting

```bash
# Lint frontend code
cd src/Verisoft.Frontend
npm run lint
```

## 📋 Requirements

- SQL Server (for data storage)
- RabbitMQ (for message queuing)
- Modern web browser (for frontend)

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Please read the [Framework Guidelines](docs/Framework-guidelines.md) for information about the project structure and development standards.

## 📚 Documentation

- [Framework Guidelines](docs/Framework-guidelines.md)
- [Roadmap](docs/Roadmap.md)
- [API Documentation](docs/)

---

**Copyright © 2024 Verisoft s.r.o**