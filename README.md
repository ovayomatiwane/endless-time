# endless-time

# Structure and layout

Solution
│
├─ Domain <-- Entities + DbContext + Factory
│ └─ Entities
│  
│
├─ Common <-- Shared DTOs, Enums, Utilities
│ ├─ DTOs
│ ├─ Enums
│ └─ Utilities
│
├─ Services <-- Business logic layer
│ ├─ Interfaces
│ │ └─ IUsersService.cs
│ └─ UsersService.cs
│
├─ WebApi <-- Web API
│ ├─ Controllers
│ ├─ Mapping (AutoMapper profiles referencing Common DTOs)
│ └─ Program.cs

Project Referencing
WebApi (startup project)
├─ references → Services
├─ references → Common

Services (business logic)
├─ references → Domain
├─ references → Common
├─ references → AutoMapper

Domain (entities + DbContext)
├─ references → nothing (except EF Core & Configuration NuGet packages)

Common (DTOs, Enums, Utilities)
├─ references → nothing

Migrations: Open Package Manager Console
-add-migration InitialCreate -Project Domain -StartupProject WebApi
-update-database -Project Domain -StartupProject WebApi

Flow
+------------------+
| Client | <-- Frontend / Postman / Mobile app
+------------------+
|
v
+------------------+
| API | <-- WebApi Project
|------------------|
| Controllers | <-- AuthController, UsersController
| Middleware | <-- ExceptionHandlingMiddleware
| Swagger / OpenAPI|
+------------------+
|
v
+------------------+
| Services | <-- Services Project
|------------------|
| AuthService | <-- Login, registration, JWT creation
| UsersService | <-- CRUD, business rules
| PasswordService | <-- Wraps IPasswordHasher
+------------------+
|
v
+------------------+
| Common | <-- Common Project
|------------------|
| DTOs | <-- LoginRequestDto, AuthResponseDto
| Interfaces | <-- IPasswordHasher
| ApiResponse<T> | <-- Standard API responses
| Exceptions | <-- UnauthorizedException, NotFoundException
+------------------+
^
|
+------------------+
| Domain | <-- Domain Project
|------------------|
| Entities | <-- User, Post, etc.
| ApplicationDataContext | <-- EF DbContext
+------------------+
