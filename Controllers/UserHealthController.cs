using Microsoft.AspNetCore.Mvc;
using FitnessApp.Entities;
using FitnessApp.Enums;
using FitnessApp.DataProvider;

namespace FitnessApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserHealthController : ControllerBase
    {
        //dodajemo i funkciju za racunanje bmi-a.
        private readonly UserHealthDP _health;
        public UserHealthController(UserHealthDP health )
        {
            _health = health;
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            await _health.DeleteAsync(userId);
            return NoContent();
        }
        [HttpDelete("{userId:guid}/notes")]
        public async Task<IActionResult> DeleteNotes(Guid userId)
        {
            await _health.SetNotesAsync(userId, "");
            return NoContent();
        }
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<UserHealth>> GetUserHealth(Guid userId)
        {
            var results = await _health.GetByUser(userId);
            if (results == null)
                return NotFound("Korisnik nema nista zapisano");
            return Ok(results);
        }
        [HttpPut("{userId:guid}/height")]
        public async Task<IActionResult> SetHeight(Guid userId, [FromBody] decimal height)
        {
            await _health.SetHeightAsync(userId, height);
            return NoContent();
        }
        [HttpPut("{userId:guid}/weight")]
        public async Task<IActionResult> SetWeight(Guid userId, [FromBody] decimal weight)
        {
            await _health.SetWeightAsync(userId, weight);
            return NoContent();
        }
        [HttpPut("{userId:guid}/notes")]
        public async Task<IActionResult> SetNotes(Guid userId, [FromBody] string notes)
        {
            await _health.SetNotesAsync(userId, notes);
            return NoContent();
        }
        [HttpPut("{userId:guid}/chronic")]
        public async Task<IActionResult> AddChronicCond(Guid userId, [FromBody] string cc)
        {
            await _health.AddChronicCondAsync(userId, Enum.Parse<ChronicCondEnum>(cc));
            return NoContent();
        }
        [HttpPut("{userId:guid}/conditions")]
        public async Task<IActionResult> SetChronicCond(Guid userId, [FromBody] List<string> conds)
        {
            var condEnum = conds.Select(c => Enum.Parse<ChronicCondEnum>(c)).ToList();
            await _health.SetChronicCondAsync(userId, condEnum);
            return NoContent();
        }
        [HttpGet("{userId:guid}/bmi")]
        public async Task<ActionResult<decimal>> GetBmi(Guid userId)
        {
            var results = await _health.GetBmiAsync(userId);
            if (results == null)
                return NotFound("Visina i tezina moraju biti pravilno uneti");
            return Ok(results);
        }
    }
}
