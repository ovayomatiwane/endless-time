namespace Common.Responses
{
    public class ApiResponse<TData>
    {
        public bool Success { get; init; }

        public TData? Data { get; init; }

        public string? Message { get; init; }

        public List<string> Errors { get; init; } = new();

        public static ApiResponse<TData> Ok(TData data, string? message = null)
            => new()
            {
                Success = true,
                Data = data,
                Message = message
            };
    }

    public class ApiResponse
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public List<string> Errors { get; init; } = new();

        public static ApiResponse Fail(string message, params string[] errors)
            => new()
            {
                Success = false,
                Message = message,
                Errors = errors.ToList()
            };
    }
}
