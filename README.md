# Asystent Zakupowy

Asystent Zakupowy to responsywna aplikacja webowa ASP.NET Core MVC do porownywania cen produktow w roznych sklepach. System pokazuje ceny jednostkowe, promocje oraz kalkuluje, w ktorym sklepie najtaniej kupic cala liste zakupow.

## Funkcje

- publiczna lista ofert z wyszukiwarka produktow,
- wyszukiwanie po fragmentach nazw, opisach, kategoriach i nazwach sklepow,
- porownywanie ceny jednostkowej, np. zl/kg lub zl/l,
- kalkulator koszyka z wyborem konkretnego produktu i rankingiem sklepow,
- panel administracyjny zabezpieczony logowaniem,
- CRUD produktow, sklepow, ofert i tresci strony,
- seed danych testowych przy pierwszym uruchomieniu,
- rozszerzona baza demonstracyjna z popularnymi sklepami, m.in. Biedronka i Lidl,
- responsywny interfejs Bootstrap 5 + wlasny CSS i JavaScript.

Ceny w seedzie sa przykladowe i sluza do prezentacji dzialania aplikacji. Nie sa aktualna oferta handlowa sklepow.

## Technologie

- ASP.NET Core MVC 8,
- Entity Framework Core 8,
- SQLite jako domyslna baza lokalna,
- opcjonalnie SQL Server LocalDB po zmianie konfiguracji,
- Bootstrap 5,
- JavaScript Vanilla,
- Cookie Authentication.

## Dane logowania admina

- E-mail: `admin@pricecomp.local`
- Haslo: `Admin123!`

Dane mozna zmienic w pliku `appsettings.json` w sekcji `AdminAccount`.

## Uruchomienie lokalne

1. Zainstaluj .NET SDK 8 lub 9.
2. Nie musisz instalowac osobnego serwera bazy danych. Domyslnie projekt uzywa pliku SQLite `App_Data/pricecomp.db`.
3. Otworz terminal w katalogu projektu:

```powershell
cd "C:\Users\HP\Documents\projekt techniki\PriceCompWeb"
```

4. Przywroc pakiety:

```powershell
dotnet restore
```

5. Uruchom aplikacje:

```powershell
dotnet run
```

6. Otworz adres pokazany w terminalu, np. `http://localhost:5000` albo `https://localhost:5001`.

Przy pierwszym uruchomieniu aplikacja sama utworzy baze `App_Data/pricecomp.db` i wstawi dane testowe.

## Zmiana bazy na SQL Server LocalDB

W pliku `appsettings.json` ustaw:

```json
"DatabaseProvider": "SqlServer"
```

Aplikacja uzyje wtedy connection stringa `SqlServerConnection`.

## Struktura projektu

- `Controllers` - kontrolery MVC dla strony publicznej, logowania i panelu admina.
- `Models` - encje bazy danych oraz modele widokow.
- `Data` - kontekst EF Core i seeder danych.
- `Services` - logika liczenia koszyka.
- `Views` - widoki Razor.
- `wwwroot` - CSS, JavaScript, Bootstrap i obrazy.

## Dokumentacja

- `TECHNICAL_DOCUMENTATION.md` - opis architektury, bazy danych i najwazniejszych funkcji.
- `USER_MANUAL.md` - instrukcja obslugi dla uzytkownika nietechnicznego.
