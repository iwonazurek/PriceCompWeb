using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCompWeb.Models;

public class Offer
{
    public int Id { get; set; }

    [Display(Name = "Produkt")]
    public int ProductId { get; set; }

    public Product? Product { get; set; }

    [Display(Name = "Sklep")]
    public int StoreId { get; set; }

    public Store? Store { get; set; }

    [Range(0.01, 100000)]
    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Cena")]
    public decimal Price { get; set; }

    [Range(0.01, 100000)]
    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Cena promocyjna")]
    public decimal? PromoPrice { get; set; }

    [StringLength(200)]
    [Display(Name = "Opis promocji")]
    public string? PromoDescription { get; set; }

    [Display(Name = "Dostepna")]
    public bool IsAvailable { get; set; } = true;

    [Display(Name = "Ostatnia aktualizacja")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public decimal EffectivePrice => PromoPrice ?? Price;

    [NotMapped]
    public decimal UnitPrice => Product is null || Product.Quantity <= 0 ? EffectivePrice : EffectivePrice / Product.Quantity;
}
