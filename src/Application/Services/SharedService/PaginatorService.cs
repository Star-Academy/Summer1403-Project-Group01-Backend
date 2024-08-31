using Application.DTOs;

namespace Application.Services.SharedService;

public static class PaginatorService
{
    public static PaginatedList<T> Paginate<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var sourceList = source as IList<T> ?? source.ToList();
            
        var count = sourceList.Count;
        var items = sourceList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}