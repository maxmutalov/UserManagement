namespace UserManagement.Api.Shared.Extensions
{
    public static class StringExtensions
    {
        public static Boolean ToBoolean(this String str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            if (str.ToLower() == "true")
                return true;
            if (str.ToLower() == "false") return false;
            else
                throw new InvalidOperationException("The string cannot be converted to bool type");
        }
    }
}
