using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Models.Config;
using Models.Config;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PotionsController : ControllerBase
    {
        private readonly IPotionFinderService _potionFinderService;

        public PotionsController(IPotionFinderService potionFinderService)
        {
            _potionFinderService = potionFinderService ?? throw new ArgumentNullException(nameof(potionFinderService));
        }

        [HttpGet]
        public IActionResult FindPotion([FromQuery] string potionName = "")
        {
            var spec = string.IsNullOrWhiteSpace(potionName)
                ? new ChestPotionSpec { IsRandomPotion = true } 
                : new ChestPotionSpec { Name = potionName };

            var potion = _potionFinderService.GetPotion(spec);

            return potion == null
                ? new NotFoundObjectResult(potion)
                : new OkObjectResult(potion);
        }
    }
}
