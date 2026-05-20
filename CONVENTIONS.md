# Conventions & Décisions Techniques — CovaldysPilot

## Architecture
- Clean Architecture : Domain → Application → Infrastructure → API
- DTOs dans la couche Application (partagés entre Application et API)
- Mappers dans la couche API
- Interfaces dans la couche Application
- Implémentations dans la couche Infrastructure
- Méthodes d'extension pour les injections de dépendances (pas dans Program.cs)
- Configurations Fluent API dans des dossiers séparés

## Base de données
- Guid pour tous les IDs (généré par EF Core, pas dans l'entité)
- BaseEntity : Id (Guid), CreatedAt (DateTime), UpdatedAt (DateTime?)
- Images stockées en URL (string), pas en binaire en DB
- SiteConfiguration : singleton, pas de BaseEntity

## Entités Domain
- Nommage en anglais dans le code
- Membre → User
- Evenement → Event
- Inscription → SignIn
- Avis → Review
- Article → Article
- Catégorie → Category

## Décisions métier
- Article.Author (string) → nom affiché sur le site (libre)
- Article.UserId (FK) → pour l'audit (quel compte a écrit)
- EventCategory → table de liaison manuelle (pas auto EF Core)
- SiteConfiguration → pas de BaseEntity (singleton)
- EventStatus.EnAttente → valeur par défaut dans l'entité (règle métier)
- Rate Limiting désactivé en Development, actif en Production

## Enums
- Role : Membre, Admin
- Genre : Homme, Femme, Autre
- EventStatus : EnAttente, EnCours, Termine, Annule

## Sécurité
- JWT : 15 minutes
- RefreshToken : 7 jours
- BCrypt pour les mots de passe
- Rate Limiting : 30 req/min global, 5 req/5min pour auth

## Conventions Git
- Branches : feature/nom-ticket (ex: feature/B1-03-inscription-membre)
- Commits : [B1-03] Message descriptif
- Ne jamais commiter sur main directement
- Toujours passer par develop

## Technologies
- Backend : ASP.NET Core 10, EF Core, SQL Server
- Frontend : Angular 21, Angular Material
- Logger : Serilog (Console + File)
- Déploiement : Docker + GitHub Actions CI/CD

## Fichiers sensibles (jamais commités)
- appsettings.Development.json
- appsettings.Local.json
- .env
- Logs/