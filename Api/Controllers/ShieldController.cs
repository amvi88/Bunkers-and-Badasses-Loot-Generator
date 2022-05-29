using Business.Factories;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    [ApiController] 
    [Route("[controller]")]
    public class ShieldController : ControllerBase
    {
        private readonly IItemFactory<Shield> _shieldFactory;

        public ShieldController(IItemFactory<Shield> shieldFactory)
        {
            _shieldFactory = shieldFactory ?? throw new ArgumentNullException(nameof(shieldFactory));
        }

        [HttpPost]
        public IActionResult Post(int playerLevel)
        {
            var shield = _shieldFactory.Manufacture(playerLevel, null);
            return Ok(shield);
        }
    }
}