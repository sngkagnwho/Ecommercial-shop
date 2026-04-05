namespace mtkpm.Admin.Core.Utilities
{
    /// <summary>
    /// Generic result pattern for operation outcomes
    /// </summary>
    public class Result<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public static Result<T> Ok(T data, string message = "Success")
        {
            return new Result<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static Result<T> Fail(string message, List<string>? errors = null)
        {
            return new Result<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }

    /// <summary>
    /// Non-generic result pattern
    /// </summary>
    public class Result
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();

        public static Result Ok(string message = "Success")
        {
            return new Result
            {
                Success = true,
                Message = message
            };
        }

        public static Result Fail(string message, List<string>? errors = null)
        {
            return new Result
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}
