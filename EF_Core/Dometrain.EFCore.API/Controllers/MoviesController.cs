using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Dometrain.EFCore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController(MoviesContext context) : Controller
{
    [HttpGet]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await context.Movies
            .AsNoTracking() // Improves performance for read-only queries by disabling change tracking, which is not needed when just retrieving data - less objects to track in memory.
            .Include(x => x.Actors) // Eager loading of related entities (Actors) to avoid lazy loading issues.
            .ToListAsync();

        foreach (var movie in movies.OfType<TelevisionMovie>())
        {
            //var actors = movie.Actors; // Accessing Actors to trigger lazy loading for TelevisionMovie instances.
            // await context.Entry(movie).Reference(m => m.Genre).LoadAsync(); // Explicitly load Genre for TelevisionMovie instances.
        }

        return Ok(movies);
    }

    // Compiled queries are pre-compiled and cached by EF Core, which can improve performance for frequently executed queries.
    private static Func<MoviesContext, AgeRating, IEnumerable<MovieTitle>> CreateCompiledQuery =>
        EF.CompileQuery((MoviesContext ctx, AgeRating ar) =>
            ctx.Movies
                .Where(movie => movie.AgeRating <= ar)
                .Select(movie => new MovieTitle { Id = movie.Identifier, Title = movie.Title }));

    [HttpGet("until-age/{ageRating}")]
    [ProducesResponseType(typeof(List<MovieTitle>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUntilAge([FromRoute] AgeRating ageRating)
    {
        var filteredTitles = CreateCompiledQuery(context, ageRating).ToList();

        /*var filteredTitles = await _context.Movies
            .Where(movie => movie.AgeRating <= ageRating)
            .Select(movie => new MovieTitle { Id = movie.Identifier, Title = movie.Title})
            .ToListAsync();*/

        return Ok(filteredTitles);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        // Queries database, returns first match, null if not found.
        // var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        // Similar to FirstOrDefault, but throws if more than one match is found.
        // var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);
        // Serves match from memory if already fetched, otherwise queries DB.
        //var movie = await _context.Movies.FindAsync(id);
        
        var movie = await context.Movies
            .Include(movie => movie.Genre)
            .SingleOrDefaultAsync(m => m.Identifier == id);
        
        return movie == null
            ? NotFound()
            : Ok(movie);
    }
    
    [HttpGet("by-year/{year:int}")]
    [ProducesResponseType(typeof(List<MovieTitle>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllByYear([FromRoute] int year)
    {
        var filteredTitles = await context.Movies
            .Where(movie => movie.ReleaseDate.Year == year)
            .Select(movie => new MovieTitle { Id = movie.Identifier, Title = movie.Title})
            .ToListAsync();

        return Ok(filteredTitles);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Movie movie)
    {
        await context.Movies.AddAsync(movie);
        
        // movie has no ID
        
        await context.SaveChangesAsync();
        
        // movie has an ID

        return CreatedAtAction(nameof(Get), new { id = movie.Identifier }, movie);
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Movie movie)
    {
        var existingMovie = await context.Movies.FindAsync(id);

        if (existingMovie is null)
            return NotFound();

        existingMovie.Title = movie.Title;
        existingMovie.ReleaseDate = movie.ReleaseDate;
        existingMovie.Synopsis = movie.Synopsis;

        await context.SaveChangesAsync();

        return Ok(existingMovie);
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        var existingMovie = await context.Movies.FindAsync(id);

        if (existingMovie is null)
            return NotFound();

        context.Movies.Remove(existingMovie);
        // _context.Remove(existingMovie);
        // _context.Movies.Remove( new Movie { Id = id });

        await context.SaveChangesAsync();

        return Ok();
    }
}