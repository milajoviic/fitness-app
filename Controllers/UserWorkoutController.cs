using Cassandra;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Dto;
using FitnessApp.Entities;
using FitnessApp.DataProvider;

namespace FitnessApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserWorkoutController : ControllerBase
    {
        private readonly UserWorkoutDP _repo;
        public UserWorkoutController(UserWorkoutDP repo) => _repo = repo;

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid userId, DateTimeOffset workoutDate, Guid workoutId)
        {
            await _repo.DeleteAsync(userId, workoutDate, workoutId);
            return NoContent();
        }
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<List<UserWorkouts>>> GetForUser(Guid userId)
        {
            var workouts = await _repo.GetWorkoutByUser(userId);
            return Ok(workouts);
        }
        [HttpPost]
        public async Task<ActionResult<UserWorkouts>> Create(UserWorkoutDto.CreateWorkoutDto dto)
        {
            var workout = new UserWorkouts
            {
                UserId = dto.UserId,
                WorkoutDate = dto.WorkoutDate,
                TypeOfWorkout = dto.TypeOfWorkout,
                IsRestDay = dto.IsRestDay,
                Notes = dto.Notes
            };
            await _repo.InsertAsync(workout);
            return CreatedAtAction(nameof(GetForUser), new { userId = workout.UserId }, workout);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Dto.UserWorkoutDto.UpdateWorkoutDto dto)
        {
            var workout = new UserWorkouts
            {
                UserId = dto.UserId,
                WorkoutDate = dto.WorkoutDate,
                WorkoutId = dto.WorkoutId,
                TypeOfWorkout = dto.TypeOfWorkout,
                IsRestDay = dto.IsRestDay,
                Notes = dto.Notes
            };
            await _repo.UpdateAsync(workout);
            return NoContent();
        }            
    }
}
