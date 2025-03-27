### Technical Stack  

  - Backend: .NET 8, C# 12
  - Database: SQL Server, Entity Framework Core (Code First)
  - Security: JWT Authentication, BCrypt Password Hashing (with salting)
  - Validation: FluentValidation (Fail Fast approach)
  - Testing: xUnit

### Project Architecture overview

Key Architectural Patterns Used:
 - Clean Architecture (Separation of concerns)
 - Repository Pattern (Data abstraction)
 - Unit of Work (Transaction management)
 - FluentValidation (Request validation)
 - Dependency Injection (Loosely coupled services)
 - Caching Middleware (Performance optimization for GET requests)

### Project Structure

- Curotec.WebApi - HTTP request handling
- Curotec.Application -  Business logic, DTOs, service interfaces
- Curotec.Domain - Place to acomodate our core entities
- Curotec.Infrastructure - Database (EF mappings, migrations, repositories)
- Curotec.Test - Where all the unit tests live

### Database Schema

Simple TODO tracking system with 3 tables:

  - Users - User accounts
  - TodoLists - TODO headers
  - TodoItems - TODO details

### Getting Started

Run the following:
```
cd .\API\Curotec.API\; dotnet run

```
The application will:

    Automatically apply migrations
    Create the database
    Seed initial user data


### Missing features / Room to improvements

Due to time constraints, these areas could be enhanced:

  - Implement the front end project 
  - Add integration tests
  - Implement refresh tokens
  - Enhance documentation (Swagger/OpenAPI)
  - Implement the TODO update/delete functionality
  - Implement `offset/limit` pagination for performance improvement on endpoints







