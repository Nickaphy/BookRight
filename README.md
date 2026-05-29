# BookRight

A clinic booking system built with C# / .NET 10, Blazor, Entity Framework Core, and SQL Server.

The project follows Clean Architecture with DDD, CQS, and a Facade layer separating the UI from the application logic.

## Prerequisites

- .NET 10 SDK
- SQL Server or Docker with MSSQL
- servername: (localdb)\MSSQLLocalDB

## Setup
Set startup project the bookright ui
1. Update the connection string in `BookRight.Infrastructure/Persistence/AppDbContext.cs` to point at your database.

2. Run migrations:
   ```bash
   dotnet ef migrations add InitialCreate --project BookRight.Infrastructure --startup-project BookRight.UI
   dotnet ef database update --project BookRight.Infrastructure --startup-project BookRight.UI
   ```

3. Run the project:
   ```bash
   dotnet run --project BookRight.UI
   ```

## Project Structure

| Layer | Description |
|---|---|
| `BookRight.Domain` | Entities, value objects, and business rules |
| `BookRight.Application` | Use cases, commands, and repository interfaces |
| `BookRight.Infrastructure` | EF Core, database context, and migrations |
| `BookRight.Facade` | DTOs and entry point between UI and application |
| `BookRight.UI` | Blazor Server frontend |

## Git Workflow

Feature branches are used for all new work and merged into `main` via pull requests. No direct pushes to `main`.
