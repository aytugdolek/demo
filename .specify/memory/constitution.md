<!--
Sync Impact Report
Version change: unversioned-template -> 1.0.0
Modified principles:
- Template Principle 1 -> I. Clean Architecture Boundaries
- Template Principle 2 -> II. CLI-First User Experience
- Template Principle 3 -> III. Test-First Delivery (NON-NEGOTIABLE)
- Template Principle 4 -> IV. Contracted Data & Persistence Discipline
- Template Principle 5 -> V. Documentation-Backed Decisions
Added sections:
- Technical Standards & Constraints
- Delivery Workflow & Quality Gates
Removed sections:
- None
Templates requiring updates:
- ✅ .specify/templates/plan-template.md
- ✅ .specify/templates/spec-template.md
- ✅ .specify/templates/tasks-template.md
- ✅ .github/agents/speckit.tasks.agent.md
- ✅ .github/agents/speckit.constitution.agent.md
Follow-up TODOs:
- None
-->

# Colorado Business Entity Transaction History CLI Constitution

## Core Principles

### I. Clean Architecture Boundaries
The solution MUST enforce Clean Architecture boundaries. Domain entities and
rules belong in Core. Use cases, ports, orchestration, and validation belong in
Application. External API clients, SQL access, migrations, and configuration
belong in Infrastructure. Spectre.Console commands and terminal rendering belong
in Cli. Cross-layer shortcuts, UI-driven business logic, and direct
Infrastructure dependencies from Core are prohibited.

Rationale: This product combines CLI interaction, external API contracts, and
local persistence; strict boundaries keep the code testable and resilient to
change.

### II. CLI-First User Experience
Every user-facing capability MUST be accessible through the CLI. Running the
application with no parameters MUST open the interactive paging workflow, render
20 records per page, and support `>`, `<`, and `Q`. `--download` MUST execute as
a distinct workflow that creates a local file from the Socrata query endpoint.
CLI output MUST remain readable in a terminal-first workflow, and long fields
MUST be truncated or otherwise handled intentionally rather than dumped raw.

Rationale: The CLI is the product, not a thin wrapper around internal services.

### III. Test-First Delivery (NON-NEGOTIABLE)
Implementation MUST follow xUnit-based TDD. For every user story, tests MUST be
written first, MUST fail before implementation starts, and MUST pass before
refactoring is considered complete. Core and Application logic MUST have unit
tests. Paging behavior, API mapping, and database integration points MUST have
targeted tests at the appropriate layer. No feature is complete without the
tests needed to demonstrate red-green-refactor evidence.

Rationale: The application mixes external data contracts, console behavior, and
database state; regressions are cheapest to catch through test-first delivery.

### IV. Contracted Data & Persistence Discipline
The Socrata dataset, its documented fields, and SQL persistence concerns MUST be
modeled as explicit contracts. External DTOs MUST remain separate from domain
models and persistence entities. The application MUST use Microsoft.Data.SqlClient
with Windows authentication against `localhost\DEMO` and `DemoDb` for local
development, and schema evolution MUST be managed through FluentMigrator.
Connection strings, app tokens, and other environment-specific values MUST stay
out of hard-coded business logic.

Rationale: External schemas and local storage evolve independently, so explicit
mapping and migration discipline prevent brittle coupling.

### V. Documentation-Backed Decisions
Implementation, architecture, and troubleshooting decisions MUST prefer
authoritative sources in this order: official Socrata dataset documentation,
Context7 library documentation, and Microsoft Docs or MCP-backed Microsoft
documentation for .NET, C#, SQL Server, and client libraries. Decisions that
materially affect architecture, contracts, or operations MUST be recorded in repo
documentation or planning artifacts. Unverified blog-driven decisions MUST NOT be
adopted when official guidance is available.

Rationale: This project spans a fast-moving .NET stack and an external API;
documented sources reduce guesswork and rework.

## Technical Standards & Constraints

- The runtime MUST target `.NET 10` and use the latest C# language features
	supported by the chosen SDK.
- Spectre.Console MUST be the CLI presentation library.
- SQL Server Developer Edition 17.0 on `localhost\DEMO` with database `DemoDb`
	is the development persistence target.
- FluentMigrator MUST manage schema changes.
- The primary remote API MUST be the Colorado Business Entity Transaction History
	dataset `casm-dbbj`.
- The default experience MUST page 20 items at a time.
- The solution MUST remain operable on Windows local developer workstations.

## Delivery Workflow & Quality Gates

- Feature specifications MUST define independently testable user stories, edge
	cases, functional requirements, success criteria, and explicit assumptions.
- Implementation plans MUST pass a Constitution Check before research or design
	proceeds and again after design artifacts are produced.
- Task lists MUST place test tasks before implementation tasks for each user
	story and MUST preserve Clean Architecture boundaries in file paths and code
	ownership.
- Migration strategy, API contract handling, and configuration management MUST be
	defined before story implementation begins when those concerns affect the
	feature.
- Pull requests and reviews MUST verify constitution compliance, test evidence,
	and `.gitignore` hygiene for generated or local-only artifacts.

## Governance

- This constitution overrides conflicting local conventions, ad hoc preferences,
	and template defaults.
- Amendments MUST update `.specify/memory/constitution.md` and synchronize
	impacted templates, agent files, and workflow guidance in the same change.
- Semantic versioning applies to this constitution: MAJOR for incompatible
	governance changes or principle removal or redefinition, MINOR for new
	principles or materially expanded mandates, PATCH for clarifications and
	non-semantic wording fixes.
- Compliance review is mandatory for every plan, task list, and pull request.
	Violations MUST be fixed or explicitly justified in the relevant artifact
	before implementation continues.
- Ratification occurs when this constitution is first committed for the
	repository. Subsequent amendments MUST update the Last Amended date and the
	Sync Impact Report.

**Version**: 1.0.0 | **Ratified**: 2026-03-16 | **Last Amended**: 2026-03-16
