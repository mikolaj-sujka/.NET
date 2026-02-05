using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EFCore.API.Data;

public class MoviesContext : DbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("""
                                    Data Source=(localdb)\MSSQLLocalDB;
                                    Initial Catalog=ef-core-learning-test-db;
                                    Integrated Security=True;
                                    Encrypt=True;
                                    TrustServerCertificate=False;
                                    MultipleActiveResultSets=False;
                                    """);
        // Not proper logging
        optionsBuilder.LogTo(Console.WriteLine);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoviesContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}