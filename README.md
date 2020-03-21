# Blog
## NuGet packages
* Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
* Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
## Assumptions 
**The application**
* Is created in the .net core 3.1, using model-view-controller,  
* Using Jquery,
* Will be have simple CRUD system,
* No CSS frameworks, vanilla css. I will only use sass preprocessor,  
* Education destination,
* **Entity Framework!**,
* Blog is about poems, i will put there some my own poems :)

# Application informations
Application provides register only for first user in database, first registered user will be a administrator.

# Helper view  
It is worth to create this view
```sql
CREATE VIEW [dbo].[View]
	AS SELECT dbo.AspNetRoles.Name as "Role name", dbo.AspNetUsers.Name as "User name", dbo.AspNetUsers.Id as "User Id" 
from dbo.AspNetRoles join dbo.AspNetUserRoles on dbo.AspNetUserRoles.RoleId = dbo.AspNetRoles.Id
join dbo.AspNetUsers on dbo.AspNetUserRoles.UserId = dbo.AspnetUsers.Id;
```
