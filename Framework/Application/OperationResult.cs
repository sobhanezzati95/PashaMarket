namespace Framework.Application;
public class OperationResult
{
    public bool IsSucceeded { get; set; } = false;
    public string? Message { get; set; }
    public static OperationResult Succeeded(string message = "عملیات با موفقیت انجام شد")
    {
        return new OperationResult()
        {
            IsSucceeded = true,
            Message = message,
        };
    }
    public static OperationResult Failed(string message)
    {
        return new OperationResult()
        {
            IsSucceeded = false,
            Message = message,
        };
    }
    public static OperationResult Failed(object passwordsNotMatch)
    {
        throw new NotImplementedException();
    }
}