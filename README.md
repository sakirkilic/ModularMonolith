# 🚀 ModularMonolith

Bu repository, eğitim amaçlı ama **production-level mantıkla** geliştirilen bir Modular Monolith backend template/proje iskeletidir.

---

## 🎯 Amaç

* Modern backend mimarilerini öğrenmek
* Reusable bir backend template oluşturmak
* Modüler ve sürdürülebilir bir yapı kurmak
* İleride microservice’e evrilebilecek bir temel hazırlamak

---

## 🧠 Mimari Yaklaşım

### 🧱 Modular Monolith

* Tek deploy
* Modül bazlı ayrım
* Her modül kendi sınırına sahip
* Modüller birbirine doğrudan bağımlı değildir

---

### 🧩 Clean Architecture (Modül içinde)

Her modül kendi içinde katmanlara ayrılır:

* **Domain** → iş kuralları
* **Application** → use-case orchestration
* **Infrastructure** → DB / external servisler
* **API** → dış dünyaya açılan katman

---

### 🧠 DDD-lite

* Entity davranış içerir
* Anemic model yoktur
* Business logic entity içinde tutulur

---

### 🔄 CQRS (hazır altyapı)

* Command / Query ayrımı yapılmıştır
* MediatR kullanılır
* Pipeline behavior desteği vardır

---

## 🏗️ Proje Yapısı

```text
ModularMonolith/
 ├── src/
 │   ├── BuildingBlocks/
 │   │     └── BuildingBlocks.Domain
 │   │         ├── BaseEntity.cs
 │   │         └── AuditableEntity.cs
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
 ├── ModularMonolith.sln
 ├── README.md
```

---

## ⚙️ Kullanılan Teknolojiler

* .NET 9
* ASP.NET Web API
* MediatR
* FluentValidation
* Entity Framework Core
* SQL Server
* Serilog

---

## 🧱 Domain Katmanı

### BaseEntity

* Tüm entity’lerin temel sınıfıdır
* Id ve DomainEvent yönetimini içerir

### AuditableEntity

* Audit ve lifecycle alanlarını içerir:

  * CreatedAtUtc
  * UpdatedAtUtc
  * IsDeleted
  * DeletedAtUtc

---

## 📦 Product Modülü

### Özellikler

* Ürün oluşturma
* Ürün güncelleme
* Ürün listeleme (pagination + filtering + sorting)
* Ürün getirme (id ile)
* Soft delete
* Hard delete
* Restore

---

## 🔄 Product Lifecycle

```text
Active → SoftDeleted → Restored
Active/SoftDeleted → HardDeleted
```

---

## ♻️ Soft Delete

* Veri fiziksel olarak silinmez
* `IsDeleted = true` yapılır
* `DeletedAtUtc` set edilir
* Query’lerde filtrelenir

---

## ❌ Hard Delete

* Veri fiziksel olarak silinir
* Repository üzerinden `Remove` yapılır
* Özel endpoint ile çalışır

---

## 🔁 Restore

* Soft delete edilmiş kayıt geri alınır
* `IsDeleted = false`
* `DeletedAtUtc = null`

---

## 🔍 Filtering / Sorting / Pagination

* IQueryable üzerinden çalışır
* DB tarafında execute edilir
* RAM’de filtreleme yapılmaz

---

## ⚠️ Exception Handling

Global exception middleware ile:

* ValidationException → 400
* BusinessRuleException → 400
* NotFoundException → 404
* Unknown → 500

---

## 📜 Logging

### Middleware bazlı logging

* Request başlangıcı
* Request bitişi
* Status code
* Süre (ms)

---

### Exception logging

* Warning → business / validation hataları
* Error → beklenmeyen hatalar

---

## 📊 Serilog

* Structured logging
* Console + File sink
* Günlük log rotation
* Log retention (7 gün)

---

## 🔗 Correlation ID

Her request için benzersiz id üretilir:

* Header: `X-Correlation-Id`
* Loglara otomatik eklenir
* Request takibi kolaylaşır

---

## 🧪 Test Edilebilirlik

* Application katmanı bağımsızdır
* Infrastructure soyutlanmıştır
* Handler’lar test edilebilir yapıdadır

---

## 📌 Kod Standartları

* Class’larda kısa açıklamalar (`//`)
* Gereksiz yorum yok
* Temiz ve okunabilir kod
* Tutarlı isimlendirme

---

## 🧠 Öğrenilen Konular

Bu projede şu konular uygulanmıştır:

* Modular Monolith
* Clean Architecture
* DDD-lite
* CQRS
* MediatR Pipeline
* Validation Behavior
* Exception Middleware
* Logging Middleware
* Serilog
* Correlation ID
* Soft Delete / Hard Delete / Restore
* Repository Pattern
* EF Core mapping & migrations

---

## 🚀 Roadmap (Yaklaşan Adımlar)

* Global Query Filters (IsDeleted otomatik filtre)
* Authorization / Authentication (JWT)
* Caching (Redis)
* Event-driven yapı (RabbitMQ)
* Outbox Pattern
* Background Jobs
* Health Checks

---

## ⚠️ Not

Bu proje:

* eğitim amaçlıdır
* ama production mindset ile geliştirilmektedir

Amaç sadece çalışması değil:
👉 doğru mimariyi kurmaktır
