using Business.Factories;
using Business.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    [ApiController] 
    [Route("[controller]")]
    public class GunController : ControllerBase
    {
        private readonly IGunFactory _gunFactory;

        public GunController(IGunFactory gunFactory)
        {
            _gunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRandom(int playerLevel, Rarity? rarity)
        {
            var gun = _gunFactory.Manufacture(playerLevel, rarity);
            return Ok(gun);
        }

        [HttpPost("{gunType}")]
        public async Task<IActionResult> CreateTyped(int playerLevel, [FromRoute]  GunType gunType, Rarity? rarity)
        {
            var gun = _gunFactory.Manufacture(playerLevel, rarity, gunType);
            return Ok(gun);
        }
    }

}