# ServeMe

ServeMe is a social-services marketplace built with ASP.NET Core MVC. The platform helps customers browse and book services for weddings, graduations, tourism trips, halls, catering, photography, gifts, tailoring, and event organization.

The project includes public browsing pages, role-based authentication, provider tools, admin management screens, wallet workflows, SQL Server persistence, seeded demo data, and a Docker Compose setup for running the full app locally.

## Main Features

- Public marketplace for browsing services by category
- Wedding, graduation, and tourism service catalogs
- Service details with images, prices, ratings, and comments
- Customer, provider, and admin demo accounts
- Role-based authentication and protected dashboards
- Provider dashboard for managing services, orders, wallet activity, and requests
- Admin dashboard for managing users, providers, categories, services, banners, blogs, deposits, and withdrawals
- Customer wallet, deposits, withdrawals, order tracking, and service booking
- SQL Server database with Entity Framework Core migrations
- Automatic database migration and demo-data seeding on startup
- Dockerized local environment with the web app and SQL Server

## Tech Stack

- ASP.NET Core MVC (.NET 8)
- C#
- Entity Framework Core
- ASP.NET Core Identity
- Microsoft SQL Server
- Razor Views
- Bootstrap
- Docker
- Docker Compose

## Run Locally With Docker

Requirements:

- Docker Desktop
- 4 GB or more memory available for Docker is recommended because SQL Server runs in a container

Clone the repository, then run:

```powershell
docker compose up --build
```

Open the app:

```text
http://localhost:8080
```

The first run can take a few minutes because Docker downloads images, restores NuGet packages, starts SQL Server, applies migrations, and seeds demo data.

For later runs, use:

```powershell
docker compose up
```

## Demo Accounts

```text
Admin:    admin@serveme.local / Admin@12345
Provider: provider@serveme.local / Provider@12345
Customer: customer@serveme.local / Customer@12345
```

## Important Pages

```text
Home:       http://localhost:8080
Wedding:    http://localhost:8080/Marrige
Graduation: http://localhost:8080/Graduation
Tourism:    http://localhost:8080/Tourism
Login:      http://localhost:8080/Identity/Account/Login
```

## Database

The Docker setup starts Microsoft SQL Server automatically.

On application startup, the project:

- applies Entity Framework Core migrations
- creates required roles
- creates demo users
- seeds public site content
- seeds service categories, services, images, comments, and wallet data

## Project Report

The graduation project report is available here:

[docs/social-services-site-serveme-2025.pdf](docs/social-services-site-serveme-2025.pdf)

## Useful Docker Commands

Stop containers:

```powershell
docker compose down
```

Stop containers and delete the local database volume:

```powershell
docker compose down -v
```

Rebuild after project file or Docker changes:

```powershell
docker compose up --build
```

## Environment Variables

`.env.example` is included as a safe template for local Docker configuration.

Do not commit a real `.env` file.

```text
MSSQL_SA_PASSWORD=ServeMe_Docker12345
```

## Troubleshooting

If NuGet restore times out during the first Docker build, run the build again:

```powershell
docker compose build web
docker compose up
```

If port `8080` is already in use, change the left side of the port mapping in `docker-compose.yml`:

```yaml
ports:
  - "8081:8080"
```

Then open:

```text
http://localhost:8081
```

If SQL Server takes time to start, wait until the web container logs show that the app is listening on port `8080`.
