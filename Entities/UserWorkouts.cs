using System;
using FitnessApp.Enums;

namespace FitnessApp.Entities
{
    public class UserWorkouts
    {
        public Guid UserId { get; set; }
        public DateTimeOffset WorkoutDate { get; set; }
        public Guid WorkoutId { get; set; }
        public bool IsRestDay { get; set; }
        public string? Notes { get; set; }
        public WorkoutType TypeOfWorkout { get; set; }
    }
}
