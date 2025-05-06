namespace ExpenseFlow.Api.Controllers;

using System.Security.Claims;
using System.Text;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly IConfiguration _config;

  public AuthController(UserManager<ApplicationUser> userManager,
                        SignInManager<ApplicationUser> signInManager,
                        IConfiguration configuration)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _config = configuration;
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginRequest login)
  {
    if (login == null) return BadRequest("Login request cannot be null.");
    if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
      return BadRequest("Email and password cannot be empty.");



    var user = await _userManager.FindByEmailAsync(login.Email);
    if (user == null) return Unauthorized();

    var passOk = await _userManager.CheckPasswordAsync(user, login.Password);
    if (!passOk) return Unauthorized();

    var roles = await _userManager.GetRolesAsync(user);
    var token = GenerateJwtToken(user, roles);
    return Ok(new { token });
  }

  private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
  {
    var jwt = _config.GetSection("JwtSettings");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expires = DateTime.UtcNow
                      .AddMinutes(double.Parse(jwt["DurationInMinutes"]!));

    var claims = new List<Claim> {
  new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
  };


    claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

    var token = new JwtSecurityToken(
      issuer: jwt["Issuer"],
      audience: jwt["Audience"],
      claims: claims,
      expires: expires,
      signingCredentials: creds
    );
    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
