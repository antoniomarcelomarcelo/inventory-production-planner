# Inventory Production Planner

Web application for managing products and raw materials and generating
optimized production plans based on available stock.

Developed as a technical assessment project.

------------------------------------------------------------------------

## Tech Stack

### Backend

-   .NET 8
-   ASP.NET Core Web API
-   Entity Framework Core
-   PostgreSQL
-   xUnit
-   GitHub Actions CI

### Frontend

-   Angular 14
-   Reactive Forms
-   Bootstrap 5

------------------------------------------------------------------------

## Architecture

The system follows a clean separation between:

- Backend API (RESTful architecture)
- Frontend SPA (Single Page Application)
- Database persistence layer
- Service layer containing business rules

### Key Design Decisions

- API and frontend fully separated (RNF002)
- Production calculation logic isolated in `ProductionPlannerService`
- Prioritization of higher value products during planning
- Identification of production bottleneck (limiting raw material)

------------------------------------------------------------------------

## Features

### Products
- Create, update, delete and list products
- Associate multiple raw materials (Bill of Materials)
- Update BOM independently

### Raw Materials
- CRUD operations
- Stock quantity control

### Production Plan
- Calculates producible quantity per product
- Identifies bottleneck raw material
- Calculates total value per product
- Returns grand total production value
- Prioritizes higher value products

------------------------------------------------------------------------

## How to Run

### Backend

``` bash
cd backend
dotnet restore
dotnet ef database update
dotnet run --project Inventory.Api
```

Swagger:

https://localhost:7166/swagger

------------------------------------------------------------------------

### Frontend

``` bash
cd frontend/inventory-web
npm install
ng serve --proxy-config proxy.conf.json
```

App:

http://localhost:4200

------------------------------------------------------------------------

## Tests

Run backend tests:

``` bash
dotnet test
```

------------------------------------------------------------------------

## CI

GitHub Actions automatically builds and runs tests on push.

------------------------------------------------------------------------

## Future Improvements

- JWT-based authentication and role-based authorization
- Docker containerization for backend and frontend
- Production cost calculation (raw material cost vs. product margin)
- Export production plan to PDF or Excel
- Pagination and filtering for large datasets
- Frontend unit tests (Angular TestBed / Jasmine)
- Global HTTP interceptor for error handling
- Dashboard with stock and production KPIs

------------------------------------------------------------------------

## Author

Antonio Marcelo\
Full Stack Developer (.NET / Angular)
