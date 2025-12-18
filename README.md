# 🛒 E-Commerce Backend API (.NET)

A scalable **E-Commerce Backend API** built with **ASP.NET Core**, following **Clean Architecture principles** and modern backend best practices. The system supports product management with **filtering, sorting, searching, pagination**, secure authentication using **JWT**, **Redis caching**, and **Stripe payment integration**.

---

## 🚀 Features

### 🧾 Products

* Products categorized by **Brand** and **Type**
* Advanced querying:

  * Filter by **BrandId** and **TypeId**
  * Search by product name
  * Sorting:

    * Name (ASC / DESC)
    * Price (ASC / DESC)
  * Pagination with configurable page size

### 🔍 Product Query Parameters

Implemented using a dedicated DTO:

* `BrandId`
* `TypeId`
* `Search`
* `Sort` (Enum-based)
* `PageNumber`
* `PageSize` (with max limit enforcement)

---

### 🧠 Architecture & Design Patterns

* **Onion Architecture** (Domain-centric, dependency rule enforced)
* **Clean Architecture**
* **Specification Pattern** (for flexible querying & filtering)
* **Unit of Work Pattern**
* **Repository Pattern**
* **Result Pattern** (standardized API responses)
* **Factory Pattern**
* **Delegates** where applicable
* **Dependency Injection** (built-in ASP.NET Core DI)

---

### 🔐 Authentication & Security

* JWT Authentication using **JWT Bearer Tokens**
* Secure token generation and validation
* Role-based access control (extensible)

---

### 🛍 Basket & Caching

* Shopping basket stored in **Redis**
* Cached basket data for fast access
* Redis used for:

  * Basket caching
  * Performance optimization

---

### 💳 Payments

* Integrated with **Stripe Payment Gateway**
* Secure handling of payment intents
* Client secret generation for frontend usage

---

### 📦 Orders Service

* Create orders from user basket
* Persist orders with:

  * Order items
  * Delivery method
  * Subtotal & total price calculation
* Order status lifecycle (e.g. Pending, Payment Received, Failed)
* Retrieve orders:

  * By authenticated user
  * By order ID
* Integrated with:

  * **Unit of Work** for transaction consistency
  * **Result Pattern** for safe responses
  * **Stripe** for payment confirmation

---

## 🧅 Project Structure (Onion Architecture)

The solution is organized using **Onion Architecture** and split into multiple **Class Library projects**, clearly separating responsibilities and enforcing dependency rules.

```
E-Commerce
│
├── ApplicationCoreLayer
│   │
│   ├── Domain (Class Library)
│   │   ├── Entities
│   │   └── Contracts (Interfaces)
│   │
│   ├── ServicesAbstractions (Class Library)
│   │   ├── Service Interfaces
│   │   └── DTO Contracts
│   │
│   └── Services (Class Library)
│       ├── Business Logic Implementations
│       ├── Result Pattern
│       └── Factories
│
├── InfrastructureLayer
│   │
│   └── E-Commerce.Persistence (Class Library)
│       ├── DbContexts
│       │   ├── StoreDbContext
│       │   └── StoreIdentityDbContext
│       ├── DataSeeding
│       └── Repositories
│
├── PresentationLayer
│   │
│   ├── E-Commerce.Presentation (Class Library)
│   │   ├── Controllers
│   │   └── Attributes
│   │
│   └── E-Commerce.WebApi (Startup Project)
│       ├── Attributes
│       ├── Custom Middlewares
│       │   └── Global Exception Handler
│       └── Program.cs / Dependency Injection
│
└── Shared
    ├── DTOs
    └── Result Pattern Implementations (Result<T>, Error Handling, Standard Responses)
```

### 🔗 Dependency Rule

* **Domain** has no dependencies
* **ServicesAbstractions** depends only on Domain
* **Services** depends on Domain & ServicesAbstractions
* **Infrastructure** depends on Application Core layers
* **Presentation & Web API** depend on all inner layers

This structure ensures **high maintainability, testability, and scalability**.

---

## 🔧 appsettings.json Example

To run the project locally, configure the following sections in your `appsettings.json` file:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "server=.; database=E-CommerceDb; Trusted_Connection=true; TrustServerCertificate=true;",
    "IdentityConnection": "server=.; database=E-CommerceDb.Identity; Trusted_Connection=true; TrustServerCertificate=true;",
    "RedisConnection": "localhost"
  },
  "JWTOptions": {
    "Issuer": "https://localhost:7047",
    "Audience": "MyStore",
    "SecurityKey": "YOUR_SECRET_KEY",
    "DurationInDays": 1
  },
  "URLs": {
    "BaseUrl": "https://localhost:7047"
  },
  "StripeSettings": {
    "SecretKey": "YOUR_STRIPE_SECRET_KEY",
    "EndpointSecret": "YOUR_STRIPE_ENDPOINT_SECRET"
  }
}
```

🔐 **Important Notes:**

* Never commit real **JWT** or **Stripe** secret keys
* Use environment variables or user-secrets in production
* Redis must be running locally or via Docker

### ▶️ Run the Application

1. Clone the repository
2. Update `appsettings.json`
3. Apply EF Core migrations
4. Run the API

---

## 📈 Future Improvements

* Wishlist feature
* Product reviews & ratings
* Admin dashboard
* Refresh tokens
* Docker support

---

## 👩‍💻 Author

**Rojena Shehata**
Backend .NET Developer
📧 Email: [rojenasheahata@gmail.com](mailto:rojenasheahata@gmail.com)
🔗 GitHub | LinkedIn

---

⭐ If you find this project helpful, feel free to star the repository!
