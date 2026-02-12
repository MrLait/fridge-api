# Init project and different commands

## build the project

mkdir src tests build/scripts

dotnet new sln -n Fridge

dotnet new webapi  -n Fridge.Api          -f net8.0 -o src/Fridge.Api
dotnet new classlib -n Fridge.Application -f net8.0 -o src/Fridge.Application
dotnet new classlib -n Fridge.Domain      -f net8.0 -o src/Fridge.Domain
dotnet new classlib -n Fridge.Infrastructure -f net8.0 -o src/Fridge.Infrastructure

dotnet sln add src/Fridge.Api/Fridge.Api.csproj
dotnet sln add src/Fridge.Application/Fridge.Application.csproj
dotnet sln add src/Fridge.Domain/Fridge.Domain.csproj
dotnet sln add src/Fridge.Infrastructure/Fridge.Infrastructure.csproj

## add references

dotnet add src/Fridge.Api reference src/Fridge.Application
dotnet add src/Fridge.Api reference src/Fridge.Infrastructure

dotnet add src/Fridge.Application reference src/Fridge.Domain

dotnet add src/Fridge.Infrastructure reference src/Fridge.Application
dotnet add src/Fridge.Infrastructure reference src/Fridge.Domain

## add tests

dotnet new xunit -n Fridge.Application.Tests -f net8.0 -o tests/Fridge.Application.Tests
dotnet sln add tests/Fridge.Application.Tests/Fridge.Application.Tests.csproj
dotnet add tests/Fridge.Application.Tests reference src/Fridge.Application

## add application packages

dotnet add src/Fridge.Application package MediatR --version 14.0.0
dotnet add .\src\Fridge.Application\Fridge.Application.csproj package FluentValidation --version 12.1.1
dotnet add .\src\Fridge.Application\Fridge.Application.csproj package FluentValidation.DependencyInjectionExtensions --version 12.1.1

dotnet add src/Fridge.Application/Fridge.Application.csproj package Microsoft.EntityFrameworkCore --version 8.0.11
dotnet add .\src\Fridge.Application\Fridge.Application.csproj  package Microsoft.EntityFrameworkCore.Relational --version 8

## add infrastructure packages

dotnet add .\src\Fridge.Infrastructure\Fridge.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add .\src\Fridge.Infrastructure\Fridge.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add .\src\Fridge.Infrastructure\Fridge.Infrastructure.csproj package Microsoft.EntityFrameworkCore --version 8.0.11

## add api packages

dotnet add src/Fridge.Api package FluentValidation.AspNetCore --version 11.3.1
dotnet add src/Fridge.Api package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add src/Fridge.Api package Microsoft.AspNetCore.OpenApi --version 8.0.22
dotnet add src/Fridge.Api package Swashbuckle.AspNetCore --version 6.6.2

## Migration commands

dotnet ef migrations add InitialCreate -p src/Fridge.Infrastructure -s src/Fridge.Api
dotnet ef database update -p src/Fridge.Infrastructure -s src/Fridge.Api

```powershell
dotnet ef migrations add AddUniqueIndexOnFridgeProducts `
  -p src/Fridge.Infrastructure `
  -s src/Fridge.Api
```

```powershell
dotnet ef database update `
  -p src/Fridge.Infrastructure `
  -s src/Fridge.Api
```

### Add Stored Procedure

(
    dotnet ef migrations add AddRestockStoredProcedure -p src/Fridge.Infrastructure -s src/Fridge.Api
    then update method Up() and Down()
    dotnet ef database update -p src/Fridge.Infrastructure -s src/Fridge.Api
)

### Check Stored Procedure in a database

docker exec -it fridge-sql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Fr1dge!p@ssw0rd" -C -d Fridge -Q "EXEC dbo.sp_RestockZeroQuantity;"

docker exec -it fridge-sql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Fr1dge!p@ssw0rd" -C -d Fridge -Q "SELECT fridge_id, product_id, COUNT(*) AS cnt FROM fridge_products GROUP BY fridge_id, product_id HAVING COUNT(*) > 1;"

## Different SQL Commands

"SELECT i.name, i.is_unique FROM sys.indexes i WHERE i.object_id = OBJECT_ID('dbo.fridge_products');"

"SELECT MigrationId, ProductVersion FROM dbo.__EFMigrationsHistory ORDER BY MigrationId;"
