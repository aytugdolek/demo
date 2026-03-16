# Tasks: Solution Skeleton

**Input**: Design documents from `/specs/001-solution-skeleton/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/, quickstart.md

**Tests**: Tests are REQUIRED by the constitution. Every user story phase starts with failing xUnit tests before implementation begins.

**Organization**: Tasks are grouped by user story so each story remains independently testable and demoable after the shared setup and foundational work is complete.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g. `US1`, `US2`, `US3`)
- Every task includes an exact file path

## Phase 1: Setup

**Purpose**: Initialize the repository-level solution artifacts and shared build configuration.

- [X] T001 Create the solution file at `Colorado.BusinessEntityTransactionHistory.sln`
- [X] T002 Create shared .NET SDK and compiler settings in `Directory.Build.props`
- [X] T003 [P] Create central package version management in `Directory.Packages.props`
- [X] T004 [P] Create the baseline CLI configuration file in `src/Cli/appsettings.json`

---

## Phase 2: Foundational

**Purpose**: Establish the project shells and shared test support that every user story depends on.

**⚠️ CRITICAL**: No user story work should begin until this phase is complete.

- [X] T005 [P] Create the core project file in `src/Core/Colorado.BusinessEntityTransactionHistory.Core.csproj`
- [X] T006 [P] Create the application project file in `src/Application/Colorado.BusinessEntityTransactionHistory.Application.csproj`
- [X] T007 [P] Create the infrastructure project file in `src/Infrastructure/Colorado.BusinessEntityTransactionHistory.Infrastructure.csproj`
- [X] T008 [P] Create the CLI project file in `src/Cli/Colorado.BusinessEntityTransactionHistory.Cli.csproj`
- [X] T009 [P] Create the core unit test project file in `tests/Unit/Core.UnitTests/Colorado.BusinessEntityTransactionHistory.Core.UnitTests.csproj`
- [X] T010 [P] Create the application unit test project file in `tests/Unit/Application.UnitTests/Colorado.BusinessEntityTransactionHistory.Application.UnitTests.csproj`
- [X] T011 [P] Create the infrastructure integration test project file in `tests/Integration/Infrastructure.IntegrationTests/Colorado.BusinessEntityTransactionHistory.Infrastructure.IntegrationTests.csproj`
- [X] T012 [P] Create the CLI contract test project file in `tests/Contract/Cli.ContractTests/Colorado.BusinessEntityTransactionHistory.Cli.ContractTests.csproj`
- [X] T013 Add the solution membership and Clean Architecture project references in `Colorado.BusinessEntityTransactionHistory.sln`
- [X] T014 [P] Create shared CLI command runner test support in `tests/Contract/Cli.ContractTests/TestSupport/CliCommandRunner.cs`

**Checkpoint**: The solution shells, project references, and test harness foundation exist; user stories can now be implemented.

---

## Phase 3: User Story 1 - Start From A Stable Solution Baseline (Priority: P1) 🎯 MVP

**Goal**: Deliver a buildable Clean Architecture solution where the default CLI startup shows a baseline-ready message without invoking live integrations.

**Independent Test**: Run `dotnet build`, then `dotnet run --project src/Cli` and confirm the CLI starts successfully, prints the baseline-ready output, and the project references still follow the planned dependency direction.

### Tests for User Story 1 ⚠️

- [X] T015 [P] [US1] Add a no-argument CLI contract test in `tests/Contract/Cli.ContractTests/BaselineStartupContractTests.cs`
- [X] T016 [P] [US1] Add a startup summary unit test in `tests/Unit/Application.UnitTests/Startup/GetBaselineStartupSummaryTests.cs`

### Implementation for User Story 1

- [X] T017 [P] [US1] Create the baseline startup summary model in `src/Application/Startup/BaselineStartupSummary.cs`
- [X] T018 [US1] Implement the baseline startup use case in `src/Application/Startup/GetBaselineStartupSummary.cs`
- [X] T019 [US1] Implement the baseline CLI command in `src/Cli/Commands/BaselineCommand.cs`
- [X] T020 [US1] Wire no-argument startup and `--help` handling in `src/Cli/Program.cs`

**Checkpoint**: User Story 1 provides a functioning solution shell and the first demoable CLI experience.

---

## Phase 4: User Story 2 - Verify The Baseline With Automated Tests (Priority: P2)

**Goal**: Prove the baseline can be developed test-first by adding runnable unit and integration coverage that does not require live services.

**Independent Test**: Run `dotnet test` and confirm the unit, integration, and contract test projects all pass without live network or database access.

### Tests for User Story 2 ⚠️

- [X] T021 [P] [US2] Add a core architecture unit test in `tests/Unit/Core.UnitTests/Architecture/SolutionProjectTests.cs`
- [X] T022 [P] [US2] Add an application validation outcome unit test in `tests/Unit/Application.UnitTests/Configuration/ValidationOutcomeTests.cs`
- [X] T023 [P] [US2] Add an infrastructure composition smoke test in `tests/Integration/Infrastructure.IntegrationTests/Composition/InfrastructureCompositionTests.cs`

### Implementation for User Story 2

- [X] T024 [P] [US2] Create the solution project model in `src/Core/Architecture/SolutionProject.cs`
- [X] T025 [P] [US2] Create the validation outcome model in `src/Application/Configuration/ValidationOutcome.cs`
- [X] T026 [US2] Implement application service registration in `src/Application/DependencyInjection.cs`
- [X] T027 [US2] Implement infrastructure service registration for smoke-testable composition in `src/Infrastructure/DependencyInjection.cs`

**Checkpoint**: User Story 2 establishes the repeatable automated test baseline required for later feature work.

---

## Phase 5: User Story 3 - Prepare Configuration And Integration Seams (Priority: P3)

**Goal**: Add explicit configuration validation and placeholder integration seams so later API and persistence slices can build on stable contracts.

**Independent Test**: Run `dotnet run --project src/Cli -- --validate-config` with valid and invalid configuration values and confirm the CLI reports validation success or failure without making live remote or database calls.

### Tests for User Story 3 ⚠️

- [X] T028 [P] [US3] Add a `--validate-config` CLI contract test in `tests/Contract/Cli.ContractTests/ValidateConfigContractTests.cs`
- [X] T029 [P] [US3] Add a runtime configuration validator unit test in `tests/Unit/Application.UnitTests/Configuration/RuntimeConfigurationValidatorTests.cs`
- [X] T030 [P] [US3] Add a configuration binding integration test in `tests/Integration/Infrastructure.IntegrationTests/Configuration/ConfigurationBindingIntegrationTests.cs`

### Implementation for User Story 3

- [X] T031 [P] [US3] Create the Socrata options contract in `src/Application/Configuration/SocrataOptions.cs`
- [X] T032 [P] [US3] Create the DemoDb options contract in `src/Application/Configuration/DemoDbOptions.cs`
- [X] T033 [P] [US3] Create the remote access port in `src/Application/Abstractions/IRemoteTransactionHistoryPort.cs`
- [X] T034 [P] [US3] Create the persistence port in `src/Application/Abstractions/IPersistencePort.cs`
- [X] T035 [US3] Implement the runtime configuration validator in `src/Application/Configuration/RuntimeConfigurationValidator.cs`
- [X] T036 [US3] Implement the placeholder remote adapter in `src/Infrastructure/Remote/PlaceholderRemoteTransactionHistoryAdapter.cs`
- [X] T037 [US3] Implement the placeholder persistence adapter in `src/Infrastructure/Persistence/PlaceholderPersistenceAdapter.cs`
- [X] T038 [US3] Update infrastructure registration for options and placeholder adapters in `src/Infrastructure/DependencyInjection.cs`
- [X] T039 [US3] Implement the configuration validation command in `src/Cli/Commands/ValidateConfigCommand.cs`
- [X] T040 [US3] Wire `--validate-config` handling in `src/Cli/Program.cs`

**Checkpoint**: User Story 3 adds the explicit validation path and stable application seams for future remote and persistence work.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Finish the slice with verification and documentation updates that affect the whole feature.

- [X] T041 [P] Update the baseline setup notes in `.doc/README.md`
- [X] T042 Run and verify the documented workflow in `specs/001-solution-skeleton/quickstart.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1: Setup** has no dependencies and starts immediately.
- **Phase 2: Foundational** depends on Phase 1 and blocks all user stories.
- **Phase 3: User Story 1** depends on Phase 2 and is the MVP slice.
- **Phase 4: User Story 2** depends on Phase 2 and can proceed independently after the foundation exists.
- **Phase 5: User Story 3** depends on Phase 2 and can proceed independently after the foundation exists.
- **Phase 6: Polish** depends on the user stories you want included in the delivery.

### User Story Dependencies

- **US1**: No dependency on other user stories after Phase 2.
- **US2**: No dependency on other user stories after Phase 2.
- **US3**: No dependency on other user stories after Phase 2, though it touches `src/Cli/Program.cs` and should merge carefully if worked in parallel with US1.

### Within Each User Story

- Tests must be written first and must fail before implementation begins.
- Models and contracts should be created before service or command wiring.
- Application and infrastructure wiring should be complete before CLI entry-point changes are finalized.
- Each story should be validated independently before moving on.

## Parallel Opportunities

- Phase 1 tasks `T003` and `T004` can run in parallel after `T001` and `T002` define the solution and shared build rules.
- Phase 2 tasks `T005` through `T012` and `T014` can run in parallel because they target separate project files and test support files.
- Within US1, `T015`, `T016`, and `T017` can run in parallel before `T018` through `T020` complete the flow.
- Within US2, `T021`, `T022`, and `T023` can run in parallel, followed by parallel model work in `T024` and `T025`.
- Within US3, `T028`, `T029`, and `T030` can run in parallel, followed by parallel contract and placeholder tasks `T031` through `T037` where file paths do not overlap.

## Parallel Example: User Story 1

```text
T015 tests/Contract/Cli.ContractTests/BaselineStartupContractTests.cs
T016 tests/Unit/Application.UnitTests/Startup/GetBaselineStartupSummaryTests.cs
T017 src/Application/Startup/BaselineStartupSummary.cs
```

## Parallel Example: User Story 2

```text
T021 tests/Unit/Core.UnitTests/Architecture/SolutionProjectTests.cs
T022 tests/Unit/Application.UnitTests/Configuration/ValidationOutcomeTests.cs
T023 tests/Integration/Infrastructure.IntegrationTests/Composition/InfrastructureCompositionTests.cs
```

## Parallel Example: User Story 3

```text
T031 src/Application/Configuration/SocrataOptions.cs
T032 src/Application/Configuration/DemoDbOptions.cs
T033 src/Application/Abstractions/IRemoteTransactionHistoryPort.cs
T034 src/Application/Abstractions/IPersistencePort.cs
```

## Implementation Strategy

### MVP First

1. Complete Phase 1.
2. Complete Phase 2.
3. Complete Phase 3 (US1).
4. Validate `dotnet build` and `dotnet run --project src/Cli`.
5. Demo the baseline solution shell before expanding scope.

### Incremental Delivery

1. Setup + Foundational creates the shared solution and test shells.
2. US1 delivers the first working CLI baseline.
3. US2 adds the repeatable automated test baseline.
4. US3 adds explicit configuration validation and placeholder integration seams.
5. Phase 6 updates docs and verifies the quickstart path.

### Parallel Team Strategy

1. One developer completes Setup and Foundational work.
2. After Phase 2, one developer can take US1, another US2, and another US3.
3. Coordinate changes to `src/Cli/Program.cs` between US1 and US3 to avoid merge conflicts.

## Notes

- MVP scope is **User Story 1** after Setup and Foundational work.
- All tasks follow the required checklist format with task ID, optional parallel marker, optional story label, and exact file path.
- Test tasks intentionally precede implementation tasks for every user story to satisfy the constitution’s TDD rule.