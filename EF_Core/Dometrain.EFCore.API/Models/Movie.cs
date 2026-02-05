namespace Dometrain.EFCore.API.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Title { get; set; }    
    public DateTime ReleaseDate { get; set; }
    public string? Synopsis { get; set; }
    public AgeRating AgeRating { get; set; }

    // Navigation properties
    public Genre Genre { get; set; }
    public int MainGenreId { get; set; }

    public Person Director { get; set; }
    public ICollection<Person> Actors { get; set; } = new HashSet<Person>();
}

public enum AgeRating
{
    All = 0,
    Elementary = 6,
    Teen = 12,
    Mature = 16,
    Adult = 18
}

public class MovieTitle
{
    public int Id { get; set; }
    public string? Title { get; set; }  
}