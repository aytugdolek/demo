# Product Documentation

This folder contains the product-level documentation for the Colorado Business Entity Transaction History CLI.

## Documents

- `backlog.md`: iteration backlog and Speckit workflow plan for demoing the project step by step.
- `product-requirements.md`: product goals, user behavior, CLI features, and external API contract.
- `technical-standards.md`: stack decisions, architecture rules, testing standards, database configuration, and implementation constraints.

## Product Summary

The product is a .NET 10 command-line application that retrieves Colorado Business Entity Transaction History data from the Socrata dataset `casm-dbbj` and presents it in a paged terminal UI.

The repository now includes the Feature 1 solution skeleton baseline:

- a Clean Architecture solution with `Core`, `Application`, `Infrastructure`, and `Cli` projects
- aligned xUnit projects for core, application, infrastructure, and CLI contract coverage
- centralized SDK and package version management at the repository root
- an explicit `--validate-config` CLI path for verifying the Socrata token and DemoDb connection string without live integration calls

The default experience is interactive paging with 20 records per page.

User controls:

- `>`: next page
- `<`: previous page
- `Q`: quit

The CLI also supports a `--download` mode that calls the dataset query endpoint and saves the response to a file for offline use.

## Current Baseline Commands

From the repository root:

- `dotnet restore`
- `dotnet build`
- `dotnet test`
- `dotnet run --project src/Cli`
- `dotnet run --project src/Cli -- --validate-config`

## Authoritative Sources Used

- Socrata dataset documentation: `https://dev.socrata.com/foundry/data.colorado.gov/casm-dbbj`
- Download/query endpoint: `https://data.colorado.gov/api/v3/views/casm-dbbj/query.json`
- Context7 library references used for implementation guidance:
  - Spectre.Console
  - FluentMigrator
  - xUnit.net
- Microsoft documentation and code samples used for .NET language-version and SQL connection-string guidance.