using System.Linq.Expressions;
using System.Reflection;

namespace Malfurion.EntityFramework;

public class DbContextBase : DbContext
{
    public DbContextBase(DbContextOptions options) : base(options) { }
    public override int SaveChanges()
    {
        var entries = this.ChangeTracker.Entries()
            .Where(i => i.State is not EntityState.Unchanged);

        var utcTime = DateTime.UtcNow;
        foreach (var entry in entries)
        {
            if (entry.Entity is not Models.EntityBase model)
                continue;

            if (entry.State is EntityState.Added)
                model.Created = utcTime;

            if (entry.State is EntityState.Deleted && model is Models.IPseudoDeletion delModel)
            {
                entry.State = EntityState.Modified;
                delModel.IsDeleted = true;
            }

            model.LastUpdated = utcTime;
        }

        return base.SaveChanges();
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