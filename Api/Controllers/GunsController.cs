﻿using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Builder;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GunsController : ControllerBase
    {
        private readonly IGunService _gunService;
        private readonly IGuildService _guildService;

        public GunsController(IGunService gunService, IGuildService guildService)
        {
            _gunService = gunService ?? throw new ArgumentNullException(nameof(gunService));
            _guildService = guildService ?? throw new ArgumentNullException(nameof(guildService));
        }

        [HttpPost]
        public IActionResult CreateGun(GunRandomizerFactoryParameters randomizerFactoryParameters)
        {
            if (randomizerFactoryParameters is null)
            {
                throw new ArgumentNullException(nameof(randomizerFactoryParameters));
            }

            var guilds = randomizerFactoryParameters.GunType == null
                ? _guildService.GetGuildsThatProduce(Models.Common.ItemType.Gun)
                : _guildService.GetGuildsThatProduceGunType(randomizerFactoryParameters.GunType);

            if (!string.IsNullOrWhiteSpace(randomizerFactoryParameters.Guild) && !guilds.Any(g => randomizerFactoryParameters.Guild.Equals(g.Name) || randomizerFactoryParameters.Guild.Equals(g.AlternameName)))
            {
                return new BadRequestObjectResult($"The guild {randomizerFactoryParameters.Guild} does not know how to build this.");
            }

            var guns = Enumerable.Range(0, randomizerFactoryParameters.BatchSize).Select(i => _gunService.RandomizeGun(randomizerFactoryParameters).Item);
            return new OkObjectResult(guns);
        }
    }
}
