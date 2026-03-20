# Quickstart: Download Workflow

## Goal

Validate the `--download` feature once implemented: configure the required environment, run the test suite, and exercise the non-interactive download path.

## Prerequisites

- Windows workstation
- .NET 10 SDK installed
- PowerShell session from the repository root
- Valid Socrata app token for the Colorado dataset

## Configure Local Environment

Set the Socrata app token before starting the CLI:

```powershell
$env:Socrata__AppToken = "<your-app-token>"
```

If you also want the baseline application host configuration available, keep the existing demo database connection string configured as well:

```powershell
$env:ConnectionStrings__DemoDb = "Server=localhost\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;"
```

## Run Tests

```powershell
dotnet test
```

Expected result:

- Application tests for download orchestration pass.
- Infrastructure tests for file-writing behavior pass.
- CLI contract tests pass for `--download`, success reporting, and existing-file failure handling.

## Run The Download Demo

```powershell
dotnet run --project src/Cli -- --download
```

Expected result:

- The command completes without entering the paging prompt loop.
- The CLI reports the saved file path.
- A single local file is created at `downloads/casm-dbbj-query.json` under the current working directory.

## Validate Failure Handling

- Run `--download` with the target file already present to confirm the command fails without overwriting it.
- Remove or invalidate the Socrata app token to confirm the command reports a clear failure message.
- Point `Socrata__MockResponsePath` at an empty or malformed JSON file if you need to confirm payload validation failures.
- Run from a directory that cannot be written to if you need to confirm local file-write failures.

## Notes

- The interactive paging flow should still work when `--download` is omitted.
- This slice does not add database persistence or migration steps.