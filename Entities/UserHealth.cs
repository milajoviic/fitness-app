using FitnessApp.Enums;

namespace FitnessApp.Entities
{
    public class UserHealth
    {
        public Guid UserId { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string? Notes { get; set; }
        public List<ChronicCondEnum>? ChronicCond { get; set; }
    }
}
