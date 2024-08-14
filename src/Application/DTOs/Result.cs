namespace Application.DTOs;

public class Result
{
    public string Message { get; protected set; } = String.Empty;
    public bool Succeed { get; protected set; }

    public Result Ok()
    {
        return new Result
        {
            Succeed = true
        };
    }

    public Result Fail(string message)
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

    public Result<T> Ok(T value)
    {
        return new Result<T>
        {
            Succeed = true,
            Value = value
        };
    }

    public Result<T> Fail(string message)
    {
        return new Result<T>
        {
            Succeed = false,
            Message = message
        };
    }
}