using Microsoft.EntityFrameworkCore;
using PriceCompWeb.Data;
using PriceCompWeb.Models;

namespace PriceCompWeb.Services;

public class ShoppingBasketService
{
    private readonly PriceCompDbContext _db;

    public ShoppingBasketService(PriceCompDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<BasketResult>> CalculateAsync(IEnumerable<string> requestedProducts)
    {
        var items = requestedProducts
            .Select(item => item.Trim())
            .Where(item => item.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (items.Count == 0)
        {
            return Array.Empty<BasketResult>();
        }

        var offers = await _db.Offers
            .Include(offer => offer.Product)
            .Include(offer => offer.Store)
            .Where(offer => offer.IsAvailable)
            .ToListAsync();

        var stores = offers
            .Where(offer => offer.Store is not null)
            .Select(offer => offer.Store!)
            .DistinctBy(store => store.Id)
            .ToList();

        var results = new List<BasketResult>();

        foreach (var store in stores)
        {
            var result = new BasketResult
            {
                Store = store,
                DeliveryCost = store.DeliveryCost
            };

            foreach (var item in items)
            {
                var match = offers
                    .Where(offer => offer.StoreId == store.Id && offer.Product is not null)
                    .Where(offer => ProductSearch.Matches(offer, item))
                    .OrderByDescending(offer => ProductSearch.Score(offer, item))
                    .ThenBy(offer => offer.EffectivePrice)
                    .FirstOrDefault();

                if (match is null)
                {
                    result.MissingProducts.Add(item);
                    continue;
                }

                result.SelectedOffers.Add(match);
                result.ProductsTotal += match.EffectivePrice;
            }

            results.Add(result);
        }

        return results
            .OrderBy(result => result.IsComplete ? 0 : 1)
            .ThenBy(result => result.Total)
            .ToList();
    }

    public async Task<IReadOnlyList<BasketResult>> CalculateByProductIdsAsync(IEnumerable<int> productIds)
    {
        var ids = productIds
            .Where(id => id > 0)
            .Distinct()
            .ToList();

        if (ids.Count == 0)
        {
            return Array.Empty<BasketResult>();
        }

        var products = await _db.Products
            .Where(product => ids.Contains(product.Id))
            .ToDictionaryAsync(product => product.Id);

        var offers = await _db.Offers
            .Include(offer => offer.Product)
            .Include(offer => offer.Store)
            .Where(offer => offer.IsAvailable && ids.Contains(offer.ProductId))
            .ToListAsync();

        var stores = offers
            .Where(offer => offer.Store is not null)
            .Select(offer => offer.Store!)
            .DistinctBy(store => store.Id)
            .ToList();

        var results = new List<BasketResult>();

        foreach (var store in stores)
        {
            var result = new BasketResult
            {
                Store = store,
                DeliveryCost = store.DeliveryCost
            };

            foreach (var productId in ids)
            {
                var match = offers
                    .Where(offer => offer.StoreId == store.Id && offer.ProductId == productId)
                    .OrderBy(offer => offer.EffectivePrice)
                    .FirstOrDefault();

                if (match is null)
                {
                    result.MissingProducts.Add(products.TryGetValue(productId, out var product) ? product.Name : $"Produkt #{productId}");
                    continue;
                }

                result.SelectedOffers.Add(match);
                result.ProductsTotal += match.EffectivePrice;
            }

            results.Add(result);
        }

        return results
            .OrderBy(result => result.IsComplete ? 0 : 1)
            .ThenBy(result => result.Total)
            .ToList();
    }
}
