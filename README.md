# MulliganDeck

![CI](https://github.com/javierpradad/MulliganDeck/actions/workflows/ci.yml/badge.svg)

A REST API for managing Magic: The Gathering card collections and decks, with
deck validation according to each format's rules (Commander, Standard, etc.).

**Live demo:** https://mulligandeck-api.onrender.com/api/cards

> Note: hosted on a free tier, so the first request after inactivity may take
> ~30 seconds to wake up.

## Tech stack

- **.NET 10** / ASP.NET Core
- **Entity Framework Core** + **PostgreSQL**
- **Docker** & Docker Compose
- **JWT** authentication with role-based authorization
- **xUnit** (unit and integration tests)
- **GitHub Actions** (CI)

## Architecture

Layered architecture with dependencies pointing inward toward the domain:

- **Domain** — entities and business rules, no external dependencies.
- **Infrastructure** — data access (EF Core), external integrations (Scryfall).
- **Api** — controllers, authentication, configuration.

The domain layer contains the core game logic (deck validation) as pure C#,
fully unit-tested and independent of the database or web framework.

## Features

- Full card catalog imported from the [Scryfall API](https://scryfall.com/docs/api)
  (~38,000 cards) via streaming bulk import
- Background worker that keeps the catalog in sync daily
- Deck validation engine (size, copy limits, color identity)
- User registration and login with hashed passwords (BCrypt) and JWT
- Multi-user data isolation — each user sees only their own decks and collection
- Role-based authorization (admin-only endpoints)

## Running locally

1. Copy the example environment file:
```
cp .env.example .env
```
2. Fill in `.env` with your own values (database password, JWT key, admin password).
3. Start everything:
```
docker compose up --build
```
4. The API will be available at http://localhost:8080

## Tests
```
dotnet test
```
Includes unit tests for the deck validator and integration tests for the API
(using an in-memory SQLite database).

## Roadmap

- Deck and collection management endpoints (add/remove cards, validate)
- Card printings and prices on demand
- Web frontend
