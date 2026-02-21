using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fridge.Api.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Fridge.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IConfiguration cfg) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!((request.Username == "user" && request.Password == "user") ||
            (request.Username == "admin" && request.Password == "admin")))
        {
            return Unauthorized();
        }

        var jwt = cfg.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var role = request.Username == "admin" ? "Admin" : "User";

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, request.Username),
            new(ClaimTypes.Name, request.Username),
            new(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new LoginResponse(accessToken));
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var username = User.Identity?.Name;
        var role = User.FindFirstValue(ClaimTypes.Role);

        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Value).ToArray());

        return Ok(new MeResponse(username, role, claims));
    }

}