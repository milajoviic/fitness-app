namespace FitnessApp.Dto
{
    public class UserWorkoutDto
    {
        public record CreateWorkoutDto(
            DateTimeOffset WorkoutDate,
            string TypeOfWorkout,
            bool IsRestDay,
            string? Notes
        );
        public record UpdateWorkoutDto(
            DateTimeOffset WorkoutDate,
            Guid WorkoutId,
            string TypeOfWorkout,
            bool IsRestDay,
            string? Notes
        );
    }
}
