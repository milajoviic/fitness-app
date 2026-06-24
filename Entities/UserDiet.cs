namespace FitnessApp.Entities
{
    public class UserDiet
    {
        public string Breakfast { get; set; }
        public string Lunch { get; set; }
        public string Dinner { get; set; }
        public List<string>? Snacks { get; set; }
        public List<string>? Additional { get; set; } //moze da se napravi i kao enumeracija sa navedenim dodacima
        public DateTime Day { get; set; }
        public int? Calories { get; set; }
    }
}
