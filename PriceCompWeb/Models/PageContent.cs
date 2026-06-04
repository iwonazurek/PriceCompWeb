using System.ComponentModel.DataAnnotations;

namespace PriceCompWeb.Models;

public class PageContent
{
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string Key { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    [Display(Name = "Tytul")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1200)]
    [Display(Name = "Tresc")]
    public string Body { get; set; } = string.Empty;
}
