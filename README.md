# SeturProject - Microservices with MassTransit & RabbitMQ

Bu proje, **ContactService** ve **ReportService** isimli iki mikroservis ile **Event-Driven Architecture** prensibine uygun olarak geliştirilmiştir.  
Servisler, **MassTransit** kütüphanesi aracılığıyla **RabbitMQ** üzerinden haberleşir.  
Ayrıca, **CQRS** ve **Clean Architecture** yaklaşımları kullanılmıştır.

---

## 📌 Mimarinin Genel Yapısı

### 1. **ContactService**
- Kullanıcı ekleme-silme kişi yönetimi işlemlerini yapar. 
- Contact ekleme-silme yönetimi işlemlerini yapar. 
- **ReportService**'ten gelen olayları dinler (`ReportCreatedEventConsumer`).

### 2. **ReportService**
- Olay yayınlamak için **IPublishEndpoint** (MassTransit) kullanır.
- Gelen olaylara göre rapor verilerini oluşturur ve saklar.
- PostgreSQL veritabanı kullanır.
- **BackgroundService** ile rapor işlemlerini asenkron yürütür.

### 3. **SharedKernel**
- Tüm mikroservislerin kullandığı ortak event ve interface tanımlarını barındırır.
- Örn: `IGenericServices<T>`, `IGenericRepository<T>` vb.

---

## ⚙️ Kullanılan Teknolojiler

- **.NET 9**
- **MassTransit** (v8.x)
- **RabbitMQ**
- **PostgreSQL**
- **Entity Framework Core**
- **AutoMapper**
- **CQRS (MediatR)**
- **xUnit** + **MassTransit.Testing**

---
## Migrations ve Çlıştırmalar
- **ContactService.API "Set As Startup Project" sonrasında  add-migration initial ile migration oluşturulabilir.**
- Akabinde **Update-Database** komutu ile veritabanı yoksa eklenebilir.**
- POST https://localhost:7033/api/persons ile person oluşturulabilir.

- **ReportService.API "Set As Startup Project" sonrasında  add-migration initial ile migration oluşturulabilir.**
- Akabinde **Update-Database** komutu ile veritabanı güncellenebilir.**


## Klasör Yapısı
```
SeturProject/
│
├── ContactService/
│   ├── API/
│   ├── Application/
│   ├── Infrastructure/
│   │   └── Migrations/      ← ContactService için EF Core migration dosyaları
│   └── Domain/
│
├── ReportService/
│   ├── API/
│   ├── Application/
│   ├── Infrastructure/
│   │   └── Migrations/      ← ReportService için EF Core migration dosyaları
│   ├── Domain/
│   └── Tests/
│
├── SharedKernel/
│   ├── Events/
│   ├── Interfaces/
│   └── Infrastructure/
│
└── README.md
```


