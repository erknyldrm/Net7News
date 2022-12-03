using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Net7OutputCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [OutputCache]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        //[OutputCache]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
