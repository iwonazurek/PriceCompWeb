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
        var items = model.ItemsText
            .Split(new[] { "\r\n", "\n", "," }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        model.Results = await _basketService.CalculateAsync(items);
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
