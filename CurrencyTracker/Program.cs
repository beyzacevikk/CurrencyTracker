using System.Net.Http;
using System.Text.Json;

namespace CurrencyTracker;

public class CurrencyResponse
{
    public string Base { get; set; } = string.Empty;
    public Dictionary<string, decimal> Rates { get; set; } = new();
}

public class Currency
{
    public string Code { get; set; } = string.Empty;
    public decimal Rate { get; set; }
}

internal class Program
{
    private const string ApiUrl = "https://api.frankfurter.app/latest?from=TRY";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        List<Currency> currencies;
        try
        {
            currencies = await FetchCurrenciesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("API'den veri alınırken hata oluştu:");
            Console.WriteLine(ex.Message);
            Console.WriteLine("Uygulama sonlandırılıyor...");
            return;
        }

        while (true)
        {
            PrintMenu();
            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    ListAllCurrencies(currencies);
                    break;
                case "2":
                    SearchByCode(currencies);
                    break;
                case "3":
                    ListGreaterThanValue(currencies);
                    break;
                case "4":
                    SortByValue(currencies);
                    break;
                case "5":
                    ShowStatistics(currencies);
                    break;
                case "0":
                    Console.WriteLine("Çıkış yapılıyor...");
                    return;
                default:
                    Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Devam etmek için ENTER'a basın...");
            Console.ReadLine();
        }
    }

    private static void PrintMenu()
    {
        Console.Clear();
        Console.WriteLine("===== CurrencyTracker =====");
        Console.WriteLine("1. Tüm dövizleri listele");
        Console.WriteLine("2. Koda göre döviz ara");
        Console.WriteLine("3. Belirli bir değerden büyük dövizleri listele");
        Console.WriteLine("4. Dövizleri değere göre sırala");
        Console.WriteLine("5. İstatistiksel özet göster");
        Console.WriteLine("0. Çıkış");
        Console.Write("Seçiminiz: ");
    }

    private static async Task<List<Currency>> FetchCurrenciesAsync()
    {
        using var httpClient = new HttpClient();

        using var response = await httpClient.GetAsync(ApiUrl);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var currencyResponse = JsonSerializer.Deserialize<CurrencyResponse>(json, JsonOptions)
                              ?? throw new InvalidOperationException("API yanıtı çözümlenemedi.");

        // LINQ Select (ZORUNLU)
        var list = currencyResponse.Rates
            .Select(kvp => new Currency
            {
                Code = kvp.Key,
                Rate = kvp.Value
            })
            .ToList();

        return list;
    }

    // 1️⃣ Tüm Dövizleri Listele — LINQ Select (ZORUNLU)
    private static void ListAllCurrencies(List<Currency> currencies)
    {
        Console.WriteLine();
        Console.WriteLine("Tüm dövizler (TRY bazlı):");
        Console.WriteLine("--------------------------");

        var rows = currencies
            .Select(c => $"{c.Code,-6} : {c.Rate}")
            .ToList();

        foreach (var row in rows)
        {
            Console.WriteLine(row);
        }
    }

    // 2️⃣ Koda Göre Döviz Ara — LINQ Where (Büyük/küçük harf duyarsız) (ZORUNLU)
    private static void SearchByCode(List<Currency> currencies)
    {
        Console.Write("Aranacak döviz kodu (ör: USD): ");
        var codeInput = (Console.ReadLine() ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(codeInput))
        {
            Console.WriteLine("Kod boş olamaz.");
            return;
        }

        var results = currencies
            .Where(c => c.Code.Equals(codeInput, StringComparison.OrdinalIgnoreCase))
            .ToList();

        Console.WriteLine();
        if (results.Count == 0)
        {
            Console.WriteLine($"'{codeInput}' kodlu döviz bulunamadı.");
            return;
        }

        foreach (var c in results)
        {
            Console.WriteLine($"{c.Code} : {c.Rate}");
        }
    }

    // 3️⃣ Belirli Bir Değerden Büyük Dövizler — LINQ Where (ZORUNLU)
    private static void ListGreaterThanValue(List<Currency> currencies)
    {
        Console.Write("Eşik değer girin (ör: 0.03): ");
        var input = (Console.ReadLine() ?? string.Empty).Trim();

        if (!decimal.TryParse(input, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var threshold))
        {
            // TR kullanıcıları için virgül desteği
            if (!decimal.TryParse(input, out threshold))
            {
                Console.WriteLine("Geçersiz sayı girdiniz.");
                return;
            }
        }

        var results = currencies
            .Where(c => c.Rate > threshold)
            .OrderByDescending(c => c.Rate)
            .ToList();

        Console.WriteLine();
        if (results.Count == 0)
        {
            Console.WriteLine($"{threshold} değerinden büyük döviz bulunamadı.");
            return;
        }

        Console.WriteLine($"{threshold} değerinden büyük dövizler:");
        Console.WriteLine("--------------------------");
        foreach (var c in results)
        {
            Console.WriteLine($"{c.Code,-6} : {c.Rate}");
        }
    }

    // 4️⃣ Dövizleri Değere Göre Sırala — LINQ OrderBy / OrderByDescending (ZORUNLU)
    private static void SortByValue(List<Currency> currencies)
    {
        Console.Write("Sıralama (A=Artan, Z=Azalan): ");
        var input = (Console.ReadLine() ?? string.Empty).Trim();

        IEnumerable<Currency> sorted = input.Equals("Z", StringComparison.OrdinalIgnoreCase)
            ? currencies.OrderByDescending(c => c.Rate)
            : currencies.OrderBy(c => c.Rate);

        Console.WriteLine();
        Console.WriteLine("Sıralı dövizler:");
        Console.WriteLine("--------------------------");
        foreach (var c in sorted)
        {
            Console.WriteLine($"{c.Code,-6} : {c.Rate}");
        }
    }

    // 5️⃣ İstatistiksel Özet — LINQ Count, Max, Min, Average (ZORUNLU)
    private static void ShowStatistics(List<Currency> currencies)
    {
        if (currencies.Count == 0)
        {
            Console.WriteLine("Veri yok.");
            return;
        }

        var count = currencies.Count(); // LINQ Count (ZORUNLU)
        var max = currencies.Max(c => c.Rate); // LINQ Max (ZORUNLU)
        var min = currencies.Min(c => c.Rate); // LINQ Min (ZORUNLU)
        var avg = currencies.Average(c => c.Rate); // LINQ Average (ZORUNLU)

        var maxCurrency = currencies.First(c => c.Rate == max);
        var minCurrency = currencies.First(c => c.Rate == min);

        Console.WriteLine();
        Console.WriteLine("İstatistiksel Özet");
        Console.WriteLine("--------------------------");
        Console.WriteLine($"Toplam döviz sayısı : {count}");
        Console.WriteLine($"En yüksek kur       : {maxCurrency.Code} ({max})");
        Console.WriteLine($"En düşük kur        : {minCurrency.Code} ({min})");
        Console.WriteLine($"Ortalama kur        : {avg}");
    }
}
