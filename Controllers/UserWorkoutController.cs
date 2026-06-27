using Cassandra;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Dto;
using FitnessApp.Entities;
using FitnessApp.Enums;
using FitnessApp.DataProvider;

namespace FitnessApp.Controllers
{
    [ApiController]
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
        public async Task<IActionResult> Delete(Guid userId, DateTimeOffset workoutDate, Guid workoutId)
        {
            if (userId == null)
                return NotFound($"Korisnik ciji je id {userId} ne postoji");

            await _repo.DeleteAsync(userId, workoutDate, workoutId);
            return NoContent();
        }
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<List<UserWorkouts>>> GetForUser(Guid userId)
        {
            if (userId == null)
                return NotFound($"Korisnik ciji je id {userId} ne postoji");

            var workouts = await _repo.GetWorkoutByUser(userId);
            return Ok(workouts);
        }
        [HttpPost]
        public async Task<ActionResult<UserWorkouts>> Create(UserWorkoutDto.CreateWorkoutDto dto)
        {
            var userValidation = await _users.GetById(dto.UserId);
            if (userValidation == null)
                return NotFound($"Korisnik ciji je id {dto.UserId} ne postoji");

            var workout = new UserWorkouts
            {
                UserId = dto.UserId,
                WorkoutDate = dto.WorkoutDate,
                TypeOfWorkout = Enum.Parse<WorkoutType>(dto.TypeOfWorkout),
                IsRestDay = dto.IsRestDay,
                Notes = dto.Notes
            };
            await _repo.InsertAsync(workout);
            return CreatedAtAction(nameof(GetForUser), new { userId = workout.UserId }, workout);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Dto.UserWorkoutDto.UpdateWorkoutDto dto)
        {
            var userValidation = await _users.GetById(dto.UserId);
            if (userValidation == null)
                return NotFound($"Korisnik ciji je id {dto.UserId} ne postoji");

            var workout = new UserWorkouts
            {
                UserId = dto.UserId,
                WorkoutDate = dto.WorkoutDate,
                WorkoutId = dto.WorkoutId,
                TypeOfWorkout = Enum.Parse<WorkoutType>(dto.TypeOfWorkout),
                IsRestDay = dto.IsRestDay,
                Notes = dto.Notes
            };
            await _repo.UpdateAsync(workout);
            return NoContent();
        }            
    }
}
