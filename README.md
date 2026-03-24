# ModularMonolith

Bu repository, eğitim amaçlı ama production mantığıyla geliştirilen bir Modular Monolith backend template/proje iskeletidir.

---

## 🎯 Amaç

- Modern backend mimarilerini öğrenmek
- Reusable bir backend template oluşturmak
- Modüler ve sürdürülebilir bir yapı kurmak
- İleride microservice’e evrilebilecek bir temel hazırlamak

---

## 🧠 Mimari Yaklaşım

### Modular Monolith

- Tek deploy
- Modül bazlı ayrım
- Her modül kendi sınırına sahip
- Modüller birbirine doğrudan bağımlı değildir
- Microservice’e geçiş için uygun yapı

### Clean Architecture (modül içinde)

Her modül kendi içinde katmanlara ayrılır:

- Domain
- Application
- Infrastructure
- API

Bu sayede her modül kendi mini bounded context’i gibi davranır.

### DDD-lite

Tam ağır DDD yerine, öğrenme ve production dengesi gözetilerek:

- Aggregate Root
- Value Object
- Domain Event
- Entity taban yapıları
- Modül bazlı error catalog yaklaşımı

uygulanır.

### CQRS ve Event-Driven

Henüz uygulanmadı, ama mimari bu yapılara uygun hazırlanıyor.

---

## 📁 Proje Yapısı

```text
ModularMonolith/
 ├── src/
 │   ├── BuildingBlocks/
 │   │   └── BuildingBlocks.Domain/
 │   ├── Modules/
 │   │   └── Product/
 │   │       ├── Product.Domain/
 │   │       │   ├── Entities/
 │   │       │   ├── Errors/
 │   │       │   ├── Events/
 │   │       │   └── ValueObjects/
 │   │       ├── Product.Application/
 │   │       ├── Product.Infrastructure/
 │   │       └── Product.API/
 │   └── API/
 ├── ModularMonolith.sln
 └── README.md