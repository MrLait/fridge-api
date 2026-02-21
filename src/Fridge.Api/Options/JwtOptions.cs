using Fridge.Api.Constants;

namespace Fridge.Api.Options;

public class JwtOptions
{
    public const string SectionName = ApiConstants.Jwt.SectionName;

    public string Key { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; }
}