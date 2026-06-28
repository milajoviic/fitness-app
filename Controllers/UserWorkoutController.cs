using Cassandra;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Dto;
using FitnessApp.Entities;
using FitnessApp.Enums;
using FitnessApp.DataProvider;
using FitnessApp.Auth;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserWorkoutController : ControllerBase
    {
        private readonly UserWorkoutDP _repo;
        private readonly UserDP _users;
        public UserWorkoutController(UserWorkoutDP repo, UserDP users)
        {
            _repo = repo;
            _users = users;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DateTimeOffset workoutDate, Guid workoutId)
        {
            Guid userId = User.GetUserId();
            await _repo.DeleteAsync(userId, workoutDate, workoutId);
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<List<UserWorkouts>>> GetForUser()
        {
            Guid userId = User.GetUserId();
            var workouts = await _repo.GetWorkoutByUser(userId);
            return Ok(workouts);
        }

        [HttpPost]
        public async Task<ActionResult<UserWorkouts>> Create(UserWorkoutDto.CreateWorkoutDto dto)
        {
            Guid userId = User.GetUserId();
            var workout = new UserWorkouts
            {
                UserId = userId,
                WorkoutDate = dto.WorkoutDate,
                TypeOfWorkout = Enum.Parse<WorkoutType>(dto.TypeOfWorkout),
                IsRestDay = dto.IsRestDay,
                Notes = dto.Notes
            };
            await _repo.InsertAsync(workout);
            return CreatedAtAction(nameof(GetForUser), new { }, workout);
        }
        
        [HttpPut]
        public async Task<IActionResult> Update(Dto.UserWorkoutDto.UpdateWorkoutDto dto)
        {
            Guid userId = User.GetUserId();

            var workout = new UserWorkouts
            {
                UserId = userId,
                WorkoutDate = dto.WorkoutDate,
                WorkoutId = dto.WorkoutId,
                TypeOfWorkout = Enum.Parse<WorkoutType>(dto.TypeOfWorkout),
                IsRestDay = dto.IsRestDay,
                Notes = dto.Notes
            };
            await _repo.UpdateAsync(workout);
            return NoContent();
        }            
        //to do: kreirati funkciju za dodavanje note-ova posebno.
    }
}
