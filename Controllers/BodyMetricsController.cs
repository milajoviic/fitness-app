using Microsoft.AspNetCore.Mvc;
using FitnessApp.Dto;
using FitnessApp.Entities;
using FitnessApp.DataProvider;
using Microsoft.AspNetCore.Authorization;
using FitnessApp.Auth;

namespace FitnessApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BodyMetricsController : ControllerBase
    {
        private readonly BodyMetricsDP _metrics;

        public BodyMetricsController(BodyMetricsDP metrics)
        {
            _metrics = metrics;
        }

        [HttpPost]
        public async Task<ActionResult<UserBodyMetrics>> Create(BodyMetricsDto.CreateBodyMetricsDto dto)
        {
            Guid userId = User.GetUserId();

            var metrics = new UserBodyMetrics
            {
                UserId = userId,
                BodyPart = dto.BodyPart,
                Value = dto.Value
            };
            await _metrics.InsertAsync(metrics);
            return Ok(new { metrics.UserId, metrics.BodyPart, metrics.MetricId });
        }
        [HttpGet("{bodyPart}")]
        public async Task<ActionResult<List<UserBodyMetrics>>> GetMetricsForBodyPart(string bodyPart)
        {
            Guid userId = User.GetUserId();
            var result = await _metrics.GetBodyMetrics(userId, bodyPart);
            return Ok(result);
        }
        [HttpDelete("{bodyPart}/{metricId:guid}")]
        public async Task<IActionResult> Delete(string bodyPart, Guid metricId)
        {
            Guid userId = User.GetUserId();
            await _metrics.DeleteAsync(userId, bodyPart, metricId);
            return NoContent();
        }
    }
}
