# 🧱 ModularMonolith

Bu repository, eğitim amaçlı fakat production yaklaşımıyla geliştirilen bir **Modular Monolith backend template**’idir.

Amaç, modern backend mimarilerini uygulayarak **tekrar kullanılabilir (reusable)** bir proje iskeleti oluşturmaktır.

---

## 🎯 Amaç

- Modern backend mimarilerini öğrenmek
- Reusable backend template oluşturmak
- Modüler ve sürdürülebilir yapı kurmak
- İleride microservice’e evrilebilecek temel hazırlamak

---

## 🧠 Mimari Yaklaşım

### Modular Monolith

- Tek deploy
- Modül bazlı ayrım
- Her modül kendi sınırına sahip
- Modüller birbirine doğrudan bağımlı değildir

---

### Clean Architecture (modül içinde)

Her modül aşağıdaki katmanlara sahiptir:

- Domain → iş kuralları
- Application → use-case’ler
- Infrastructure → veri erişimi
- API → dış dünya ile iletişim

---

### CQRS (başlangıç)

- Command → write işlemleri
- Query → read işlemleri

---

## 📁 Proje Yapısı

```text
ModularMonolith/
 ├── src/
 │   ├── BuildingBlocks/
 │   │   └── BuildingBlocks.Domain
 │   │       ├── Exceptions/
 │   │       └── Primitives/
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