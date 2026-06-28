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
    public class PeriodController : ControllerBase
    {
        private readonly PeriodDP _period;

        public PeriodController(PeriodDP period)
        {
            _period = period;
        }
        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> Delete(Guid userId, DateTime start)
        {
            await _period.DeleteAsync(userId, start);
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<List<PeriodByUser>>> GetForUser(Guid userId)
        {
            var results = await _period.GetByUser(userId);
            return Ok(results);
        }
        [HttpPut]
        public async Task<IActionResult> SetNotes(Guid userId, [FromBody] string notes)
        {
            await _period.SetNotesAsync(userId, notes);
            return NoContent();
        }
        [HttpDelete("{userId:guid}/notes")]
        public async Task<IActionResult> DeleteNote(Guid userId)
        {
            await _period.SetNotesAsync(userId, "");
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<PeriodByUser>> Create(Guid userId, PeriodDto.PeriodInputDto dto)
        {
            //ne znam da li treba da se dodaje provera jer ce kasnije da se uvede token.
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
        [HttpGet("{userId:guid}/phase")]
        public async Task<ActionResult<string>> GetPhase(Guid userId)
        {
            var history = await _period.GetByUser(userId);
            if (history.Count == 0)
                return NotFound("Nisu uneti nikakvi predhodni ciklusi");
            CycleEnum phase = _period.GetCurrentPhase(history[0].StartDate);
            return Ok(phase.ToString());
        }
    }

