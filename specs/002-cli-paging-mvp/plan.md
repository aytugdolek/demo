# Implementation Plan: CLI Paging MVP

**Branch**: `002-cli-paging-mvp` | **Date**: 2026-03-16 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/002-cli-paging-mvp/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

Deliver the first usable CLI demo for Colorado business entity transaction history by replacing the baseline no-argument startup with a live interactive paging workflow. The implementation will call the Socrata `query.json` dataset endpoint with the configured app token, request deterministic newest-first pages of 20 records, return ordered page results from Application, render stable columns with Spectre.Console tables in `src/Cli`, and keep navigation, mapping, and remote access testable independently through Application and Infrastructure boundaries.

## Technical Context

**Language/Version**: C# targeting .NET 10 with latest supported C# features  
**Primary Dependencies**: Spectre.Console, Microsoft.Extensions.Hosting, Microsoft.Extensions.Options.ConfigurationExtensions, System.Net.Http, xUnit  
**Storage**: N/A for this feature; no local persistence, migrations, or cache writes are introduced  
**Testing**: xUnit with Application and Core unit tests, Infrastructure integration tests using controlled HTTP seams, and CLI contract tests  
**Target Platform**: Windows local developer workstations running the .NET CLI
**Project Type**: Clean Architecture CLI application  
**Performance Goals**: Show the first populated page in under 10 seconds under normal network conditions; keep page navigation responsive enough for live demos  
**Constraints**: Running with no arguments must enter the interactive workflow; pages are fixed at 20 records; navigation is limited to `>`, `<`, and `Q`; records must be ordered newest-first with a deterministic tie-breaker; CLI rendering must remain readable for long fields; `src/Cli` acts as the composition root and may reference Infrastructure only for dependency-registration and host wiring, not for business logic  
**Scale/Scope**: One interactive paging workflow, one Socrata dataset integration, one remote adapter, one set of stable terminal columns, and tests for navigation, mapping, remote access, and CLI behavior

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Clean Architecture boundaries are explicit in the planned directory structure:
  `src/Core`, `src/Application`, `src/Infrastructure`, `src/Cli`, and aligned
  test projects.
- CLI-first behavior is defined for the feature, including no-argument behavior,
  explicit interactive controls, and any non-interactive modes such as
  `--download`.
- xUnit TDD is planned for every user story, with failing tests scheduled before
  implementation tasks.
- External API contracts, SQL persistence boundaries, configuration handling,
  and FluentMigrator impact are identified when the feature touches those
  concerns.
- Research and design sources include authoritative guidance from official docs,
  Context7, and Microsoft Docs or MCP resources when applicable.

**Initial Gate Result**: PASS

- Clean Architecture boundaries remain explicit: paging orchestration will live in `src/Application`, Socrata access in `src/Infrastructure/Remote`, and terminal rendering in `src/Cli`.
- `src/Cli` is the composition root for the runnable app and may wire Infrastructure registrations, but feature behavior and business rules still flow through Application contracts.
- CLI-first behavior is fully defined for this slice: no arguments start paging, `>` and `<` navigate, and `Q` exits.
- TDD remains mandatory, with unit, contract, and integration coverage planned before implementation tasks are generated.
- Remote contract shape, configuration reuse, and the explicit absence of SQL or FluentMigrator work in this feature are identified before implementation.
- Research uses the official Socrata dataset docs, Spectre.Console documentation, and Microsoft .NET configuration guidance.

## Project Structure

### Documentation (this feature)

```text
specs/002-cli-paging-mvp/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
src/
├── Core/
├── Application/
├── Infrastructure/
└── Cli/

tests/
├── Unit/
├── Integration/
└── Contract/
```

**Structure Decision**: Use the existing repository layout already established by Feature 1. Implementation will extend `src/Application`, `src/Infrastructure/Remote`, and `src/Cli`, while tests land in the existing `tests/Unit/Application.UnitTests`, `tests/Unit/Core.UnitTests`, `tests/Integration/Infrastructure.IntegrationTests`, and `tests/Contract/Cli.ContractTests` projects. No new top-level projects are required for this slice. `src/Cli` will serve as the composition root and may add a project reference to `src/Infrastructure` strictly to wire runtime dependencies.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |

## Phase 0: Research Output

- [research.md](./research.md) documents the selected Socrata request pattern, deterministic ordering strategy, terminal rendering approach, configuration reuse, and TDD test topology.

## Phase 1: Design Output

- [data-model.md](./data-model.md) defines paging session state, remote query/result models, CLI table rows owned by `src/Cli`, and navigation commands.
- [contracts/cli-contract.md](./contracts/cli-contract.md) defines the interactive CLI behavior, controls, output rules, and exit semantics for the paging MVP.
- [contracts/remote-transaction-history-contract.md](./contracts/remote-transaction-history-contract.md) defines the application-facing remote paging contract and the dataset field mapping required for this slice.
- [quickstart.md](./quickstart.md) documents how to configure the app token, run tests, and exercise the interactive paging demo once implementation begins.

## Post-Design Constitution Check

**Post-Design Gate Result**: PASS

- The design keeps the CLI as the product surface while isolating page navigation, remote access, and rendering behind layer-appropriate contracts.
- Test-first delivery remains explicit across unit, integration, and contract levels.
- External dataset fields and request rules are documented before implementation, while SQL and FluentMigrator are explicitly out of scope for this slice.
- Design decisions are backed by official Socrata, Spectre.Console, and Microsoft documentation.
