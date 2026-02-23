# AppNot 📝

ASP.NET Core MVC ile geliştirilmiş bir not ve öğrenme yönetimi uygulaması.

## 🚀 Özellikler

- Kullanıcı kaydı ve girişi
- Not oluşturma ve yönetimi
- XP ve rozet sistemi
- Lig sıralaması
- Öğretmen paneli
- Haftalık program takibi

## 🛠️ Teknolojiler

- **ASP.NET Core 8 MVC**
- **Entity Framework Core**
- **SQL Server Express**
- **Bootstrap 5**

## ⚙️ Kurulum

### Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Adımlar

1. **Repoyu klonla:**
   ```bash
   git clone https://github.com/KULLANICI_ADI/AppNot.git
   cd AppNot
   ```

2. **appsettings.json dosyasını oluştur:**
   ```bash
   cp NotUyg/appsettings.example.json NotUyg/appsettings.json
   ```
   Ardından `appsettings.json` içindeki `YOUR_SERVER` kısmını kendi SQL Server adınla değiştir:
   ```json
   "Server=BILGISAYAR_ADIN\\SQLEXPRESS;Database=NotUygDb;..."
   ```

3. **Veritabanını oluştur:**
   ```bash
   cd NotUyg
   dotnet ef database update
   ```

4. **Uygulamayı başlat:**
   ```bash
   dotnet run
   ```

   Tarayıcında `https://localhost:5001` adresine git.

## 📁 Proje Yapısı

```
AppNot/
├── NotUyg/
│   ├── Controllers/      # MVC Controller'lar
│   ├── Data/             # DbContext ve Repository'ler
│   ├── Entity/           # Veritabanı modelleri
│   ├── Migrations/       # EF Migration'ları
│   ├── Models/           # View modelleri
│   ├── Views/            # Razor sayfaları
│   └── wwwroot/          # Statik dosyalar (CSS, JS, resimler)
└── README.md
```

## 📄 Lisans

Bu proje MIT lisansı altında sunulmaktadır.
