namespace Dometrain.EFCore.API.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public List<Movie> Movies { get; set; } = new();
    }
}
