# 🚀 ModularMonolith

Bu repository, eğitim amaçlı fakat **production-level (üretim seviyesinde) mantıkla** geliştirilen bir **Modular Monolith (modüler tek parça uygulama)** backend template / proje iskeletidir.

Amaç sadece çalışan bir API üretmek değil; **doğru mimariyi**, **doğru katman sınırlarını** ve **ölçeklenebilir geliştirme yaklaşımını** adım adım inşa etmektir.

---

## 🎯 Amaç

* Modern backend mimarilerini öğrenmek
* Tekrar kullanılabilir (**reusable**) bir backend template oluşturmak
* Modüler ve sürdürülebilir bir yapı kurmak
* İleride microservice mimarisine evrilebilecek sağlam bir temel hazırlamak
* Sadece CRUD değil; güvenlik, audit, logging, caching, testing gibi gerçek proje ihtiyaçlarını da kapsamak

---

## 🧠 Mimari Yaklaşım

### 🧱 Modular Monolith (modüler tek parça uygulama)

* Tek deploy
* Modül bazlı ayrım
* Her modül kendi sınırına sahip
* Modüller birbirine doğrudan bağımlı değildir
* Tek uygulama içinde modüler büyüme hedeflenir

---

### 🧩 Clean Architecture (temiz mimari) — modül içinde

Her modül kendi içinde şu katmanlara ayrılır:

* **Domain (alan katmanı)** → iş kuralları, entity davranışları
* **Application (uygulama katmanı)** → use-case (kullanım senaryosu), handler, validation, policy mantığı
* **Infrastructure (altyapı katmanı)** → veri erişimi, JWT üretimi, cache implementasyonu, framework bağımlılıkları
* **API (sunum katmanı)** → controller, HTTP, authentication / authorization giriş noktaları

---

### 🧠 DDD-lite (hafif alan odaklı tasarım)

* Entity davranış içerir
* Anemic model yoktur
* Business logic (iş mantığı) entity içinde tutulur
* Use-case mantığı Application katmanında yönetilir

---

### 🔄 CQRS (Command Query Responsibility Segregation / komut-sorgu sorumluluk ayrımı)

* Command (komut) ve Query (sorgu) ayrımı uygulanmıştır
* MediatR kullanılır
* Pipeline Behavior (hat davranışı) desteği vardır
* Validation, caching ve performance gibi çapraz davranışlar merkezileştirilmiştir

---

## 🏗️ Proje Yapısı

```text
ModularMonolith/
 ├── src/
 │   ├── BuildingBlocks/
 │   │   └── BuildingBlocks.Domain
 │   │       ├── BaseEntity.cs
 │   │       └── AuditableEntity.cs
 │   │
 │   ├── Modules/
 │   │   └── Product/
 │   │       ├── Product.Domain
 │   │       ├── Product.Application
 │   │       ├── Product.Infrastructure
 │   │       └── Product.API
 │   │
 │   ├── API/
 │
 ├── tests/
 │   ├── Product.Domain.UnitTests
 │   ├── Product.Application.UnitTests
 │   └── Product.API.IntegrationTests
 │
 ├── ModularMonolith.sln
 ├── README.md
```

---

## ⚙️ Kullanılan Teknolojiler

* .NET 9
* ASP.NET Core Web API
* MediatR
* FluentValidation
* Entity Framework Core
* SQL Server
* Serilog
* JWT Bearer Authentication
* xUnit
* Moq
* ASP.NET Core Integration Testing

---

## 🧱 BuildingBlocks.Domain

### BaseEntity

* Tüm entity’lerin temel sınıfıdır
* `Id` ve `DomainEvent` yönetimini içerir

### AuditableEntity

Aşağıdaki audit ve lifecycle alanlarını içerir:

* `CreatedAtUtc`
* `CreatedBy`
* `UpdatedAtUtc`
* `UpdatedBy`
* `IsDeleted`
* `DeletedAtUtc`
* `DeletedBy`

Bu yapı sayesinde tüm modüllerde ortak bir denetim izi (audit trail) yaklaşımı kurulabilir.

---

## 📦 Product Modülü

İlk örnek modül olarak `Product` geliştirilmiştir.

### Mevcut özellikler

* Ürün oluşturma
* Ürün güncelleme
* Ürün listeleme
* Ürün detay getirme
* Sayfalama (**pagination**)
* Filtreleme (**filtering**)
* Sıralama (**sorting**)
* Soft delete (yumuşak silme)
* Hard delete (kalıcı silme)
* Restore (geri alma)

---

## 🔄 Product Lifecycle (ürün yaşam döngüsü)

```text
Active → SoftDeleted → Restored
Active / SoftDeleted → HardDeleted
```

---

## ♻️ Soft Delete (yumuşak silme)

* Veri fiziksel olarak silinmez
* `IsDeleted = true` yapılır
* `DeletedAtUtc` set edilir
* `DeletedBy` set edilir
* Global Query Filter (küresel sorgu filtresi) ile normal sorgulardan gizlenir

---

## ❌ Hard Delete (kalıcı silme)

* Veri fiziksel olarak silinir
* Ayrı use-case (kullanım senaryosu) olarak modellenmiştir
* Soft delete’ten bilinçli olarak ayrılmıştır

---

## 🔁 Restore (geri alma)

* Soft delete edilmiş kayıt geri alınır
* `IsDeleted = false`
* `DeletedAtUtc = null`
* `DeletedBy = null`

---

## 🔍 Filtering / Sorting / Pagination

* `IQueryable` üzerinden çalışır
* DB tarafında execute edilir
* RAM’de filtreleme yapılmaz
* Sayfalama ile büyük veri setleri kontrollü şekilde döndürülür

---

## 🔐 Authentication / Authorization

### Authentication (kimlik doğrulama)

* JWT tabanlıdır
* Token üretimi Infrastructure katmanında yapılır
* Demo login akışı ile test edilebilir

### Authorization (yetkilendirme)

* Policy-based authorization (yetki kuralı tabanlı yetkilendirme) uygulanmıştır
* Permission claim (yetki iddiası) yaklaşımı kullanılır
* Role (rol) → permission (yetki) üretimi yapılır

### Örnek permission’lar

* `products.manage`
* `products.hard_delete`

---

## 👤 Current User (mevcut kullanıcı) altyapısı

Application katmanı `HttpContext` bilmez.
Bunun yerine `ICurrentUserService` soyutlaması kullanılır.

Bu sayede:

* mevcut kullanıcı bilgisi alınabilir
* audit alanları kullanıcı ile ilişkilendirilebilir
* handler’lar framework bağımsız kalır

---

## ⚠️ Exception Handling (hata yönetimi)

Global exception middleware ile merkezi hata yönetimi vardır.

### Desteklenen hata tipleri

* `ValidationException` → 400
* `BusinessRuleException` → 400
* `NotFoundException` → 404
* beklenmeyen hatalar → 500

---

## 📜 Logging (loglama)

### Middleware tabanlı request logging

* request başlangıcı
* request bitişi
* status code
* süre (ms)

### Exception logging

* Warning → validation / business / not found hataları
* Error → beklenmeyen sistem hataları

---

## 📊 Serilog

* Structured logging (yapılandırılmış loglama)
* Console sink
* File sink
* Günlük log rotation
* Log retention (7 gün)

---

## 🔗 Correlation ID (istek izleme kimliği)

Her request için benzersiz bir correlation id üretilir veya mevcut header’dan alınır.

* Header: `X-Correlation-Id`
* Loglara otomatik eklenir
* Tek bir request’in tüm loglarını ilişkilendirmeyi kolaylaştırır

---

## ⚡ Performance Behavior (performans davranışı)

MediatR pipeline içinde request süreleri ölçülür.

* her request’in süresi loglanır
* yavaş request’ler ayrıca işaretlenebilir
* performans gözlemlenebilirliği artar

---

## 🧠 Caching (önbellekleme)

* `ICacheService` soyutlaması vardır
* `MemoryCacheService` implementasyonu kullanılır
* Cacheable query yaklaşımı uygulanmıştır
* CachingBehavior ile cache logic (önbellek mantığı) handler dışına alınmıştır
* Write işlemleri sonrası cache invalidation (önbellek temizleme) yapılır

---

## 🧪 Test Altyapısı

### Domain Unit Tests (alan katmanı birim testleri)

* Product entity kuralları test edilir
* create / update davranışları doğrulanır

### Application Unit Tests (uygulama katmanı birim testleri)

* handler davranışları test edilir
* not found / business rule / save / cache invalidation gibi akışlar doğrulanır
* Moq ile bağımlılıklar izole edilir

### API Integration Tests (entegrasyon testleri)

* authentication akışı
* token alma
* authorize / forbidden senaryoları
* endpoint seviyesinde uçtan uca akışlar

---

## 📌 Kod Standartları

* Class’larda kısa açıklamalar `//` formatında yazılır
* Gereksiz yorum kullanılmaz
* Temiz ve okunabilir kod tercih edilir
* Feature-based klasörleme uygulanır
* Teknik terimler mümkün olduğunca iş anlamına yakın isimlerle kullanılır

---

## 🧠 Şu Ana Kadar Uygulanan Konular

Bu projede şu konular aktif olarak uygulanmıştır:

* Modular Monolith
* Clean Architecture
* DDD-lite
* CQRS
* MediatR
* Validation Behavior
* Caching Behavior
* Performance Behavior
* Exception Middleware
* Request Logging Middleware
* Serilog
* Correlation ID
* JWT Authentication
* Policy / Permission tabanlı Authorization
* Current User altyapısı
* Soft Delete / Hard Delete / Restore
* Audit alanları
* Repository Pattern
* EF Core mapping / migration
* Unit Testing
* Integration Testing

---

## 🚀 Şu Anki Durum

Bu proje artık sadece temel CRUD gösterimi değildir.

Mevcut durumda proje:

* mimari omurgası kurulmuş
* ilk modülü olgunlaşmış
* güvenlik altyapısı eklenmiş
* audit ve logging desteği kazanmış
* test kültürü başlatılmış
* production-level backend template olmaya ciddi şekilde yaklaşmış

bir yapıdadır.

---

## 🛣️ Roadmap (sonraki adımlar)

### Yakın vadede

* Integration test kapsamını genişletmek
* gerçek kullanıcı / kullanıcı tablosu / parola doğrulama
* refresh token
* ikinci modül eklemek
* Redis cache

### Orta vadede

* Domain Events’in gerçek kullanımı
* Outbox Pattern
* Background Jobs
* Health Checks
* Retry / resilience yapıları

### İleri seviye

* RabbitMQ veya benzeri message broker
* Docker
* CI/CD
* Testcontainers
* Monitoring / Metrics / Dashboard
* Distributed tracing

---

## ⚠️ Not

Bu proje eğitim amaçlıdır, ancak yaklaşımı öğretici demo seviyesinde kalmayıp production mindset (üretim bakış açısı) ile ilerlemektedir.

Amaç:
**sadece çalışan bir sistem yapmak değil, doğru mimari kararları da sistemli şekilde inşa etmektir.**
