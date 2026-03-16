# Configuration Contract: Solution Skeleton

## Purpose

Define the configuration shape required by the baseline solution and the explicit validation path.

## Required Keys

### Remote Access

- Logical key: `Socrata:AppToken`
- Environment variable form: `Socrata__AppToken`
- Requirement: required for `--validate-config`

### Local Persistence

- Logical key: `ConnectionStrings:DemoDb`
- Environment variable form: `ConnectionStrings__DemoDb`
- Requirement: required for `--validate-config`

## Expected Source Precedence

1. Command-line arguments
2. Environment variables
3. `appsettings.Development.json`
4. `appsettings.json`

## Validation Rules

- `Socrata:AppToken` must be non-empty.
- `ConnectionStrings:DemoDb` must be non-empty and parseable as a SQL Server connection string.
- The local development connection intent remains:

```text
Server=localhost\DEMO;Database=DemoDb;Integrated Security=True;Connect Timeout=0;TrustServerCertificate=True;
```

- Secret or machine-specific values must not be committed to source control.

## Notes

- Feature 1 validates presence and shape only. It does not require a live network call or live database connection.
- Additional configuration keys for paging, downloads, migrations, or logging can be introduced in later features without breaking this baseline contract.