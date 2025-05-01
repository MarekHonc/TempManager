# Webová aplikace pro řízení vytápění

TempManager je webová aplikace vyvinutá pro **Technickou univerzitu v Liberci**, konkrétně pro **řízení vytápění budovy A Fakulty mechatroniky**. Umožňuje zaměstnancům monitorovat a upravovat teplotu v jednotlivých místnostech v reálném čase. Aplikace využívá existující infrastrukturu **KNX** a **PLC Tecomat Foxtrot**, která nabízí moderní, bezpečné a responzivní uživatelské rozhraní.

## ✨ Funkce

- Centrální řízení teploty v místnostech
- Monitoring teploty, vlhkosti a CO₂
- Automatická detekce nových místností
- Správa oprávnění a uživatelů
- Admin rozhraní s historií zásahů a dat
- Přihlášení přes Shibboleth (SSO)
- Nasazení pomocí Dockeru

## 🧱 Architektura

**Použitý stack:**

- **Backend:** .NET 8 (ASP.NET Core), Entity Framework, SignalR  
- **Frontend:** Knockout.js, Bootstrap  
- **Databáze:** PostgreSQL  
- **Autentizace:** Shibboleth (SAML 2.0)  
- **Nasazení:** Docker, Apache jako reverse proxy  

## 🚀 Nasazení

1. Klonuj repozitář:
   ```bash
   git clone https://github.com/MarekHonc/TempManager.git
   cd TempManager
   ```

2. Spusť kontejnery:
   ```bash
   docker compose pull
   docker compose up
   ```

3. Otevři aplikaci:  
   Lokálně na `http://localhost:8080`  
   V produkci: [https://vytapeni.tul.cz](https://vytapeni.tul.cz)

## 🔐 Autentizace

Aplikace využívá **Shibboleth** pro jednotné přihlášení. Identity Provider univerzity zajišťuje ověření uživatelů a předává jejich údaje prostřednictvím hlaviček do backendu.

## ⚙️ Administrace

Administrátoři mají přístup k:
- Správě místností, podlaží a map
- Uživatelským oprávněním
- Historii měření a změn teplot
- Zpětné vazbě od uživatelů

## 🧩 Modulární struktura

Projekt je rozdělen do několika knihoven:
- `Web` – hlavní frontend a backend
- `BL` – business logika
- `DL` – datová vrstva a přístup k DB
- `Shibboleth` – autentizace
- `PLC Api Client` – komunikace s Foxtrotem
- `Common` – sdílené typy

## 🔄 Udržitelnost

- Automatická retence historických dat (14 dní)
- Docker umožňuje jednoduchý upgrade a nasazení
- Obnova SSL certifikátů pomocí Certbotu
- Aktualizace pomocí `docker compose pull && up`

## 👨‍💻 Autor

**Marek Honc**  
Diplomová práce – Technická univerzita v Liberci (2025)  
Repozitář: [github.com/MarekHonc/TempManager](https://github.com/MarekHonc/TempManager)
