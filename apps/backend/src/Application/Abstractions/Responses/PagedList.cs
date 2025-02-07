using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Responses;

public static class PagedList
{
    public static async Task<PagedList<T>> CreateAsync<T>(
        IQueryable<T> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        int totalCount = await query.CountAsync(cancellationToken);

        IList<T> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
        };
    }

    public static PagedList<T> Create<T>(IList<T> items, int page, int pageSize, int totalCount)
    {
        return new PagedList<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
        };
    }
}

public class PagedList<T>
{
    internal PagedList() { }

    public IList<T> Items { get; init; } = [];
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;
}
