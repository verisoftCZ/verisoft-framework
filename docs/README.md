# Verisoft Framework Documentation

This directory contains comprehensive documentation for the Verisoft Framework, including guidelines, architecture decisions, and development practices.

## 📚 Documentation Overview

### Framework Guidelines
- **[Framework-guidelines.md](Framework-guidelines.md)** - Complete project structure and development guidelines
- **[Framework-guidelines/](Framework-guidelines/)** - Detailed framework documentation with examples and best practices

### Project Roadmap
- **[Roadmap.md](Roadmap.md)** - Project roadmap and future development plans
- **[Roadmap/](Roadmap/)** - Detailed roadmap documentation and milestones

## 🏗️ Architecture Documentation

The Verisoft Framework follows a modular, enterprise-grade architecture:

### Backend Architecture
- **Clean Architecture** - Separation of concerns with distinct layers
- **Domain-Driven Design** - Business logic organized around domain models
- **Microservices** - Independent, deployable services
- **CQRS Pattern** - Command Query Responsibility Segregation where applicable

### Frontend Architecture
- **Component-Based** - Reusable Angular components
- **State Management** - NgRx for predictable state management
- **Micro-Frontend Ready** - Module federation support
- **Design System** - Consistent UI components and patterns

## 🛠️ Development Guidelines

### Code Standards
- Follow established coding conventions
- Use StyleCop for .NET projects
- Use ESLint and Prettier for TypeScript/Angular
- Maintain high test coverage

### Project Structure
```
/                   # Root repository folder
├── build/          # Build scripts and configuration
├── deploy/         # Deployment scripts and configuration
├── docs/           # This documentation
├── src/            # All source code
│   ├── apps/       # Frontend applications
│   ├── infra/      # Infrastructure components
│   ├── services/   # Backend microservices
│   ├── shared/     # Shared libraries and components
│   └── tests/      # Test projects
```

### Development Workflow
1. **Feature Development** - Work on feature branches
2. **Code Review** - Pull request reviews required
3. **Testing** - Unit and integration tests
4. **Documentation** - Update relevant documentation
5. **Deployment** - CI/CD pipeline deployment

## 🔧 Technologies

### Backend Stack
- **.NET 8.0** - Main framework
- **Entity Framework Core** - Data access
- **MassTransit** - Message queuing
- **SQL Server** - Primary database
- **RabbitMQ** - Message broker

### Frontend Stack
- **Angular 18** - Frontend framework
- **TypeScript** - Programming language
- **NX** - Monorepo build system
- **PrimeNG** - UI component library
- **NgRx** - State management

### DevOps & Infrastructure
- **Docker** - Containerization
- **Docker Compose** - Local development
- **Azure DevOps** - CI/CD pipelines
- **Azure** - Cloud infrastructure

## 📋 Getting Started

### For Developers
1. Read the [Framework Guidelines](Framework-guidelines.md)
2. Set up your development environment
3. Review the project structure
4. Start with the demo projects

### For Architects
1. Review the architecture decisions
2. Understand the framework patterns
3. Review the roadmap and planned features
4. Consider extension points and customization

### For Project Managers
1. Review the [Roadmap](Roadmap.md)
2. Understand the framework capabilities
3. Plan project timelines accordingly
4. Review available documentation

## 🤝 Contributing to Documentation

To contribute to this documentation:

1. **Follow Markdown Standards** - Use consistent formatting
2. **Include Examples** - Provide practical code examples
3. **Keep it Updated** - Ensure documentation matches current code
4. **Add Diagrams** - Use diagrams for complex concepts
5. **Link Related Content** - Cross-reference related documents

## 📄 License

This documentation is part of the Verisoft Framework and is licensed under the MIT License. See the [LICENSE](../LICENSE) file for details.

---

**Verisoft Framework Documentation - Copyright © 2024 Verisoft s.r.o**