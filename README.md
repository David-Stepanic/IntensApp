# IntensApp

**IntensApp** je .NET (C#) web API za upravljanje kandidatima i njihovim vestinama. Projekat koristi **Entity Framework Core** i **PostgreSQL** za bazu podataka, sa podrskom za case-insensitive pretragu, jedinstvene vrednosti i sprecavanje neplaniranih duplikata.

---

## Tehnologije

- .NET 8 Web API
- C#
- Entity Framework Core
- PostgreSQL
- Swagger (OpenAPI)
- Postman (za testiranje)

---

## Struktura projekta

- **Models/** – entiteti: `Candidate`, `Skill`, `CandidateSkill`
- **DTOs/** – DTO objekti za update i read (`CandidateUpdateDto`, `CandidateReadDto`)
- **Repositories/** – sloj za pristup bazi (CRUD operacije)
- **Services/** – biznis logika, normalizacija podataka, validacija
- **Data/** – `AppDbContext` i seed podaci
- **Controllers/** – API endpointi (CRUD + pretraga)

---

## Baza podataka (PostgreSQL)

- Tip `DateTime` u modelima mapiran kao `timestamp with time zone` zbog rada sa PostgreSQL bazom
- Ogranicenja:
  - Email kandidata je jedinstven
  - Imena vestina su jedinstvena
  - Case-insensitive pretraga i upit
  - Sprecavanje neplaniranih duplikata u vestinama kandidata
- Seed podaci u `AppDbContext`:
  - Vestine: `C# Programming`, `Java Programming`, `JavaScript`, `PHP`, `Golang`, `Rust`
  - Kandidati: 4 primera sa povezanim vestinama

---

## Instalacija i pokretanje

1. **Kloniranje repozitorijuma**
   ```bash
   git clone <repo-url>
   cd IntensApp

2. **Ucitavanje paketa**
   ```bash
   dotnet restore

3. **Konfiguracija konekcije ka PostgreSQL**
   Dodati u appsettings.json:
     
   ```json
    {
      "ConnectionStrings": 
      {
        "DefaultConnection": "Host=localhost;Database=intensapp;Username=postgres;Password=password"
      }
    }

4. **Napraviti migracije i update baze**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update

5. **Pokrenuti aplikaciju**
   ```bash
   dotnet run
   
6. **SwaggerUI**
   Otvoriti u browseru:
   http://localhost:5138/swagger/index.html




   
