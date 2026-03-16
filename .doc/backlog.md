# Product Backlog

## Purpose

This backlog turns the existing product documentation into a practical delivery
sequence for incremental feature delivery.

It is optimized for short demoable increments instead of attempting the whole
project in one pass.

## Source Documents

This backlog is derived from:

- `README.md`
- `product-requirements.md`
- `technical-standards.md`

## Delivery Strategy

The project should be implemented in small vertical slices that remain aligned
with the constitution:

- Clean Architecture boundaries are preserved from the first feature.
- xUnit TDD is used in every slice.
- Spectre.Console is used for all user-facing CLI behavior.
- Socrata API integration is isolated from CLI rendering and persistence.
- SQL Server and FluentMigrator are introduced deliberately rather than mixed
  into the first UI demo unless needed by that slice.

## Recommended Features

### Feature 1 - Solution Skeleton [COMPLETED]

**Branch Name**: `001-solution-skeleton`

**Goal**: Establish the .NET 10 Clean Architecture solution and testing baseline.

**Why first**: Every later demo depends on a stable project structure, test
setup, and dependency boundaries.

**Scope**:

- Create the solution and projects for `src/Core`, `src/Application`,
  `src/Infrastructure`, and `src/Cli`
- Add xUnit test projects
- Configure shared SDK settings for `.NET 10` and latest C# features
- Add base configuration loading for API token and SQL connection string
- Add placeholder interfaces for API access and persistence

**Demo outcome**:

- Solution builds successfully
- Test projects run successfully
- Clean Architecture project references are in place

### Feature 2 - CLI Paging MVP

**Branch Name**: `002-cli-paging-mvp`

**Goal**: Deliver the first usable CLI demo with live API paging and terminal
navigation.

**Why now**: This is the core product value and the most important visible demo.

**Scope**:

- Call the Socrata dataset query endpoint
- Use the configured app token
- Display 20 records per page in Spectre.Console
- Support `>`, `<`, and `Q`
- Render stable columns for transaction history records
- Keep paging logic testable independently from rendering

**Demo outcome**:

- Running the app without parameters fetches page 1
- Users can move forward and backward through pages
- The CLI exits cleanly with `Q`

### Feature 3 - Download Workflow

**Branch Name**: `003-download-workflow`

**Goal**: Add the non-interactive `--download` path.

**Why next**: It is a self-contained feature with clear user value and low UI
complexity.

**Scope**:

- Add `--download` command handling
- Call `https://data.colorado.gov/api/v3/views/casm-dbbj/query.json`
- Save the response to a local file
- Report success or failure clearly in the CLI
- Cover file creation logic with tests where practical

**Demo outcome**:

- Running the app with `--download` creates a file locally
- The result is separate from the interactive paging flow

### Feature 4 - Database and Migration Baseline

**Branch Name**: `004-db-migration-baseline`

**Goal**: Introduce SQL Server connectivity and FluentMigrator without coupling
it to the earlier CLI slices.

**Why here**: Persistence is important, but it should not slow down the first
product demos.

**Scope**:

- Configure SQL Server access for `localhost\DEMO`
- Target database `DemoDb`
- Use Windows Authentication and timeout `0`
- Add FluentMigrator runner setup
- Create the first baseline migration
- Define persistence entities separately from API DTOs and domain models

**Demo outcome**:

- The app or maintenance workflow can validate database connectivity
- The baseline migration can run successfully

### Feature 5 - Persistence-Aware Data Flow

**Branch Name**: `005-persistence-aware-flow`

**Goal**: Decide and implement how local persistence participates in the product.

**Why separate**: The docs leave this as an open decision, so it should be
explicitly designed instead of implied.

**Scope options**:

- Cache API responses locally for reuse
- Store downloaded records for offline queries
- Track sync metadata and last retrieval state

**Decision gate**:

Before planning this iteration, decide whether the default paging flow remains
live-only or can optionally use locally persisted data.

**Demo outcome**:

- A clear persistence story exists and is observable in the app behavior

### Feature 6 - Polish and Operator Experience

**Branch Name**: `006-operator-experience`

**Goal**: Improve diagnostics, resilience, and demo quality.

**Scope**:

- Improve CLI status and error messages
- Add structured logging where useful
- Improve truncation and record presentation
- Add quickstart steps for local setup and demo execution
- Harden network timeout and failure handling

**Demo outcome**:

- The CLI is easier to operate and easier to demonstrate live

## Prioritized Backlog Items

## P1

- Create the Clean Architecture .NET solution skeleton
- Implement interactive paging CLI MVP
- Add xUnit coverage for paging and application logic

## P2

- Add `--download` workflow
- Add SQL Server and FluentMigrator baseline support
- Define persistence mapping boundaries

## P3

- Add local caching or persistence-aware workflows
- Improve diagnostics and demo polish
- Add richer operational documentation

## Demo Order

For a practical demo sequence, use this order:

1. Feature 1: show the architecture and test scaffolding
2. Feature 2: show the live CLI paging MVP
3. Feature 3: show `--download`
4. Feature 4: show migration and database connectivity
5. Feature 5: show the chosen persistence behavior
6. Feature 6: show the polished operator experience

## Notes for Future Backlog Use

- Keep each feature narrowly scoped to one demoable outcome.
- Do not combine paging UI, database persistence, and download behavior into one
  oversized feature unless a later feature genuinely requires it.
- Preserve TDD in every feature even when the change is mostly CLI-facing.
- Use the official Socrata docs, Context7, and Microsoft Docs when research is
  required during planning.