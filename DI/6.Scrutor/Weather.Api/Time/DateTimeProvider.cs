namespace Weather.Api.Time;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTimeNow => DateTime.Now;

    public DateTime DateTimeUtcNow => DateTime.UtcNow;
}
