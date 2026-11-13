# PhotoMarket

A photo marketplace backend built with **C#, ASP.NET Core 8, and Microsoft SQL Server**. The project provides a RESTful Web API for managing photos, photographers, tags, user accounts, and download history, alongside a separate ASP.NET Core MVC application.

The solution is structured into separate layers for API endpoints, business logic, data access, and shared models.

## Features

### Photo management
- Retrieve all photos or view individual photo details.
- Search photos by name, title, or description.
- Filter results by photographer and tag.
- Paginate search results.
- Create, update, and delete photo records.
- Associate photos with photographers and multiple tags.

### Photographer and tag management
- Full CRUD operations for photographers and tags.
- Manage many-to-many relationships between photos and tags.
- Prevent deletion of photographers who still have assigned photos.

### User accounts and authentication
- Register accounts and authenticate users with username and password.
- Generate JWT bearer tokens after successful login.
- Change passwords and update user profile information.
- Assign new accounts the default `User` role.
- Return user data through DTOs rather than exposing password hashes in registration responses.

### Download history and logging
- Record photo downloads and associate them with users.
- Retrieve download records by user or photo.
- Retrieve users with download history.
- Log photo creation, updates, and deletion events.
- Access log-reading endpoints using JWT authentication.

## Tech Stack

| Category | Technologies |
|---|---|
| Language | C# |
| Framework | .NET 8, ASP.NET Core |
| Backend | ASP.NET Core Web API, ASP.NET Core MVC |
| Data access | Entity Framework Core 8 |
| Database | Microsoft SQL Server |
| Authentication | JWT Bearer Authentication |
| API documentation | Swagger / OpenAPI |
| Frontend assets | Razor Views, Bootstrap, jQuery |

## Architecture

The solution follows a layered architecture that separates responsibilities across multiple projects:

```text
PhotoMarket-DWA/
│
├── PhotoMarket.sln
│
├── PhotoMarket.Model/
│   └── Shared models
│
├── PhotoMarket.DAL/
│   ├── Models/
│   └── PhotoMarketContext.cs
│
├── PhotoMarket.BL/
│   ├── Interfaces/
│   └── Services/
│
├── WebAPI/
│   ├── Controllers/
│   ├── Mapping/
│   ├── Models/
│   └── Program.cs
│
└── WebApp/
    ├── Controllers/
    ├── Views/
    └── wwwroot/
```

### Layer responsibilities

- **WebAPI** — Handles HTTP requests, exposes REST endpoints, validates incoming data, and returns API responses.
- **PhotoMarket.BL** — Contains service interfaces and business logic for authentication, photos, photographers, tags, users, downloads, and logging.
- **PhotoMarket.DAL** — Handles database access through Entity Framework Core and defines the database context and entities.
- **PhotoMarket.Model** — Holds shared models used across the solution.
- **WebApp** — A separate ASP.NET Core MVC project.

Business services are registered through dependency injection. The API uses Entity Framework Core with SQL Server to access relational data.

## Database Model

The application uses a relational database with the following main entities:

| Entity | Purpose |
|---|---|
| `AppUser` | Stores user account information |
| `Role` | Defines user roles |
| `Photo` | Stores photo metadata, price, and image path |
| `Photographer` | Stores photographer information |
| `Tag` | Defines searchable photo tags |
| `PhotoTag` | Connects photos and tags |
| `Download` | Records photo download history |
| `Log` | Stores application log entries |

A photo belongs to one photographer and can have multiple tags. The `PhotoTag` entity represents the many-to-many relationship between photos and tags. Download records connect users with the photos they have downloaded.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Microsoft SQL Server](https://www.microsoft.com/sql-server)
- Visual Studio 2022 or Visual Studio Code
- A SQL Server database compatible with the project's entity model

### 1. Clone the repository

```bash
git clone https://github.com/moshytoralgebra/PhotoMarket-DWA.git
cd PhotoMarket-DWA
```

### 2. Configure the database

The API expects a SQL Server connection string under `ConnectionStrings:DefaultConnection`.

Configure the following settings using .NET User Secrets or environment variables:

| Configuration | Environment variable |
|---|---|
| Database connection | `ConnectionStrings__DefaultConnection` |
| JWT signing key | `Jwt__Key` |
| JWT issuer | `Jwt__Issuer` |
| JWT audience | `Jwt__Audience` |
| JWT expiration | `Jwt__ExpirationMinutes` |

Example for PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost;Database=PhotoMarket;Trusted_Connection=True;TrustServerCertificate=True"
$env:Jwt__Key = "replace-with-a-long-random-secret"
$env:Jwt__Issuer = "PhotoMarket"
$env:Jwt__Audience = "PhotoMarketUsers"
$env:Jwt__ExpirationMinutes = "60"
```

Use a strong signing key and never commit real credentials or secrets to source control.

**Database setup:** The repository contains an Entity Framework Core database context and entity definitions, but does not currently provide a complete database creation script or migration setup. A compatible database must be created separately. The `Role` table must also contain a role named `User` before account registration can succeed.

### 3. Run the Web API

From the repository root:

```bash
dotnet run --project WebAPI
```

The current HTTP launch profile uses:

- API: `http://localhost:5214`
- Swagger UI: `http://localhost:5214/swagger`

Swagger is available when the API runs in the Development environment.

### 4. Run the MVC application

In a separate terminal:

```bash
dotnet run --project WebApp
```

The current HTTP launch profile uses:

- MVC application: `http://localhost:5113`

The MVC project currently contains the standard ASP.NET Core MVC scaffold and is not yet a complete marketplace frontend.

## API Overview

The REST API exposes the following controller routes:

| Controller | Base route | Functionality |
|---|---|---|
| Auth | `/api/auth` | Registration, login, password changes |
| Photo | `/api/photo` | Photo retrieval, search, filtering, CRUD |
| Photographer | `/api/photographer` | Photographer management |
| Tag | `/api/tag` | Tag management |
| User | `/api/user` | User retrieval and profile updates |
| Download | `/api/download` | Download history management |
| Logs | `/api/logs` | Application log retrieval |

### Example: Search photos

```http
GET /api/photo/search?searchTerm=sunset&photographerId=2&tagId=3&page=1&pageSize=10
```

### Example: Register a user

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

### Example: Authenticate

```http
POST /api/auth/login
Content-Type: application/json

{
  "userName": "sampleuser",
  "password": "your-password"
}
```

A successful login returns a JWT token. To access the protected logging endpoints, include it in the request header:

```http
Authorization: Bearer <your-token>
```

For the complete request and response models, explore the API through Swagger UI.

## Technical Concepts Demonstrated

- Layered application architecture and separation of concerns.
- Dependency injection and service interfaces.
- RESTful API design and HTTP methods.
- Entity Framework Core and relational database modelling.
- One-to-many and many-to-many relationships.
- DTOs and model mapping.
- JWT token generation and validation.
- CRUD operations and data filtering.
- Pagination and query handling.
- API documentation with Swagger.
- Exception handling and application logging.

## Security Considerations

This is an academic development project, not a production-ready application. Some implementation details require improvement before deployment:

- **JWT secret:** A development signing key is currently present in the tracked configuration file. Replace it with a securely generated key stored outside source control, and rotate it if it has been used elsewhere.
- **Password hashing:** The current implementation uses SHA-256 without a password-specific salt. A dedicated password-hashing solution, such as ASP.NET Core Identity's password hasher, should be used instead.
- **Authorization:** JWT authentication is configured, and the logging controller requires authorization. Most other API controllers do not currently enforce authentication or role-based authorization.
- **Error handling:** Production-grade validation and error handling should be added, and raw exception messages should not be returned to clients.

## Author

**Mariia Oshytor**

[GitHub Profile](https://github.com/moshytoralgebra) · [Repository](https://github.com/moshytoralgebra/PhotoMarket-DWA)
