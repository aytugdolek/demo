# Quickstart: CLI Paging MVP

## Goal

Validate the Feature 2 paging workflow once implemented: configure the Socrata app token, run the test suite, and exercise the interactive CLI against the live dataset.

## Prerequisites

- Windows workstation
- .NET 10 SDK installed
- PowerShell session from the repository root
- Valid Socrata app token for the Colorado dataset

## Configure Local Environment

Set the app token before starting the CLI:

```powershell
$env:Socrata__AppToken = "<your-app-token>"
```

Optional: keep the existing baseline validation path available if you also want to verify the database connection string contract from Feature 1.

```powershell
$env:ConnectionStrings__DemoDb = "Server=localhost\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;"
```

## Run Tests

```powershell
dotnet test
```

Expected result:

- Application paging tests pass.
- Infrastructure remote-contract tests pass without requiring the live API.
- CLI contract tests pass for no-argument startup, navigation input handling, and quit behavior.

## Run The Interactive Paging Demo

```powershell
dotnet run --project src/Cli
```

Expected result:

- The first page of live transaction-history results is displayed.
- The page shows 20 rows when data is available.
- Rows appear newest first by received date.
- The terminal displays stable columns for transaction ID, entity ID, name, history, and received date.
- Local prebuilt startup measurement with a deterministic mock response completed in approximately 1.74 seconds, which is within the 10-second MVP target.

## Navigate The Session

- Enter `>` to move to the next page.
- Enter `<` to move to the previous page.
- Enter `Q` to exit the session.

Expected result:

- Moving backward from page 1 keeps the session on page 1 and shows boundary feedback.
- Requesting a page with no further results shows a clear message.
- Entering `Q` exits cleanly.