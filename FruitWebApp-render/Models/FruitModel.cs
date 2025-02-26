using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FruitWebApp.Models;

public class FruitModel
{
    [Key]
    public int id { get; set; }

    [Display(Name="Network Client Name")]
    public string? name { get; set; }
    [Display(Name ="Active")]
    public bool instock { get; set; }
}
