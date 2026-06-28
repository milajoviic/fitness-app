using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FitnessApp.Dto;
using FitnessApp.Entities;
using FitnessApp.Enums;
using FitnessApp.DataProvider;
using FitnessApp.Auth;

namespace FitnessApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserDP _users;
        private readonly TokenService _tokens;
        private readonly RefreshTokenDP _refresh;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthController(UserDP users, TokenService tokens, RefreshTokenDP refresh)
        {
            _users = users;
            _tokens = tokens;
            _refresh = refresh;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto.RegisterUserDto dto)
        {
            var u = await _users.GetByEmail(dto.Email);
            if (u != null)
                return Conflict("Email vec postoji");
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = Enum.Parse<GenderEnum>(dto.Gender),
                BirthDate = dto.BirthDate
            };
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);
            await _users.AddAsync(user);
            return Ok("Registracija je uspesna");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto.LoginDto dto)
        {
            var user = await _users.GetByEmail(dto.Email);
            if (user == null)
                return Unauthorized("Pogresan mejl ili sifra");
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Pogresan mejl ili sifra");
            return Ok(await CreateTokens(user));
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var stored = await _refresh.GetAsync(refreshToken);
            if (stored == null)
                return Unauthorized("Nevazeci refresh-token");
            if(stored.Value.expiresAt<DateTimeOffset.UtcNow)
            {
                await _refresh.DeleteAsync(refreshToken);
                return Unauthorized("Refresh token je istekao, prijavite se ponovo");
            }
            var user = await _users.GetById(stored.Value.userId);
            if (user == null)
                return Unauthorized("Korisnik ne postoji");
            await _refresh.DeleteAsync(refreshToken);
            return Ok(await CreateTokens(user));
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await _refresh.DeleteAsync(refreshToken);
            return NoContent();
        }

        private async Task<object> CreateTokens(User user)
        {
            string access = _tokens.CreateAccessToken(user);
            string refresh = _tokens.CreateRefreshToken();
            var expires = DateTimeOffset.UtcNow.AddDays(JwtConfig.RefreshTokenDays);

            await _refresh.SaveAsync(refresh, user.UserId, expires);
            return new { accessToken = access, refreshToken = refresh };
        }
    }
}
