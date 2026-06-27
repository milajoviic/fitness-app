namespace FitnessApp.Dto
{
    public class UserDto
    {
        public record RegisterUserDto 
        (
            string Email,
            string Password,
            string FirstName,
            string LastName,
            string Gender,
            DateTime BirthDate
        );

        public record LoginDto
        (
            string Email,
            string Password
        );

        public record UpdateUserDto
        (
            string FirstName,
            string LastName,
            string Gender,
            DateTime BirthDate
        );
    }
}
