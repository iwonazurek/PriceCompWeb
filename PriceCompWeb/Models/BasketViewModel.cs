namespace PriceCompWeb.Models;

public class BasketViewModel
{
    public string ItemsText { get; set; } = string.Empty;
    public IReadOnlyList<BasketResult> Results { get; set; } = Array.Empty<BasketResult>();
}

public class BasketResult
{
    public Store Store { get; set; } = new();
    public decimal ProductsTotal { get; set; }
    public decimal DeliveryCost { get; set; }
    public decimal Total => ProductsTotal + DeliveryCost;
    public List<string> MissingProducts { get; set; } = new();
    public List<Offer> SelectedOffers { get; set; } = new();
    public bool IsComplete => MissingProducts.Count == 0;
}
