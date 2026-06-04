using System.Globalization;
using System.Text;
using PriceCompWeb.Models;

namespace PriceCompWeb.Services;

public static class ProductSearch
{
    public static bool Matches(Offer offer, string? query)
    {
        return Score(offer, query) > 0;
    }

    public static int Score(Offer offer, string? query)
    {
        var queryTokens = Tokenize(query).ToList();
        if (queryTokens.Count == 0)
        {
            return 1;
        }

        var product = offer.Product;
        var searchableText = string.Join(" ", new[]
        {
            product?.Name,
            product?.Category,
            product?.Description,
            offer.Store?.Name,
            offer.PromoDescription
        });

        var text = Normalize(searchableText);
        var textTokens = Tokenize(searchableText).ToList();

        var score = 0;
        foreach (var queryToken in queryTokens)
        {
            if (text.Contains(queryToken, StringComparison.Ordinal))
            {
                score += 10;
                continue;
            }

            if (textTokens.Any(textToken => IsCloseMatch(textToken, queryToken)))
            {
                score += 5;
                continue;
            }

            return 0;
        }

        return score;
    }

    public static bool ProductMatches(Product? product, string? query)
    {
        if (product is null)
        {
            return false;
        }

        return Score(new Offer { Product = product, Store = new Store() }, query) > 0;
    }

    private static IEnumerable<string> Tokenize(string? value)
    {
        return Normalize(value)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(token => token.Length > 1);
    }

    private static string Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var normalized = value
            .ToLowerInvariant()
            .Replace('ł', 'l')
            .Replace('ą', 'a')
            .Replace('ć', 'c')
            .Replace('ę', 'e')
            .Replace('ń', 'n')
            .Replace('ó', 'o')
            .Replace('ś', 's')
            .Replace('ź', 'z')
            .Replace('ż', 'z')
            .Normalize(NormalizationForm.FormD);

        var builder = new StringBuilder(normalized.Length);
        foreach (var character in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(character);
            if (category == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            builder.Append(char.IsLetterOrDigit(character) ? character : ' ');
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }

    private static bool IsCloseMatch(string textToken, string queryToken)
    {
        if (textToken.Contains(queryToken, StringComparison.Ordinal))
        {
            return true;
        }

        if (queryToken.Length < 4 || textToken.Length < 4)
        {
            return false;
        }

        if (queryToken.Contains(textToken, StringComparison.Ordinal))
        {
            return true;
        }

        var maxDistance = queryToken.Length <= 5 ? 1 : 2;
        return LevenshteinDistance(textToken, queryToken) <= maxDistance;
    }

    private static int LevenshteinDistance(string left, string right)
    {
        var distances = new int[left.Length + 1, right.Length + 1];

        for (var i = 0; i <= left.Length; i++)
        {
            distances[i, 0] = i;
        }

        for (var j = 0; j <= right.Length; j++)
        {
            distances[0, j] = j;
        }

        for (var i = 1; i <= left.Length; i++)
        {
            for (var j = 1; j <= right.Length; j++)
            {
                var cost = left[i - 1] == right[j - 1] ? 0 : 1;
                distances[i, j] = Math.Min(
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost);
            }
        }

        return distances[left.Length, right.Length];
    }
}
