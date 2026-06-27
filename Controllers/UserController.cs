using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FitnessApp.Dto;
using FitnessApp.Entities;
using FitnessApp.Enums;
using FitnessApp.DataProvider;


namespace FitnessApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDP _user;
        private readonly PasswordHasher<User> _hasher = new();

        public UserController(UserDP user) => _user = user;

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto.RegisterUserDto dto)
        {
            if (await _user.GetByEmail(dto.Email) != null)
                return Conflict("Korisnik sa ovom email adresom vec postoji");

            var u = new User
            {
                UserId = Guid.NewGuid(),
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = Enum.Parse<GenderEnum>(dto.Gender),
                BirthDate = dto.BirthDate
            };
            u.PasswordHash = _hasher.HashPassword(u, dto.Password);
            await _user.AddAsync(u);
            return Ok(new { u.UserId, u.Email });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto.LoginDto dto)
        {
            var user = await _user.GetByEmail(dto.Email);
            if (user == null)
                return Unauthorized("Pogresan email ili lozinka");

            var passwordCheck = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (passwordCheck == PasswordVerificationResult.Failed)
                return Unauthorized("Pogresan email ili lozinka");

            return Ok(new { user.UserId, user.Email, user.FirstName });
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> Update(string email, UserDto.UpdateUserDto dto)
        {
            var user = await _user.GetByEmail(email);
            if (user == null) return NotFound("Korisnik ne postoji.");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Gender = Enum.Parse<GenderEnum>(dto.Gender);
            user.BirthDate = dto.BirthDate;

            await _user.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("{email}")] 
        public async Task<IActionResult> Delete(string email)
        {
            var user = await _user.GetByEmail(email);
            if (user == null)
                return NotFound("Korisnik sa ovom email adresom ne postoji");

            await _user.DeleteAsync(user.UserId, user.Email);
            return NoContent();
        }
    }
}
