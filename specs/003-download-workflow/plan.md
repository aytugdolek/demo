# Implementation Plan: Download Workflow

**Branch**: `003-download-workflow` | **Date**: 2026-03-20 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/003-download-workflow/spec.md`

## Summary

Add the non-interactive `--download` CLI path so the user can fetch the Colorado transaction-history payload, save it to a local file, and receive a clear success or failure message without entering the interactive paging workflow. The implementation will keep the CLI as the product surface, move download orchestration into Application, keep file I/O in Infrastructure, and preserve the existing paging flow when `--download` is not used.

## Technical Context

**Language/Version**: C# targeting .NET 10 with the latest supported C# features  
**Primary Dependencies**: Spectre.Console, Microsoft.Extensions.Hosting, Microsoft.Extensions.Options, System.Net.Http, System.IO, xUnit  
**Storage**: Local file output only; no database or migration changes in this slice  
**Testing**: xUnit unit tests, Infrastructure integration tests for file-writing and remote seams, and CLI contract tests for command behavior  
**Target Platform**: Windows local developer workstations running the .NET CLI  
**Project Type**: Clean Architecture CLI application  
**Performance Goals**: Complete a successful download within 30 seconds under normal network conditions and avoid any interactive prompt during the `--download` path  
**Constraints**: `--download` must bypass the paging loop; existing no-argument paging behavior must remain unchanged; existing files must not be overwritten; clean architecture boundaries must remain intact; the feature must stay test-first  
**Scale/Scope**: One new CLI command path, one download orchestration use case, one file-writing adapter, and targeted tests for success, conflict, and failure scenarios

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Clean Architecture boundaries are explicit in the planned directory structure: `src/Core`, `src/Application`, `src/Infrastructure`, `src/Cli`, and the aligned test projects.
- CLI-first behavior is defined for the feature, including the existing no-argument paging flow and the new non-interactive `--download` path.
- xUnit TDD is planned for every user story, with failing tests scheduled before implementation tasks.
- External data handling, local file storage, and configuration behavior are identified before implementation begins.
- Research and design artifacts are backed by the repository constitution and existing CLI architecture.

**Initial Gate Result**: PASS

- The planned structure keeps command parsing in `src/Cli`, orchestration in `src/Application`, and file I/O in `src/Infrastructure`.
- The feature does not introduce SQL Server or FluentMigrator work.
- The download workflow is explicitly separable from the paging workflow and can be tested independently.
- Existing paging behavior remains the default when `--download` is omitted.

## Project Structure

### Documentation (this feature)

```text
specs/003-download-workflow/
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

**Structure Decision**: Extend the existing solution layout rather than introducing new top-level projects. Add the download use case and related abstractions under `src/Application`, the file-writing adapter under `src/Infrastructure`, and the `--download` command path plus file-location reporting under `src/Cli`. Keep tests in the existing `tests/Unit/Application.UnitTests`, `tests/Integration/Infrastructure.IntegrationTests`, and `tests/Contract/Cli.ContractTests` projects.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |

## Phase 0: Research Output

- [research.md](./research.md) documents the selected orchestration approach, file-conflict handling, command-line behavior, and test split for the download workflow.

## Phase 1: Design Output

- [data-model.md](./data-model.md) defines the download request/result shapes, file target rules, and workflow state transitions.
- [contracts/cli-contract.md](./contracts/cli-contract.md) defines the `--download` command contract, success output, and failure handling.
- [quickstart.md](./quickstart.md) documents how to configure the environment and run the download workflow once implemented.

## Post-Design Constitution Check

**Post-Design Gate Result**: PASS

- The design keeps the CLI as the entry point while isolating download orchestration and file I/O behind application and infrastructure seams.
- The user-visible contract is now explicit for success, failure, and existing-file handling.
- Test-first delivery remains explicit across unit, integration, and CLI contract levels.
- No constitution conflict was introduced by the design.
