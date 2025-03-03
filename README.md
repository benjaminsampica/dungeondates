# Introduction

Uses

- [Radzen Components](https://blazor.radzen.com/)

# Getting Started

In order to run any projects you must have:

- Latest .NET SDK

In order to run the function app you must have:

- Docker

## Adding A Migration

```bash
dotnet ef migrations add InitialCreate -o Infrastructure/Databases/Migrations
```

_Note:_ Migrations cannot be applied using the managed host via `dbContext.Database.MigrateAsync()`. It simply crashes the functions host so they must be applied some other way (SQL script for example).

# Hosting
Domain has been setup on Porkbun and is hosted in Azure via a static web app with a managed Azure Function. Some of the configuration has been done manually in the portal itself.

# Debugging The Managed Function

If the managed function isn't working on the static web app, chances are a package is responsible for taking it out or an environment variable is missing and the functions host is crashing.