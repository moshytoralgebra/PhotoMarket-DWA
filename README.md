# PhotoMarket

A photo marketplace project built with **ASP.NET Core 8**, **C#**, and **Microsoft SQL Server**. The solution separates the REST API, business logic, and data-access layer, with a companion ASP.NET Core MVC application.

The project models a marketplace where photographers publish photos with descriptions, prices, and tags, while users can browse the catalogue and keep track of photo downloads.

> **Project status:** Academic project / development prototype. Review the security notes before deploying or using it with real user data.

## Features

### Photo catalogue
- Retrieve photos and individual photo details.
- Search photos by name, title, or description.
- Filter search results by photographer and tag.
- Paginate search results.
- Create, update, and delete photo records.
- Associate photos with photographers and multiple tags.

### Photographers and tags
- Create, read, update, and delete photographer records.
- Create, read, update, and delete tags.
- Prevent deletion of photographers that still have photos assigned.
- Manage the many-to-many relationship between photos and tags.

### Accounts and authentication
- Register user accounts and sign in with a username and password.
- Issue JWT bearer tokens after successful login.
- Change account passwords and update profile information.
- Assign new accounts the default `User` role configured in the database.
- Return user information through DTOs rather than exposing password hashes in registration responses.

### Download history and logging
- Record which user downloaded which photo and when.
- Retrieve download records by user or photo.
- View users who have download history.
- Record photo creation and update/delete events.
- Protect the log-reading endpoints with JWT authentication.

## Tech stack

| Area | Technologies |
| --- | --- |
| Language and runtime | C#, .NET 8 |
| Backend | ASP.NET Core Web API, ASP.NET Core MVC |
| Data access | Entity Framework Core 8, SQL Server |
| Authentication | JWT Bearer |
| API documentation | Swagger / OpenAPI (Swashbuckle) |
| Frontend assets | Razor Views, Bootstrap, jQuery |

## Solution structure

The repository contains the following projects:

```text
PhotoMarket-DWA/
├── PhotoMarket.sln
├── PhotoMarket.BL/           # Business logic and service interfaces
│   ├── Interfaces/
│   └── Services/
├── PhotoMarket.DAL/          # EF Core context and database entities
│   └── Models/
├── PhotoMarket.Model/        # Shared model project
├── WebAPI/                   # REST API, controllers, DTOs and mappings
│   ├── Controllers/
│   ├── Mapping/
│   ├── Models/
│   └── Program.cs
└── WebApp/                   # ASP.NET Core MVC application
    ├── Controllers/
    ├── Views/
    └── wwwroot/
```

### Architecture

The API uses a layered structure:

1. **WebAPI** receives HTTP requests, validates request data, maps entities to DTOs, and returns HTTP responses.
2. **PhotoMarket.BL** contains service interfaces and business operations for authentication, photos, photographers, tags, users, downloads, and logs.
3. **PhotoMarket.DAL** contains the Entity Framework Core `DbContext` and database entity classes.
4. **SQL Server** stores the application data.

The API registers business services through dependency injection. Entity Framework Core is configured to use SQL Server, and the database context is injected into the relevant services.

The `WebApp` project is a separate MVC application. Its current home page is the default ASP.NET Core MVC welcome page; it is not yet a complete marketplace frontend.

## Data model

The database context includes these main entities:

- **AppUser** — account details and role assignment.
- **Role** — user role definitions.
- **Photo** — photo metadata, price, image path, upload date, and photographer.
- **Photographer** — photographer profile information.
- **Tag** — searchable photo categories.
- **PhotoTag** — join entity connecting photos and tags.
- **Download** — records a user's download of a photo.
- **Log** — application log entries.

A photo belongs to one photographer and can have multiple tags. Users can have multiple download records, and each download references a photo.

## Getting started

### Prerequisites

- [ .NET 8 SDK ](https://dotnet.microsoft.com/download/dotnet/8.0)
- [ Microsoft SQL Server ](https://www.microsoft.com/sql-server) (local instance or another accessible SQL Server instance)
- An IDE such as Visual Studio 2022 or Visual Studio Code
- A database matching the entity model used by the project

**Database note:** The repository contains an Entity Framework Core context and entity classes, but does not currently include a database creation script or migration setup. You must provide a compatible `PhotoMarket` database and seed the roles required by the application (at minimum, a role named `User`) before registering accounts.

### 1. Clone the repository

```bash
git clone https://github.com/moshytoralgebra/PhotoMarket-DWA.git
cd PhotoMarket-DWA
```

### 2. Configure the API

The API reads its SQL Server connection string and JWT settings from configuration.

For local development, update `WebAPI/appsettings.json` or use .NET user secrets / environment variables. Do not commit real credentials or signing keys.

Example environment-variable names:

| Setting | Environment variable |
| --- | --- |
| SQL Server connection string | `ConnectionStrings__DefaultConnection` |
| JWT signing key | `Jwt__Key` |
| JWT issuer | `Jwt__Issuer` |
| JWT audience | `Jwt__Audience` |
| JWT lifetime in minutes | `Jwt__ExpirationMinutes` |

For example, in PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost;Database=PhotoMarket;Trusted_Connection=True;TrustServerCertificate=True"
$env:Jwt__Key = "replace-with-a-long-random-secret"
$env:Jwt__Issuer = "PhotoMarket"
$env:Jwt__Audience = "PhotoMarketUsers"
$env:Jwt__ExpirationMinutes = "60"
```

Use a strong, randomly generated signing key and keep it outside source control.

### 3. Run the REST API

From the repository root:

```bash
dotnet run --project WebAPI
```

With the repository's current HTTP launch profile, the API runs at:

- API base URL: `http://localhost:5214`
- Swagger UI: `http://localhost:5214/swagger` (Development environment)

Swagger is enabled only when the API runs in the Development environment.

### 4. Run the MVC application

In a second terminal:

```bash
dotnet run --project WebApp
```

With the current HTTP launch profile, the MVC application runs at `http://localhost:5113`.

## API overview

The API uses controller-based routes under `/api`.

| Controller | Route | Purpose |
| --- | --- | --- |
| Auth | `/api/auth` | Register, log in, and change password |
| Photo | `/api/photo` | Browse, search, create, update, and delete photos |
| Photographer | `/api/photographer` | Manage photographer records |
| Tag | `/api/tag` | Manage tags |
| User | `/api/user` | Retrieve users, view download history, and update profiles |
| Download | `/api/download` | Record and retrieve download history |
| Logs | `/api/logs` | Retrieve recent logs and log count |

### Example requests

Search photos by a term, photographer, and tag, returning the first 10 results:

```http
GET /api/photo/search?searchTerm=sunset&photographerId=2&tagId=3&page=1&pageSize=10
```

Register an account:

```http
POST /api/auth/register
Content-Type: application/json

{
  "userName": "sampleuser",
  "email": "sample@example.com",
  "firstName": "Sample",
  "lastName": "User",
  "password": "your-password"
}
```

Log in:

```http
POST /api/auth/login
Content-Type: application/json

{
  "userName": "sampleuser",
  "password": "your-password"
}
```

A successful login returns a JWT token. The log endpoints accept that token in the HTTP `Authorization` header:

```http
Authorization: Bearer <your-token>
```

Use Swagger UI to explore the available routes and request/response models.

## Security and limitations

This repository is an academic development project, not a production-ready service. Before deploying it or using real accounts:

- **Replace the development JWT signing key.** A development key is currently present in the tracked API configuration. Remove it from source control, provide the replacement through user secrets or environment variables, and rotate it if it has ever been used outside local development.
- **Upgrade password storage.** The current implementation uses plain SHA-256 hashing without a password-specific salt. Use a password-hashing solution designed for credentials, such as ASP.NET Core Identity or a properly configured password-hashing library.
- **Review authorization on every endpoint.** JWT validation is configured and the log controller has an `[Authorize]` attribute, but most other API controllers do not currently enforce authentication or role-based access. Do not assume that receiving a token protects all API operations.
- **Review validation and error handling.** Add production-grade input validation and avoid returning raw exception messages to clients.
- **Configure secrets and database access safely.** Do not commit real connection strings, credentials, or signing keys.

## Possible next steps

- Add a database setup script or EF Core migrations and seed data.
- Apply authentication and role-based authorization consistently across protected endpoints.
- Replace the current password-hashing implementation.
- Complete the MVC frontend and connect it to the API.
- Add automated tests for services, controllers, and authentication flows.

## Author

**Mariia Oshytor**  
[GitHub](https://github.com/moshytoralgebra)
