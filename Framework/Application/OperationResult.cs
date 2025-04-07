namespace Framework.Application
{
    public class OperationResult<T>
    {
        public T? Data { get; set; }
        public bool IsSuccedded { get; set; } = false;
        public string? Message { get; set; }

        public static OperationResult<T> Succedded(T? data, string message = "عملیات با موفقیت انجام شد")
        {
            return new OperationResult<T>()
            {
                Data = data,
                IsSuccedded = true,
                Message = message,
            };
        }

        public static OperationResult<T> Succedded(string message = "عملیات با موفقیت انجام شد")
        {
            return new OperationResult<T>()
            {
                IsSuccedded = true,
                Message = message,
            };
        }

        public static OperationResult<T> Failed(string message)
        {
            return new OperationResult<T>()
            {
                IsSuccedded = false,
                Message = message,
            };
        }

        public static OperationResult<T> Failed(object passwordsNotMatch)
        {
            throw new NotImplementedException();
        }
    }
}
