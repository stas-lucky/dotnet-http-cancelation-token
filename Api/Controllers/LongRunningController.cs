using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LongRunningController : ControllerBase
    {
        private readonly ILogger<LongRunningController> _logger;

        public LongRunningController(ILogger<LongRunningController> logger)
        {
            _logger = logger;
        }

        [HttpGet("process")]
        public async Task<IActionResult> Process(CancellationToken ct)
        {
            _logger.LogInformation("Processing started.");
            try
            {
                for (int i = 0; i < 30; i++)
                {
                    // Simulate work
                    await Task.Delay(1000, ct);
                    _logger.LogInformation("Processed step {Step}", i + 1);
                }
                return Ok("Processing completed successfully.");
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Processing was cancelled by the client.");
                return StatusCode(499, "Client Closed Request"); // 499 Client Closed Request
            }
        }
    }
}
