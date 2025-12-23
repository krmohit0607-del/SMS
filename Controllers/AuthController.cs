using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SMS.API.Data;
using SMS.API.DTOs.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SMS.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto request)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                return Unauthorized("Invalid credentials");

            var claims = new List<Claim>
{
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
    new(ClaimTypes.Role, user.Role.ToString()),
};

            // IMPORTANT: only for SchoolAdmin
            if (user.SchoolId != null)
            {
                claims.Add(new Claim("SchoolId", user.SchoolId.ToString()!));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["Jwt:ExpireMinutes"])
                ),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                role = user.Role.ToString()
            });
        }

        [Authorize]
        [HttpGet("debug")]
        public IActionResult Debug()
        {
            return Ok(new
            {
                User.Identity.IsAuthenticated,
                Roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value),
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}
