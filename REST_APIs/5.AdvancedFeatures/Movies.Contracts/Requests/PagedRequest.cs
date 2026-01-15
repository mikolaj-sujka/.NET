namespace Movies.Contracts.Requests;

public class PagedRequest // could be also an interface
{
    public required int PageNumber { get; set; } = 1;
    public required int PageSize { get; set; } = 10;
}