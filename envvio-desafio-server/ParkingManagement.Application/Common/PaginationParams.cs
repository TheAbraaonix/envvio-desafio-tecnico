namespace ParkingManagement.Application.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? SortBy { get; set; }
    
    public string SortOrder { get; set; } = "asc"; // "asc" or "desc"

    // Calculate skip count for database query
    public int Skip => (Page - 1) * PageSize;
}
