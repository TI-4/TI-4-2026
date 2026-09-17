# Pre-Deployment Checklist

Before deploying this project to a production environment, ensure the following technical debt and security configurations are resolved:

### API Gateway
- [ ] **CORS Security**: Update `Program.cs` to remove `AllowAnyOrigin()` and restrict the CORS policy to the exact production frontend domain.
- [ ] **Allowed Hosts**: Update `appsettings.json` to lock down `AllowedHosts` to the production domain (remove `*`).

### Databases & Infrastructure
- [ ] **Passwords**: Change the default `rootpassword` in `docker-compose.yml` to use secure, injected environment variables.
- [ ] **Port Exposure**: Audit exposed ports in `docker-compose.yml` to ensure internal databases (Postgres/Mongo) are not exposed publicly to the internet if they don't need to be.
- [ ] **JWT Secrets**: The JWT Secret Key (JwtConfig__Key) is currently hardcoded in plain text. Move this to a secure environment variable or a Secret Manager (like Azure Key Vault or AWS Secrets Manager) before production.
