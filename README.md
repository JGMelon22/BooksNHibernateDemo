# NHibernate Students API   
A side project made to study about NHibernate ORM with DDD and Simple SimpleMediator.

### 🧰 Tech Stack

<div style="display: flex; gap: 10px;">
    <img height="32" width="32" src="https://cdn.simpleicons.org/dotnet" alt=".NET" title=".NET" />
    <img height="32" width="32" src="https://cdn.simpleicons.org/swagger" alt="Swagger" title="Swagger" />
    <img height="32" src="https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white" alt="Microsoft SQL Server" title="Microsoft SQL Server" />
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