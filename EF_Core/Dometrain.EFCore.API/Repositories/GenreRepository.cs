using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EfCore.API.Repositories;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAll();
    Task<Genre?> Get(int id);
    Task<Genre> Create(Genre genre);
    Task<Genre?> Update(int id, Genre genre);
    Task<bool> Delete(int id);
    Task<IEnumerable<Genre>> GetAllFromQuery();
    Task<IEnumerable<GenreName>> GetAllGenreNames();
}

public class GenreRepository(MoviesContext context, IUnitOfWorkManager uowManager) : IGenreRepository
{
    public async Task<IEnumerable<Genre>> GetAll()
    {
        return await context.Genres.ToListAsync();
    }

    public async Task<Genre?> Get(int id)
    {
        return await context.Genres.FindAsync(id);
    }

    public async Task<Genre> Create(Genre genre)
    {
        await context.Genres.AddAsync(genre);

        if(!uowManager.IsUnitOfWorkStarted)
            await context.SaveChangesAsync();

        return genre;
    }

    public async Task<Genre?> Update(int id, Genre genre)
    {
        var existingGenre = await context.Genres.FindAsync(id);

        if (existingGenre is null)
            return null;

        existingGenre.Name = genre.Name;

        if(!uowManager.IsUnitOfWorkStarted)
            await context.SaveChangesAsync();

        return existingGenre;
    }

    public async Task<bool> Delete(int id)
    {
        var existingGenre = await context.Genres.FindAsync(id);

        if (existingGenre is null)
            return false;

        context.Genres.Remove(existingGenre);

        if(!uowManager.IsUnitOfWorkStarted)
            await context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Genre>> GetAllFromQuery()
    {
        var minimumGenreId = 2;

        var genres = await context.Genres
            //.FromSqlRaw("SELECT * FROM Genres WHERE Id >= {0}", minimumGenreId)
            .FromSql($"SELECT * FROM Genres WHERE Id >= {minimumGenreId}")
            .Where(genre => genre.Name != "Comedy")
            .ToListAsync();

        return genres;
    }

    public async Task<IEnumerable<GenreName>> GetAllGenreNames()
    {
        // var genreName = await context.GenreNames.ToListAsync();

        // This is a more efficient way to get only the names of the genres without loading the entire Genre entities into memory.
        // does not require a DbSet<GenreName> to be defined in the context, as it is a simple DTO (Data Transfer Object) that matches the shape of the data being queried.
        var names = await context.Database
            .SqlQuery<GenreName>($"SELECT Name FROM Genres")
            .ToListAsync();

        return names;
    }
}