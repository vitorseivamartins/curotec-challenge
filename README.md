### Technical Stack  

  - Backend: .NET 8, C# 12
  - Frontend: Angular 17
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

Due to time constraints, these areas are left to be enhanced:

  - Add more unit testings
  - Add integration tests
  - Implement refresh tokens
  - Enhance documentation (Swagger/OpenAPI)
  - Implement the TODO update/delete functionality
  - Implement `offset/limit` pagination for performance improvement on endpoints


### Screenshots

![image](https://github.com/user-attachments/assets/5016db64-bdef-430d-98f3-bca20b5fc056)


![image](https://github.com/user-attachments/assets/5e4f60c5-08bb-41ac-850e-60425db0be02)


![image](https://github.com/user-attachments/assets/a5b01e90-0d00-44ea-bc80-88d36d959069)


https://github.com/user-attachments/assets/9f640e4b-6127-4737-a0b2-9ff981ad039b



