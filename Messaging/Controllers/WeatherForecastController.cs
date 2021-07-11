using Messaging.Infrastructure.Consumers;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace Messaging.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IFileCollector _collector;

        public WeatherForecastController(IFileCollector collector)
        {
            _collector = collector;
        }

        [Route("api/setmessage")]
        [HttpGet]
        public async Task<IActionResult> SetAsync()
        {
            await _collector.Publish<IMessageInfo>(new MessageInfo()
            {
                Content = null,
                FileName = "FileName"
            });
            return Ok();
        }
    }
}
