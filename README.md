🚀 CoachingSystem API | Koçluk ve Danışmanlık Yönetim Sistemi

Bu proje, Koçluk ve Danışmanlık sistemleri için oluşturulmuş modern bir ASP.NET Core Web API projesidir. Proje, Katmanlı Mimari (Clean Architecture prensipleri), JWT Tabanlı Kimlik Doğrulama (Authentication) ve Rol Tabanlı Yetkilendirme (Authorization) kullanılarak geliştirilmiştir.

🎯 Proje Durumu (Güncel)

Kimlik Doğrulama (Login/Register): ✅ Tamamlandı.

Yetkilendirme (Authorization): ✅ Tamamlandı (Coach, Client, Admin rolleri kısıtlı).

Veritabanı: PostgreSQL (Entity Framework Core ile) kullanılmaktadır.

Ana Tablolar: Users ve Sessions tabloları mevcuttur.

🛠️ 1. Gerekli Ön Koşullar

Projeyi bilgisayarınızda çalıştırmadan önce aşağıdaki yazılımların kurulu olduğundan emin olun:

.NET 8 SDK: Projenin çalıştığı ana çerçeve.

PostgreSQL Veritabanı: Verilerin saklandığı veritabanı sunucusu.

pgAdmin (veya DBeaver): Veritabanını yönetmek için görsel bir araç (isteğe bağlı ama önerilir).

Postman veya Swagger UI: API uç noktalarını test etmek için.

⚙️ 2. Kurulum Adımları

Bu adımlar, projeyi ilk defa indirip çalıştırmak için gereklidir.

2.1 Veritabanı Ayarları (appsettings.json)

CoachingSystem.API klasöründeki appsettings.json dosyasını açın. ConnectionStrings ve Jwt (JWT Anahtarı) bölümlerini kendi PostgreSQL ayarlarınızla güncelleyin.

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


2.2 Veritabanını Oluşturma ve Güncelleme

Projenizin ana klasöründe (CoachingSystem dizini) terminali açın ve veritabanını oluşturmak/güncellemek için aşağıdaki komutları sırayla çalıştırın:

# 1. Var olan tüm veritabanı şemasını siler ve yeniden oluşturur (İlk çalıştırma için idealdir)
dotnet ef database drop --startup-project CoachingSystem.API 

# 2. Tüm tabloları ve UserRole Enum'ını (Session tablosu dahil) oluşturur
dotnet ef database update --startup-project CoachingSystem.API 


2.3 Projeyi Çalıştırma

API'yi başlatmak için:

dotnet run --project CoachingSystem.API


Uygulama başladığında konsolda Now listening on: http://localhost:5016 gibi bir adres göreceksiniz.

🧪 3. API Testi ve Kullanım Kılavuzu

Tüm uç noktalarınızı Swagger UI üzerinden test etmeniz önerilir. Tarayıcınızda şu adresi açın:

http://localhost:5016/swagger

3.1 Adım: Kullanıcı Kaydı (Register)

Önce Coach ve Client rollerinde iki kullanıcı oluşturun.

Uç Nokta: POST /api/Auth/register

Roller: Coach (Koç) ve Client (Danışan) rolüyle iki farklı kullanıcı kaydedin.

Örn. Coach JSON:

{
    "firstName": "Koç", 
    "lastName": "Deneme", 
    "email": "coach@test.com", 
    "password": "12345678", 
    "role": "Coach"
}


3.2 Adım: Giriş ve Token Alma

Yetkilendirme için JWT Token'ınızı alın.

Uç Nokta: POST /api/Auth/login

Sonuç: Yanıtta gelen uzun token string'ini kopyalayın.

3.3 Adım: Yetkilendirme (Authorization)

Kopyaladığınız token'ı API'ye tanıtın.

Swagger UI'da sağ üstteki "Authorize" butonuna tıklayın.

Bearer şemasını seçin.

Token'ı yapıştırın (Bearer öneki olmadan sadece string'i yapıştırın).

3.4 Adım: Rol Tabanlı Testler (Kısıtlamaları Kontrol Etme)

Bu testleri, 3.3 Adım'da yüklediğiniz token ile yapın.

Uç Nokta

Metot

Token Rolü

Beklenen Kod

Kontrol Edilen Kural

/api/Sessions/all

GET

Coach

200 OK

Coach'un listeleme yetkisi var.

/api/Sessions/all

GET

Client

403 Forbidden

Client'ın yetkisi yok.

/api/Sessions

POST

Coach

200 OK

Coach'un seans oluşturma yetkisi var.

/api/Sessions

POST

Client

403 Forbidden

Client'ın oluşturma yetkisi yok.

🤝 4. Proje Katmanları ve Mimarisi

Proje, Sorumlulukların Ayrılması (Separation of Concerns) ilkesine uygun olarak dört temel katmana ayrılmıştır:

Katman

Sorumluluk

Örnek Dosyalar

CoachingSystem.API

HTTP isteklerini yönetir, Controller'ları ve Program.cs yapılandırmasını içerir.

AuthController.cs, Program.cs

CoachingSystem.Application

Uygulama iş mantığını (Servisler) ve Repository/Service arayüzlerini (IUserRepository) içerir.

IUserRepository.cs

CoachingSystem.Infrastructure

Veri erişimini (ApplicationDbContext), Repository uygulamalarını (UserRepository) ve harici servisleri (JwtTokenService) içerir.

ApplicationDbContext.cs, JwtTokenService.cs

CoachingSystem.Domain

Projenin temel varlıklarını (User.cs, Session.cs) ve sabitlerini (UserRole.cs) içerir.

User.cs, UserRole.cs
