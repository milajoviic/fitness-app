using FitnessApp.Enums;
namespace FitnessApp.Entities
{
    public class UserDiet
    {
        public Guid UserId { get; set; }
        public string Breakfast { get; set; }
        public string Lunch { get; set; }
        public string Dinner { get; set; }
        public List<string>? Snacks { get; set; }
        public List<SupplementsEnum>? Supplements { get; set; } 
        public DateTime LogDay { get; set; }
    }
}
