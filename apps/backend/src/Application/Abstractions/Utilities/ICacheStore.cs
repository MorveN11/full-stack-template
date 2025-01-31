namespace Application.Abstractions.Utilities;

public interface ICacheStore
{
    Task EvictByTagAsync(string tag, CancellationToken cancellationToken);
}
