namespace UserManagement.Api.Shared.Authentication
{
#pragma warning disable CS8618
    public sealed class JwtSettings
    {
        public const string SectionName = "JwtSettings";

        public string Issuer { get; init; }
        public string Audience { get; init; }
        public int ExpiryInMinutes { get; init; }
        public string SecurityKey { get; init; }
    }
#pragma warning restore CS8618
}
