# 🚗 TripSplit — inteligentny kalkulator kosztów podróży

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)]()
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-5C2D91?logo=dotnet&logoColor=white)]()
[![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework-Core%209.x-512BD4)]()
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## 🧭 O aplikacji

**TripSplit** to nowoczesna aplikacja webowa stworzona w technologii **ASP.NET Core MVC (.NET 9)**,  
która służy do **planowania, zapisywania i rozliczania kosztów przejazdów samochodowych**.

Została zaprojektowana z myślą o rodzinach, znajomych i grupach podróżujących wspólnie — tak, aby łatwo policzyć:
- koszt całkowity podróży (paliwo + parking + opłaty),
- koszt **na osobę**,
- zużycie paliwa i szacowany zasięg auta.

TripSplit automatycznie oblicza wszystkie dane, zapisuje przejazdy i przypomina o **ważności OC oraz badań technicznych** samochodu.

---

## ✨ Co potrafi TripSplit

| Moduł | Funkcje |
|-------|----------|
| **🚗 Przejazdy** | Dodawanie trasy (start, cel, dystans, spalanie, liczba osób, koszty) i automatyczne obliczenia kosztów. |
| **🛞 Auta** | Lista samochodów z informacjami o spalaniu, pojemności baku, rodzaju paliwa. |
| **📅 Przypomnienia** | Oznaczenia i alerty o wygasających terminach **OC** lub **badań technicznych**. |
| **📊 Obliczenia** | Koszt paliwa, zużycie (L/100 km), koszt całkowity i na osobę — liczone w czasie rzeczywistym. |
| **🎨 Interfejs** | Responsywny, pastelowy design (Airflow Pastel), tryb **ciemny/jasny**, animowane przyciski, przyjazne karty. |
| **🔐 Użytkownicy** | Rejestracja, logowanie, sesje użytkowników — oparte o **ASP.NET Identity**. |

---

## 🖥️ Demo aplikacji

Jeśli chcesz przetestować aplikację lokalnie lub w środowisku testowym, możesz zalogować się na konto demo:

```
📧 Login: demo@tripsplit.pl  
🔑 Hasło: Demo123!
```

> Konto demo ma dostęp do przykładowych przejazdów i aut — dane resetują się przy restarcie aplikacji.

---

## ⚙️ Jak uruchomić aplikację lokalnie

### 1️⃣ Klonowanie repozytorium

```bash
git clone https://github.com/MSternol/TripSplit.git
cd TripSplit
```

---

### 2️⃣ Konfiguracja połączenia z bazą danych

Upewnij się, że masz zainstalowaną lokalną bazę danych **SQL Server (LocalDB)**.  
Aplikacja korzysta z connection stringa:

```
Server=(localdb)\MSSQLLocalDB;Database=TripSplitDb;Trusted_Connection=True;MultipleActiveResultSets=true
```

---

### 🧩 Dodanie pliku `appsettings.json`

Jeśli w repozytorium nie ma jeszcze pliku `appsettings.json`, utwórz go w folderze:

```
TripSplit.Web/appsettings.json
```

Wklej poniższą zawartość — to podstawowa konfiguracja, wystarczająca do uruchomienia aplikacji lokalnie:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TripSplitDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "App": {
    "SeedDemoUser": true,
    "DemoUserEmail": "demo@tripsplit.pl",
    "DemoUserPassword": "Demo123!"
  },
  "AllowedHosts": "*"
}
```

💡 Ten plik:
- ustawia połączenie do lokalnej bazy danych **TripSplitDb**,  
- włącza logowanie i prostą obsługę błędów,  
- pozwala automatycznie utworzyć konto **demo@tripsplit.pl / Demo123!** przy pierwszym uruchomieniu.

---

### 3️⃣ Utworzenie bazy danych i tabel

W terminalu w folderze projektu uruchom:

```bash
dotnet ef database update --project TripSplit.Infrastructure --startup-project TripSplit.Web
```

To stworzy bazę danych **TripSplitDb** z wszystkimi tabelami (użytkownicy, przejazdy, auta itp.).

> Jeżeli masz zmiany modelu i brak migracji, utwórz je:
>
> ```bash
> dotnet ef migrations add Init --project TripSplit.Infrastructure --startup-project TripSplit.Web
> dotnet ef database update --project TripSplit.Infrastructure --startup-project TripSplit.Web
> ```

---

### 4️⃣ Uruchomienie aplikacji

```bash
dotnet run --project TripSplit.Web
```

Aplikacja uruchomi się pod adresem:

👉 https://localhost:7289    
lub  
👉 http://localhost:5283

Zaloguj się kontem demo (lub utwórz własne konto rejestrując się w aplikacji).

---

## 🧩 Architektura projektu

Aplikacja została zbudowana według zasad **Clean Architecture (Domenowo-Warstwowej)**:

```
TripSplit
├─ Domain/              → encje, logika biznesowa, Value Objects
├─ Application/         → CQRS (MediatR), DTO, walidacje, UseCase’y
├─ Infrastructure/      → EF Core, Identity, repozytoria, migracje, seedy
└─ Web/                 → ASP.NET MVC + Razor + UI + CSS
```

---

## 🔧 Technologie

- **ASP.NET Core 9.0 MVC**
- **Entity Framework Core 9**
- **MediatR + CQRS pattern**
- **AutoMapper**
- **ASP.NET Identity (AppUser, AppRole)**
- **xUnit + FluentAssertions** — testy jednostkowe  
- **CSS Grid + Flexbox + Custom Tokens (Airflow Pastel)**

---

## 🧠 Logika obliczeń

Każdy przejazd automatycznie oblicza:

| Wzór | Opis |
|------|------|
| `litersUsed = (distanceKm / 100) × avgConsumption` | zużycie paliwa |
| `fuelCostTotal = litersUsed × fuelPricePerL` | koszt paliwa |
| `tripCostTotal = fuelCostTotal + parking + extras` | koszt całkowity |
| `costPerPerson = tripCostTotal / peopleCount` | koszt na osobę |

---

## 🧱 Struktura katalogów

```
TripSplit/
├─ TripSplit.sln
│
├─ TripSplit.Domain/                     # warstwa domenowa (czysta logika biznesowa)
│  ├─ Common/                            # klasy bazowe (BaseEntity, AuditableEntity, itp.)
│  ├─ Entities/                          # encje domenowe (Trip, Car, User itp.)
│  ├─ Enums/                             # typy wyliczeniowe (FuelType, TripStatus, itp.)
│  ├─ Events/                            # zdarzenia domenowe (TripCreatedEvent, itp.)
│  ├─ Repositories/                      # interfejsy repozytoriów domenowych
│  ├─ Services/                          # serwisy domenowe (np. kalkulacje)
│  └─ ValueObjects/                      # obiekty wartości (Location, Distance, Money)
│
├─ TripSplit.Application/                # logika aplikacyjna (CQRS + DTO + MediatR)
│  ├─ Abstractions/                      # interfejsy (np. IDateTime, ICurrentUserService)
│  ├─ Common/                            # wyjątki, walidacje, behavior-y pipeline’u
│  ├─ DTOs/                              # modele transferowe (TripDto, CarDto, itp.)
│  ├─ Features/                          # moduły z Command / Query / Handler
│  ├─ Mapping/                           # profile AutoMappera
│  ├─ Pipeline/                          # behavior-y MediatR (np. Logging, Validation)
│  └─ DependencyInjection.cs             # AddApplication()
│
├─ TripSplit.Infrastructure/             # warstwa infrastruktury (EF Core + Identity)
│  ├─ Identity/                          # AppUser, AppRole, konfiguracja Identity
│  ├─ Migrations/                        # migracje EF Core
│  ├─ Persistence/                       # AppDbContext, konfiguracje encji
│  ├─ Repositories/                      # implementacje repozytoriów
│  ├─ Seed/                              # DbSeeder – dodaje dane testowe / demo
│  └─ DependencyInjection.cs             # AddInfrastructure()
│
├─ TripSplit.Web/                        # warstwa prezentacji (ASP.NET Core MVC)
│  ├─ wwwroot/                           # zasoby statyczne (CSS, JS, ikony, fonty)
│  │  ├─ css/
│  │  ├─ js/
│  │  └─ img/
│  ├─ Areas/                             # (opcjonalnie) ASP.NET Identity / Admin
│  ├─ Controllers/                       # CarsController, TripsController, AccountController
│  ├─ Mapping/                           # WebMappingProfile.cs (DTO -> VM)
│  ├─ Models/                            # ViewModel-e (TripVm, CarVm, SharedVm)
│  ├─ Services/                          # CurrentUserService, helpers
│  ├─ UI/                                # elementy UI (np. FuelTypeItems.cs)
│  ├─ Views/                             # Razor Views (Cars, Trips, Shared)
│  ├─ appsettings.json                   # konfiguracja aplikacji
│  └─ Program.cs                         # punkt wejścia + pipeline
│
└─ TripSplit.Tests/                      # testy jednostkowe (xUnit + FluentAssertions)
   ├─ Application/                       # testy CQRS / handlerów
   ├─ Domain/                            # testy logiki domenowej
   ├─ Infrastructure/                    # testy EF, repozytoriów
   ├─ Support/                           # mocki, helpery, utils
   └─ UnitTest1.cs                       # przykładowy test

```

---

## 🔐 Bezpieczeństwo

- ✅ **ASP.NET Identity** — logowanie, rejestracja, szyfrowane hasła  
- ✅ **Anti-Forgery tokeny** (`@Html.AntiForgeryToken()`)  
- ✅ **Ochrona XSS / CSRF / SQL Injection** (ASP.NET Core)  
- ✅ **Filtry [Authorize]** w kontrolerach  

---

## 🗺️ Roadmap (plan rozwoju)

- 🌍 Integracja z Google Maps (automatyczne obliczanie dystansu)  
- 💶 Wiele walut i dynamiczne ceny paliw  
- 📩 Powiadomienia e-mail o kończących się OC/badaniach  
- 📱 Aplikacja mobilna (Blazor Hybrid / MAUI)  
- 📊 Eksport raportów (PDF / Excel)  

---

## 🧾 Licencja

Projekt udostępniany na licencji **MIT**.  
Zobacz pełną treść w pliku [LICENSE](LICENSE).

---

## 👨‍💻 Autor

**Mariusz Sternol**  
Projekt stworzony jako nowoczesna aplikacja edukacyjno-produkcyjna w ASP.NET Core.  
📧 kontakt: `MariuszSternol@outlook.com`  
🌐 GitHub: [@MSternol](https://github.com/MSternol)

---

> 💡 Jeśli aplikacja Ci się podoba — zostaw ⭐ na repozytorium!  
> Pomaga to rozwijać projekt i motywuje do dalszej pracy 🚀
