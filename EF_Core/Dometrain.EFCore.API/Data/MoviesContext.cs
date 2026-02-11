using Dometrain.EFCore.API.Data.EntityMapping;
using Dometrain.EFCore.API.Data.Interceptors;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EFCore.API.Data;

public class MoviesContext : DbContext
{
    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base (options)
    { }
    
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<ExternalInformation> ExternalInformations => Set<ExternalInformation>();
    public DbSet<Actor> Actors => Set<Actor>();
    // public DbSet<GenreName> GenreNames => Set<GenreName>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GenreMapping());
        modelBuilder.ApplyConfiguration(new MovieMapping());
        modelBuilder.ApplyConfiguration(new ExternalInformationMapping());
        modelBuilder.ApplyConfiguration(new ActorMapping());

        modelBuilder.ApplyConfiguration(new CinemaMovieMapping());
        modelBuilder.ApplyConfiguration(new TelevisionMovieMapping());

        /*modelBuilder.Entity<GenreName>()
            .HasNoKey()
            .ToSqlQuery($"SELECT Name FROM dbo.Genres");*/
    }

    // OnConfiguring is method that is called when the context is being configured. It is used to configure the context options, such as the database provider, connection string, and other settings. In this case, we are adding an interceptor to the context options. The interceptor will be called whenever the SaveChanges method is called on the context. This allows us to perform additional actions before or after the changes are saved to the database.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new SaveChangesInterceptor());
    }
}