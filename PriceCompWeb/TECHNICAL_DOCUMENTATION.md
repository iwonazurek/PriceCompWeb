# Dokumentacja techniczna

## Cel projektu

Celem projektu jest stworzenie responsywnej aplikacji webowej z warstwa prezentacji oraz panelem administracyjnym. Aplikacja pomaga porownywac ceny produktow w roznych sklepach, liczy cene jednostkowa oraz wskazuje najtanszy sklep dla listy zakupow.

## Architektura

Projekt wykorzystuje wzorzec MVC:

- Model: klasy `Product`, `Store`, `Offer`, `PageContent`.
- View: widoki Razor w katalogu `Views`.
- Controller: `HomeController`, `AccountController`, `AdminController`.

Dodatkowa logika biznesowa znajduje sie w serwisie `ShoppingBasketService`. Warstwa dostepu do danych jest oparta o `PriceCompDbContext` i Entity Framework Core.

## Backend

Aplikacja dziala na ASP.NET Core MVC 8. Panel administracyjny jest zabezpieczony mechanizmem Cookie Authentication. Dane admina sa przechowywane w konfiguracji `appsettings.json`.

Najwazniejsze kontrolery:

- `HomeController` - widok publiczny ofert, wyszukiwarka i kalkulator koszyka.
- `AccountController` - logowanie i wylogowanie admina.
- `AdminController` - panel administracyjny oraz CRUD.

## Frontend

Frontend zostal zbudowany w Razor + Bootstrap 5 + wlasny CSS. Interfejs jest responsywny i korzysta z:

- CSS Grid w panelu administracyjnym,
- Flexbox w naglowkach i ukladach akcji,
- media queries dla widoku mobilnego,
- JavaScript do potwierdzania usuwania i wstawiania przykladowej listy zakupow.

## Baza danych

Baza danych: domyslnie SQLite w pliku `App_Data/pricecomp.db`. Projekt ma tez przygotowany connection string do SQL Server LocalDB i moze zostac przelaczony przez ustawienie `DatabaseProvider` w `appsettings.json`.

Kontekst: `PriceCompDbContext`.

Tabele:

### Products

Przechowuje produkty.

- `Id`
- `Name`
- `Category`
- `Quantity`
- `UnitName`
- `Description`
- `ImageUrl`

### Stores

Przechowuje sklepy.

- `Id`
- `Name`
- `Type`
- `City`
- `Address`
- `WebsiteUrl`
- `DeliveryCost`

### Offers

Laczy produkt ze sklepem i przechowuje cene.

- `Id`
- `ProductId`
- `StoreId`
- `Price`
- `PromoPrice`
- `PromoDescription`
- `IsAvailable`
- `UpdatedAt`

Relacje:

- `Product` 1:N `Offer`
- `Store` 1:N `Offer`

### PageContents

Przechowuje edytowalne tresci strony.

- `Id`
- `Key`
- `Title`
- `Body`

## Logika koszyka

Koszyk ma dwa etapy. Najpierw uzytkownik wpisuje ogolne nazwy produktow, np. `mleko` albo `chleb`. Aplikacja wyszukuje pasujace produkty i jezeli dla jednej pozycji istnieje wiecej niz jedna mozliwosc, pokazuje formularz doprecyzowania. Uzytkownik wybiera wtedy konkretny produkt, np. `Mleko UHT 2% 1 l` zamiast ogolnego hasla `mleko`.

Po wyborze konkretnych produktow `ShoppingBasketService` sprawdza, czy sklepy maja wybrane produkty w ofercie. Dla kazdego sklepu wybierana jest najtansza oferta danego produktu. Wynik zawiera:

- sklep,
- koszt produktow,
- koszt dostawy,
- cene laczna,
- liste brakujacych produktow,
- wybrane oferty.

Dzieki temu koszyk nie wybiera losowo lub automatycznie produktu z ogolnego zapytania. Uzytkownik sam decyduje, ktory wariant produktu chce porownac.

## Wyszukiwanie

Wyszukiwanie korzysta z klasy `ProductSearch`. Zapytanie jest normalizowane: aplikacja ignoruje wielkosc liter, usuwa polskie znaki i rozbija tekst na tokeny. Dopasowanie obejmuje nazwe produktu, kategorie, opis, nazwe sklepu i opis promocji. Prosta logika fuzzy search pozwala znalezc produkt nawet wtedy, gdy uzytkownik wpisze fragment nazwy, np. `mlek` zamiast pelnej nazwy produktu.

## Seeder danych

`DatabaseSeeder` uruchamia sie przy starcie aplikacji. Metoda `EnsureCreatedAsync()` tworzy baze, jezeli jej nie ma. Seeder dodaje przykladowe produkty, sklepy, oferty i tresci strony. Dziala idempotentnie, czyli dopisuje brakujace dane demonstracyjne bez usuwania danych dodanych pozniej z panelu admina.

## Bezpieczenstwo

Panel admina wymaga zalogowania. Akcje modyfikujace dane sa zabezpieczone przez `[ValidateAntiForgeryToken]`. Haslo admina w tej wersji jest haslem demonstracyjnym w konfiguracji, co wystarcza do projektu studenckiego, ale w aplikacji produkcyjnej nalezaloby uzyc ASP.NET Core Identity i hashowania hasel.

## Najwazniejsze funkcjonalnosci

- wyszukiwanie produktow,
- sortowanie ofert wedlug ceny jednostkowej,
- obsluga promocji,
- kalkulator koszyka z doprecyzowaniem produktu,
- CRUD produktow,
- CRUD sklepow,
- CRUD ofert,
- CRUD tresci strony,
- responsywna nawigacja.
