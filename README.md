# Fridge App (Web API + MVC Client)

Client-server application for managing fridges and products stored inside them.

## Tech stack

- .NET 8
- ASP.NET Core Web API
- EF Core + MS SQL Server
- MediatR + CQRS style handlers
- FluentValidation
- Swagger / OpenAPI
- Docker (SQL Server)

## Repository structure

### API (`fridge-api`)

```code
fridge-api/
  docker-compose.yml
  Fridge.sln
  src/
    Fridge.Api
    Fridge.Application
    Fridge.Domain
    Fridge.Infrastructure
  tests/
```

### Client (`fridge-client`)

Separate repository containing ASP.NET Core MVC application.

## 1) Start MS SQL Server (Docker)

From the **fridge-api** repository root:

```powershell
docker compose up -d
```

## Default connection (used by API in Development)

- Server: `localhost,1433`
- Database: `Fridge`
- User: `sa`
- Password: `Fr1dge!p@ssw0rd`

## 2) Run Web API

From the **fridge-api** repository root:

```powershell
dotnet run --project .\src\Fridge.Api
```

Swagger (Development):

- `http://localhost:5074/swagger`

## 3) Run MVC client

From the **fridge-client** repository root:

```powershell
dotnet run
```

## 4) SQL queries (Additional task #2)

SQL scripts are located in:

```code
src/sql/
```

### Run all scripts

From the **fridge-api** repository root:

```powershell
sqlcmd -S localhost,1433 -U sa -P "Fr1dge!p@ssw0rd" -C -d Fridge -i ".\src\sql\run_all.sql"
```

### Run a single script

```powershell
sqlcmd -S localhost,1433 -U sa -P "Fr1dge!p@ssw0rd" -C -d Fridge -i ".\src\sql\01_task1_products_in_A_models.sql"
```

## 5) Main features

### API

- Fridges CRUD
- Products list + update
- Fridge models list
- Add/remove product in a fridge
- Stored procedure based restock:
  - finds fridge_products with `quantity = 0`
  - increases quantity by product `default_quantity`

### Client (MVC)

- Home page with links + ability to call API method #6 (restock)
- Fridges list in a table
  - navigation to Create/Edit
  - delete with modal confirmation
- Create fridge with initial list of products
- Edit fridge
- View and manage products in a fridge
- Products list + product edit page
