using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PriceCompWeb.Data;
using Microsoft.AspNetCore.Mvc;
using PriceCompWeb.Models;
using PriceCompWeb.Services;

namespace PriceCompWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PriceCompDbContext _db;
    private readonly ShoppingBasketService _basketService;

    public HomeController(ILogger<HomeController> logger, PriceCompDbContext db, ShoppingBasketService basketService)
    {
        _logger = logger;
        _db = db;
        _basketService = basketService;
    }

    public async Task<IActionResult> Index(string? query)
    {
        var offers = await _db.Offers
            .Include(offer => offer.Product)
            .Include(offer => offer.Store)
            .Where(offer => offer.IsAvailable)
            .OrderBy(offer => offer.Product!.Name)
            .ToListAsync();

        if (!string.IsNullOrWhiteSpace(query))
        {
            offers = offers
                .Where(offer => ProductSearch.Matches(offer, query))
                .OrderByDescending(offer => ProductSearch.Score(offer, query))
                .ThenBy(offer => offer.UnitPrice)
                .ToList();
        }

        var model = new HomeIndexViewModel
        {
            Query = query,
            Hero = await _db.PageContents.FirstOrDefaultAsync(content => content.Key == "home.hero"),
            Products = await _db.Products.OrderBy(product => product.Name).ToListAsync(),
            Stores = await _db.Stores.OrderBy(store => store.Name).ToListAsync(),
            Offers = offers.OrderBy(offer => offer.UnitPrice).ToList()
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Basket()
    {
        return View(new BasketViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Basket(BasketViewModel model)
    {
        var selectedProductIds = model.SelectedProductIds
            .Where(id => id > 0)
            .Distinct()
            .ToList();

        if (selectedProductIds.Count > 0)
        {
            model.SelectedProductIds = selectedProductIds;
            model.Results = await _basketService.CalculateByProductIdsAsync(selectedProductIds);
            return View(model);
        }

        var items = model.ItemsText
            .Split(new[] { "\r\n", "\n", "," }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (items.Count == 0)
        {
            ModelState.AddModelError(nameof(model.ItemsText), "Wpisz przynajmniej jeden produkt.");
            return View(model);
        }

        model.SelectionItems = await BuildBasketSelectionAsync(items);

        if (model.SelectionItems.Any(item => item.Options.Count != 1))
        {
            return View(model);
        }

        model.SelectedProductIds = model.SelectionItems
            .Select(item => item.Options[0].Id)
            .ToList();

        model.Results = await _basketService.CalculateByProductIdsAsync(model.SelectedProductIds);
        model.SelectionItems = Array.Empty<BasketSelectionItem>();
        return View(model);
    }

    private async Task<IReadOnlyList<BasketSelectionItem>> BuildBasketSelectionAsync(IReadOnlyList<string> items)
    {
        var offers = await _db.Offers
            .Include(offer => offer.Product)
            .Include(offer => offer.Store)
            .Where(offer => offer.IsAvailable && offer.Product != null)
            .ToListAsync();

        return items
            .Select(item => new BasketSelectionItem
            {
                Query = item,
                Options = offers
                    .Where(offer => ProductSearch.Matches(offer, item))
                    .GroupBy(offer => offer.ProductId)
                    .Select(group =>
                    {
                        var offer = group.First();
                        return new
                        {
                            Product = offer.Product!,
                            Score = group.Max(candidate => ProductSearch.Score(candidate, item))
                        };
                    })
                    .OrderByDescending(match => match.Score)
                    .ThenBy(match => match.Product.Name)
                    .Take(8)
                    .Select(match => new BasketProductOption
                    {
                        Id = match.Product.Id,
                        Name = match.Product.Name,
                        Category = match.Product.Category,
                        Quantity = match.Product.Quantity,
                        UnitName = match.Product.UnitName
                    })
                    .ToList()
            })
            .ToList();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
