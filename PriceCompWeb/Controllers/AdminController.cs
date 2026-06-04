using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PriceCompWeb.Data;
using PriceCompWeb.Models;

namespace PriceCompWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly PriceCompDbContext _db;

    public AdminController(PriceCompDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.ProductsCount = await _db.Products.CountAsync();
        ViewBag.StoresCount = await _db.Stores.CountAsync();
        ViewBag.OffersCount = await _db.Offers.CountAsync();
        ViewBag.ContentsCount = await _db.PageContents.CountAsync();
        return View();
    }

    public async Task<IActionResult> Products()
    {
        return View(await _db.Products.OrderBy(product => product.Name).ToListAsync());
    }

    [HttpGet]
    public IActionResult CreateProduct()
    {
        return View("ProductForm", new Product());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        if (!ModelState.IsValid)
        {
            return View("ProductForm", product);
        }

        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Produkt zostal dodany.";
        return RedirectToAction(nameof(Products));
    }

    [HttpGet]
    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);
        return product is null ? NotFound() : View("ProductForm", product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(Product product)
    {
        if (!ModelState.IsValid)
        {
            return View("ProductForm", product);
        }

        _db.Products.Update(product);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Produkt zostal zaktualizowany.";
        return RedirectToAction(nameof(Products));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is not null)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Products));
    }

    public async Task<IActionResult> Stores()
    {
        return View(await _db.Stores.OrderBy(store => store.Name).ToListAsync());
    }

    [HttpGet]
    public IActionResult CreateStore()
    {
        return View("StoreForm", new Store());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStore(Store store)
    {
        if (!ModelState.IsValid)
        {
            return View("StoreForm", store);
        }

        _db.Stores.Add(store);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Sklep zostal dodany.";
        return RedirectToAction(nameof(Stores));
    }

    [HttpGet]
    public async Task<IActionResult> EditStore(int id)
    {
        var store = await _db.Stores.FindAsync(id);
        return store is null ? NotFound() : View("StoreForm", store);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditStore(Store store)
    {
        if (!ModelState.IsValid)
        {
            return View("StoreForm", store);
        }

        _db.Stores.Update(store);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Sklep zostal zaktualizowany.";
        return RedirectToAction(nameof(Stores));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteStore(int id)
    {
        var store = await _db.Stores.FindAsync(id);
        if (store is not null)
        {
            _db.Stores.Remove(store);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Stores));
    }

    public async Task<IActionResult> Offers()
    {
        var offers = await _db.Offers
            .Include(offer => offer.Product)
            .Include(offer => offer.Store)
            .OrderBy(offer => offer.Product!.Name)
            .ThenBy(offer => offer.Store!.Name)
            .ToListAsync();

        return View(offers);
    }

    [HttpGet]
    public async Task<IActionResult> CreateOffer()
    {
        await LoadOfferListsAsync();
        return View("OfferForm", new Offer());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOffer(Offer offer)
    {
        if (!ModelState.IsValid)
        {
            await LoadOfferListsAsync();
            return View("OfferForm", offer);
        }

        offer.UpdatedAt = DateTime.UtcNow;
        _db.Offers.Add(offer);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Oferta zostala dodana.";
        return RedirectToAction(nameof(Offers));
    }

    [HttpGet]
    public async Task<IActionResult> EditOffer(int id)
    {
        var offer = await _db.Offers.FindAsync(id);
        if (offer is null)
        {
            return NotFound();
        }

        await LoadOfferListsAsync();
        return View("OfferForm", offer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditOffer(Offer offer)
    {
        if (!ModelState.IsValid)
        {
            await LoadOfferListsAsync();
            return View("OfferForm", offer);
        }

        offer.UpdatedAt = DateTime.UtcNow;
        _db.Offers.Update(offer);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Oferta zostala zaktualizowana.";
        return RedirectToAction(nameof(Offers));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteOffer(int id)
    {
        var offer = await _db.Offers.FindAsync(id);
        if (offer is not null)
        {
            _db.Offers.Remove(offer);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Offers));
    }

    public async Task<IActionResult> Contents()
    {
        return View(await _db.PageContents.OrderBy(content => content.Key).ToListAsync());
    }

    [HttpGet]
    public IActionResult CreateContent()
    {
        return View("ContentForm", new PageContent());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateContent(PageContent content)
    {
        if (!ModelState.IsValid)
        {
            return View("ContentForm", content);
        }

        _db.PageContents.Add(content);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Tresc zostala dodana.";
        return RedirectToAction(nameof(Contents));
    }

    [HttpGet]
    public async Task<IActionResult> EditContent(int id)
    {
        var content = await _db.PageContents.FindAsync(id);
        return content is null ? NotFound() : View("ContentForm", content);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditContent(PageContent content)
    {
        if (!ModelState.IsValid)
        {
            return View("ContentForm", content);
        }

        _db.PageContents.Update(content);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Tresc zostala zaktualizowana.";
        return RedirectToAction(nameof(Contents));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteContent(int id)
    {
        var content = await _db.PageContents.FindAsync(id);
        if (content is not null)
        {
            _db.PageContents.Remove(content);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Contents));
    }

    private async Task LoadOfferListsAsync()
    {
        ViewBag.Products = new SelectList(await _db.Products.OrderBy(product => product.Name).ToListAsync(), "Id", "Name");
        ViewBag.Stores = new SelectList(await _db.Stores.OrderBy(store => store.Name).ToListAsync(), "Id", "Name");
    }
}
