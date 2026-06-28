using Microsoft.AspNetCore.Mvc;
using FitnessApp.Entities;
using FitnessApp.Enums;
using FitnessApp.DataProvider;
using Microsoft.AspNetCore.Authorization;
using FitnessApp.Auth;

namespace FitnessApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserHealthController : ControllerBase
    {
        private readonly UserHealthDP _health;
        public UserHealthController(UserHealthDP health )
        {
            _health = health;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            Guid userId = User.GetUserId();
            await _health.DeleteAsync(userId);
            return NoContent();
        }
        [HttpDelete("notes")]
        public async Task<IActionResult> DeleteNotes()
        {
            Guid userId = User.GetUserId();
            await _health.SetNotesAsync(userId, "");
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<UserHealth>> GetUserHealth()
        {
            Guid userId = User.GetUserId();
            var results = await _health.GetByUser(userId);
            if (results == null)
                return NotFound("Korisnik nema nista zapisano");
            return Ok(results);
        }
        [HttpPut("height")]
        public async Task<IActionResult> SetHeight([FromBody] decimal height)
        {
            Guid userId = User.GetUserId();
            await _health.SetHeightAsync(userId, height);
            return NoContent();
        }
        [HttpPut("weight")]
        public async Task<IActionResult> SetWeight([FromBody] decimal weight)
        {
            Guid userId = User.GetUserId();
            await _health.SetWeightAsync(userId, weight);
            return NoContent();
        }
        [HttpPut("notes")]
        public async Task<IActionResult> SetNotes([FromBody] string notes)
        {
            Guid userId = User.GetUserId();
            await _health.SetNotesAsync(userId, notes);
            return NoContent();
        }
        [HttpPut("chronic")]
        public async Task<IActionResult> AddChronicCond([FromBody] string cc)
        {
            Guid userId = User.GetUserId();
            await _health.AddChronicCondAsync(userId, Enum.Parse<ChronicCondEnum>(cc));
            return NoContent();
        }
        [HttpPut("conditions")]
        public async Task<IActionResult> SetChronicCond([FromBody] List<string> conds)
        {
            Guid userId = User.GetUserId();
            var condEnum = conds.Select(c => Enum.Parse<ChronicCondEnum>(c)).ToList();
            await _health.SetChronicCondAsync(userId, condEnum);
            return NoContent();
        }
        [HttpGet("bmi")]
        public async Task<ActionResult<decimal>> GetBmi()
        {
            Guid userId = User.GetUserId();
            var results = await _health.GetBmiAsync(userId);
            if (results == null)
                return NotFound("Visina i tezina moraju biti pravilno uneti");
            return Ok(results);
        }
    }
}
