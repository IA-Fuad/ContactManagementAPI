using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.WebAPI.Common;

public interface IPagination
{
    public int Page { get; }
    public int PageSize { get; }
    public string? SortBy { get; }
    public bool? SortAsc { get;  }
}

public record PagedResult<T>
{
    public int TotalCount { get; private set; }
    public List<T> Items { get; private set; }
    public int Page { get; private set; }
    public int PageSize { get; private set; }
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => (Page * PageSize) < TotalCount;

    public PagedResult(List<T> items, int totalCount, int page, int pageSize)
    {
        Items = items; 
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }
}

public class PaginationRequestValidator<TRequest> : AbstractValidator<TRequest> where TRequest : IPagination
{
    public PaginationRequestValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

public static class PaginationExtensions
{
    public static async Task<PagedResult<T>> ToPagedResult<T>(this IQueryable<T> query, IPagination pagination, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pagination);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(pagination.Page, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(pagination.PageSize, 0);

        int totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, pagination.Page, pagination.PageSize);
    }
}