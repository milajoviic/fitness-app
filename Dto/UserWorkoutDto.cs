using FitnessApp.Enums;

namespace FitnessApp.Dto
{
    public class UserWorkoutDto
    {
        public record CreateWorkoutDto(
            Guid UserId,
            DateTimeOffset WorkoutDate,
            WorkoutType TypeOfWorkout,
            bool IsRestDay,
            string? Notes
        );
        public record UpdateWorkoutDto(
            Guid UserId,
            DateTimeOffset WorkoutDate,
            Guid WorkoutId,
            WorkoutType TypeOfWorkout,
            bool IsRestDay,
            string? Notes
        );
    }
}
