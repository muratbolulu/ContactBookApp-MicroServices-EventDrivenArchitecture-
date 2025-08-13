# SeturProject - Microservices with MassTransit & RabbitMQ

Bu proje, **ContactService** ve **ReportService** isimli iki mikroservis ile **Event-Driven Architecture** prensibine uygun olarak geliştirilmiştir.  
Servisler, **MassTransit** kütüphanesi aracılığıyla **RabbitMQ** üzerinden haberleşir.  
Ayrıca, **CQRS** ve **Clean Architecture** yaklaşımları kullanılmıştır.

---

## 📌 Mimarinin Genel Yapısı

### 1. **ContactService**
- Kullanıcı ekleme, güncelleme ve silme gibi kişi yönetimi işlemlerini yapar.
- Kişi oluşturulduğunda `PersonCreatedEvent` olayı yayınlar.
- Olay yayınlamak için **IPublishEndpoint** (MassTransit) kullanır.

### 2. **ReportService**
- **ContactService**'ten gelen olayları dinler (`PersonCreatedEventConsumer`).
- Gelen olaylara göre rapor verilerini oluşturur ve saklar.
- PostgreSQL veritabanı kullanır.
- **BackgroundService** ile rapor işlemlerini asenkron yürütür.

### 3. **SharedKernel**
- Tüm mikroservislerin kullandığı ortak event ve interface tanımlarını barındırır.
- Örn: `PersonCreatedEvent`, `IGenericRepository<T>` vb.

---

## ⚙️ Kullanılan Teknolojiler

- **.NET 8**
- **MassTransit** (v8.x)
- **RabbitMQ**
- **PostgreSQL**
- **Entity Framework Core**
- **AutoMapper**
- **CQRS (MediatR)**
- **xUnit** + **MassTransit.Testing**

---

## Klasör Yapısı

SeturProject/
│
├── ContactService/
│   ├── API/
│   ├── Application/
│   ├── Infrastructure/
│   └── Domain/
│
├── ReportService/
│   ├── API/
│   ├── Application/
│   ├── Infrastructure/
│   ├── Domain/
│   └── Tests/
│
├── SharedKernel/
│   ├── Events/
│   ├── Interfaces/
│   └── Infrastructure/
│
└── README.md
