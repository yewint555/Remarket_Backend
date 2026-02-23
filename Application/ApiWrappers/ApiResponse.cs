namespace Application.ApiWrappers;

public class ApiResponse<T>
{
   

    public bool IsSuccess { get; set; }
    public string Message { get; set; } = default!;
    public string ErrorMessage { get; set; } = default!;
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public static ApiResponse<T> Success(bool success,T data, string message, int statuscode)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data,
            StatusCode = 200
        };
    }
    public static ApiResponse<T> Failure(string message, int statuscode)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            ErrorMessage = message,
            StatusCode = statuscode

        };
    }
}