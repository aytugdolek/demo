# Quickstart: Solution Skeleton

## Goal

Verify the Feature 1 baseline once implemented: restore the solution, build it, run tests, and validate configuration through the explicit CLI path.

## Prerequisites

- Windows workstation
- .NET 10 SDK installed
- SQL Server Developer Edition reachable at `localhost\DEMO` if you want to use the planned local connection string value
- PowerShell session from the repository root

## Configure Local Environment

Set the required environment variables before running the validation path:

```powershell
$env:Socrata__AppToken = "<your-app-token>"
$env:ConnectionStrings__DemoDb = "Server=localhost\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;"
```

## Restore, Build, and Test

```powershell
dotnet restore
dotnet build
dotnet test
```

## Validate Baseline Configuration

```powershell
dotnet run --project src/Cli -- --validate-config
```

Expected result:

- The CLI reports configuration validation success.
- No live Socrata request is made.
- No live database operation is required.

## Smoke-Test Normal Startup

```powershell
dotnet run --project src/Cli
```

Expected result:

- The CLI starts successfully.
- The baseline-ready message is shown.
- The run does not depend on configuration being present unless `--validate-config` is invoked.