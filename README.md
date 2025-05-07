# ExpenseFlow

ExpenseFlow, sahada çalışan personel masraflarının kaydını, onayını ve raporlamasını sağlayan .NET 8 tabanlı bir REST API projesidir.
 Projede üç ana katman ve bir test dizini bulunmaktadır:


## Ön Koşullar

- Docker  
- Docker Compose  

Projeyi çalıştırmak için .NET SDK, EF Core CLI veya başka paket kurulumu yapmanıza gerek yok; tüm ihtiyaçlar Docker Compose ile otomatik sağlanır.

## Çalıştırma Adımları

1. Depoyu klonlayın:   Depoyu klonlayın:  

```sh

   git clone https://github.com/Nigarselekk/ExpenseFlow.git
   cd ExpenseFlow

```
2. Docker konteynerlerini oluşturup başlatın:   

```sh

  docker-compose up --build

```


Bu komut, Postgres veritabanı ve API servislerini ayağa kaldırır. Migration’lar ve seed işlemleri otomatik olarak çalışır.  

## Test Hesapları

Aşağıdaki kimlik bilgilerini kullanarak uygulamayı test edebilirsiniz. Default admin ve default personnel şu şekildedir:

| Rol         | E-posta                     | Şifre           |
| ----------- | --------------------------- | --------------- |
| **Admin**     | admin@expenseflow.com       | Admin123!       |
| **Personnel** | personnel@expenseflow.com   | Personnel123!   |


## JWT Token Alma
Örneğin personel için :

```sh
curl -X POST http://localhost:5146/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"personnel@expenseflow.com","password":"Personnel123!"}'
  ```
veya api/Auth/login endpointini kullanabilirsiniz

Dönen token değerini kopyalayın.

## Yetkilendirme
Swagger UI’da sağ üstteki Authorize butonuna tıklayın.
Bearer <token> formatında token’ı yapıştırın.

## API Kullanımı

Artık Swagger üzerinden tüm endpoint’ler kullanılabilir.


##  Proje Yapısı

## 1. Proje Katmanları

### 1.1 ExpenseFlow.Api
- **Controllers/**: HTTP endpoint’lerinin tanımlandığı sınıflar:
  - `AuthController` (Login/Register)
  - `ExpenseCategoriesController` (CRUD)
  - `ExpensesController` (Masraf CRUD + onay/red)
  - `PersonnelController` (Personel CRUD)
  - `AccountInfosController` (Hesap bilgileri CRUD)
  - `ExpenseAttachmentsController` (Fiş/fatura yükleme CRUD)
  - `PaymentTransactionsController` (Ödeme kayıtları CRUD)
  - `ReportsController` (Raporlama endpoint’leri)
- **Program.cs**: DI (Dependency Injection), middleware ve Swagger ayarları
- **Dockerfile**: API’yi container içinde paketlemek için

### 1.2 ExpenseFlow.Application
- **Cqrs/Commands**: Create/Update/Delete işlemlerine karşılık gelen `IRequest` tanımları
- **Cqrs/Queries**: Listeleme veya detay getirme sorguları ve özel raporlama sorguları
- **Handlers/**: Command/Query handler sınıfları, EF Core üzerinden veri işlemlerini gerçekleştirir
- **Requests/**: API’den gelen JSON body yapısını temsil eden DTO’lar
- **Responses/**: Handler’ların döndürdüğü sonuç DTO’ları

### 1.3 ExpenseFlow.Domain
- `Entities/`: Proje domain model sınıfları:
  - `Personnel`: Saha personeli (Id, Name, Role)
  - `PersonnelRole`: Enum (Admin, Personnel)
  - `ExpenseCategory`: Masraf kategorisi (Yemek, Ulaşım vs.)
  - `Expense`: Masraf kaydı (Personnel → ExpenseCategory ilişkisi)
  - `AccountInfo`: Personel banka bilgisi (IBAN, Banka adı)
  - `ExpenseAttachment`: Masraf fiş/fatura dosya yolu
  - `PaymentTransaction`: Onaylanan masraf için EFT simülasyonu
- `Enums`: `ExpenseStatus` (Pending, Approved, Rejected)

### 1.4 ExpenseFlow.Infrastructure
- **DbContext/ExpenseFlowDbContext.cs**: DbSet tanımları ve Fluent API ilişkileri
- **Identity/IdentityDataSeeder.cs**: Default `Admin` ve `Personnel` kullanıcıları oluşturur
- **Migrations/**: Migration dosyaları (.NET EF Core)

### 1.5 tests
- Birim testler (`ExpenseFlow.UnitTests`)
- Entegrasyon testler (`ExpenseFlow.IntegrationTests`)





1. Authentication

| Metot | URL               | Rol Gereksinimi | Açıklama             |
| ----- | ----------------- | --------------- | -------------------- |
| POST  | `/api/Auth/login` | —               | JWT token almak için |


2.Expense Categories

| Metot  | URL                           | Rol Gereksinimi  | Açıklama                    |
| ------ | ----------------------------- | ---------------- | --------------------------- |
| GET    | `/api/ExpenseCategories`      | Admin, Personnel | Tüm kategorileri listeler   |
| GET    | `/api/ExpenseCategories/{id}` | Admin, Personnel | Tek kategori getirir        |
| POST   | `/api/ExpenseCategories`      | Admin            | Yeni kategori oluşturur     |
| PUT    | `/api/ExpenseCategories/{id}` | Admin            | Mevcut kategoriyi günceller |
| DELETE | `/api/ExpenseCategories/{id}` | Admin            | Kategoriyi siler            |


3.Expenses

| Metot  | URL                          | Rol Gereksinimi   | Açıklama                                   |
| ------ | ---------------------------- | ----------------- | ------------------------------------------ |
| GET    | `/api/Expenses`              | Admin, Personnel  | Tüm masrafları listeler                    |
| GET    | `/api/Expenses/{id}`         | Admin, Personnel  | Tek masrafı getirir                        |
| POST   | `/api/Expenses`              | Personnel         | Yeni masraf oluşturur                      |
| PUT    | `/api/Expenses/{id}`         | Personnel         | Mevcut masrafı günceller                   |
| DELETE | `/api/Expenses/{id}`         | Personnel (kendi) | Masrafı siler (sadece kendi masrafı)       |
| POST   | `/api/Expenses/{id}/approve` | Admin             | Masrafı onaylar, EFT simülasyonu oluşturur |
| POST   | `/api/Expenses/{id}/reject`  | Admin             | Masrafı reddeder, gerekçeyi atar           |


4.Personnel

| Metot  | URL                   | Rol Gereksinimi | Açıklama                     |
| ------ | --------------------- | --------------- | ---------------------------- |
| GET    | `/api/Personnel`      | Admin           | Tüm personelleri listeler    |
| GET    | `/api/Personnel/{id}` | Admin           | Tek personel getirir         |
| POST   | `/api/Personnel`      | Admin           | Yeni personel ekler          |
| PUT    | `/api/Personnel/{id}` | Admin           | Personel bilgisini günceller |
| DELETE | `/api/Personnel/{id}` | Admin           | Personeli siler              |


5.AccountInfo
| Metot  | URL                      | Rol Gereksinimi  | Açıklama                       |
| ------ | ------------------------ | ---------------- | ------------------------------ |
| GET    | `/api/AccountInfos`      | Admin, Personnel | Tüm hesap bilgilerini listeler |
| GET    | `/api/AccountInfos/{id}` | Admin, Personnel | Tek hesap bilgisini getirir    |
| POST   | `/api/AccountInfos`      | Personnel        | Yeni hesap bilgisi ekler       |
| PUT    | `/api/AccountInfos/{id}` | Personnel        | Hesap bilgisini günceller      |
| DELETE | `/api/AccountInfos/{id}` | Admin, Personnel | Hesap bilgisini siler          |


6.ExpenseAttachment

| Metot  | URL                            | Rol Gereksinimi  | Açıklama               |
| ------ | ------------------------------ | ---------------- | ---------------------- |
| GET    | `/api/ExpenseAttachments`      | Admin, Personnel | Tüm ekleri listeler    |
| GET    | `/api/ExpenseAttachments/{id}` | Admin, Personnel | Tek eki getirir        |
| POST   | `/api/ExpenseAttachments`      | Personnel        | Yeni ek ekler          |
| PUT    | `/api/ExpenseAttachments/{id}` | Personnel        | Ek bilgisini günceller |
| DELETE | `/api/ExpenseAttachments/{id}` | Personnel        | Eki siler              |



7.Payment Transactions

| Metot  | URL                             | Rol Gereksinimi  | Açıklama                           |
| ------ | ------------------------------- | ---------------- | ---------------------------------- |
| GET    | `/api/PaymentTransactions`      | Admin, Personnel | Tüm işlemleri listeler             |
| GET    | `/api/PaymentTransactions/{id}` | Admin, Personnel | Tek işlemi getirir                 |
| POST   | `/api/PaymentTransactions`      | Admin            | Yeni EFT işlemi (simülasyon) ekler |
| PUT    | `/api/PaymentTransactions/{id}` | Admin            | İşlem bilgisini günceller          |
| DELETE | `/api/PaymentTransactions/{id}` | Admin            | İşlemi siler                       |


8.Reports
| Metot | URL                                   | Rol Gereksinimi  | Açıklama                                           |
| ----- | ------------------------------------- | ---------------- | -------------------------------------------------- |
| GET   | `/api/Reports/by-category?from=&to=`  | Admin, Personnel | Belirtilen tarihler arasında kategori bazlı toplam |
| GET   | `/api/Reports/by-personnel?from=&to=` | Admin, Personnel | Personele göre masraf toplamları                   |
| GET   | `/api/Reports/trend?from=&to=`        | Admin, Personnel | Zaman trendine göre masraf dağılımı                |





 



