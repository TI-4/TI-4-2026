# Pre-Deployment Checklist

Before deploying this project to a production environment, ensure the following technical debt and security configurations are resolved:

### Security & Identity
- [ ] **Database Credentials**: Change the default `rootpassword` for both **PostgreSQL** and **MongoDB** in `docker-compose.yml` to use secure, injected environment variables.
- [ ] **JWT Secrets**: The JWT Secret Key (`JwtConfig__Key`) is currently hardcoded. Move this to a secure environment variable or a Secret Manager (like Azure Key Vault).

### Docker & Containers
- [ ] **Prebuilt Images**: Do not build images on the fly in production (`build: .` in compose). Images should be pre-built via CI/CD and pulled from a registry like [Docker Hub](https://hub.docker.com/).
- [ ] **Multi-stage Builds**: Ensure all API `Dockerfile`s use multi-stage layered builds to keep production image sizes small and secure.
- [ ] **Non-Root Execution**: Do not run containers as root. Add `USER app` (or equivalent non-root user) to all Dockerfiles to prevent privilege escalation attacks.

### Networking & Ports
- [ ] **Port Redirection**: The APIs (Campus, Schedule, etc.) expose Port 80 internally by default (via `ENTRYPOINT ["dotnet", "API.dll"]`). Ensure these are correctly mapped and redirected in `docker-compose.yml`.
- [ ] **Port Assignments**: For local debugging, strictly adhere to the `50xx` standard (e.g., 5001, 5002). For production, randomize host ports or place them securely behind the API Gateway without exposing them directly.
- [ ] **Database Exposure**: Audit exposed ports in `docker-compose.yml` to ensure internal databases (Postgres/Mongo) are not exposed publicly to the internet.
- [ ] **Network Architecture**: Review Docker networking topologies ([Overlay](https://docs.docker.com/engine/network/drivers/overlay/), [Host](https://docs.docker.com/engine/network/drivers/host/), [Bridge](https://docs.docker.com/engine/network/drivers/bridge/)) to choose the correct driver for your production scaling strategy.

### API Gateway & Codebase
- [ ] **CORS Security**: Update `Program.cs` to remove `AllowAnyOrigin()` and restrict the CORS policy to the exact production frontend domain.
- [ ] **Allowed Hosts**: Update `appsettings.json` to lock down `AllowedHosts` to the production domain (remove `*`).
- [ ] **Code Cleanup**: Thoroughly clean up unused files, unreferenced routes, dead code, and caches before the final production build.
