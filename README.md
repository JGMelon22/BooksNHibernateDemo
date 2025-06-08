# NHibernate Students API   
A side project made to study about NHibernate ORM with DDD and Simple SimpleMediator.

### 🧰 Tech Stack

<div style="display: flex; gap: 10px;">
    <img height="32" width="32" src="https://cdn.simpleicons.org/dotnet" alt=".NET" title=".NET" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/swagger" alt="Swagger" title="Swagger" />
    <img height="32" src="https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white" alt="Microsoft SQL Server" title="Microsoft SQL Server" />
</div>

### 📸 Project Preview

<div style="display: flex; gap: 20px; flex-wrap: wrap;">
  <div>
    <strong>Swagger UI</strong><br/>
    <img src="https://github.com/user-attachments/assets/4161b747-9097-4c7e-b7d8-a54ca3bfa43c" alt="Swagger UI" width="650"/>
  </div>
  <div>
    <strong>VS Code Project Structure</strong><br/>
    <img src="https://github.com/user-attachments/assets/3fd85e0b-4202-4356-baf4-9703165a5879" alt="VS Code Screenshot" width="650"/>
  </div>
</div>

### 🧩 Dependencies
#### **API Layer** (`NHibernateDemo.API`)
- [`Microsoft.AspNetCore.OpenApi`](https://www.nuget.org/packages/Microsoft.AspNetCore.OpenApi) — OpenAPI support for ASP.NET Core.  
- [`MiniValidation`](https://www.nuget.org/packages/MiniValidation) — Minimal, fast validation library for .NET.  
- [`NHibernate`](https://www.nuget.org/packages/NHibernate) — ORM for database access.  
- [`Swashbuckle.AspNetCore`](https://www.nuget.org/packages/Swashbuckle.AspNetCore) — Swagger/OpenAPI generation for ASP.NET Core.  

---

#### **Application Layer** (`NHibernateDemo.Application`)
- [`NetDevPack.SimpleMediator`](https://www.nuget.org/packages/NetDevPack.SimpleMediator) — Simple implementation of the Mediator pattern for .NET.  

---

#### **Infrastructure Layer** (`NHibernateDemo.Infrastructure`)
- [`FluentNHibernate`](https://www.nuget.org/packages/FluentNHibernate) — Fluent API for NHibernate ORM mappings.  
- [`Microsoft.Data.SqlClient`](https://www.nuget.org/packages/Microsoft.Data.SqlClient) — SQL Server data provider.  
- [`Microsoft.Extensions.Logging.Abstractions`](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions) — Logging abstractions for dependency injection and flexibility.  
- [`NHibernate`](https://www.nuget.org/packages/NHibernate) — ORM for interacting with relational databases.

#### **Unit Tests** (`NHibernateDemo.Application.UnitTests` and `NHibernateDemo.API.UnitTests`)
- `coverlet.collector`
- `Microsoft.NET.Test.Sdk`
- [`Moq`](https://www.nuget.org/packages/Moq) — The most popular and friendly mocking library for .NET.
- [`Shouldly`](https://www.nuget.org/packages/shouldly/) — An assertion framework which focuses on giving great error messages when the assertion fails while being simple and terse.
- `xunit`
- `xunit.runner.visualstudio`

### 📚 References
[NHibernate - The object-relational mapper for .NET](https://nhibernate.info/) \
[NHibernate - Relational Persistence for Idiomatic .NET](https://nhibernate.info/doc/nhibernate-reference/index.html) \
[ASP .NET MVC -  CRUD com Fluent NHibernate - I](https://www.macoratti.net/16/01/mvc_crudnhb1.htm) \
[ASP .NET Core -  Usando o NHibernate e FluentNHibernate](https://www.macoratti.net/19/07/aspnc_nhib1.htm)