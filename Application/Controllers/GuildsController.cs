using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Common;

namespace Application.Controllers
{
    [Controller]
    [AllowAnonymous]
    [Route("api/[controller]")]

    public class GuildsController: ControllerBase
    {
        private readonly IGuildService _service;

        public GuildsController(IGuildService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGuilds()
        {
            var guilds = _service.GetGuilds();
            return Ok(guilds);
        }

        [HttpGet("itemtype/{itemType}")]
        public async Task<IActionResult> GetGuildsThatProduce(ItemType itemType)
        {
            var guilds = _service.GetGuildsThatProduce(itemType);
            return Ok(guilds);
        }

        [HttpGet("gunType/{gunType}")]
        public async Task<IActionResult> GetGuildsThatProduceGun(GunType? gunType)
        {
            var guilds = _service.GetGuildsThatProduceGunType(gunType);
            return Ok(guilds);
        }
    }
}