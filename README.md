# Introduction

Uses

- [MudBlazor](https://mudblazor.com/)
- Cosmos DB
- Azure Static Web Apps

# Getting Started

In order to run any projects you must have:

- Latest .NET SDK

In order to run the function app you must have:

- Docker

## Cosmos DB

There is some jank with the Cosmos DB emulator and the partition key settings. You must create the container (`DungeonDates`) manually after starting the application with a partition key of `/id`.

# Hosting
Domain has been setup on Porkbun and is hosted in Azure via a static web app with a managed Azure Function. Some of the configuration has been done manually in the portal itself.

# Debugging The Managed Function

If the managed function isn't working on the static web app, chances are a package is responsible for taking it out or an environment variable is missing and the functions host is crashing.