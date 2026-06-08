namespace PriceCompWeb.Models;

public class BasketViewModel
{
    public string ItemsText { get; set; } = string.Empty;
    public List<int> SelectedProductIds { get; set; } = new();
    public IReadOnlyList<BasketSelectionItem> SelectionItems { get; set; } = Array.Empty<BasketSelectionItem>();
    public IReadOnlyList<BasketResult> Results { get; set; } = Array.Empty<BasketResult>();
    public bool NeedsSelection => SelectionItems.Any();
    public bool HasMissingSelections => SelectionItems.Any(item => item.Options.Count == 0);
}

public class BasketSelectionItem
{
    public string Query { get; set; } = string.Empty;
    public IReadOnlyList<BasketProductOption> Options { get; set; } = Array.Empty<BasketProductOption>();
}

public class BasketProductOption
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string UnitName { get; set; } = string.Empty;
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
