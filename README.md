Gtihub: https://github.com/Jiakuo0x/Malfurion.EntityFramework

# Instructions for use
## Create a context
This operation is necessary.
``` csharp
using Malfurion.EntityFramework;

public class TestContext : DbContextBase
{
    public TestContext(DbContextOptions options) : base(options) { }
}
```

## Create a model
If you create a model that based on **EntityBase**,
the system will automatically update the 'Created' and 'LastUpdated' timestamps in your database.
``` csharp
using Malfurion.EntityFramework.Models;

public class User : EntityBase
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class User : EntityBase<Guid>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
```

## Create a pseudo deletion model
First, your model needs to inherit from **EntityBase**.  
Secondly, you need to inherit the **IPseudoDeletion** interface.   
In doing so, your deletion operation will not actually delete the data from the database, but instead mark it, and this marked data will not be retrieved in queries.   
If you need to query the deleted data, you should use the **IgnoreQueryFilters()** methoed.
``` csharp
public class Student : EntityBase, IPseudoDeletion
{
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}
```