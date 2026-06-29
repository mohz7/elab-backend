# eLab Backend-Graduation project

> ASP.NET Core REST API powering the eLab medical laboratory management platform — handling authentication, lab test bookings, results, payments, AI analysis, and multi-branch management.

---

## ✨ Features

- **JWT Authentication** — Secure login for patients, staff, and admins with role-based access control
- **Patient Management** — Full patient profiles with medical history, emergency contacts, and insurance info
- **Test Catalog** — Manage laboratory tests by category, sample type, and turnaround time
- **Appointment Booking** — Multi-branch booking system with time slot management
- **Results Management** — Upload, approve, and deliver lab results to patients
- **Stripe Payments** — Online payment integration with multi-currency support (USD / ILS)
- **AI Analysis** — AI-powered result interpretation chat sessions per patient
- **Offers & Pricing** — Discount management across branches and test categories
- **Report Templates** — Customizable result report templates
- **Reference Ranges** — Define normal value ranges per test for automated flagging
- **Multi-Branch Support** — Manage multiple lab branches with independent staff and scheduling
- **Real-time Notifications** — Alert patients on booking and result updates
- **Staff & Admin Portals** — Dedicated role-specific endpoints

---

## 🧰 Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core 9 | Web API framework |
| C# | Language |
| Entity Framework Core | ORM / Database access |
| SQL Server | Database |
| JWT Bearer | Authentication |
| Stripe.NET | Payment processing |
| Clean Architecture | Project structure pattern |

---

## 🗂️ Project Structure

```
eLab/
├── eLab.PL/          # Presentation Layer — Controllers, API endpoints
├── eLab.BLL/         # Business Logic Layer — Services, DTOs
├── eLab.DAL/         # Data Access Layer — Entities, Repositories, DbContext
└── eLab.sln          # Solution file
```

---

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server (local or remote)
- Visual Studio 2022+ or VS Code

### Installation

```bash
# Clone the repo
git clone https://github.com/mohz7/elab-backend.git
cd elab-backend
```

Open `eLab.sln` in Visual Studio, then:

1. Update `appsettings.json` with your database connection string and API keys
2. Run migrations to set up the database:

```bash
cd eLab.DAL
dotnet ef database update
```

3. Run the project:

```bash
cd eLab.PL
dotnet run
```

The API will be available at `https://localhost:7000` (or as configured).

---

## 🔑 Configuration (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=eLabDB;Trusted_Connection=True;"
  },
  "JWT": {
    "Key": "your_jwt_secret_key",
    "Issuer": "eLab",
    "Audience": "eLab"
  },
  "Stripe": {
    "SecretKey": "your_stripe_secret_key"
  }
}
```

> ⚠️ Never commit real API keys to GitHub. Use environment variables or user secrets in production.

---

## 📡 API Overview

| Group | Endpoints |
|---|---|
| Auth | Register, Login, Forgot Password |
| Patients | Profile, Medical History, Bookings, Results |
| Tests | Catalog, Categories, Prices, Offers |
| Bookings | Create, View, Update Status |
| Results | Upload, Approve, View by Patient |
| Payments | Stripe Checkout, Payment Status |
| AI | Create Session, Send Message, Get History |
| Admin | Users, Staff, Branches, Templates, Ranges |
| Notifications | Send, Mark as Read |

---

## 👤 User Roles

| Role | Description |
|---|---|
| Patient | Book tests, view results, use AI analysis, pay online |
| Staff | Manage bookings, upload results, access patient profiles |
| Admin | Full system control — all of the above plus catalog, pricing, staff, and branch management |

---

## 🔗 Related

- [eLab Frontend](https://github.com/mohz7/elab-fixed) — React web application
