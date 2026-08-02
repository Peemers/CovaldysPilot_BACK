# 🌿 CovaldysPilot API

> API backend en Clean Architecture pour la gestion d'une communauté : membres, événements, articles et avis.

[![Build and Test .NET](https://github.com/Peemers/covaldys-api/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/Peemers/covaldys-api/actions/workflows/build-and-test.yml)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![EF Core](https://img.shields.io/badge/EF%20Core-10.0.8-512BD4)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoftsqlserver&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&logoColor=white)
![License](https://img.shields.io/badge/license-non%20définie-lightgrey)

---

## Sommaire

- [Présentation](#présentation)
- [Fonctionnalités](#fonctionnalités)
- [Captures d'écran](#captures-décran)
- [Démonstration](#démonstration)
- [Architecture](#architecture)
- [Arborescence du projet](#arborescence-du-projet)
- [Technologies](#technologies)
- [Packages NuGet](#packages-nuget)
- [Prérequis](#prérequis)
- [Installation](#installation)
- [Variables d'environnement](#variables-denvironnement)
- [Configuration](#configuration)
- [Utilisation](#utilisation)
- [Documentation API](#documentation-api)
- [Base de données](#base-de-données)
- [Qualité du code](#qualité-du-code)
- [Tests](#tests)
- [Docker](#docker)
- [GitHub Actions / CI-CD](#github-actions--ci-cd)
- [Déploiement](#déploiement)
- [Performance](#performance)
- [Sécurité](#sécurité)
- [Roadmap](#roadmap)
- [Bonnes pratiques](#bonnes-pratiques)
- [Contribution](#contribution)
- [Versioning](#versioning)
- [Licence](#licence)
- [Auteur](#auteur)
- [Remerciements](#remerciements)

---

## Présentation

**CovaldysPilot** est une API REST développée en **ASP.NET Core 10** qui sert de backend à une plateforme de gestion communautaire. Elle permet à une organisation de gérer ses **membres (utilisateurs)**, d'organiser des **événements** (avec catégories, tarifs et statuts), de suivre les **inscriptions** à ces événements, de publier des **articles**, et de collecter des **avis (reviews)**.

Le projet résout le besoin d'une association ou structure similaire de centraliser :
- la gestion des adhérents et de leurs rôles (Membre / Admin),
- la planification et le suivi du cycle de vie des événements,
- la communication (articles, emails transactionnels),
- la modération et l'administration via des endpoints dédiés.

Il est destiné à être consommé par un frontend séparé (un projet `CovaldysPilot_FRONT` en Angular est référencé dans le `docker-compose.yml`, mais n'est pas inclus dans ce dépôt).

> 🚧 À compléter — nom officiel de l'organisation/association cible (le fichier `Lesson.md` mentionne en interne "GreenPilot", à clarifier).

---

## Fonctionnalités

### 👤 Gestion des utilisateurs
- Inscription, authentification et gestion de profil (`UserController`)
- Rôles `Membre` / `Admin`
- Administration des comptes (`AdminUserController`)

### 🔐 Authentification & sécurité
- Connexion via JWT + refresh token (`AuthController`)
- Rafraîchissement de session sécurisé

### 📅 Événements
- Création, consultation et gestion des événements (`EventController`)
- Catégorisation des événements (`CategoryController`)
- Statuts de cycle de vie : `EnAttente`, `EnCours`, `Termine`, `Annule` (avec motif d'annulation)
- Tarification des événements
- Mise à jour automatique du statut des événements via un service en arrière-plan (`EventStatusBackgroundService`)
- Administration des événements (`AdminEventController`)

### ✍️ Inscriptions aux événements
- Inscription des membres à un événement (`SignInController`)
- Administration des inscriptions (`AdminSignInController`)

### 📰 Articles
- Publication et consultation d'articles avec images (`ArticleController`)

### ⭐ Avis
- Dépôt et consultation d'avis (`ReviewController`)

### ⚙️ Configuration du site
- Paramètres globaux de type singleton (`AdminSiteConfigurationController`)

### 🛠️ Fonctionnalités transverses
- Envoi d'emails transactionnels (MailKit)
- Stockage d'images sur Azure Blob Storage
- Export de données Excel (ClosedXML)
- Journalisation structurée (Serilog, console + fichier)
- Limitation de débit (Rate Limiting) globale et sur l'authentification

---

## Captures d'écran

> 🚧 Aucune capture d'écran n'est présente dans ce dépôt à ce jour. Ce projet étant une API sans interface graphique propre, les captures pertinentes proviendraient de la documentation Scalar (`/scalar`) ou du frontend associé.

---

## Démonstration

> 🚧 Aucune démonstration publique (URL live) n'est référencée dans ce dépôt.

- **API** : exposée en local via Docker sur `http://localhost:5000` (voir [Docker](#docker)).
- **Frontend** : un projet séparé `CovaldysPilot_FRONT` est référencé dans `docker-compose.yml` (dossier `../CovaldysPilot_FRONT`, hors de ce dépôt).

---

## Architecture

Le projet suit une **Clean Architecture** stricte, avec un flux de dépendances à sens unique documenté dans `CONVENTIONS.md` :

```text
                ┌───────────────────────────┐
                │   CovaldysPilot.API        │  ← Controllers, Middlewares, Program.cs
                │   (ASP.NET Core Web API)   │
                └─────────────┬──────────────┘
                              │ dépend de
                ┌─────────────▼──────────────┐
                │ CovaldysPilot.Infrastructure│  ← EF Core, Repositories, JWT, Email,
                │                             │     Blob Storage, Background Services
                └─────────────┬──────────────┘
                              │ dépend de
                ┌─────────────▼──────────────┐
                │  CovaldysPilot.Application  │  ← DTOs, Mappers, Services,
                │                             │     Interfaces (Repositories/Services)
                └─────────────┬──────────────┘
                              │ dépend de
                ┌─────────────▼──────────────┐
                │    CovaldysPilot.Domain     │  ← Entités, Enums, BaseEntity
                │      (cœur métier, 0 dép.)  │
                └────────────────────────────┘

                CovaldysPilot.Tests ──► référence Application + Domain
```

**Principes appliqués (`CONVENTIONS.md` / `Lesson.md`) :**
- Le `Domain` ne dépend d'aucune autre couche.
- Les **DTOs**, **Mappers** et **interfaces** (Repositories/Services) vivent dans `Application`.
- Les **implémentations concrètes** (Repositories, JWT, Email, Blob Storage) vivent dans `Infrastructure`.
- `Program.cs` reste minimaliste : toute la configuration DI passe par des **méthodes d'extension** (`AddApplicationServices`, `AddInfrastructureServices`, `AddJwtAuthentication`, `AddCorsPolicy`, `AddRateLimiterPolicies`).
- Pas de MediatR ni de CQRS : approche **KISS** assumée, service layer classique.
- Pas d'AutoMapper : mapping via des méthodes d'extension statiques dédiées.

---

## Arborescence du projet

```text
CovaldysPilot.API/             # Point d'entrée : Controllers, Middlewares, Program.cs, Scalar
├── Controllers/                # Endpoints REST (Auth, User, Event, Article, Review, Admin*...)
├── Extensions/                 # DI : Jwt, Cors, RateLimiter
├── Middlewares/                # ExceptionMiddleware (gestion centralisée des erreurs)
├── Scalar/                     # Transformer OpenAPI (schéma Bearer)
└── Properties/launchSettings.json

CovaldysPilot.Application/     # Logique applicative, indépendante de l'infra
├── DTOs/                        # Contrats d'entrée/sortie par domaine (Article, Event, User...)
├── Services/                    # Logique métier (AuthService, EventService, ...)
├── Mappers/                     # Entity <-> DTO (extension methods)
├── Interfaces/Repositories/     # Contrats des repositories
├── Interfaces/Services/         # Contrats des services
├── Extensions/                  # AddApplicationServices (DI)
├── Helpers/                     # PasswordHelper, ExcelHelper
└── Email/Templates/             # Templates d'emails

CovaldysPilot.Domain/          # Cœur métier, aucune dépendance externe
├── Entities/                     # User, Event, Article, Review, SignIn, Category, ...
└── Enums/                        # Role, Genre, EventStatus

CovaldysPilot.Infrastructure/   # Implémentations techniques
├── DataBase/Context/              # CovaldysPilotDbContext
├── DataBase/Configurations/        # Fluent API EF Core (séparée des entités)
├── Migrations/                     # Historique des migrations EF Core
├── Repositories/                    # Implémentations concrètes
├── Security/                        # JwtService
├── Email/                           # EmailService, EmailSettings
├── Services/                        # BlobStorageService, EventStatusBackgroundService
└── Extensions/                      # AddInfrastructureServices (DI)

CovaldysPilot.Tests/            # Tests unitaires (xUnit + NSubstitute)

.github/workflows/                # CI/CD GitHub Actions
Dockerfile                        # Build multi-stage de l'API
docker-compose.yml                 # SQL Server + API + Angular (frontend externe)
CONVENTIONS.md                     # Décisions techniques et conventions du projet
```

---

## Technologies

| Technologie | Version | Usage |
|---|---|---|
| .NET / ASP.NET Core | 10.0 | Framework de l'API |
| Entity Framework Core | 10.0.8 | ORM, migrations |
| SQL Server | 2022 (image Docker) | Base de données relationnelle |
| Scalar.AspNetCore | 2.14.14 | Documentation API interactive (OpenAPI) |
| Serilog | 10.0.0 (AspNetCore) | Journalisation structurée (console + fichier) |
| JWT Bearer | 10.0.8 | Authentification par jeton |
| BCrypt.Net-Next | 4.2.0 | Hachage des mots de passe |
| Azure.Storage.Blobs | 12.29.0 | Stockage des images |
| MailKit | 4.17.0 | Envoi d'emails transactionnels |
| ClosedXML | 0.105.0 | Génération de fichiers Excel |
| xUnit | 2.9.2 | Framework de tests |
| NSubstitute | 5.1.0 | Mocking pour les tests |
| Docker | — | Conteneurisation de l'API et de la base |
| GitHub Actions | — | Intégration et livraison continues |

---

## Packages NuGet

**CovaldysPilot.API**
| Package | Rôle |
|---|---|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | Authentification JWT au niveau de l'API |
| `Microsoft.AspNetCore.OpenApi` | Génération du document OpenAPI |
| `Microsoft.EntityFrameworkCore.Design` | Outillage des migrations EF Core |
| `Scalar.AspNetCore` | Interface de documentation API |
| `Serilog.AspNetCore` / `Serilog.Sinks.Console` / `Serilog.Sinks.File` | Journalisation |

**CovaldysPilot.Application**
| Package | Rôle |
|---|---|
| `BCrypt.Net-Next` | Hachage des mots de passe |
| `ClosedXML` | Export Excel |
| `Microsoft.Extensions.DependencyInjection.Abstractions` | Contrats DI |
| `Microsoft.Extensions.Logging.Abstractions` | Contrats de logging |
| `Microsoft.Extensions.Options` | Pattern Options |

**CovaldysPilot.Infrastructure**
| Package | Rôle |
|---|---|
| `Azure.Storage.Blobs` | Stockage des fichiers/images |
| `BCrypt.Net-Next` | Hachage des mots de passe |
| `ClosedXML` | Export Excel |
| `MailKit` | Envoi d'emails |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | Génération/validation JWT |
| `Microsoft.EntityFrameworkCore.Design` / `.SqlServer` | ORM et migrations |
| `Microsoft.Extensions.Configuration.Abstractions` / `.Options` | Configuration typée |

**CovaldysPilot.Tests**
| Package | Rôle |
|---|---|
| `xunit` / `xunit.runner.visualstudio` | Framework et exécution des tests |
| `NSubstitute` / `NSubstitute.Analyzers.CSharp` | Mocking |
| `Microsoft.NET.Test.Sdk` | SDK de test |
| `coverlet.collector` | Collecte de couverture de code |

---

## Prérequis

| Outil | Version |
|---|---|
| .NET SDK | 10.0.x |
| Docker & Docker Compose | requis pour l'exécution conteneurisée |
| SQL Server | 2022 (fourni via `docker-compose.yml`, ou instance locale) |
| Git | — |

> 🚧 Node.js/Angular ne sont requis que pour le frontend `CovaldysPilot_FRONT`, qui est un dépôt séparé non inclus ici.

---

## Installation

### 1. Cloner le dépôt

```bash
git clone https://github.com/Peemers/covaldys-api.git
cd covaldys-api
```

### 2. Configurer l'environnement

```bash
cp .env.example .env
# renseigner les valeurs (connection string, JWT, email, Azure Storage, CORS...)
```

### 3. Restaurer et builder

```bash
dotnet restore
dotnet build --configuration Release
```

### 4. Base de données & migrations

Les migrations sont appliquées **automatiquement au démarrage** de l'API (`db.Database.Migrate()` dans `Program.cs`). Aucune commande manuelle n'est requise si `ConnectionStrings:DefaultConnection` est correctement configurée.

Pour les appliquer manuellement (développement) :

```bash
dotnet ef database update --project CovaldysPilot.Infrastructure --startup-project CovaldysPilot.API
```

> ⚠️ `CONVENTIONS.md` précise que le schéma de base de données doit rester stable — éviter de générer de nouvelles migrations sans validation préalable.

### 5. Lancer l'API en local

```bash
dotnet run --project CovaldysPilot.API
```

L'API démarre sur `http://localhost:5005` (profil `http`) ou `https://localhost:7124` (profil `https`), voir `launchSettings.json`.

### 6. Frontend

Le frontend Angular (`CovaldysPilot_FRONT`) est un dépôt distinct, référencé uniquement par `docker-compose.yml` (`../CovaldysPilot_FRONT`). Se référer à sa propre documentation.

---

## Variables d'environnement

Définies dans `.env.example` (valeurs vides, aucun secret réel) et consommées via `docker-compose.yml` :

| Variable | Description | Requis |
|---|---|---|
| `SA_PASSWORD` | Mot de passe SQL Server (conteneur) | ✅ |
| `CONNECTION_STRING` | Chaîne de connexion SQL Server de l'API | ✅ |
| `AZURE_STORAGE_CONNECTION_STRING` | Chaîne de connexion Azure Blob Storage | ✅ |
| `AZURE_STORAGE_CONTAINER_NAME` | Nom du conteneur Blob (images) | ✅ |
| `JWT_SECRET` | Clé secrète de signature JWT | ✅ |
| `JWT_ISSUER` | Émetteur du JWT | ✅ |
| `JWT_AUDIENCE` | Audience du JWT | ✅ |
| `JWT_EXPIRY_MINUTES` | Durée de vie du token d'accès (défaut métier : 15 min) | ✅ |
| `JWT_REFRESH_EXPIRY_DAYS` | Durée de vie du refresh token (défaut métier : 7 jours) | ✅ |
| `EMAIL_HOST` | Hôte SMTP | ✅ |
| `EMAIL_PORT` | Port SMTP | ✅ |
| `EMAIL_USERNAME` | Identifiant SMTP | ✅ |
| `EMAIL_PASSWORD` | Mot de passe SMTP | ✅ |
| `EMAIL_FROM` | Adresse d'expédition | ✅ |
| `EMAIL_FROM_NAME` | Nom d'expéditeur affiché | ✅ |
| `CORS_ALLOWED_ORIGINS` | Origines autorisées par la politique CORS | ✅ |

> 🔒 Aucune valeur réelle n'est présente ni dans `.env.example` ni dans ce README.

---

## Configuration

- **`appsettings.json`** : configuration par défaut/production (valeurs sensibles pointées vers `appsettings.Development.json`, qui est ignoré par Git).
- **`appsettings.Development.json`** : non versionné (listé dans les fichiers sensibles de `CONVENTIONS.md`), contient les valeurs réelles en local.
- **`launchSettings.json`** : deux profils, `http` (port 5005) et `https` (ports 7124/5005), environnement `Development`.
- **`docker-compose.yml`** : injecte la configuration via variables d'environnement (`ConnectionStrings__DefaultConnection`, `JwtSettings__*`, `EmailSettings__*`, `AzureStorage__*`, `CorsSettings__AllowedOrigins`), environnement forcé en `Production`.

---

## Utilisation

### Exemple — Authentification

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "********"
}
```

### Exemple — Appel authentifié

```http
GET /api/event
Authorization: Bearer <token>
```

> 🚧 Les schémas exacts de requête/réponse (DTOs) sont disponibles via la documentation interactive Scalar en environnement de développement (voir ci-dessous) plutôt que documentés manuellement ici, afin d'éviter toute divergence avec le code.

---

## Documentation API

La documentation interactive est générée via **Scalar** (`Scalar.AspNetCore`) à partir du document **OpenAPI** natif d'ASP.NET Core, et n'est exposée **qu'en environnement Development** (`Program.cs`) :

```
/scalar
```

Un schéma de sécurité **Bearer** est préconfiguré (`BearerSecuritySchemeTransformer`) pour tester les endpoints authentifiés directement depuis l'interface.

---

## Base de données

- **Moteur** : Microsoft SQL Server 2022.
- **ORM** : Entity Framework Core 10.0.8, avec configurations **Fluent API** séparées des entités (`Infrastructure/DataBase/Configurations`).
- **Migrations** (`Infrastructure/Migrations`) :
  1. `InitialCreate`
  2. `SeedAdminUser`
  3. `AddFirstNameLastNamePhoneToUser`
  4. `AddPriceToEvent`
  5. `AddCancellationReasonToEvent`
  6. `RemoveSiteConfigSeed`
  7. `AddSiteConfigurationSeed`
- **Application des migrations** : automatique au démarrage de l'API (`db.Database.Migrate()`).
- **Modèle** : entités `User`, `Event`, `EventCategory`, `Category`, `Article`, `ArticleImage`, `Review`, `SignIn`, `RefreshToken`, `SiteConfiguration`, toutes héritant de `BaseEntity` (`Id` Guid, `CreatedAt`, `UpdatedAt`) à l'exception de `SiteConfiguration` (singleton, sans `BaseEntity`).

---

## Qualité du code

Conventions réellement appliquées, documentées dans `CONVENTIONS.md` / `Lesson.md` :

- **Repository Pattern** : interfaces dans `Application/Interfaces/Repositories`, implémentations dans `Infrastructure/Repositories`.
- **Service Layer** : logique métier centralisée dans `Application/Services`.
- **Mapper Pattern** (sans AutoMapper) : méthodes d'extension statiques dans `Application/Mappers`.
- **Dependency Injection** via méthodes d'extension dédiées (`AddApplicationServices`, `AddInfrastructureServices`, `AddJwtAuthentication`, `AddCorsPolicy`, `AddRateLimiterPolicies`), gardant `Program.cs` minimaliste.
- **KISS** : pas de MediatR, pas de CQRS, pas d'abstractions génériques complexes (choix explicite documenté).
- **Nullable Reference Types** activé sur tous les projets, `ImplicitUsings` activé.
- **Nommage anglais** du code, indépendamment de la langue métier (ex : Membre → `User`, Evenement → `Event`, Inscription → `SignIn`).

---

## Tests

- **Framework** : xUnit 2.9.2, avec NSubstitute 5.1.0 pour le mocking.
- **Organisation** : un projet dédié `CovaldysPilot.Tests`, référençant `Application` et `Domain`.
- **Suites existantes** : `AuthServiceTests`, `CategoryServiceTests`, `SiteConfigurationServiceTests`.
- **Couverture** : `coverlet.collector` est présent comme dépendance, mais aucun seuil ou rapport de couverture n'est configuré dans la CI actuelle.

### Lancer les tests

```bash
dotnet test
```

---

## Docker

### Dockerfile (multi-stage)

- **Build** : `mcr.microsoft.com/dotnet/sdk:10.0`, restauration puis `dotnet publish` de `CovaldysPilot.API`.
- **Runtime** : `mcr.microsoft.com/dotnet/aspnet:10.0`, expose le port `8080`.

### docker-compose.yml

Orchestre trois services :

| Service | Image / Build | Port hôte |
|---|---|---|
| `sqlserver` | `mcr.microsoft.com/mssql/server:2022-latest` | 1433 |
| `api` | build via `Dockerfile` local | 5000 → 8080 |
| `angular` | build via `../CovaldysPilot_FRONT/Dockerfile` (dépôt externe) | 4200 → 80 |

Le service `api` attend que `sqlserver` soit `healthy` (healthcheck `sqlcmd`) avant de démarrer.

### Build & lancement

```bash
docker compose up --build
```

---

## GitHub Actions / CI-CD

Workflow : `.github/workflows/build-and-test.yml`

| Job | Déclencheur | Étapes |
|---|---|---|
| `build` | push sur `develop`/`main`, PR vers `main` | checkout → setup .NET 10 → `dotnet restore` → `dotnet build` (Release) → `dotnet test` |
| `docker` | uniquement après succès de `build`, et seulement sur push `main` | login `ghcr.io` → build & push de l'image API vers `ghcr.io/peemers/covaldys-api:latest` et `:<sha>` |

---

## Déploiement

- **Image Docker** publiée sur GitHub Container Registry : `ghcr.io/peemers/covaldys-api`, à chaque push sur `main` (après succès des tests).
- **Docker Compose** disponible pour un déploiement local/hébergé combinant API + SQL Server (+ frontend externe).

> 🚧 Aucune plateforme d'hébergement cible (Azure, AWS, VPS...) n'est référencée dans ce dépôt.

---

## Performance

> 🚧 Aucune optimisation de performance spécifique (ex. requêtes en lecture seule optimisées, mise en cache) n'a été identifiée dans le code actuel au-delà de la limitation de débit décrite ci-dessous.

---

## Sécurité

- **Authentification** : JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`), validation stricte de l'issuer, de l'audience, de la durée de vie et de la clé de signature (`JwtExtensions.cs`).
- **Autorisation** : basée sur les rôles (`Role` : Membre/Admin), claim `role` mappé explicitement.
- **Mots de passe** : hachés avec BCrypt (`PasswordHelper`).
- **Refresh tokens** : entité dédiée `RefreshToken`, expiration configurée (7 jours par défaut métier selon `CONVENTIONS.md`).
- **Rate Limiting** (`RateLimiterExtensions.cs`) : 30 requêtes/minute par IP globalement, bucket dédié à l'authentification (9 tokens, +3 toutes les 20s) — **désactivé en Development, actif en Production**.
- **CORS** : politique nommée `CovaldysPolicy`, origines configurables via `CorsSettings:AllowedOrigins`.
- **HTTPS** : redirection forcée (`UseHttpsRedirection`).
- **Gestion des erreurs** : middleware d'exception global (`ExceptionMiddleware`), premier de la pipeline.
- **Secrets** : jamais commités — `.env`, `appsettings.Development.json`, `appsettings.Local.json` et `Logs/` sont explicitement exclus (`CONVENTIONS.md`, `.gitignore`).

---

## Roadmap

> 🚧 Aucune roadmap explicite (issues, projet GitHub, fichier TODO) n'a été trouvée dans ce dépôt. État d'avancement déduit de l'historique des migrations :

- [x] Authentification JWT + refresh token
- [x] Gestion des utilisateurs et rôles
- [x] Gestion des événements, catégories et statuts
- [x] Inscriptions aux événements
- [x] Articles et avis
- [x] Configuration globale du site (singleton)
- [x] Pipeline CI/CD (build, tests, image Docker)
- [ ] 🚧 À définir par l'équipe — prochaines fonctionnalités

---

## Bonnes pratiques

- Documentation XML (`/// <summary>`) au-dessus des signatures de méthodes publiques.
- Aucun commentaire à l'intérieur du corps des méthodes — code auto-explicatif.
- `var` utilisé uniquement quand le type est explicite à l'instanciation.
- Interdiction du mot-clé `dynamic`.
- `.AsNoTracking()` recommandé pour les requêtes de lecture seule (règle documentée dans `Lesson.md`).

---

## Contribution

Ce projet suit un workflow Git strict, documenté dans `CONVENTIONS.md` :

1. **Fork** ou clone du dépôt.
2. Créer une branche depuis `develop` : `feature/nom-ticket` (ex. `feature/B1-03-inscription-membre`).
3. Committer avec le format : `[TICKET] Message descriptif`.
4. **Ne jamais committer directement sur `main`** — toujours passer par `develop`.
5. Ouvrir une Pull Request vers `develop` (ou `main` selon le processus de release en vigueur).

---

## Versioning

> 🚧 Aucun schéma de versionnage sémantique (tags, releases) n'a été détecté. Les images Docker sont taguées `latest` et par SHA de commit (`ghcr.io/peemers/covaldys-api:<sha>`).

---

## Licence

> 🚧 Aucun fichier `LICENSE` n'est présent dans ce dépôt. La licence reste **à définir**.

---

## Auteur

- **Math Peeters** — [Peemers](https://github.com/Peemers)

---

## Remerciements

> 🚧 Aucune mention de tiers, bibliothèque particulière à créditer au-delà des packages listés, ou contributeur externe n'a été trouvée dans ce dépôt.
