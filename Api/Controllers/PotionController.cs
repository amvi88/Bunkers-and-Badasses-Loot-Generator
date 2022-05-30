using Business.Factories;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api 
{
    [ApiController] 
    [Route("[controller]")]
    public class PotionController : ControllerBase
    {
        private readonly IPotionFactory _potionFactory;

        public PotionController(IPotionFactory potionFactory)
        {
            _potionFactory = potionFactory ?? throw new ArgumentNullException(nameof(potionFactory));
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var potion = _potionFactory.Brew();
            return Ok(potion);
        }
    }

}