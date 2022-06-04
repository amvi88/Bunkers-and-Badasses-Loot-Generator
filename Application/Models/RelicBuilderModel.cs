using Business.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application.Models
{
    [IgnoreAntiforgeryToken]
    public class RelicBuilderModel : PageModel
    {
        public Rarity? Rarity { get; set; }        
    }
}