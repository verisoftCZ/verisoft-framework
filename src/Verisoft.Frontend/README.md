# Verisoft.Frontend

Modern Angular 18 frontend application built with NX monorepo architecture, providing a scalable and maintainable foundation for enterprise web applications.

## 🏗️ Architecture

The frontend is built using:

- **Framework:** Angular 18.2.8
- **Build System:** NX 20.0.0
- **Language:** TypeScript 5.5.4
- **UI Components:** PrimeNG 17.18.11 with Bootstrap 5.3.3
- **State Management:** NgRx 18.0.2
- **Icons:** PrimeIcons 7.0.0
- **Internationalization:** ngx-translate 15.0.0

## 🚀 Getting Started

### Prerequisites

- [Node.js 18+](https://nodejs.org/)
- [npm](https://www.npmjs.com/) (comes with Node.js)

### Installation

1. **Navigate to the frontend directory:**
   ```bash
   cd src/Verisoft.Frontend
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Start the development server:**
   ```bash
   npm start
   ```

4. **Open your browser:**
   - Application: `http://localhost:4200`

## 🛠️ Development

### Available Scripts

```bash
# Start development server
npm start

# Build for production
npm run build

# Run tests
npm test

# Run linting
npm run lint

# Fix linting issues
npm run lint:fix

# Generate dependency graph
npm run graph

# Publish packages
npm run publish
```

### NX Commands

```bash
# Serve the application
nx serve Verisoft.Frontend

# Build the application
nx build Verisoft.Frontend

# Run tests
nx test Verisoft.Frontend

# Run linting on all projects
nx run-many --target=lint --all

# Generate code
nx generate @nx/angular:component my-component
```

## 📁 Project Structure

```
src/
├── libs/                       # Shared libraries
│   ├── core/                   # Core functionality
│   ├── store/                  # State management
│   ├── ui-core/               # Core UI components
│   ├── ui-govcz/              # Government design system components
│   ├── ui-primeng/            # PrimeNG component wrappers
│   └── security/              # Security and authentication
├── apps/                      # Applications (if using NX workspace)
└── assets/                    # Static assets
```

## 🔧 Technologies & Dependencies

### Core Dependencies
- **Angular 18.2.8** - Main framework
- **RxJS 7.8.0** - Reactive programming
- **NgRx 18.0.2** - State management
- **PrimeNG 17.18.11** - UI component library
- **Bootstrap 5.3.3** - CSS framework
- **ngx-translate 15.0.0** - Internationalization

### Development Dependencies
- **NX 20.0.0** - Monorepo build system
- **TypeScript 5.5.4** - Language
- **Jest 29.7.0** - Testing framework
- **ESLint 8.57.0** - Code linting
- **Prettier 2.6.2** - Code formatting

## 🎨 UI Components

The application uses a combination of:

- **PrimeNG Components** - Rich UI components for data display and input
- **Gov Design System** - Government-compliant design system components
- **Bootstrap Classes** - Utility classes for layout and styling
- **Custom Components** - Application-specific components

## 🔐 Security Features

- Authentication integration with backend APIs
- Role-based access control
- Secure token handling
- Password strength validation with zxcvbn

## 🧪 Testing

### Running Tests

```bash
# Run unit tests
npm test

# Run tests with coverage
npm test -- --coverage

# Run tests in watch mode
npm test -- --watch
```

### Test Configuration

- **Jest** for unit testing
- **Testing Library** for component testing
- **Cypress** for end-to-end testing (if configured)

## 🌐 Internationalization

The application supports multiple languages using ngx-translate:

```typescript
// Usage in components
constructor(private translate: TranslateService) {
  this.translate.setDefaultLang('en');
}

// Usage in templates
{{ 'HELLO_WORLD' | translate }}
```

## 📦 Building for Production

```bash
# Build for production
npm run build

# Build with specific configuration
nx build Verisoft.Frontend --configuration=production
```

The build artifacts will be stored in the `dist/` directory.

## 🔧 Configuration

### Environment Files

- `src/environments/environment.ts` - Development environment
- `src/environments/environment.prod.ts` - Production environment

### NX Configuration

- `nx.json` - NX workspace configuration
- `project.json` - Project-specific configuration
- `tsconfig.json` - TypeScript configuration

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](../../LICENSE) file for details.

## 🤝 Contributing

Please follow the established coding standards and ensure all tests pass before submitting changes.

### Code Style

- Use ESLint and Prettier for code formatting
- Follow Angular style guide
- Use conventional commits for commit messages

---

**Part of the Verisoft Framework - Copyright © 2024 Verisoft s.r.o**