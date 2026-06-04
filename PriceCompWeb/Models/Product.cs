using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCompWeb.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Podaj nazwe produktu.")]
    [StringLength(120)]
    [Display(Name = "Nazwa")]
    public string Name { get; set; } = string.Empty;

    [StringLength(80)]
    [Display(Name = "Kategoria")]
    public string Category { get; set; } = string.Empty;

    [Range(0.01, 10000)]
    [Column(TypeName = "decimal(10,3)")]
    [Display(Name = "Ilosc w opakowaniu")]
    public decimal Quantity { get; set; } = 1;

    [Required]
    [StringLength(20)]
    [Display(Name = "Jednostka")]
    public string UnitName { get; set; } = "szt";

    [StringLength(500)]
    [Display(Name = "Opis")]
    public string? Description { get; set; }

    [StringLength(400)]
    [Display(Name = "Adres obrazka")]
    public string? ImageUrl { get; set; }

    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
