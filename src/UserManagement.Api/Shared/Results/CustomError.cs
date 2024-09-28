namespace UserManagement.Api.Shared.Results
{
    public sealed record CustomError(List<String> Code, List<String> Massage)
    {
        public static CustomError None = new([String.Empty], [String.Empty]);
        public static CustomError NullValue = new(["Error.NullValue"], ["Null value was provided"]);
        public static CustomError UserNotFound = new(["Error.UserNotFound"], ["User was not found"]);
        public static CustomError UsersNotFound = new(["Error.UsersNotFound"], ["Users were not found"]);
        public static CustomError UsersIsNotBlocked = new(["Error.UsersIsNotBlocked"], ["Users is not blocked"]);
        public static CustomError UsersIsAlreadyBlocked = new(["Error.UsersIsAlreadyBlocked"], ["Users is already blocked"]);
        public static CustomError InvalidParameters =
            new(["Error.InvalidParameters"], ["Invalid parameters were provided"]);
    }
}
