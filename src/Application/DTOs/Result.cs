namespace Application.DTOs;

public class Result
{
    public string Message { get; protected set; } = string.Empty;
    public bool Succeed { get; protected set; }

    public static Result Ok(string message = "succeed")
    {
        return new Result
        {
            Succeed = true,
            Message = message
        };
    }

    public static Result Fail(string message = "failed")
    {
        return new Result
        {
            Succeed = false,
            Message = message
        };
    }
}

public class Result<T> : Result
{
    public T? Value { get; private set; }

    public static Result<T> Ok(T value)
    {
        return new Result<T>
        {
            Succeed = true,
            Value = value
        };
    }

    public static Result<T> Fail(string message = "failed")
    {
        return new Result<T>
        {
            Succeed = false,
            Message = message
        };
    }
}