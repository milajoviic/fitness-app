namespace FitnessApp.Entities
{
    public class UserBodyMetrics
    {
        public Guid UserId { get; set; }
        public Guid MetricId { get; set; }
        public string BodyPart { get; set; }
        public DateTimeOffset RecordedAt { get; set; }
        public decimal Value { get; set; }
    }
}
