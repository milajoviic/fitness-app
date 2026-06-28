using Microsoft.AspNetCore.Mvc;
using FitnessApp.Dto;
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
    public class PeriodController : ControllerBase
    {
        private readonly PeriodDP _period;
        private readonly UserDP _users;

        public PeriodController(PeriodDP period, UserDP users)
        {
            _period = period;
            _users = users;
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(DateTime start)
        {
            Guid userId = User.GetUserId();
            await _period.DeleteAsync(userId, start);
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<List<PeriodByUser>>> GetForUser()
        {
            Guid userId = User.GetUserId();
            var results = await _period.GetByUser(userId);
            return Ok(results);
        }
        [HttpPut("notes")]
        public async Task<IActionResult> SetNotes([FromBody] string notes)
        {
            Guid userId = User.GetUserId();
            await _period.SetNotesAsync(userId, notes);
            return NoContent();
        }
        [HttpDelete("notes")]
        public async Task<IActionResult> DeleteNote()
        {
            Guid userId = User.GetUserId();
            await _period.SetNotesAsync(userId, "");
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<PeriodByUser>> Create(PeriodDto.PeriodInputDto dto)
        {
            Guid userId = User.GetUserId();
            var user = await _users.GetById(userId);
            if (user!.Gender != GenderEnum.Female)
                return BadRequest("Neispravan pol");
            var period = new PeriodByUser
            {
                UserId = userId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Notes = dto.Notes
            };
            await _period.AddPeriodAsync(period);
            return Ok();
        }
        [HttpGet("phase")]
        public async Task<ActionResult<string>> GetPhase()
        {
            Guid userId = User.GetUserId();
            var history = await _period.GetByUser(userId);
            if (history.Count == 0)
                return NotFound("Nisu uneti nikakvi predhodni ciklusi");
            CycleEnum phase = _period.GetCurrentPhase(history[0].StartDate);
            return Ok(phase.ToString());
        }
    }
}
