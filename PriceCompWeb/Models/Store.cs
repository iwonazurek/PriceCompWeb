using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCompWeb.Models;

public enum StoreType
{
    Local = 1,
    Online = 2
}

public class Store
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Podaj nazwe sklepu.")]
    [StringLength(120)]
    [Display(Name = "Nazwa")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Typ sklepu")]
    public StoreType Type { get; set; } = StoreType.Local;

    [StringLength(120)]
    [Display(Name = "Miasto")]
    public string? City { get; set; }

    [StringLength(200)]
    [Display(Name = "Adres")]
    public string? Address { get; set; }

    [StringLength(300)]
    [Display(Name = "Strona WWW")]
    public string? WebsiteUrl { get; set; }

    [Range(0, 999)]
    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Koszt dostawy")]
    public decimal DeliveryCost { get; set; }

    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
