Gtihub: https://github.com/Jiakuo0x/Malfurion.EntityFramework

# Instructions for use
Use for context
``` csharp
using Malfurion.EntityFramework;

public class TestContext : DbContextBase
{
    public TestContext(DbContextOptions options) : base(options) { }
    public DbSet<Student>? Students { get; set; }
    public DbSet<User>? Users { get; set; }
}
```

Use for model
The system will automatically update the time of 'Created' and 'LastUpdated'
``` csharp
using Malfurion.EntityFramework.Models;

public class User : EntityBase<Guid>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
```

Using delete tags
The deletion operation will no longer be deleted in the database, but will be marked as deleted and will no longer be queried during the query. If you need to query the deleted data, you need to use 'IgnoreQueryFilters()'.
``` csharp
public class Student : EntityBase, IPseudoDeletion
{
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}
```