using Microsoft.AspNetCore.Mvc;
using FitnessApp.Dto;
using FitnessApp.Entities;
using FitnessApp.DataProvider;

namespace FitnessApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BodyMetricsController : ControllerBase
    {
        private readonly BodyMetricsDP _metrics;
        private readonly UserDP _users;

        public BodyMetricsController(BodyMetricsDP metrics, UserDP users)
        {
            _metrics = metrics;
            _users = users;
        }

        [HttpPost]
        public async Task<ActionResult<UserBodyMetrics>> Create(BodyMetricsDto.CreateBodyMetricsDto dto)
        {
            var userValidation = await _users.GetById(dto.UserId);
            if (userValidation == null)
                return NotFound("Nepostojeci id korisnika");
            var metrics = new UserBodyMetrics
            {
                UserId = dto.UserId,
                BodyPart = dto.BodyPart,
                Value = dto.Value
            };
            await _metrics.InsertAsync(metrics);
            return Ok(new { metrics.UserId, metrics.BodyPart, metrics.MetricId });
        }
        [HttpGet("{userId:guid}/{bodyPart}")]
        public async Task<ActionResult<List<UserBodyMetrics>>> GetMetricsForBodyPart(Guid userId, string bodyPart)
        {
            var result = await _metrics.GetBodyMetrics(userId, bodyPart);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid userId, string bodyPart, Guid metricId)
        {
            await _metrics.DeleteAsync(userId, bodyPart, metricId);
            return NoContent();
        }
    }
}
