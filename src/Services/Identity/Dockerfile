FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Services/Identity/Domain/IdentityService.Domain.csproj src/Services/Identity/Domain/
COPY src/Services/Identity/Application/IdentityService.Application.csproj src/Services/Identity/Application/
COPY src/Services/Identity/Infrastructure/IdentityService.Infrastructure.csproj src/Services/Identity/Infrastructure/
COPY src/Services/Identity/Api/IdentityService.Api.csproj src/Services/Identity/Api/
RUN dotnet restore src/Services/Identity/Api/IdentityService.Api.csproj

COPY src/Services/Identity/ src/Services/Identity/
RUN dotnet publish src/Services/Identity/Api/IdentityService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.Api.dll"]