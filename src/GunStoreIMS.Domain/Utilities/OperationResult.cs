namespace GunStoreIMS.Domain.Utilities

{
    public class OperationResult
    {
        public bool Succeeded { get; protected set; }
        public string? ErrorMessage { get; protected set; }

        public static OperationResult Success() => new() { Succeeded = true };
        public static OperationResult Fail(string message)
            => new() { Succeeded = false, ErrorMessage = message };
    }

    public class OperationResult<T> : OperationResult
    {
        public T? Entity  { get; protected set; }

        public static OperationResult<T> Success(T value)
            => new() { Succeeded = true, Entity = value };

        public static new OperationResult<T> Fail(string message)
            => new() { Succeeded = false, ErrorMessage = message };
    }
}
