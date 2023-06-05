namespace Malfurion.EntityFramework;

public static class ContextExtensions
{
    public static void PreSaveChanges(this DbContextBase context)
    {
        var entries = context.ChangeTracker.Entries()
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
    }
}