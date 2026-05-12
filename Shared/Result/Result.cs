namespace ims.Shared.Result
{
    public class Result
    {
        public bool IsSuccess { get; init; }
        public string Message { get; init; } = string.Empty;
        public List<string> Errors { get; init; } = [];

        public static Result Ok(string message = "Success") => new() { IsSuccess = true, Message = message };
        public static Result Fail(string message, params string[] errors) => new()
        {
            IsSuccess = false,
            Message = message,
            Errors = errors.ToList()
        };
    }

    public class Result<T> : Result
    {
        public T? Data { get; init; }

        public static Result<T> Ok(T data, string message = "Success") => new()
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };

        public new static Result<T> Fail(string message, params string[] errors) => new()
        {
            IsSuccess = false,
            Message = message,
            Errors = errors.ToList()
        };
    }
}
