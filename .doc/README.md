# Product Documentation

This folder contains the product-level documentation for the Colorado Business Entity Transaction History CLI.

## Documents

- `product-requirements.md`: product goals, user behavior, CLI features, and external API contract.
- `technical-standards.md`: stack decisions, architecture rules, testing standards, database configuration, and implementation constraints.

## Product Summary

The product is a .NET 10 command-line application that retrieves Colorado Business Entity Transaction History data from the Socrata dataset `casm-dbbj` and presents it in a paged terminal UI.

The default experience is interactive paging with 20 records per page.

User controls:

- `>`: next page
- `<`: previous page
- `Q`: quit

The CLI also supports a `--download` mode that calls the dataset query endpoint and saves the response to a file for offline use.

## Authoritative Sources Used

- Socrata dataset documentation: `https://dev.socrata.com/foundry/data.colorado.gov/casm-dbbj`
- Download/query endpoint: `https://data.colorado.gov/api/v3/views/casm-dbbj/query.json`
- Context7 library references used for implementation guidance:
  - Spectre.Console
  - FluentMigrator
  - xUnit.net
- Microsoft documentation and code samples used for .NET language-version and SQL connection-string guidance.