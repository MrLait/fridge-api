namespace Fridge.Api.Contracts.Auth;

public sealed record MeResponse(
    string? Username,
    string? Role,
    Dictionary<string, string[]> Claims
);