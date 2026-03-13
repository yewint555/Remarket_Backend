namespace Application.Dtos;

public record PagedResponseDto<T>
{
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public T Data { get; init; }

    public PagedResponseDto(int totalItems, int pageNumber, int pageSize, T data)
    {
        TotalItems = totalItems;
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        Data = data;
    }
}