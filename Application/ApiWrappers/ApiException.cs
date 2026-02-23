namespace Application.ApiWrappers;

public class ApiException
{
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; } = default!;
}