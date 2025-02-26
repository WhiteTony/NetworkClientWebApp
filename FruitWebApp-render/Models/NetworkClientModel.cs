using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FruitWebApp.Models;

public class NetworkClientModel
{
  [Key]
  public int id { get; set; }
  [Display(Name="Network Client Name")]
  public string? name { get; set; }
  [Display(Name="Network Client Ip Address")]
  public string? ip { get; set; }
  [Display(Name="Network Client Mac Address")]
  public string? mac { get; set; }
  [Display(Name="Active?")]
  public bool isActive { get; set; }	
}
