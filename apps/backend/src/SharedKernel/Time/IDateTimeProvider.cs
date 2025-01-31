namespace SharedKernel.Time;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
