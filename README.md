# 📈 StockApp — Real-Time Stock Trading Platform

> A full-stack ASP.NET Core MVC application for real-time stock price tracking, portfolio management, and order execution — built with clean architecture principles.

---

## 🖥️ Live Demo Preview

```
Explore stocks → Select a ticker → Watch live prices → Buy / Sell → Export orders as PDF
```

---

## ✨ Features

- 🔴 **Real-Time Price Feed** — WebSocket integration with Finnhub API streams live trade data directly into an interactive Chart.js price chart
- 🔍 **Stock Explorer** — Browse and search US-listed stocks with company profiles pulled from Finnhub's REST API
- 📊 **Live Price Chart** — Candlestick-style line chart updates every 6 server ticks with timestamps
- 🛒 **Buy & Sell Orders** — Place orders with full server-side validation and persistent storage
- 🗂️ **Order History** — View all past buy/sell orders with trade amounts and timestamps
- 📄 **PDF Export** — Download a complete order history report via Rotativa (wkhtmltopdf)
- 🚨 **Structured Error Handling** — Custom `FinhubException`, middleware logging, and user-friendly error pages
- 📋 **Structured Logging** — Serilog with Console and Seq sinks, enriched with request/response context

---

## 🏗️ Architecture

This project follows **Clean Architecture** (also known as Onion Architecture), separating concerns into three layers:

```
StockApp (Presentation)
│   Controllers, Views, ViewComponents, Middleware, ViewModels
│
├── StockApp.Core (Domain + Application)
│   ├── Domain/Entities         — BuyOrder, SellOrder
│   ├── Domain/RepositoryContracts
│   ├── DTOs                    — Request/Response models
│   ├── ServiceContracts        — Interfaces for all services
│   ├── Services                — Business logic (Finnhub, Stock orders)
│   ├── CustomValidators        — MinimumDateValidatorAttribute
│   ├── Helpers                 — ValidationHelper
│   └── Exceptions              — FinhubException
│
└── StockApp.Infrastructure (Data Access)
    ├── Data/StockMarketDbContext
    ├── Repositories             — StocksRepository, FinhubRepository
    └── Migrations
```

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 MVC |
| Language | C# 12 |
| ORM | Entity Framework Core 8 |
| Database | SQL Server (LocalDB / Express) |
| Real-Time | WebSocket (Finnhub WSS) |
| HTTP Client | `IHttpClientFactory` |
| Charting | Chart.js 3.9 |
| PDF Generation | Rotativa.AspNetCore |
| Logging | Serilog + Seq |
| Validation | Data Annotations + Custom Validators |
| DI | ASP.NET Core built-in DI |

---

## 📐 Design Patterns Used

- **Repository Pattern** — `IStocksRepository`, `IFinhubRepository` abstract all data access
- **Service Layer Pattern** — Business logic isolated from controllers and infrastructure
- **Options Pattern** — `TradingOptions` bound from `appsettings.json` via `IOptions<T>`
- **View Components** — `SelectedStockViewComponent` for reusable Razor components
- **Middleware Pipeline** — `ExceptionHandlingMiddleware` for centralized logging and exception propagation
- **DTO Pattern** — Clean separation between domain entities and API-facing models (`BuyOrderRequest` / `BuyOrderResponse`)

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or LocalDB
- A free [Finnhub API key](https://finnhub.io/)
- *(Optional)* [Seq](https://datalust.co/seq) for structured log viewing

### 1. Clone the repository

```bash
git clone https://github.com/your-username/StockApp.git
cd StockApp
```

### 2. Configure secrets

Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) to store your Finnhub token safely:

```bash
cd StockApp
dotnet user-secrets set "FinnhubToken" "your_finnhub_api_key_here"
```

### 3. Update the connection string

Edit `appsettings.json` and set your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=YOUR_SERVER;Initial Catalog=StockDb;Integrated Security=True;..."
}
```

### 4. Apply database migrations

```bash
dotnet ef database update --project StockApp.Infrastructure --startup-project StockApp
```

### 5. Run the application

```bash
dotnet run --project StockApp
```

Navigate to `http://localhost:5149`

---

## 📸 App Structure Overview

```
/Stocks/Explore            → Browse all or top 25 popular stocks
/Stocks/Explore/{symbol}   → View company profile + current price
/Trade/Index/{symbol}      → Live price chart + buy/sell panel
/Trade/Orders              → Full order history
/Trade/OrdersPDF           → Download orders as PDF
/Error                     → Friendly error page with context
```

---

## ⚙️ Configuration Reference

| Key | Description |
|---|---|
| `FinnhubToken` | Your Finnhub API key (store in User Secrets) |
| `TradingOptions:DefaultOrderQuantity` | Pre-filled quantity in the order form |
| `TradingOptions:Top25PopularStocks` | Comma-separated list of featured tickers |
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `Serilog:WriteTo` | Logging sinks (Console, Seq) |

---

## 🗂️ Project Structure

```
StockApp.sln
├── StockApp/                        # Web/Presentation layer
│   ├── Controllers/
│   │   ├── HomeController.cs
│   │   ├── StocksController.cs
│   │   └── TradeController.cs
│   ├── Views/
│   │   ├── Stocks/Explore.cshtml
│   │   ├── Trade/Index.cshtml
│   │   ├── Trade/Orders.cshtml
│   │   ├── Trade/OrdersPDF.cshtml
│   │   └── Shared/
│   ├── ViewComponents/
│   │   └── SelectedStockViewComponent.cs
│   ├── Middleware/
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── ViewModel/
│   ├── wwwroot/
│   │   ├── Scripts/
│   │   │   ├── ChartScript.js
│   │   │   └── FinnhubScript.js
│   │   └── StyleSheet.css
│   ├── TradingOptions.cs
│   └── Program.cs
│
├── StockApp.Core/                   # Domain + Application layer
│   ├── Domain/
│   │   ├── Entities/
│   │   └── RepositoryContracts/
│   ├── DTOs/
│   ├── ServiceContracts/
│   ├── Services/
│   ├── CustomValidators/
│   ├── Helpers/
│   └── Exceptions/
│
└── StockApp.Infrastructure/         # Data Access layer
    ├── Data/
    ├── Repositories/
    └── Migrations/
```

---

## 🔒 Security Notes

- The Finnhub API token is **never hardcoded** — it uses ASP.NET Core User Secrets in development and should use environment variables or Azure Key Vault in production.
- All order inputs are validated server-side using Data Annotations and a custom `MinimumDateValidatorAttribute`.

---

## 📝 License

This project is open source and available under the [MIT License](LICENSE).

---

## 🙋‍♂️ Author

**Baher** — built as a portfolio project demonstrating clean architecture, real-time data integration, and production-grade ASP.NET Core patterns.

> ⭐ If you found this project useful or interesting, consider giving it a star!
