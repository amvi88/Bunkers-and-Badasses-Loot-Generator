using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Business.Services;

namespace Api.Controllers 
{
    [Controller]
    [Route("[controller]")]
    [AllowAnonymous]
    public class GuildController: ControllerBase
    {
        private readonly IGuildService _guildService;

        public GuildController(IGuildService guildService)
        {
            _guildService = guildService ?? throw new ArgumentNullException(nameof(guildService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var guilds = _guildService.GetGuilds();
            if (guilds == null || !guilds.Any())
            {
                return NotFound();
            }

            return Ok(guilds);
        }

    }
}