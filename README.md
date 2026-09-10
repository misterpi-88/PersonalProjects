# PersonalProjects

Personal projects for learning and practice.

## Projects

| Project | Description | Stack |
|---|---|---|
| [`ToDo/`](ToDo/) | To-do task Web API built with a layered architecture (Api / Application / Infrastructure). | .NET, Minimal APIs, EF Core, Serilog |

## ToDo

A small task-management API used to practice clean layering:

- **Api** – Minimal API endpoints, request pipeline, configuration
- **Application** – DTOs, service interfaces, endpoint filters, business logic
- **Infrastructure** – EF Core `DbContext`, entity configuration, repositories, migrations

### Run locally

```bash
cd ToDo
dotnet run --project Api
```

The API expects a local SQL Server instance. Configure the connection string in
`ToDo/Api/appsettings.Development.json` (key `ConnectionStrings:ToDoDbContext`).

Apply migrations:

```bash
dotnet ef database update --project Infrastructure --startup-project Api
```
