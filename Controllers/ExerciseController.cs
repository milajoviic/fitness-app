using Microsoft.AspNetCore.Mvc;
using FitnessApp.Dto;
using FitnessApp.Entities;
using FitnessApp.Enums;
using FitnessApp.DataProvider;

namespace FitnessApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly ExerciseDP _exercise;
        private readonly UserWorkoutDP _workouts;

        public ExerciseController(ExerciseDP exercise) => _exercise = exercise;

        [HttpGet("{workoutId:guid}")]
        public async Task<ActionResult<List<ExercisesByWorkout>>> GetByWorkout(Guid workoutId)
        {
            if (workoutId == Guid.Empty)
                return NotFound($"Nije naveden trening");
            var exs = await _exercise.GetByWorkout(workoutId);
            return Ok(exs);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteExercise(Guid workoutId, int excOrder)
        {
            if (workoutId == Guid.Empty)
                return NotFound("Ne moze da se pronadje navedeni trening");

            await _exercise.DeleteAsync(workoutId, excOrder);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ExercisesByWorkout>> Create(ExerciseDto.CreateExerciseDto dto)
        {
            if (dto.WorkoutId == Guid.Empty)
                return NotFound("Nije naveden trening u kojem je radjena vezba");
            var exercise = new ExercisesByWorkout
            {
                WorkoutId = dto.WorkoutId,
                Name = dto.Name,
                Reps = dto.Reps,
                RestMinutes = dto.RestMinutes,
                Sets = dto.Sets,
                WeightKg = dto.WeightKg
            };
            await _exercise.InsertAsync(exercise);
            return CreatedAtAction(nameof(GetByWorkout), new { workoutId = exercise.WorkoutId }, exercise);
        }
        [HttpPut]
        public async Task<IActionResult> Update(ExerciseDto.UpdateExerciseDto dto)
        {
            if (dto.WorkoutId == Guid.Empty)
                return NotFound("Nije unet trening u kome je vezba odradjena");
            var exercise = new ExercisesByWorkout
            {
                WorkoutId = dto.WorkoutId,
                ExcOrder = dto.ExcOrder,
                Name = dto.Name,
                Reps = dto.Reps,
                RestMinutes = dto.RestMinutes,
                Sets = dto.Sets,
                WeightKg = dto.WeightKg
            };
            await _exercise.UpdateAsync(exercise);
            return NoContent();
        }
    }
}
