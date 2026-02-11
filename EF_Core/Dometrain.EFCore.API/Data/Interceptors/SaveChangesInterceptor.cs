using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Dometrain.EFCore.API.Data.Interceptors
{
    // Interceptor to perform actions before or after SaveChanges is called on the DbContext
    // Can be used for logging, auditing, validation, etc.
    public class SaveChangesInterceptor : ISaveChangesInterceptor
    {
        public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context as MoviesContext;

            if(context is null) return result;

            // Access the ChangeTracker to inspect the entities being tracked by the DbContext
            var tracker = context.ChangeTracker;

            // Entries returns a collection of EntityEntry objects for the specified entity type (Genre in this case)
            // This allows you to inspect the state of the entities (Added, Modified, Deleted, Unchanged) and perform actions accordingly

            var deletedEntries = tracker.Entries<Genre>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in deletedEntries)
            {
                var genre = entry.Entity;
                Console.WriteLine($"Genre with ID {genre.Id} is being deleted.");

                entry.Property<bool>("Deleted").CurrentValue = true;
                entry.State = EntityState.Modified;
            }

            return result;
        }

        public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(SavingChanges(eventData, result));
        }
    }
}
