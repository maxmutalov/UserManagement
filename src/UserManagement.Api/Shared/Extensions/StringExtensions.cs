using System.Text;

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

        public static string ToSnakeCase(this String input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var builder = new StringBuilder();
            builder.Append(char.ToLowerInvariant(input[0]));

            for (var i = 1; i < input.Length; i++)
            {
                var c = input[i];
                if (char.IsUpper(c))
                {
                    builder.Append('_');
                    builder.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }
    }
}
