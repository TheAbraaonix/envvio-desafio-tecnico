namespace ParkingManagement.Application.Common;

public class PaginatedResult<T>
{
    public IEnumerable<T> Data { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }

    public PaginatedResult(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
    {
        Data = data;
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        HasPrevious = currentPage > 1;
        HasNext = currentPage < TotalPages;
    }

    // Helper method to create paginated result
    public static PaginatedResult<T> Create(IEnumerable<T> data, int totalCount, PaginationParams paginationParams)
    {
        return new PaginatedResult<T>(data, totalCount, paginationParams.Page, paginationParams.PageSize);
    }
}
