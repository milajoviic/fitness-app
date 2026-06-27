namespace FitnessApp.Dto
{
    public class ExerciseDto
    {
        public record CreateExerciseDto
        (
            Guid WorkoutId,
            string Name,
            int Reps,
            int RestMinutes,
            int Sets,
            decimal WeightKg
        );
        public record UpdateExerciseDto
        (
            Guid WorkoutId,
            int ExcOrder,
            string Name,
            int Reps,
            int Sets, 
            int RestMinutes,
            decimal WeightKg
        );
    }
}
