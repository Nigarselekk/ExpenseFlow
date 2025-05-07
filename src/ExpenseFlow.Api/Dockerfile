# --------------------------------------------------------------------------------
# Build stage
# --------------------------------------------------------------------------------
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src
    
    # Copy the solution & project files
    COPY ExpenseFlow.sln ./
    COPY src/ExpenseFlow.Domain/ExpenseFlow.Domain.csproj src/ExpenseFlow.Domain/
    COPY src/ExpenseFlow.Infrastructure/ExpenseFlow.Infrastructure.csproj src/ExpenseFlow.Infrastructure/
    COPY src/ExpenseFlow.Application/ExpenseFlow.Application.csproj src/ExpenseFlow.Application/
    COPY src/ExpenseFlow.Api/ExpenseFlow.Api.csproj src/ExpenseFlow.Api/
    
    # Restore dependencies
    RUN dotnet restore
    
    # Copy everything else and build
    COPY . .
    WORKDIR /src/ExpenseFlow.Api
    RUN dotnet build -c Release -o /app/build
    
    # --------------------------------------------------------------------------------
    # Publish stage
    # --------------------------------------------------------------------------------
    FROM build AS publish
    RUN dotnet publish -c Release -o /app/publish
    
    # --------------------------------------------------------------------------------
    # Runtime stage
    # --------------------------------------------------------------------------------
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
    WORKDIR /app
    
    # (optional) make Kestrel listen on port 80
    ENV ASPNETCORE_URLS=http://+:80
    
    COPY --from=publish /app/publish .
    
    # Expose the port
    EXPOSE 80
    
    ENTRYPOINT ["dotnet", "ExpenseFlow.Api.dll"]
    