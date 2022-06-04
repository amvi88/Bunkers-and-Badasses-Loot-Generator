using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class ItemModelBuilder : PageModel
    {
        [Required]
        [Range(1, 30)]
        public int PlayerLevel { get; set; }  = 1;    
    }
}