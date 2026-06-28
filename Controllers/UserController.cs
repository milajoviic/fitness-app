using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FitnessApp.Dto;
using FitnessApp.Entities;
using FitnessApp.Enums;
using FitnessApp.DataProvider;
using FitnessApp.Auth;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDP _user;
        private readonly PasswordHasher<User> _hasher = new();

        public UserController(UserDP user) => _user = user;

        [HttpGet("me")]
        public async Task<ActionResult<User>> GetMe()
        {
            Guid userId = User.GetUserId();
            var user = await _user.GetById(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserDto.UpdateUserDto dto)
        {
            Guid userId = User.GetUserId();
            var user = await _user.GetById(userId);
            if (user == null) return NotFound("Korisnik ne postoji.");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Gender = Enum.Parse<GenderEnum>(dto.Gender);
            user.BirthDate = dto.BirthDate;

            await _user.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete] 
        public async Task<IActionResult> Delete()
        {
            Guid userId = User.GetUserId();
            var user = await _user.GetById(userId);
            if (user == null)
                return NotFound("Korisnik sa ovom email adresom ne postoji");

            await _user.DeleteAsync(user.UserId, user.Email);
            return NoContent();
        }
    }
}
