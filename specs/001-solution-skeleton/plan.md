# Implementation Plan: Solution Skeleton

**Branch**: `001-solution-skeleton` | **Date**: 2026-03-16 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-solution-skeleton/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

Establish the .NET 10 Clean Architecture solution baseline for the Colorado Business Entity Transaction History CLI. The implementation will create four production projects and aligned test projects, centralize SDK settings, wire host-based configuration for the Socrata token and SQL connection string, add placeholder integration ports, and expose a dedicated `--validate-config` CLI path while deferring live paging, download, and persistence behavior to later slices.

## Technical Context

<!--
  ACTION REQUIRED: Replace the content in this section with the technical details
  for the project. The structure here is presented in advisory capacity to guide
  the iteration process.
-->

**Language/Version**: C# targeting .NET 10, with `LangVersion` set to latest supported or `preview` if the installed SDK requires it for latest features  
**Primary Dependencies**: Spectre.Console, Microsoft.Extensions.Hosting, Microsoft.Extensions.Configuration, Microsoft.Extensions.Options, Microsoft.Data.SqlClient, FluentMigrator, xUnit  
**Storage**: SQL Server Developer Edition 17.0 on `localhost\DEMO`, database `DemoDb` (configuration only in this feature; no schema or data writes yet)  
**Testing**: xUnit with layer-aligned unit, integration, and CLI contract test projects  
**Target Platform**: Windows local developer workstations running a .NET CLI application
**Project Type**: Clean Architecture CLI application  
**Performance Goals**: Restore, build, and test the baseline on a prepared workstation within 10 minutes; configuration validation completes without network or database I/O  
**Constraints**: No live API or database required for baseline tests; no hard-coded secrets; dependency direction must remain constitution-compliant; configuration validation runs only through an explicit CLI path  
**Scale/Scope**: 4 production projects, 4 initial test projects, 2 required configuration values, and 2 placeholder application integration ports

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

- Clean Architecture boundaries are explicit in the planned solution and test topology.
- CLI-first behavior is defined for this slice as baseline startup plus explicit `--validate-config`; interactive paging and `--download` remain explicitly deferred to later features from the backlog.
- TDD remains mandatory, with test projects planned before feature behavior implementation.
- Configuration, remote contract seams, SQL boundaries, and FluentMigrator impact are identified even though live behavior is deferred.
- Research decisions are based on the repository’s technical standards and constitution, which already anchor official Socrata, Context7, and Microsoft documentation sources.

## Project Structure

### Documentation (this feature)

```text
specs/001-solution-skeleton/
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

**Structure Decision**: [Document the selected structure and reference the real
directories captured above]
Use the documented Clean Architecture layout from the constitution and technical standards. The repository currently contains planning artifacts only, and implementation will create `src/Core`, `src/Application`, `src/Infrastructure`, `src/Cli`, plus `tests/Unit/Core.UnitTests`, `tests/Unit/Application.UnitTests`, `tests/Integration/Infrastructure.IntegrationTests`, and `tests/Contract/Cli.ContractTests`.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |

## Phase 0: Research Output

- [research.md](./research.md) documents the selected solution shape, centralized build settings, host-based configuration design, explicit validation path, application ports, and test topology.

## Phase 1: Design Output

- [data-model.md](./data-model.md) defines the structural entities for projects, configuration, ports, and validation outcomes.
- [contracts/cli-contract.md](./contracts/cli-contract.md) defines the baseline CLI behaviors for no-argument startup and `--validate-config`.
- [contracts/configuration-contract.md](./contracts/configuration-contract.md) defines required configuration keys, source precedence, and validation rules.
- [quickstart.md](./quickstart.md) documents the expected restore, build, test, and validation workflow once implementation begins.

## Post-Design Constitution Check

**Post-Design Gate Result**: PASS

- The planned project and test structure preserves Clean Architecture boundaries.
- The feature’s CLI surface is documented without overreaching into later paging or download slices.
- TDD expectations remain explicit across unit, integration, and contract layers.
- Configuration and integration contracts are documented before implementation.
- No constitution violations require justification.
