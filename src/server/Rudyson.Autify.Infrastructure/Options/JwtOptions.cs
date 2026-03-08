namespace Rudyson.Autify.Infrastructure.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationMinutes { get; set; }
    public required string Secret { get; set; }
}
