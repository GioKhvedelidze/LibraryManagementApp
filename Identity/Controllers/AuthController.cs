using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Models;
using Identity.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public static User user = new User();

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="userDto">The user data transfer object.</param>
    /// <returns>The registered user.</returns>
    [HttpPost("register")]
    public  ActionResult<User> Register(UserDto userDto)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

        user.Username = userDto.Username;
        user.PasswordHash = passwordHash;

        return Ok(user);
    }
    
    
    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="userDto">The user data transfer object.</param>
    /// <returns>A JWT token.</returns>
    [HttpPost("login")]
    public  ActionResult<User> Login(UserDto userDto)
    {
        if (user.Username != userDto.Username)
        {
            return BadRequest("Username or Password is wrong.");
        }

        if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
        {
            return BadRequest("Username or Password is wrong.");
        }
        
        var jwtToken = CreateToken(user);

        var response = new LoginResponseDto
        {
            AccessToken = jwtToken
        };

        return Ok(response);
    }
    
    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}