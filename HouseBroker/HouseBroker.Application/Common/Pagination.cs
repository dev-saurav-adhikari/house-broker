using System.Collections;

namespace HouseBroker.Infrastructure.Services;

public class Pagination<T> 
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }

    public Pagination() {}

    public Pagination(List<T> items, int totalCount, int currentPage, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
    }

    public Pagination(IQueryable<T> source, int pageNumber, int pageSize)
    {
        TotalCount = source.Count();
        Items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        CurrentPage = pageNumber;
        PageSize = pageSize;
    }
    
}