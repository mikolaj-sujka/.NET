using System.Text.Json.Serialization;

namespace Dometrain.EFCore.API.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [JsonIgnore]
    public byte[] ConcurrencyToken { get; set; } = Array.Empty<byte>(); // Concurrency token for optimistic concurrency control, initialized to an empty byte array to avoid null reference issues.

    [JsonIgnore]
    public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
    public DateTime CreatedDate { get; set; }
}