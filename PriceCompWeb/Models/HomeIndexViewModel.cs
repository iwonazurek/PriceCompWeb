namespace PriceCompWeb.Models;

public class HomeIndexViewModel
{
    public string? Query { get; set; }
    public PageContent? Hero { get; set; }
    public IReadOnlyList<Offer> Offers { get; set; } = Array.Empty<Offer>();
    public IReadOnlyList<Product> Products { get; set; } = Array.Empty<Product>();
    public IReadOnlyList<Store> Stores { get; set; } = Array.Empty<Store>();
}
