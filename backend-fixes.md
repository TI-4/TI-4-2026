# Notas reunion 21-09

- cambiar creds del mongodb y postgres
- no buildear dockerimage como root
- dockerimages en capas
- /taller-2/src/Services/Campus/Dockerfile puerto 80 expuesto **si o si redireccionar cuando montados en docker-compose.yml** (ENTRYPOINT ["dotnet", "Campus.API.dll"]) 
- /taller-2/src/Services/Schedule/Dockerfile otro puerto 80 expuesto  **si o si redireccionar cuando montados en docker-compose.yml** (ENTRYPOINT ["dotnet", "Schedule.API.dll"])
- limpiar cache, archivos y rutas no utilizadas
- no buildear la imagen en compose up, tenerla prebuildeada y/o subida a https://hub.docker.com
- cambiar redireccion de puertos a puertos generados random (debug siempre en puertos 50xx)
- revisar docs de docker network [overlay](https://docs.docker.com/engine/network/drivers/overlay/) , [host](https://docs.docker.com/engine/network/drivers/host/) , [bridge](https://docs.docker.com/engine/network/drivers/bridge/) , [vlan](https://docs.docker.com/engine/network/drivers/ipvlan/)

## Referencia docker-compose

```yaml
identity-api:
  build:
      context: .
      dockerfile: src/Services/Identity/Dockerfile
  ports:
      - "5001:80"
  environment: # todos los env values deben ir dentro del .env
          ConnectionStrings__IdentityDb: 
          JwtConfig__Key: 
          JwtConfig__Issuer: 
          JwtConfig__Audience: 
          JwtConfig__TokenValidationMins: 
  depends_on:
      - postgres # crear nuevo postgress para cada ms

# ---- Campus ---- #
  campus-api:
    build:
      context: .
      dockerfile: src/Services/Campus/Dockerfile
    container_name: uct_campus_api
    ports:
      - "5002:80"
    environment: # todos los env values deben ir dentro del .env
      ConnectionStrings__CampusDb: 
    depends_on:
      - postgres # crear nuevo postgress para cada ms
    networks:
      - uct_network

  # ---- Incident ---- #
  incident-api:
    build:
      context: .
      dockerfile: src/Services/Incident/Incident.API/Dockerfile 
    ports:
      - "5003:80"
    environment: # todos los env values deben ir dentro del .env
      MongoDB__ConnectionString: 
      MongoDB__DatabaseName: 
    depends_on:
      - mongodb 
    networks:
      - uct_network

  # ---- Schedule ---- #
  schedule-api:
    build:
      context: .
      dockerfile: src/Services/Schedule/Dockerfile
    container_name: uct_schedule_api
    ports:
      - "5004:80"
    environment: # todos los env values deben ir dentro del .env
      ConnectionStrings__ScheduleDb: 
    depends_on:
      - postgres # crear nuevo postgress para cada ms
    networks:
      - uct_network

networks:
  uct_network:
    driver: bridge # 

volumes: 
  postgres_data:
  mongodb_data:
```

## Corrección Propuesta (`correccion-propuesta.yaml`)

```yaml
version: "3.8"

services:
  # ==========================================
  # Microservices APIs
  # ==========================================

  # ---- Identity ---- #
  identity-api:
    build:
      context: .
      dockerfile: src/Services/Identity/Dockerfile
    container_name: uct_identity_api
    ports:
      - "${IDENTITY_PORT:-5001}:80"
    environment:
      ConnectionStrings__IdentityDb: ${IDENTITY_DB_CONNECTION_STRING}
      JwtConfig__Key: ${JWT_KEY}
      JwtConfig__Issuer: ${JWT_ISSUER}
      JwtConfig__Audience: ${JWT_AUDIENCE}
      JwtConfig__TokenValidationMins: ${JWT_TOKEN_VALIDATION_MINS}
    depends_on:
      identity-db:
        condition: service_healthy
    networks:
      - uct_network

  # ---- Campus ---- #
  campus-api:
    build:
      context: .
      dockerfile: src/Services/Campus/Dockerfile
    container_name: uct_campus_api
    ports:
      - "${CAMPUS_PORT:-5002}:80"
    environment:
      ConnectionStrings__CampusDb: ${CAMPUS_DB_CONNECTION_STRING}
    depends_on:
      campus-db:
        condition: service_healthy
    networks:
      - uct_network

  # ---- Incident ---- #
  incident-api:
    build:
      context: .
      dockerfile: src/Services/Incident/Incident.API/Dockerfile
    container_name: uct_incident_api
    ports:
      - "${INCIDENT_PORT:-5003}:80"
    environment:
      MongoDB__ConnectionString: ${INCIDENT_MONGO_CONNECTION_STRING}
      MongoDB__DatabaseName: ${INCIDENT_MONGO_DATABASE_NAME}
    depends_on:
      incident-mongo:
        condition: service_healthy
    networks:
      - uct_network

  # ---- Schedule ---- #
  schedule-api:
    build:
      context: .
      dockerfile: src/Services/Schedule/Dockerfile
    container_name: uct_schedule_api
    ports:
      - "${SCHEDULE_PORT:-5004}:80"
    environment:
      ConnectionStrings__ScheduleDb: ${SCHEDULE_DB_CONNECTION_STRING}
    depends_on:
      schedule-db:
        condition: service_healthy
    networks:
      - uct_network

  # ==========================================
  # Databases (Database-per-Service)
  # ==========================================

  identity-db:
    image: postgres:16-alpine
    container_name: uct_identity_db
    environment:
      POSTGRES_DB: ${IDENTITY_POSTGRES_DB:-identity_db}
      POSTGRES_USER: ${IDENTITY_POSTGRES_USER:-postgres}
      POSTGRES_PASSWORD: ${IDENTITY_POSTGRES_PASSWORD:-postgres}
    ports:
      - "5431:5432"
    volumes:
      - identity_db_data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${IDENTITY_POSTGRES_USER:-postgres} -d ${IDENTITY_POSTGRES_DB:-identity_db}"]
      interval: 5s
      timeout: 5s
      retries: 5
    networks:
      - uct_network

  campus-db:
    image: postgres:16-alpine
    container_name: uct_campus_db
    environment:
      POSTGRES_DB: ${CAMPUS_POSTGRES_DB:-campus_db}
      POSTGRES_USER: ${CAMPUS_POSTGRES_USER:-postgres}
      POSTGRES_PASSWORD: ${CAMPUS_POSTGRES_PASSWORD:-postgres}
    ports:
      - "5432:5432"
    volumes:
      - campus_db_data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${CAMPUS_POSTGRES_USER:-postgres} -d ${CAMPUS_POSTGRES_DB:-campus_db}"]
      interval: 5s
      timeout: 5s
      retries: 5
    networks:
      - uct_network

  schedule-db:
    image: postgres:16-alpine
    container_name: uct_schedule_db
    environment:
      POSTGRES_DB: ${SCHEDULE_POSTGRES_DB:-schedule_db}
      POSTGRES_USER: ${SCHEDULE_POSTGRES_USER:-postgres}
      POSTGRES_PASSWORD: ${SCHEDULE_POSTGRES_PASSWORD:-postgres}
    ports:
      - "5433:5432"
    volumes:
      - schedule_db_data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${SCHEDULE_POSTGRES_USER:-postgres} -d ${SCHEDULE_POSTGRES_DB:-schedule_db}"]
      interval: 5s
      timeout: 5s
      retries: 5
    networks:
      - uct_network

  incident-mongo:
    image: mongo:7.0
    container_name: uct_incident_mongo
    environment:
      MONGO_INITDB_ROOT_USERNAME: ${INCIDENT_MONGO_ROOT_USER:-root}
      MONGO_INITDB_ROOT_PASSWORD: ${INCIDENT_MONGO_ROOT_PASSWORD:-examplepassword}
    ports:
      - "27017:27017"
    volumes:
      - incident_mongo_data:/data/db
    healthcheck:
      test: ["CMD", "mongosh", "--eval", "db.adminCommand('ping')"]
      interval: 5s
      timeout: 5s
      retries: 5
    networks:
      - uct_network

# ==========================================
# Networks & Volumes
# ==========================================
networks:
  uct_network:
    driver: bridge

volumes:
  identity_db_data:
  campus_db_data:
  schedule_db_data:
  incident_mongo_data:
```


