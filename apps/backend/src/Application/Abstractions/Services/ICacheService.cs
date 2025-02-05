namespace Application.Abstractions.Services;

public interface ICacheService
{
    Task EvictByTagAsync(string tag, CancellationToken cancellationToken = default);

    Task BlacklistTokenAsync(string token, CancellationToken cancellationToken = default);

    Task<bool> IsTokenBlacklistedAsync(string token, CancellationToken cancellationToken = default);
}
