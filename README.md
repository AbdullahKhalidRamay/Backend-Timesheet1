# TimeSheetAPI

A comprehensive time tracking and project management API built with ASP.NET Core.

## Overview

TimeSheetAPI is a backend service that provides functionality for tracking time entries, managing projects, departments, teams, and users. It supports role-based permissions and includes features for approval workflows, reporting, and analytics.

## Features

- **User Authentication**: JWT-based authentication with role-based authorization
- **Time Tracking**: Create, update, and manage time entries with approval workflows
- **Project Management**: Create and manage projects with hierarchical task structures
- **Team Management**: Organize users into teams and departments
- **Reporting**: Generate reports on time entries, projects, and user activities

## Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Documentation**: Swagger/OpenAPI

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or another compatible IDE

### Setup

1. Clone the repository
2. Navigate to the project directory
3. Run the following commands:

```bash
dotnet restore
dotnet build
dotnet ef database update
dotnet run
```

4. The API will be available at `http://localhost:5126`
5. Swagger documentation is available at `http://localhost:5126/swagger`

## Database Configuration

The application uses Entity Framework Core with SQL Server. The connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TimeFlowDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

## Project Structure

- **Controllers/**: API endpoints for different resources
- **Models/**: Entity models and DTOs
- **Data/**: Database context and configurations
- **Services/**: Business logic implementation
- **Migrations/**: Database migration files

## API Documentation

API documentation is available through Swagger UI at `/swagger` when the application is running.

## License

This project is licensed under the MIT License - see the LICENSE file for details.