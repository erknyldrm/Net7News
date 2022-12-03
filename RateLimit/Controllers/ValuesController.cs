using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace RateLimit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [EnableRateLimiting("Fixed")]
        [Route("fixed")]
        public IActionResult Fixed()
        {
            return Ok();
        }

        [EnableRateLimiting("Sliding")]
        [Route("sliding")]
        public IActionResult Sliding()
        {
            return Ok();
        }

        [EnableRateLimiting("Token")]
        [Route("token")]
        public IActionResult Token()
        {
            return Ok();
        }

        [EnableRateLimiting("ConCurrency")]
        [Route("conCurrency")]
        public async Task<IActionResult> ConCurrency()
        {
            await Task.Delay(20000);
            return Ok();
        }

        [EnableRateLimiting("CustomPolicy")]
        [Route("customPolicy")]
        public async Task<IActionResult> CustomPolicy()
        {
            await Task.Delay(20000);
            return Ok();
        }
    }
}
