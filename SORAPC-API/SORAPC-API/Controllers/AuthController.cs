using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SORAPC_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SORAPC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly SorapcContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SorapcContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (_context.Users.Any(u => u.Logins == model.Login || u.Email == model.Email))
                return BadRequest("Пользователь с таким логином или email уже существует.");

            var user = new User
            {
                UserSurname = model.UserSurname,
                UserName = model.UserName,
                UserMiddlename = model.UserMiddlename,
                Email = model.Email,
                Phone = model.Phone,
                Logins = model.Login,
                Passwords = BCrypt.Net.BCrypt.HashPassword(model.Password),
                RoleId = model.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Пользователь успешно зарегистрирован.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Logins == model.Login);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Passwords))
                return Unauthorized("Неверный логин или пароль.");

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt").Get<JwtSettings>();
            var claims = new[]
            {
            new Claim("id",user.IdUsers.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Logins),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.RoleId == 1 ? "Администратор" : "Клиент")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.TokenLifetimeMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}