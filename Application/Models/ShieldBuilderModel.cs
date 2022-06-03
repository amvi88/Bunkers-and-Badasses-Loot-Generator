using Business.Models.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{

    [IgnoreAntiforgeryToken]
    public class ShieldBuilderModel : PageModel
    {
        [Required]
        [Range(1, 30)]
        public int PlayerLevel { get; set; }  = 1;    
    }
}