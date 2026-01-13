# 💱 CurrencyTracker – Döviz Takip Konsol Uygulaması

Bu proje, **Nesneye Yönelik Programlama (OOP)**, **LINQ** ve **async/await** konularının
uygulamalı olarak gösterilmesi amacıyla geliştirilmiş bir **C# Konsol Uygulaması**dır.

Uygulama, Frankfurter FREE API üzerinden **Türk Lirası (TRY) bazlı** güncel döviz kurlarını
çekmekte, verileri hafızada tutmakta ve LINQ kullanarak çeşitli sorgulamalar yapmaktadır.

---

## 🎯 Projenin Amacı

Bu projenin amacı aşağıdaki konulardaki yetkinliği göstermektir:

- API kullanarak dış veri kaynağından veri çekme  
- Asenkron programlama (`async / await`)  
- Koleksiyonlar (`List<T>`, `Dictionary<T>`)  
- LINQ sorguları (`Select`, `Where`, `OrderBy`, `Count`, `Average`)  
- Konsol tabanlı menü yönetimi  

---

## 🌐 Kullanılan API

**Frankfurter FREE API**

```text
https://api.frankfurter.app/latest?from=TRY
Döviz kurları TRY bazlıdır

Veriler gerçek zamanlı olarak API’den alınır

Hard-coded veri kullanılmamıştır

🧱 Model Sınıfları
csharp
Kodu kopyala
class CurrencyResponse
{
    public string Base { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
}

class Currency
{
    public string Code { get; set; }
    public decimal Rate { get; set; }
}
⚙️ Teknik Gereksinimler
✔ C# Konsol Uygulaması

✔ HttpClient

✔ async / await

✔ List<Currency>

✔ LINQ (Where, Select, OrderBy, Count, Average)

📋 Konsol Menü Yapısı
markdown
Kodu kopyala
===== CurrencyTracker =====
1. Tüm dövizleri listele
2. Koda göre döviz ara
3. Belirli bir değerden büyük dövizleri listele
4. Dövizleri değere göre sırala
5. İstatistiksel özet göster
0. Çıkış
🔍 Menü İşlevleri ve LINQ Kullanımı
1️⃣ Tüm Dövizleri Listele
LINQ Select
Tüm döviz kodları ve TRY karşılıkları listelenir

2️⃣ Koda Göre Döviz Ara
LINQ Where
Büyük/küçük harf duyarsız arama yapılır

3️⃣ Belirli Bir Değerden Büyük Dövizler
LINQ Where
Kullanıcının girdiği eşik değerin üzerindeki kurlar listelenir

4️⃣ Dövizleri Değere Göre Sırala
LINQ OrderBy / OrderByDescending
Artan veya azalan sıralama yapılır

5️⃣ İstatistiksel Özet
LINQ Count, Max, Min, Average
Toplam döviz sayısı
En yüksek ve en düşük kur
Ortalama kur bilgisi gösterilir

🚀 Çalıştırma
bash
Kodu kopyala
dotnet restore
dotnet run
Proje .NET 8.0 ile geliştirilmiştir.

🚫 Yasaklara Uyum
❌ Hard-coded veri kullanılmamıştır
❌ LINQ’siz çözüm yoktur
❌ GUI yoktur (tamamen konsol uygulaması
