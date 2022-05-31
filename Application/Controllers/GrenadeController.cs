using Business.Factories;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api 
{
    [ApiController] 
    [Route("[controller]")]
    public class GrenadeController : ControllerBase
    {
        private readonly IItemFactory<Grenade> _grenadeFactory;

        public GrenadeController(IItemFactory<Grenade> grenadeFactory)
        {
            _grenadeFactory = grenadeFactory ?? throw new ArgumentNullException(nameof(grenadeFactory));
        }

        [HttpPost]
        public async Task<IActionResult> Post(int playerLevel)
        {
            var grenade = _grenadeFactory.Manufacture(playerLevel, null);
            return Ok(grenade);
        }
    }

}