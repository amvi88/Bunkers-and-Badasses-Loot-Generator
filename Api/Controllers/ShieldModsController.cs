using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Builder;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShieldModsController : ControllerBase
    {
        private readonly IShieldService _shieldService;
        private readonly IShieldBatchService _shieldBatchService;
        private readonly IGuildService _guildService;

        public ShieldModsController(IShieldService shieldService, IGuildService guildService, IShieldBatchService shieldBatchService)
        {
            _shieldService = shieldService ?? throw new ArgumentNullException(nameof(shieldService));
            _guildService = guildService ?? throw new ArgumentNullException(nameof(guildService));
            _shieldBatchService = shieldBatchService ?? throw new ArgumentNullException(nameof(shieldBatchService));
        }

        [HttpPost]
        public IActionResult CreateShieldMod(ShieldFactoryParameters randomizerFactoryParameters)
        {
            if (randomizerFactoryParameters is null)
            {
                throw new ArgumentNullException(nameof(randomizerFactoryParameters));
            }

            var guilds = _guildService.GetGuildsThatProduce(Models.Common.ItemType.Shield);

            if (!string.IsNullOrWhiteSpace(randomizerFactoryParameters.Guild) && !guilds.Any(g => randomizerFactoryParameters.Guild.Equals(g.Name) || randomizerFactoryParameters.Guild.Equals(g.AlternameName)))
            {
                return new BadRequestObjectResult($"The guild {randomizerFactoryParameters.Guild} does not know how to build this.");
            }

            return new OkObjectResult(_shieldService.RandomizeShield(randomizerFactoryParameters).Item);
        }

        [HttpPost("batch")]
        public IActionResult CreateShieldMod(ShieldBatchServiceParameters randomizerFactoryParameters)
        {
            if (randomizerFactoryParameters is null)
            {
                throw new ArgumentNullException(nameof(randomizerFactoryParameters));
            }
           
            return new OkObjectResult(_shieldBatchService.GenerateBatch(randomizerFactoryParameters));
        }
    }
}
