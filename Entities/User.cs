using FitnessApp.Enums;

namespace FitnessApp.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public GenderEnum Gender { get; set; }
        public string PasswordHash { get; set; } = "";
        public DateTime BirthDate { get; set; }
    }
}
