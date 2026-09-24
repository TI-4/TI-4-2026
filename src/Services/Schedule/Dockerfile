FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Services/Schedule/Schedule.API/Schedule.API.csproj", "Schedule.API/"]
COPY ["src/Services/Schedule/Schedule.Application/Schedule.Application.csproj", "Schedule.Application/"]
COPY ["src/Services/Schedule/Schedule.Domain/Schedule.Domain.csproj", "Schedule.Domain/"]
COPY ["src/Services/Schedule/Schedule.Infrastructure/Schedule.Infrastructure.csproj", "Schedule.Infrastructure/"]

RUN dotnet restore "Schedule.API/Schedule.API.csproj"

COPY ["src/Services/Schedule/", "./"]
WORKDIR "/src/Schedule.API"
RUN dotnet build "Schedule.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Schedule.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Schedule.API.dll"]
