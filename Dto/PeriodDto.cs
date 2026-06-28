namespace FitnessApp.Dto
{
    public class PeriodDto
    {
        public record PeriodInputDto
        (
            DateTime StartDate,
            DateTime? EndDate,
            string? Notes
        );
    }
}
