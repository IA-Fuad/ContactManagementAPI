using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContactManagement.WebAPI.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ContactManagement.WebAPI.Modules.AuthModule.Endpoints;

public class Login : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", Handle)
            .WithSummary("[{userName: admin, password: password}, {userName: fundmanager, password: password}]");
    }

    public record Request(string Username, string Password);
    
    public record Response(string Token);

    private record User(string Username, string Password, string Role);
    
    // Only for demo purpose
    private static readonly List<User> _users = [
        new("admin", "password", "admin"),
        new("fundmanager", "password", "fund manager")
    ];

    private static Results<Ok<Response>, UnauthorizedHttpResult> Handle(
        [FromBody]Request request,
        IConfiguration configuration)
    {
        User? user = _users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
        
        if (user is null) return TypedResults.Unauthorized();

        JwtSecurityTokenHandler tokenHandler = new();
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!);
        SecurityTokenDescriptor tokenDescriptor = new() 
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return TypedResults.Ok(new Response(tokenString)); 
    }
}