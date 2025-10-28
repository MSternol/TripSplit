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

Otwórz plik **`TripSplit.Web/appsettings.json`** i upewnij się, że masz ustawione połączenie do **LocalDB**  
(lub własnej instancji SQL Server):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TripSplitDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

> Jeśli projekt był wcześniej na .NET 8, upewnij się, że **TargetFramework** w plikach `.csproj` to `net9.0`:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

(Opcjonalnie) Zablokuj wersję SDK w repo poprzez `global.json`:

```json
{
  "sdk": {
    "version": "9.0.100"
  }
}
```

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

👉 https://localhost:5001  
lub  
👉 http://localhost:5000

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
TripSplit.Web/
 ├─ Controllers/
 │   ├─ CarsController.cs
 │   ├─ TripsController.cs
 │   └─ AccountController.cs
 ├─ Views/
 │   ├─ Cars/
 │   ├─ Trips/
 │   └─ Shared/
 ├─ wwwroot/
 │   ├─ css/ (site.css, cars.css, trips.css, fix-pack.css)
 │   ├─ js/  (theme.js, nav.js)
 │   └─ lib/ (fonty, ikony)
 ├─ Mapping/
 └─ Services/
```

---

## 🔐 Bezpieczeństwo

- ✅ **ASP.NET Identity** — logowanie, rejestracja, szyfrowane hasła  
- ✅ **Anti-Forgery tokeny** (`@Html.AntiForgeryToken()`)  
- ✅ **Ochrona XSS / CSRF / SQL Injection** (ASP.NET Core)  
- ✅ **Filtry `[Authorize]`** w kontrolerach  

---

## 🗺️ Roadmap (plan rozwoju)

- 🌍 Integracja z Google Maps (automatyczne obliczanie dystansu)  
- 💶 Wiele walut i dynamiczne ceny paliw  
- 📩 Powiadomienia e-mail o kończących się OC/badaniach  
- 📱 Aplikacja mobilna
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
