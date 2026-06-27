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
    public class UserDietController : ControllerBase
    {
        private readonly UserDietDP _diet;

        public UserDietController(UserDietDP diet)
        {
            _diet = diet;
        }

        [HttpGet("{userId:guid}/{day}")]
        public async Task<ActionResult<UserDiet>> GetByDay(Guid userId, DateTime day)
        {
            var diet = await _diet.GetByDay(userId, day);
            if (day == null)
                return NotFound("Nista nije zapisano za ovaj dan");
            return Ok(diet);
        }
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<List<UserDiet>>> GetAllDays(Guid userId)
        {
            var diets = await _diet.GetAllDays(userId);
            return Ok(diets);
        }
        [HttpDelete("{userId:guid}/{day}")]
        public async Task<IActionResult> DeleteDay(Guid userId, DateTime day)
        {
            await _diet.DeleteDayAsync(userId, day);
            return NoContent();
        }

        [HttpPut("{userId:guid}/{day}/breakfast")]
        public async Task<IActionResult> SetBreakfast(Guid userId, DateTime day, [FromBody] string breakfast)
        {
            await _diet.SetBreakfastAsync(userId, day, breakfast);
            return NoContent();
        }
        [HttpPut("{userId:guid}/{day}/lunch")]
        public async Task<IActionResult> SetLunch(Guid userId, DateTime day, [FromBody] string lunch)
        {
            await _diet.SetLunchAsync(userId, day, lunch);
            return NoContent();
        }
        [HttpPut("{userId:guid}/{day}/dinner")]
        public async Task<IActionResult> SetDinner(Guid userId, DateTime day, [FromBody] string dinner)
        {
            await _diet.SetDinnerAsync(userId, day, dinner);
            return NoContent();
        }
        [HttpPut("{userId:guid}/{day}/calories")]
        public async Task<IActionResult> SetCalories(Guid userId, DateTime day, [FromBody] int calories)
        {
            await _diet.SetCaloriesAsync(userId, day, calories);
            return NoContent();
        }

        [HttpPut("{userId:guid}/{day}/snack")]
        public async Task<IActionResult> AddSnack(Guid userId, DateTime day, [FromBody] string snack)
        {
            await _diet.AddSnackAsync(userId, day, snack);
            return NoContent();
        }
        [HttpPut("{userId:guid}/{day}/supplement")]
        public async Task<IActionResult> AddSupplement(Guid userId, DateTime day, [FromBody] string supplement)
        {
            await _diet.AddSupplementAsync(userId, day, Enum.Parse<SupplementsEnum>(supplement));
            return NoContent();
        }

        [HttpDelete("{userId:guid}/day/snack")]
        public async Task<IActionResult> RemoveSnack(Guid userId, DateTime day, [FromBody] string snack)
        {
            await _diet.RemoveSnackAsync(userId, day, snack);
            return NoContent();
        }
        [HttpDelete("{userId:guid}/{day}/supplement")]
        public async Task<IActionResult> RemoveSupplement(Guid userId, DateTime day, [FromBody] string supplement)
        {
            await _diet.RemoveSupplementAsync(userId, day, Enum.Parse<SupplementsEnum>(supplement));
            return NoContent();
        }
    }
}
