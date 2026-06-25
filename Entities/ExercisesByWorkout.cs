namespace FitnessApp.Entities
{
    public class ExercisesByWorkout
    {
        public Guid WorkoutId { get; set; }
        public int ExcOrder { get; set; }
        public string Name { get; set; } = "";
        public int Reps { get; set; }
        public int Sets { get; set; }
        public int RestMinutes { get; set; }
        public float WeightKg { get; set; }
    }
}
