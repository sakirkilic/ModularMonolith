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

---

## 📁 Proje Yapısı

```text
ModularMonolith/
 ├── src/
 │   ├── BuildingBlocks/
 │   │     └── BuildingBlocks.Domain
 │   ├── Modules/
 │   ├── API/
 ├── ModularMonolith.sln
 ├── README.md