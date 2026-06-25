namespace FitnessApp.Entities
{
    public class UserBodyMetrics
    {
        public Guid UserId { get; set; }
        public string BodyPart { get; set; }
        public DateTime RecordedAt { get; set; }
        public float Value { get; set; }
    }
}
