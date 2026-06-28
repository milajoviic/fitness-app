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
    public class UserDietController : ControllerBase
    {
        private readonly UserDietDP _diet;

        public UserDietController(UserDietDP diet)
        {
            _diet = diet;
        }

        [HttpGet("{day}")]
        public async Task<ActionResult<UserDiet>> GetByDay(DateTime day)
        {
            Guid userId = User.GetUserId();
            var diet = await _diet.GetByDay(userId, day);
            if (day == null)
                return NotFound("Nista nije zapisano za ovaj dan");
            return Ok(diet);
        }
        [HttpGet]
        public async Task<ActionResult<List<UserDiet>>> GetAllDays()
        {
            Guid userId = User.GetUserId();
            var diets = await _diet.GetAllDays(userId);
            return Ok(diets);
        }
        [HttpDelete("{day}")]
        public async Task<IActionResult> DeleteDay(DateTime day)
        {
            Guid userId = User.GetUserId();
            await _diet.DeleteDayAsync(userId, day);
            return NoContent();
        }

        [HttpPut("{day}/breakfast")]
        public async Task<IActionResult> SetBreakfast(DateTime day, [FromBody] string breakfast)
        {
            Guid userId = User.GetUserId();
            await _diet.SetBreakfastAsync(userId, day, breakfast);
            return NoContent();
        }
        [HttpPut("{day}/lunch")]
        public async Task<IActionResult> SetLunch(DateTime day, [FromBody] string lunch)
        {
            Guid userId = User.GetUserId();
            await _diet.SetLunchAsync(userId, day, lunch);
            return NoContent();
        }
        [HttpPut("{day}/dinner")]
        public async Task<IActionResult> SetDinner(DateTime day, [FromBody] string dinner)
        {
            Guid userId = User.GetUserId();
            await _diet.SetDinnerAsync(userId, day, dinner);
            return NoContent();
        }
        [HttpPut("{day}/calories")]
        public async Task<IActionResult> SetCalories(DateTime day, [FromBody] int calories)
        {
            Guid userId = User.GetUserId();
            await _diet.SetCaloriesAsync(userId, day, calories);
            return NoContent();
        }

        [HttpPut("{day}/snack")]
        public async Task<IActionResult> AddSnack(DateTime day, [FromBody] string snack)
        {
            Guid userId = User.GetUserId();
            await _diet.AddSnackAsync(userId, day, snack);
            return NoContent();
        }
        [HttpPut("{day}/supplement")]
        public async Task<IActionResult> AddSupplement(DateTime day, [FromBody] string supplement)
        {
            Guid userId = User.GetUserId();
            await _diet.AddSupplementAsync(userId, day, Enum.Parse<SupplementsEnum>(supplement));
            return NoContent();
        }

        [HttpDelete("{day}/snack")]
        public async Task<IActionResult> RemoveSnack(DateTime day, [FromBody] string snack)
        {
            Guid userId = User.GetUserId();
            await _diet.RemoveSnackAsync(userId, day, snack);
            return NoContent();
        }
        [HttpDelete("{day}/supplement")]
        public async Task<IActionResult> RemoveSupplement(DateTime day, [FromBody] string supplement)
        {
            Guid userId = User.GetUserId();
            await _diet.RemoveSupplementAsync(userId, day, Enum.Parse<SupplementsEnum>(supplement));
            return NoContent();
        }
    }
}
