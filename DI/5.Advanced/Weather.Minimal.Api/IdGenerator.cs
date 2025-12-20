namespace Weather.Minimal.Api
{
    public class IdGenerator
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
