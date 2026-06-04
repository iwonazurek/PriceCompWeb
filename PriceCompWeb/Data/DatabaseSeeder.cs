using Microsoft.EntityFrameworkCore;
using PriceCompWeb.Models;

namespace PriceCompWeb.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(PriceCompDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (!await db.PageContents.AnyAsync())
        {
            db.PageContents.AddRange(
                new PageContent
                {
                    Key = "home.hero",
                    Title = "Asystent Zakupowy",
                    Body = "Porownuj ceny produktow, sprawdzaj promocje i zobacz, w ktorym sklepie najtaniej zrobisz caly koszyk."
                },
                new PageContent
                {
                    Key = "home.about",
                    Title = "Jak to dziala?",
                    Body = "Aplikacja porownuje ceny jednostkowe, uwzglednia promocje oraz koszty dostawy. Administrator zarzadza produktami, sklepami, ofertami i tekstami widocznymi na stronie."
                });
        }

        var products = await db.Products.ToDictionaryAsync(product => product.Name, StringComparer.OrdinalIgnoreCase);
        var stores = await db.Stores.ToDictionaryAsync(store => store.Name, StringComparer.OrdinalIgnoreCase);

        AddProduct(new Product { Name = "Mleko UHT 2% 1 l", Category = "Nabial", Quantity = 1, UnitName = "l", Description = "Mleko UHT w kartonie." });
        AddProduct(new Product { Name = "Mleko swieze 3.2% 1 l", Category = "Nabial", Quantity = 1, UnitName = "l", Description = "Mleko swieze w butelce." });
        AddProduct(new Product { Name = "Napoj mleczny czekoladowy 400 ml", Category = "Nabial", Quantity = 0.4m, UnitName = "l", Description = "Slodki napoj na bazie mleka." });
        AddProduct(new Product { Name = "Chleb pszenny 500 g", Category = "Pieczywo", Quantity = 0.5m, UnitName = "kg", Description = "Klasyczny chleb krojony." });
        AddProduct(new Product { Name = "Chleb zytni 600 g", Category = "Pieczywo", Quantity = 0.6m, UnitName = "kg", Description = "Chleb zytni na zakwasie." });
        AddProduct(new Product { Name = "Bulki kajzerki 6 szt", Category = "Pieczywo", Quantity = 6, UnitName = "szt", Description = "Pakiet bulek sniadaniowych." });
        AddProduct(new Product { Name = "Maslo ekstra 200 g", Category = "Nabial", Quantity = 0.2m, UnitName = "kg", Description = "Maslo 82% tluszczu." });
        AddProduct(new Product { Name = "Ser zolty gouda 150 g", Category = "Nabial", Quantity = 0.15m, UnitName = "kg", Description = "Ser zolty w plastrach." });
        AddProduct(new Product { Name = "Jajka M 10 szt", Category = "Nabial", Quantity = 10, UnitName = "szt", Description = "Jajka kurze rozmiar M." });
        AddProduct(new Product { Name = "Jogurt naturalny 400 g", Category = "Nabial", Quantity = 0.4m, UnitName = "kg", Description = "Jogurt naturalny bez cukru." });
        AddProduct(new Product { Name = "Cola 1.5 l", Category = "Napoje", Quantity = 1.5m, UnitName = "l", Description = "Napoj gazowany w butelce." });
        AddProduct(new Product { Name = "Woda mineralna 1.5 l", Category = "Napoje", Quantity = 1.5m, UnitName = "l", Description = "Woda niegazowana." });
        AddProduct(new Product { Name = "Sok jablkowy 1 l", Category = "Napoje", Quantity = 1, UnitName = "l", Description = "Sok owocowy 100%." });
        AddProduct(new Product { Name = "Ryz bialy 1 kg", Category = "Sypkie", Quantity = 1, UnitName = "kg", Description = "Ryz dlugoziarnisty." });
        AddProduct(new Product { Name = "Makaron spaghetti 500 g", Category = "Sypkie", Quantity = 0.5m, UnitName = "kg", Description = "Makaron pszenny." });
        AddProduct(new Product { Name = "Platki owsiane 500 g", Category = "Sypkie", Quantity = 0.5m, UnitName = "kg", Description = "Platki sniadaniowe owsiane." });
        AddProduct(new Product { Name = "Kawa mielona 250 g", Category = "Spozywcze", Quantity = 0.25m, UnitName = "kg", Description = "Kawa mielona do parzenia." });
        AddProduct(new Product { Name = "Herbata czarna 100 torebek", Category = "Spozywcze", Quantity = 100, UnitName = "szt", Description = "Herbata ekspresowa." });
        AddProduct(new Product { Name = "Cukier bialy 1 kg", Category = "Spozywcze", Quantity = 1, UnitName = "kg", Description = "Cukier krysztal." });
        AddProduct(new Product { Name = "Olej rzepakowy 1 l", Category = "Spozywcze", Quantity = 1, UnitName = "l", Description = "Olej do smazenia i gotowania." });
        AddProduct(new Product { Name = "Pomidory 1 kg", Category = "Warzywa", Quantity = 1, UnitName = "kg", Description = "Pomidory luzem." });
        AddProduct(new Product { Name = "Banany 1 kg", Category = "Owoce", Quantity = 1, UnitName = "kg", Description = "Banany luzem." });
        AddProduct(new Product { Name = "Jablka 1 kg", Category = "Owoce", Quantity = 1, UnitName = "kg", Description = "Jablka deserowe." });
        AddProduct(new Product { Name = "Filet z kurczaka 1 kg", Category = "Mieso", Quantity = 1, UnitName = "kg", Description = "Filet z piersi kurczaka." });
        AddProduct(new Product { Name = "Szynka konserwowa 150 g", Category = "Wedliny", Quantity = 0.15m, UnitName = "kg", Description = "Wedlina w plastrach." });
        AddProduct(new Product { Name = "Papier toaletowy 8 rolek", Category = "Dom", Quantity = 8, UnitName = "szt", Description = "Papier trzywarstwowy." });
        AddProduct(new Product { Name = "Plyn do naczyn 900 ml", Category = "Dom", Quantity = 0.9m, UnitName = "l", Description = "Plyn do mycia naczyn." });

        AddStore(new Store { Name = "Biedronka", Type = StoreType.Local, City = "Warszawa", Address = "ul. Przykladowa 10", DeliveryCost = 0 });
        AddStore(new Store { Name = "Lidl", Type = StoreType.Local, City = "Warszawa", Address = "ul. Zakupowa 4", DeliveryCost = 0 });
        AddStore(new Store { Name = "Carrefour", Type = StoreType.Local, City = "Warszawa", Address = "ul. Handlowa 7", DeliveryCost = 0 });
        AddStore(new Store { Name = "Auchan", Type = StoreType.Local, City = "Warszawa", Address = "ul. Parkowa 21", DeliveryCost = 0 });
        AddStore(new Store { Name = "EkoKoszyk Online", Type = StoreType.Online, WebsiteUrl = "https://example.com", DeliveryCost = 9.99m });
        AddStore(new Store { Name = "SzybkieZakupy.pl", Type = StoreType.Online, WebsiteUrl = "https://example.com", DeliveryCost = 12.99m });

        await db.SaveChangesAsync();

        var existingOffers = await db.Offers
            .Include(offer => offer.Product)
            .Include(offer => offer.Store)
            .Select(offer => offer.Product!.Name + "|" + offer.Store!.Name)
            .ToListAsync();

        var offerKeys = existingOffers.ToHashSet(StringComparer.OrdinalIgnoreCase);

        AddOffer("Mleko UHT 2% 1 l", "Biedronka", 3.29m, 2.99m, "Oferta tygodnia");
        AddOffer("Mleko UHT 2% 1 l", "Lidl", 3.19m);
        AddOffer("Mleko UHT 2% 1 l", "Carrefour", 3.49m);
        AddOffer("Mleko swieze 3.2% 1 l", "Biedronka", 3.79m);
        AddOffer("Mleko swieze 3.2% 1 l", "Lidl", 3.69m, 3.39m, "Promocja");
        AddOffer("Napoj mleczny czekoladowy 400 ml", "Biedronka", 2.99m);
        AddOffer("Napoj mleczny czekoladowy 400 ml", "Lidl", 3.19m);
        AddOffer("Chleb pszenny 500 g", "Biedronka", 3.99m);
        AddOffer("Chleb pszenny 500 g", "Lidl", 4.19m);
        AddOffer("Chleb zytni 600 g", "Biedronka", 5.49m);
        AddOffer("Chleb zytni 600 g", "Carrefour", 5.69m);
        AddOffer("Bulki kajzerki 6 szt", "Lidl", 3.49m);
        AddOffer("Bulki kajzerki 6 szt", "Biedronka", 3.29m);
        AddOffer("Maslo ekstra 200 g", "Biedronka", 7.49m, 6.99m, "Z karta sklepu");
        AddOffer("Maslo ekstra 200 g", "Lidl", 7.29m);
        AddOffer("Maslo ekstra 200 g", "Auchan", 7.79m);
        AddOffer("Ser zolty gouda 150 g", "Biedronka", 5.99m);
        AddOffer("Ser zolty gouda 150 g", "Lidl", 6.29m);
        AddOffer("Jajka M 10 szt", "Biedronka", 9.99m);
        AddOffer("Jajka M 10 szt", "Lidl", 10.49m);
        AddOffer("Jogurt naturalny 400 g", "Biedronka", 2.59m);
        AddOffer("Jogurt naturalny 400 g", "Carrefour", 2.79m);
        AddOffer("Cola 1.5 l", "Biedronka", 5.99m);
        AddOffer("Cola 1.5 l", "Lidl", 6.19m);
        AddOffer("Cola 1.5 l", "SzybkieZakupy.pl", 5.79m);
        AddOffer("Woda mineralna 1.5 l", "Biedronka", 1.69m);
        AddOffer("Woda mineralna 1.5 l", "Lidl", 1.59m);
        AddOffer("Sok jablkowy 1 l", "Carrefour", 4.49m);
        AddOffer("Sok jablkowy 1 l", "Biedronka", 4.29m);
        AddOffer("Ryz bialy 1 kg", "Biedronka", 4.99m);
        AddOffer("Ryz bialy 1 kg", "Lidl", 5.19m);
        AddOffer("Makaron spaghetti 500 g", "Biedronka", 3.49m);
        AddOffer("Makaron spaghetti 500 g", "Lidl", 3.39m);
        AddOffer("Platki owsiane 500 g", "Auchan", 4.29m);
        AddOffer("Platki owsiane 500 g", "Biedronka", 4.49m);
        AddOffer("Kawa mielona 250 g", "Biedronka", 13.99m);
        AddOffer("Kawa mielona 250 g", "Lidl", 14.49m, 12.99m, "Drugi produkt taniej");
        AddOffer("Herbata czarna 100 torebek", "Carrefour", 8.99m);
        AddOffer("Herbata czarna 100 torebek", "Auchan", 8.79m);
        AddOffer("Cukier bialy 1 kg", "Biedronka", 3.99m);
        AddOffer("Cukier bialy 1 kg", "Lidl", 4.09m);
        AddOffer("Olej rzepakowy 1 l", "Biedronka", 7.49m);
        AddOffer("Olej rzepakowy 1 l", "Lidl", 7.29m);
        AddOffer("Pomidory 1 kg", "Biedronka", 8.99m);
        AddOffer("Pomidory 1 kg", "Lidl", 8.49m);
        AddOffer("Banany 1 kg", "Biedronka", 5.99m);
        AddOffer("Banany 1 kg", "Lidl", 5.79m);
        AddOffer("Jablka 1 kg", "Carrefour", 4.49m);
        AddOffer("Jablka 1 kg", "Biedronka", 4.29m);
        AddOffer("Filet z kurczaka 1 kg", "Biedronka", 23.99m);
        AddOffer("Filet z kurczaka 1 kg", "Lidl", 24.49m);
        AddOffer("Szynka konserwowa 150 g", "Biedronka", 5.49m);
        AddOffer("Szynka konserwowa 150 g", "Lidl", 5.79m);
        AddOffer("Papier toaletowy 8 rolek", "Biedronka", 14.99m);
        AddOffer("Papier toaletowy 8 rolek", "Auchan", 13.99m);
        AddOffer("Plyn do naczyn 900 ml", "Biedronka", 5.99m);
        AddOffer("Plyn do naczyn 900 ml", "Lidl", 5.49m);

        await db.SaveChangesAsync();

        void AddProduct(Product product)
        {
            if (products.ContainsKey(product.Name))
            {
                return;
            }

            db.Products.Add(product);
            products[product.Name] = product;
        }

        void AddStore(Store store)
        {
            if (stores.ContainsKey(store.Name))
            {
                return;
            }

            db.Stores.Add(store);
            stores[store.Name] = store;
        }

        void AddOffer(string productName, string storeName, decimal price, decimal? promoPrice = null, string? promoDescription = null)
        {
            var key = productName + "|" + storeName;
            if (offerKeys.Contains(key))
            {
                return;
            }

            if (!products.TryGetValue(productName, out var product) || !stores.TryGetValue(storeName, out var store))
            {
                return;
            }

            db.Offers.Add(new Offer
            {
                ProductId = product.Id,
                StoreId = store.Id,
                Price = price,
                PromoPrice = promoPrice,
                PromoDescription = promoDescription,
                IsAvailable = true,
                UpdatedAt = DateTime.UtcNow
            });

            offerKeys.Add(key);
        }
    }
}
