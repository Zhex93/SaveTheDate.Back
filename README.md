# SaveTheDate.Back

Backend API for a Save The Date wedding invitation manager. Built with .NET 10, DDD + Hexagonal Architecture, MediatR (CQRS), Mapster, EF Core + PostgreSQL, JWT auth.

## Tech Stack

- .NET 10 / ASP.NET Core MVC
- PostgreSQL + Entity Framework Core
- MediatR 11.x (CQRS / Mediator pattern)
- Mapster (object mapping)
- FluentValidation (input validation)
- BCrypt.Net-Next (password hashing)
- JWT Bearer authentication
- Swashbuckle / OpenAPI

## Architecture

```
src/
├── SaveTheDate.Domain/          Entities, enums, repository ports, domain exceptions
├── SaveTheDate.Application/     Use cases (commands/queries/handlers), DTOs, validators
├── SaveTheDate.Infrastructure/  EF Core adapters, JWT generator, password hasher
└── SaveTheDate.Api/             Controllers, DI composition root, appsettings
```

Hexagonal: Domain defines ports (IRepository, IPasswordHasher). Application defines application ports (IJwtTokenGenerator). Infrastructure implements all adapters. Api wires everything.

## Prerequisites

- .NET 10 SDK
- PostgreSQL (15+ recommended)

## Getting Started

### 1. Clone and restore

```bash
cd SaveTheDate.Back
dotnet restore
```

### 2. Configure database

Edit `src/SaveTheDate.Api/appsettings.json` (or `appsettings.Development.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=savethedate;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Secret": "a-secret-key-at-least-32-characters-long!",
    "Issuer": "SaveTheDate",
    "Audience": "SaveTheDate"
  },
  "Seed": {
    "Admin": {
      "Email": "admin@savethedate.com",
      "Password": "Admin123!",
      "Role": "Admin"
    }
  }
}
```

### 3. Apply migrations

```bash
dotnet ef database update \
  --project src/SaveTheDate.Infrastructure \
  --startup-project src/SaveTheDate.Api
```

### 4. Run

```bash
dotnet run --project src/SaveTheDate.Api
```

API runs on `https://localhost:5001` by default. Swagger UI at `/swagger`.

### 5. First login

A default admin is seeded on first run (if configured in `Seed:Admin`):

```
POST /api/auth/login
{
  "email": "admin@savethedate.com",
  "password": "Admin123!"
}
```

Returns a JWT access token and a refresh token (in the body and as HttpOnly cookies). Use the access token as `Bearer {token}` in the Authorization header for all other endpoints, and `POST /api/auth/refresh` to renew it.

## API Endpoints

### Auth

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| POST | `/api/auth/login` | No | Login, returns access + refresh token |
| POST | `/api/auth/register` | Admin | Create new user |
| POST | `/api/auth/refresh` | No | Exchange a refresh token for a new access token |

### Roles

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/roles` | Yes | List all roles |
| GET | `/api/roles/{id}` | Yes | Get role by ID |
| POST | `/api/roles` | Yes | Create role |
| PUT | `/api/roles/{id}` | Yes | Update role |
| DELETE | `/api/roles/{id}` | Yes | Delete role |

### Guests

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/guests` | Yes | List all guests |
| GET | `/api/guests/{id}` | Yes | Get guest by ID |
| POST | `/api/guests` | Yes | Create guest |
| PUT | `/api/guests/{id}` | Yes | Update guest |
| DELETE | `/api/guests/{id}` | Yes | Delete guest |

### Invitations

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/invitations` | Yes | List all invitations |
| GET | `/api/invitations/{id}` | Yes | Get invitation by ID |
| POST | `/api/invitations` | Yes | Create invitation |
| PUT | `/api/invitations/{id}` | Yes | Update invitation |
| PATCH | `/api/invitations/{id}/state` | Yes | Change invitation state |
| DELETE | `/api/invitations/{id}` | Yes | Delete invitation |

### Invitation states

`Pending`, `Sent`, `Accepted`, `Declined`

## Security

- JWT symmetric HS256, with configurable lifetimes (`Jwt:AccessTokenExpirationMinutes`, default 60 minutes).
- Refresh tokens are random 512-bit values with configurable lifetime (`Jwt:RefreshTokenExpirationDays`, default 7 days), persisted in the `AspNetUserTokens` table and rotated on every refresh (the old token is invalidated). One active refresh token per user.
- `[Authorize]` on all write endpoints; `[AllowAnonymous]` on login and refresh.
- Passwords hashed with BCrypt.
- **Important**: Change `Jwt:Secret` to a strong key in production.

## Dependencies (all free/MIT)

| Package | Purpose |
|---------|---------|
| MediatR 11.1.0 | Mediator pattern (last MIT release) |
| Mapster | Object mapping |
| FluentValidation | Command/query validation |
| Npgsql.EntityFrameworkCore.PostgreSQL | Postgres EF Core provider |
| BCrypt.Net-Next | Password hashing |
| Microsoft.AspNetCore.Authentication.JwtBearer | JWT auth |
| Swashbuckle.AspNetCore | Swagger UI |
