using Business.Models.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{

    [IgnoreAntiforgeryToken]
    public class GunBuilderModel : ItemModelBuilder
    {
        public Rarity? Rarity { get; set; }
        public GunType GunType { get; set; }           
    }
}