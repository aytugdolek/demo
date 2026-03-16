# Data Model: Solution Skeleton

## Overview

This feature does not introduce business-domain persistence yet. The design model focuses on structural and configuration entities that the baseline solution must define before later vertical slices are added.

## Entities

### SolutionProject

- Purpose: Represents a project in the solution and the architectural role it plays.
- Fields:
  - `name`: canonical project name
  - `path`: repository-relative project path
  - `layer`: `Core`, `Application`, `Infrastructure`, `Cli`, `UnitTests`, `IntegrationTests`, or `ContractTests`
  - `outputType`: `Library` or `Exe`
  - `allowedReferences`: list of project layers this project may reference
  - `responsibilities`: short list of allowed concerns for the project
- Validation rules:
  - `Core` references no solution layer.
  - `Application` references only `Core`.
  - `Infrastructure` references only `Application` and `Core`.
  - `Cli` references only `Application`.
  - Test projects may reference the production project they verify and shared test utilities if later introduced.

### RuntimeConfiguration

- Purpose: Represents externally supplied settings required to enable later API and persistence features.
- Fields:
  - `socrataAppToken`: string, required for explicit configuration validation
  - `demoDbConnectionString`: string, required for explicit configuration validation
  - `configurationSources`: ordered list of providers used by the host
  - `isValidated`: boolean flag derived from validation
- Validation rules:
  - `socrataAppToken` must be non-empty when validation is invoked.
  - `demoDbConnectionString` must be non-empty and parseable when validation is invoked.
  - Values must come from configuration providers, not hard-coded constants in application logic.
- State transitions:
  - `Missing` → `Provided`
  - `Provided` → `Validated`
  - `Provided` → `Invalid`

### IntegrationPort

- Purpose: Represents an application-layer seam for remote data or local persistence behavior.
- Fields:
  - `name`: interface name
  - `category`: `RemoteData` or `Persistence`
  - `requestModel`: abstract input contract name
  - `responseModel`: abstract output contract name
  - `implementedBy`: Infrastructure adapter name or placeholder
  - `status`: `Placeholder` or `Implemented`
- Validation rules:
  - Ports live in `src/Application`.
  - Port contracts must not expose infrastructure-specific transport or SQL types.
  - Implementations belong in `src/Infrastructure`.

### ValidationOutcome

- Purpose: Represents the result of the explicit baseline configuration validation path.
- Fields:
  - `checkName`: validation check identifier
  - `isSuccess`: boolean
  - `severity`: `Info`, `Warning`, or `Error`
  - `message`: human-readable result
- Validation rules:
  - Error outcomes produce a non-success validation result.
  - Messages must identify the missing or invalid setting without exposing secret values.

## Relationships

- `SolutionProject` defines where each `IntegrationPort` is declared or implemented.
- `RuntimeConfiguration` is evaluated by the validation workflow and produces one or more `ValidationOutcome` records.
- `Cli` consumes validation results but does not own configuration retrieval logic.

## Notes

- No business-domain entities for transaction history records are added in this feature.
- API DTOs and persistence entities are intentionally deferred to later slices that implement live access or storage behavior.