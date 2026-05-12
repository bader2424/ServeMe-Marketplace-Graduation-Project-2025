# SERVE_ME_PROJECT

ASP.NET Core MVC service marketplace using Microsoft SQL Server.

## Requirements

- Docker Desktop
- Docker Compose, included with Docker Desktop
- At least 4 GB memory available for Docker because SQL Server is included

No local .NET SDK or local SQL Server installation is required when running with Docker.

## Quick Start

From this folder:

```powershell
cd "C:\Users\DELL\Downloads\SERVE_ME_PROJECT22-master\SERVE_ME_PROJECT22-master"
docker compose up --build
```

Open:

```text
http://localhost:8080
```

The first run can take several minutes because Docker downloads base images, restores NuGet packages, starts SQL Server, applies Entity Framework migrations, and seeds demo data. Later runs are much faster:

```powershell
docker compose up
```

Wait until the terminal shows that the web app is listening on port `8080`.

## What Docker Runs

- `web`: ASP.NET Core MVC app
- `db`: Microsoft SQL Server 2022 Developer Edition
- `serveme-sql-data`: Docker volume that keeps the database between runs

The web container applies database migrations and seeds demo users, roles, site content, and catalog data automatically.

## Demo Accounts

```text
Admin:    admin@serveme.local / Admin@12345
Provider: provider@serveme.local / Provider@12345
Customer: customer@serveme.local / Customer@12345
```

## Useful Pages

```text
http://localhost:8080/Graduation
http://localhost:8080/Marrige
http://localhost:8080/Tourism
```

## Rebuild After Code Changes

If code or project packages change:

```powershell
docker compose up --build
```

If only the database data changed and the containers already exist:

```powershell
docker compose restart web
```

## Stop Containers

```powershell
docker compose down
```

To also delete the SQL Server database volume:

```powershell
docker compose down -v
```

After `docker compose down -v`, the next `docker compose up --build` creates a fresh SQL Server database and reseeds the demo data.

## Environment

The default Docker SQL Server password is safe for local demo use only. To override it, copy `.env.example` to `.env` next to `docker-compose.yml` and change the value:

```text
MSSQL_SA_PASSWORD=Your_Strong_Password123
```

The app reads the Docker SQL connection from:

```text
ConnectionStrings__DefaultConnetion
```

## Troubleshooting

If NuGet restore times out during the first build, run the build again:

```powershell
docker compose build web
docker compose up
```

If port `8080` is already in use, change the left side of this line in `docker-compose.yml`:

```yaml
ports:
  - "8081:8080"
```

Then open `http://localhost:8081`.

If SQL Server does not start, make sure Docker Desktop is running and has enough memory.
