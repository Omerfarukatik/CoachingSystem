# 🚀 CoachingSystem API | Geliştirici Kurulum Kılavuzu

Bu kılavuz, **CoachingSystem API** projesini yerel ortamınızda **PostgreSQL** veritabanı ile sıfırdan kurmak ve **JWT tabanlı Yetkilendirme (Authorization)** mekanizmasını test etmek için gerekli tüm teknik adımları içerir.

---

## 🧩 I. ÖN KOŞULLAR

Projeyi çalıştırmadan önce aşağıdaki yazılımların kurulu olduğundan emin olun:

| Yazılım | Açıklama |
|----------|-----------|
| [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download) | Projenin temel çalışma çerçevesi |
| [PostgreSQL](https://www.postgresql.org/download/) | Veri depolama sunucusu *(lokalde çalışmalıdır)* |
| [Postman](https://www.postman.com/downloads/) / Swagger UI | API uç noktalarını test etmek için |

---

## ⚙️ II. KURULUM (SIFIRDAN)

### 🔸 1. appsettings.json Düzenlemesi
`CoachingSystem.API/appsettings.json` dosyasını açın ve PostgreSQL bilgilerinizi ekleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=CoachingSystemDb;Username=postgres;Password=sifreniz"
  },
  "Jwt": {
    "Key": "CokUzunGizliJWTKeyOlmasiGerekirEnAz32Karakter",
    "Issuer": "CoachingSystemApi",
    "Audience": "CoachingSystemApp"
  }
}
```
> 🔐 Şifre kısmını kendi PostgreSQL bilgilerinize göre değiştirin.  
> `Jwt:Key` değeri **en az 32 karakter** olmalıdır.

---

### 🔸 2. Veritabanı Yapısını Oluşturma

Proje dizininde terminal açın:

```bash
# Veritabanını sıfırla
dotnet ef database drop --startup-project CoachingSystem.API --force

# Yeni tablo ve enum’ları oluştur
dotnet ef database update --startup-project CoachingSystem.API
```

---

### 🔸 3. API’yi Başlatma

```bash
dotnet run --project CoachingSystem.API
```
Uygulama başarıyla başlatıldığında:  
👉 **http://localhost:5016/swagger**

---

## 🔐 III. YETKİLENDİRME TESTİ

Amaç: `Coach` rolü erişebilmeli, `Client` kısıtlanmalıdır.

### 🧾 1. Kullanıcı Kayıtları (Register)
**Uç Nokta:** `POST /api/Auth/register`

#### Coach:
```json
{
  "firstName": "Koç",
  "lastName": "Deneme",
  "email": "coach@test.com",
  "password": "12345678",
  "role": "Coach"
}
```

#### Client:
```json
{
  "firstName": "Danışan",
  "lastName": "Deneme",
  "email": "client@test.com",
  "password": "12345678",
  "role": "Client"
}
```

---

### 🔑 2. Giriş (Login)
**Uç Nokta:** `POST /api/Auth/login`  
Her kullanıcı için giriş yapın ve dönen JWT token’ı alın.

Swagger UI’da **Authorize (🔒)** butonuna tıklayıp token’ı yapıştırın.

---

### 🧭 3. Rol Tabanlı Testler

| Uç Nokta | Metot | Token Rolü | Beklenen Kod | Sonuç |
|-----------|--------|-------------|----------------|---------|
| `/api/Sessions/all` | GET | Coach | 200 OK | ✅ Başarılı erişim |
| `/api/Sessions/all` | GET | Client | 403 Forbidden | ❌ Yetkisiz erişim |
| `/api/Sessions` | POST | Coach | 200 OK | ✅ Seans oluşturabilir |
| `/api/Sessions` | POST | Client | 403 Forbidden | ❌ Yetkisi yok |

---

## 🧱 IV. MİMARİ ÖZETİ

| Katman | Görev | Örnek Dosyalar |
|---------|--------|----------------|
| **Domain** | Varlıklar, Rol Tanımları | `User.cs`, `UserRole.cs` |
| **Application** | İş mantığı arayüzleri | `IUserRepository.cs`, `IUserService.cs` |
| **Infrastructure** | Veri erişimi & EF Core işlemleri | `ApplicationDbContext.cs`, `UserRepository.cs` |
| **API** | HTTP Controller’lar | `AuthController.cs`, `SessionsController.cs` |

---

## 📦 V. TEKNİK BİLGİLER

- Framework: **.NET 8.0**  
- Veritabanı: **PostgreSQL 16+**  
- ORM: **Entity Framework Core**  
- Kimlik Doğrulama: **JWT (Bearer Token)**  
- Mimari: **Katmanlı (Domain, Application, Infrastructure, API)**  
- Test Aracı: **Swagger UI / Postman**

---

> ✨ **Hazırlayan:** Ömer Faruk Atik  
> 🗂️ **Repository:** [CoachingSystem](https://github.com/Omerfarukatik/CoachingSystem)
