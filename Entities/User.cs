using System;
using System.Linq;
using System.Text;
using FitnessApp.Enums;

namespace FitnessApp.Entities
{
    public class User
    {
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Email { get; set; }
        private GenderEnum Gender { get; set; }
        private int Age { get; set; }
    }
}
