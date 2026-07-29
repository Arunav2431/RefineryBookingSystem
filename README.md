# NRL Conference Hall Booking System

This repository contains the NRL Conference Hall Booking System built using .NET Core MVC and Entity Framework Core.

## Prerequisites

Before setting up the project, ensure you have the following installed on your machine:
- [.NET SDK 10.0](https://dotnet.microsoft.com/download) (or the version specified in the project)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB is fine for development)
- Git

## Local Development Setup Guide

If you are cloning or pulling this repository for the first time, follow these steps to get the application running locally on your machine.

### 1. Clone the Repository
Open your terminal (Command Prompt, PowerShell, or Git Bash) and run:
```bash
git clone <your-repository-url>
cd RefineryBookingSystem
```

### 2. Restore Dependencies
Restore all the required NuGet packages and dependencies:
```bash
dotnet restore
```

### 3. Apply Database Migrations
This project uses Entity Framework Core for database management. You need to create the local database schema and seed the initial data (Users, Roles, Conference Rooms, Cost Centres) by running:
```bash
dotnet ef database update
```
*(Note: If you don't have the EF Core tools installed globally, run `dotnet tool install --global dotnet-ef` first).*

### 4. Build the Project
Compile the project to ensure there are no build errors:
```bash
dotnet build
```

### 5. Run the Application
Start the local development server:
```bash
dotnet run
```
Once it is running, open your web browser and navigate to the URL provided in the console (usually `http://localhost:5000` or `https://localhost:5001`).

---

## Useful Commands Cheatsheet

- **Add a new migration** (after changing a model in `Models/`):
  ```bash
  dotnet ef migrations add <MigrationName>
  ```
- **Update the database** (after pulling new migrations from the team):
  ```bash
  dotnet ef database update
  ```
- **Clean the build output**:
  ```bash
  dotnet clean
  ```
