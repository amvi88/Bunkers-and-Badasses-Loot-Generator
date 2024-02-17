using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Builder;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrenadeModsController : ControllerBase
    {
        private readonly IGrenadeModService _grenadeModService;
        private readonly IGuildService _guildService;

        public GrenadeModsController(IGrenadeModService grenadeModService, IGuildService guildService)
        {
            _grenadeModService = grenadeModService ?? throw new ArgumentNullException(nameof(grenadeModService));
            _guildService = guildService ?? throw new ArgumentNullException(nameof(guildService));
        }

        [HttpPost]
        public IActionResult CreateGrenadeMod(GrenadeModFactoryParameters randomizerFactoryParameters)
        {
            if (randomizerFactoryParameters is null)
            {
                throw new ArgumentNullException(nameof(randomizerFactoryParameters));
            }

            var guilds = _guildService.GetGuildsThatProduce(Models.Common.ItemType.Grenade);

            if (!string.IsNullOrWhiteSpace(randomizerFactoryParameters.Guild) && !guilds.Any(g => randomizerFactoryParameters.Guild.Equals(g.Name) || randomizerFactoryParameters.Guild.Equals(g.AlternameName)))
            {
                return new BadRequestObjectResult($"The guild {randomizerFactoryParameters.Guild} does not know how to build this.");
            }

            return new OkObjectResult(_grenadeModService.RandomizeGrenadeMod(randomizerFactoryParameters).Item);
        }
    }
}
