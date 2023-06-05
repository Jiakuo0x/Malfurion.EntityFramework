using System.Linq.Expressions;
using System.Reflection;

namespace Malfurion.EntityFramework;

public class DbContextBase : DbContext
{
    public DbContextBase(DbContextOptions options) : base(options) { }
    public override int SaveChanges()
    {
        this.PreSaveChanges();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.PreSaveChanges();
        return await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(type => type.ClrType.IsSubclassOf(typeof(Models.EntityBase)));

        foreach (var entityType in entityTypes)
        {
            if(entityType.ClrType.IsAssignableTo(typeof(Models.IPseudoDeletion)))
            {
                var parameter = Expression.Parameter(entityType.ClrType, entityType.Name);
                var body = Expression.Equal(
                    Expression.Property(parameter, nameof(Models.IPseudoDeletion.IsDeleted)),
                    Expression.Constant(false));

                var lambda = Expression.Lambda(body, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
}