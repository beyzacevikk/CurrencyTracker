# CurrencyTracker – Döviz Takip Konsol Uygulaması

Bu proje, Frankfurter FREE API üzerinden **TRY bazlı** döviz kurlarını çekip hafızada tutan ve **LINQ** ile sorgulayan bir C# konsol uygulamasıdır.

## Zorunlu API
- https://api.frankfurter.app/latest?from=TRY

## Çalıştırma
```bash
dotnet restore
dotnet run
```

## Menü
```
===== CurrencyTracker =====
1. Tüm dövizleri listele
2. Koda göre döviz ara
3. Belirli bir değerden büyük dövizleri listele
4. Dövizleri değere göre sırala
5. İstatistiksel özet göster
0. Çıkış
```

## Teknik Notlar
- HttpClient + async/await
- Veriler `List<Currency>` içinde tutulur
- Menü fonksiyonları LINQ: Select, Where, OrderBy/OrderByDescending, Count/Max/Min/Average
