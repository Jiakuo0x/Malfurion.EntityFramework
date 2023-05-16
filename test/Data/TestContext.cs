using Malfurion.EntityFramework;

public class TestContext : DbContextBase
{
    public TestContext(DbContextOptions options) : base(options) { }
    public DbSet<Student>? Students { get; set; }
    public DbSet<User>? Users { get; set; }
}