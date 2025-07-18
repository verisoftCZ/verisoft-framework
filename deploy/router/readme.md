# Nginx Router

Nginx reverse proxy configuration for the Verisoft Framework, used to route traffic between frontend and backend services in Docker deployment.

## 🎯 Purpose

This Nginx configuration serves as a reverse proxy to:
- Route API requests to backend services
- Serve frontend static files
- Prevent CORS issues in development
- Provide a single entry point for all services
- Eliminate the need to reconfigure ports frequently

## 🚀 Usage

### With Docker Compose

The router is automatically configured when using Docker Compose:

```bash
# From the root directory
docker compose up --build
```

The router will be available at `http://localhost:80`

### Configuration

The Nginx configuration routes requests as follows:

- **Frontend:** Static files served from frontend build
- **API endpoints:** Proxied to backend services
- **Health checks:** Routed to appropriate services

## 🔧 Customization

To modify the routing configuration:

1. Edit the Nginx configuration file in this directory
2. Rebuild the Docker image:
   ```bash
   docker compose up --build router
   ```

## 📋 Requirements

- Docker
- Docker Compose
- Backend API services running
- Frontend build artifacts

## 🛠️ Development

The router configuration is optimized for development environments and includes:
- CORS handling
- Request logging
- Error handling
- Static file serving

## 📄 License

This configuration is part of the Verisoft Framework and is licensed under the MIT License. See the [LICENSE](../../LICENSE) file for details.

---

**Part of the Verisoft Framework - Copyright © 2024 Verisoft s.r.o**