FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the official .NET SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore dependencies
COPY ["src/TaskManagement.API/TaskManagement.API.csproj", "src/TaskManagement.API/"]
COPY ["src/TaskManagement.Domain/TaskManagement.Domain.csproj", "src/TaskManagement.Domain/"]
COPY ["src/TaskManagement.Kernel/TaskManagement.Kernel.csproj", "src/TaskManagement.Kernel/"]
COPY ["src/TaskManagement.Infra/TaskManagement.Infra.csproj", "src/TaskManagement.Infra/"]
RUN dotnet restore "src/TaskManagement.API/TaskManagement.API.csproj"

# Copy the entire source code and build the application
COPY . .
WORKDIR "/src/src/TaskManagement.API"
RUN dotnet build "TaskManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TaskManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: Use the runtime image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagement.API.dll"]