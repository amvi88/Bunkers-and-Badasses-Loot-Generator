using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Builder;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelicController : ControllerBase
    {
        private readonly IRelicService _relicService;

        public RelicController(IRelicService relicService)
        {
            _relicService = relicService ?? throw new ArgumentNullException(nameof(relicService));
        }

        [HttpPost]
        public IActionResult FindRelic(RelicRandomizerFactoryParameters relicRandomizerFactoryParameters)
        {
           return new OkObjectResult(_relicService.RandomizeRelic(relicRandomizerFactoryParameters).Item);
        }
    }
}
