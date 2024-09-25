namespace UserManagement.Api.Shared.Results
{
    public class Result
    {
        public Result(Boolean isSuccess, CustomError error)
        {
            if (!isSuccess && error == CustomError.None ||
                isSuccess && error != CustomError.None)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public Boolean IsSuccess { get; }
        public Boolean IsFailure => !IsSuccess;
        public CustomError Error { get; }

        public static Result Success() => new(true, CustomError.None);
        public static Result Failure(CustomError error) => new(false, error);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, CustomError.None);
        public static Result<TValue> Failure<TValue>(CustomError error) => new(default, false, error);

        public static Result<TValue> Create<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(CustomError.NullValue);
    }

    public sealed class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public Result(TValue? value, Boolean isSuccess, CustomError error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result cannot be accessed");

        public static implicit operator Result<TValue>(TValue? value) => Create(value);
    }
}
