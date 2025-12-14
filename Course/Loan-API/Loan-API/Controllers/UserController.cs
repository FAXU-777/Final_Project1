using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Loan_API.Data.Modeles;
using Loan_API.Services;
using Loan_API.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Loan_API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(UserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userService.GetAllUsers();
        return new OkObjectResult(users);
    }

    
    [HttpPost("register")]
    public IActionResult Register([FromBody] User user, [FromQuery] string password)
    {
        var validationResult = UserValidator.Validate(user, password);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        try
        {
            var registeredUser = _userService.Register(user, password);
            return Ok(registeredUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("block/{userId}")]
    [Authorize(Roles = "Accountant")]
    public IActionResult BlockUser(int userId, [FromQuery] bool block)
    {
        try
        {
            _userService.BlockUser(userId, block);
            return Ok(new { message = $"User {(block ? "blocked" : "unblocked")} successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("create-accountant")]
    [Authorize(Roles = "Accountant")]
    public IActionResult CreateAccountant([FromBody] User user, [FromQuery] string password)
    {
        var validationResult = UserValidator.Validate(user, password);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        try
        {
            var accountant = _userService.CreateAccountant(user, password);
            return Ok(new 
            { 
                message = "Accountant created successfully",
                user = new
                {
                    accountant.Id,
                    accountant.Username,
                    accountant.FirstName,
                    accountant.LastName,
                    accountant.Email,
                    accountant.Age,
                    accountant.Role
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromQuery] string username, [FromQuery] string password)
    {
        try
        {
            var user = _userService.Authenticate(username, password);
            
            
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(securityToken);

            return Ok(new
            {
                token = tokenString,
                expires = tokenDescriptor.Expires,
                user = new
                {
                    user.Id,
                    user.Username,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Age,
                    user.Role
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
    
}