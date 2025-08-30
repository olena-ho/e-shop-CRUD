# E-Shop REST API

A learning project implementing a simple **e-shop backend** in **.NET 9** with a layered architecture and **PostgreSQL** database.  
The API exposes CRUD endpoints for entities such as **Customers**, **Products**, **Categories**, and **Orders**, and is fully testable through **Swagger UI**.

---

## 🏗️ Architecture

This solution follows a classic **N-Tier Architecture**:

- **Eshop.Domain**  
  Entity classes & repository interfaces.

- **Eshop.Application**  
  DTOs and Services (business logic).

- **Eshop.Data**  
  EF Core `DbContext` + repository implementations (PostgreSQL).

- **Eshop.Api**  
  ASP.NET Core Web API layer (controllers, dependency injection, Swagger).

## 🗄️ Database

- **PostgreSQL** is used as the relational database.
- Database schema is created via **Entity Framework Core Code-First Migrations**.
