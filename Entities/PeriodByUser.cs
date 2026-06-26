namespace FitnessApp.Entities
{
    public class PeriodByUser
    {
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Notes { get; set; }
    }
}
