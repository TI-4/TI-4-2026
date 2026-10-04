FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Services/Identity/Identity.Domain/Identity.Domain.csproj src/Services/Identity/Domain/
COPY src/Services/Identity/Identity.Application/Identity.Application.csproj src/Services/Identity/Application/
COPY src/Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj src/Services/Identity/Infrastructure/
COPY src/Services/Identity/Identity.API/Identity.API.csproj src/Services/Identity/Api/
RUN dotnet restore src/Services/Identity/Identity.API/Identity.API.csproj

COPY src/Services/Identity/ src/Services/Identity/
RUN dotnet publish src/Services/Identity/Identity.API/Identity.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "Identity.API.dll"]
