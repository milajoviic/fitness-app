namespace FitnessApp.Dto
{
    public class BodyMetricsDto
    {
        public record CreateBodyMetricsDto
        (
            Guid UserId,
            string BodyPart,
            decimal Value
        );
    }
}
