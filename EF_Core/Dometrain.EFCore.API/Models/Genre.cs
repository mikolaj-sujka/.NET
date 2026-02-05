using System.Text.Json.Serialization;

namespace Dometrain.EFCore.API.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }

    // Navigation property
    [JsonIgnore] // To prevent circular references during JSON serialization, it will stop the serialization of the Movies property.
    public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
}