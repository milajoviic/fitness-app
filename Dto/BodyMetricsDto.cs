namespace FitnessApp.Dto
{
    public class BodyMetricsDto
    {
        public record CreateBodyMetricsDto
        (
            string BodyPart,
            decimal Value
        );
    }
}
