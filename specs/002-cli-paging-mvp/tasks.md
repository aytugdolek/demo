# Tasks: CLI Paging MVP

**Input**: Design documents from `/specs/002-cli-paging-mvp/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are REQUIRED by the constitution. Every user story phase MUST start with failing tests before implementation tasks begin.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., [US1], [US2], [US3])
- Include exact file paths in descriptions

## Path Conventions

- **Primary layout**: `src/Core`, `src/Application`, `src/Infrastructure`, `src/Cli`, and `tests/*` at repository root
- **Tests**: Prefer `tests/Unit`, `tests/Integration`, and `tests/Contract`
- **Contracts**: Keep remote API concerns in `src/Infrastructure/Remote` and CLI rendering in `src/Cli/Commands`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare the existing solution and test projects for live paging work without changing feature behavior yet.

- [X] T001 Update paging-related project dependencies in src/Infrastructure/Colorado.BusinessEntityTransactionHistory.Infrastructure.csproj, tests/Integration/Infrastructure.IntegrationTests/Colorado.BusinessEntityTransactionHistory.Infrastructure.IntegrationTests.csproj, and tests/Contract/Cli.ContractTests/Colorado.BusinessEntityTransactionHistory.Cli.ContractTests.csproj
- [X] T002 Update the CLI composition root to reference Infrastructure for runtime wiring while preserving `--validate-config` behavior in src/Cli/Program.cs and src/Cli/Colorado.BusinessEntityTransactionHistory.Cli.csproj

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Create the shared paging contracts, models, and test support that all user stories depend on.

**⚠️ CRITICAL**: No user story work can begin until this phase is complete.

- [X] T003 Create the paged remote contract in src/Application/Abstractions/IRemoteTransactionHistoryPort.cs and src/Application/Paging/RemoteTransactionHistoryQuery.cs
- [X] T004 [P] Create shared page result models in src/Application/Paging/TransactionHistoryPage.cs and src/Application/Paging/RemoteTransactionHistoryRecord.cs
- [X] T005 [P] Create shared session and input models in src/Application/Paging/PagingSessionState.cs and src/Application/Paging/NavigationCommand.cs
- [X] T006 [P] Create remote request and DTO scaffolding in src/Infrastructure/Remote/SocrataQueryRequest.cs and src/Infrastructure/Remote/SocrataTransactionHistoryRecordDto.cs
- [X] T007 Extend reusable interactive CLI test support in tests/Contract/Cli.ContractTests/TestSupport/CliCommandRunner.cs
- [X] T008 Configure shared paging registrations and composition-root wiring in src/Application/DependencyInjection.cs, src/Infrastructure/DependencyInjection.cs, and src/Cli/Program.cs

**Checkpoint**: Foundation ready - user story implementation can now begin.

---

## Phase 3: User Story 1 - View The First Page Of Live Results (Priority: P1) 🎯 MVP

**Goal**: Running the CLI with no arguments fetches the first live page and renders 20 newest-first records in stable columns.

**Independent Test**: Set `Socrata__AppToken`, run `dotnet run --project src/Cli`, and confirm the first page renders with stable columns and no unhandled errors.

### Tests for User Story 1 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [X] T009 [P] [US1] Add no-argument paging startup and `--validate-config` regression contract coverage in tests/Contract/Cli.ContractTests/PagingStartupContractTests.cs and tests/Contract/Cli.ContractTests/ValidateConfigContractTests.cs
- [X] T010 [P] [US1] Add first-page remote mapping integration test in tests/Integration/Infrastructure.IntegrationTests/Remote/SocrataTransactionHistoryAdapterTests.cs
- [X] T011 [P] [US1] Add initial page retrieval unit test in tests/Unit/Application.UnitTests/Paging/GetTransactionHistoryPageTests.cs

### Implementation for User Story 1

- [X] T012 [P] [US1] Implement the first-page application workflow in src/Application/Paging/GetTransactionHistoryPage.cs and src/Application/Paging/TransactionHistoryPageMapper.cs
- [X] T013 [P] [US1] Implement the Socrata paging adapter in src/Infrastructure/Remote/SocrataTransactionHistoryAdapter.cs
- [X] T014 [P] [US1] Implement stable row formatting in src/Cli/Commands/CliTableRow.cs and src/Cli/Commands/TransactionHistoryTableRowMapper.cs
- [X] T015 [US1] Implement the table renderer in src/Cli/Commands/TransactionHistoryTableRenderer.cs
- [X] T016 [US1] Replace the baseline no-argument flow with paging startup in src/Cli/Commands/InteractivePagingCommand.cs and src/Cli/Program.cs
- [X] T017 [US1] Replace placeholder remote registration with the live adapter in src/Infrastructure/DependencyInjection.cs and remove the obsolete placeholder path in src/Infrastructure/Remote/PlaceholderRemoteTransactionHistoryAdapter.cs
- [X] T018 [US1] Measure first-page startup against the 10-second goal and record the result in specs/002-cli-paging-mvp/quickstart.md

**Checkpoint**: User Story 1 should now show the first live page independently of later navigation and exit refinements.

---

## Phase 4: User Story 2 - Move Between Pages During A Session (Priority: P2)

**Goal**: Users can navigate forward and backward through paged live results while keeping page boundaries deterministic.

**Independent Test**: Start the CLI, enter `>`, verify the next page renders, then enter `<` and verify the previous page returns without restarting the app.

### Tests for User Story 2 ⚠️

- [X] T019 [P] [US2] Add paging navigation contract test in tests/Contract/Cli.ContractTests/PagingNavigationContractTests.cs
- [X] T020 [P] [US2] Add multi-page adapter integration test in tests/Integration/Infrastructure.IntegrationTests/Remote/SocrataPagingIntegrationTests.cs
- [X] T021 [P] [US2] Add paging state transition unit test in tests/Unit/Application.UnitTests/Paging/PagingSessionStateTests.cs

### Implementation for User Story 2

- [X] T022 [P] [US2] Implement navigation command parsing in src/Application/Paging/NavigationCommandParser.cs
- [X] T023 [P] [US2] Implement paging session state transitions in src/Application/Paging/PagingSessionController.cs
- [X] T024 [US2] Extend the remote adapter for deterministic next and previous page requests in src/Infrastructure/Remote/SocrataTransactionHistoryAdapter.cs
- [X] T025 [US2] Implement the interactive navigation loop in src/Cli/Commands/InteractivePagingCommand.cs
- [X] T026 [US2] Add page captions, current-page summaries, and first-page boundary feedback in src/Cli/Commands/TransactionHistoryTableRenderer.cs

**Checkpoint**: User Stories 1 and 2 should now work together, with independent verification of multi-page navigation.

---

## Phase 5: User Story 3 - End Or Recover The Paging Session Cleanly (Priority: P3)

**Goal**: Users can quit with `Q` and receive clear feedback for empty pages, invalid input, missing configuration, and upstream failures.

**Independent Test**: Start the CLI, verify `Q` exits cleanly, then simulate invalid input, empty results, and startup failures to confirm clear operator-facing messages.

### Tests for User Story 3 ⚠️

- [X] T027 [P] [US3] Add quit and failure-path contract test in tests/Contract/Cli.ContractTests/PagingExitAndFailureContractTests.cs
- [X] T028 [P] [US3] Add empty-page and upstream-failure integration test in tests/Integration/Infrastructure.IntegrationTests/Remote/SocrataPagingFailureIntegrationTests.cs
- [X] T029 [P] [US3] Add quit and invalid-input handling unit test in tests/Unit/Application.UnitTests/Paging/HandlePagingCommandTests.cs

### Implementation for User Story 3

- [X] T030 [P] [US3] Implement paging command result models in src/Application/Paging/PagingCommandResult.cs and src/Application/Paging/TransactionHistoryPageFailure.cs
- [X] T031 [P] [US3] Translate empty pages and transport failures in src/Infrastructure/Remote/SocrataTransactionHistoryAdapter.cs
- [X] T032 [US3] Implement quit, invalid-input, and failure-message handling in src/Cli/Commands/InteractivePagingCommand.cs
- [X] T033 [US3] Update help text and final operator messaging for the paging MVP in src/Cli/Program.cs and src/Cli/Commands/ValidateConfigCommand.cs

**Checkpoint**: All three user stories should now be independently testable and complete the CLI Paging MVP.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final consistency, documentation, and operator validation across the completed feature.

- [X] T034 [P] Update operator documentation for the paging demo in specs/002-cli-paging-mvp/quickstart.md and specs/002-cli-paging-mvp/contracts/cli-contract.md
- [X] T035 Run final quickstart validation and align final CLI messaging in src/Cli/Commands/InteractivePagingCommand.cs and specs/002-cli-paging-mvp/quickstart.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately.
- **Foundational (Phase 2)**: Depends on Setup completion - blocks all user stories.
- **User Story 1 (Phase 3)**: Depends on Foundational completion.
- **User Story 2 (Phase 4)**: Depends on Foundational completion and builds on the paging startup surface introduced in User Story 1.
- **User Story 3 (Phase 5)**: Depends on Foundational completion and integrates with the interactive session established by User Stories 1 and 2.
- **Polish (Phase 6)**: Depends on the desired user stories being complete.

### User Story Dependencies

- **User Story 1 (P1)**: Starts after Foundational and delivers the MVP first page.
- **User Story 2 (P2)**: Starts after Foundational but should be completed after User Story 1 because it extends the active paging session.
- **User Story 3 (P3)**: Starts after Foundational but should be completed after User Story 2 because it finalizes the interactive loop and operator feedback paths.

### Within Each User Story

- Tests MUST be written and MUST fail before implementation, including retained behavior such as `--validate-config` when an existing entry point is affected.
- Shared application models and orchestration come before CLI command wiring.
- Remote adapter behavior comes before final user-facing rendering validation.
- Story checkpoints are the required pause points for independent validation.

### Parallel Opportunities

- `T004` and `T005` can run in parallel after `T003` starts the paging contract shape.
- `T009`, `T010`, and `T011` can run in parallel for User Story 1.
- `T012`, `T013`, and `T014` can run in parallel after User Story 1 tests are in place.
- `T019`, `T020`, and `T021` can run in parallel for User Story 2.
- `T022` and `T023` can run in parallel before the CLI loop wiring in `T025`.
- `T027`, `T028`, and `T029` can run in parallel for User Story 3.
- `T030` and `T031` can run in parallel before the final CLI handling tasks.

---

## Parallel Example: User Story 1

```bash
# Launch User Story 1 tests together:
Task: "Add no-argument paging startup and --validate-config regression contract coverage in tests/Contract/Cli.ContractTests/PagingStartupContractTests.cs and tests/Contract/Cli.ContractTests/ValidateConfigContractTests.cs"
Task: "Add first-page remote mapping integration test in tests/Integration/Infrastructure.IntegrationTests/Remote/SocrataTransactionHistoryAdapterTests.cs"
Task: "Add initial page retrieval unit test in tests/Unit/Application.UnitTests/Paging/GetTransactionHistoryPageTests.cs"

# Launch User Story 1 models and adapter work together:
Task: "Implement the first-page application workflow in src/Application/Paging/GetTransactionHistoryPage.cs and src/Application/Paging/TransactionHistoryPageMapper.cs"
Task: "Implement the Socrata paging adapter in src/Infrastructure/Remote/SocrataTransactionHistoryAdapter.cs"
Task: "Implement stable row formatting in src/Cli/Commands/CliTableRow.cs and src/Cli/Commands/TransactionHistoryTableRowMapper.cs"
```

---

## Parallel Example: User Story 2

```bash
# Launch User Story 2 tests together:
Task: "Add paging navigation contract test in tests/Contract/Cli.ContractTests/PagingNavigationContractTests.cs"
Task: "Add multi-page adapter integration test in tests/Integration/Infrastructure.IntegrationTests/Remote/SocrataPagingIntegrationTests.cs"
Task: "Add paging state transition unit test in tests/Unit/Application.UnitTests/Paging/PagingSessionStateTests.cs"

# Launch User Story 2 application work together:
Task: "Implement navigation command parsing in src/Application/Paging/NavigationCommandParser.cs"
Task: "Implement paging session state transitions in src/Application/Paging/PagingSessionController.cs"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup.
2. Complete Phase 2: Foundational.
3. Complete Phase 3: User Story 1.
4. Validate no-argument startup against the live first-page scenario.
5. Demo the first visible product slice before extending navigation.

### Incremental Delivery

1. Setup and foundational work establish the shared paging contracts.
2. User Story 1 delivers the first live page.
3. User Story 2 adds next and previous navigation.
4. User Story 3 finalizes quit behavior and recovery flows.
5. Phase 6 aligns documentation and operator validation.

### Parallel Team Strategy

1. Complete Phases 1 and 2 together.
2. Assign User Story 1 to one developer first because it establishes the visible paging surface.
3. Once User Story 1 stabilizes, split User Story 2 application work and Infrastructure work in parallel.
4. Finish User Story 3 with one developer on failure translation and one on CLI messaging.

---

## Notes

- [P] tasks touch different files and can run in parallel safely.
- Every user story starts with failing tests to preserve constitution-required TDD.
- `--download`, SQL persistence, and FluentMigrator remain out of scope for this task list.
- Stop at each story checkpoint and verify the story independently before proceeding.