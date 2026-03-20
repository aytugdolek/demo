# Tasks: Download Workflow

**Input**: Design documents from `/specs/003-download-workflow/`
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
- **Contracts**: Keep remote API concerns in `src/Infrastructure/Remote` and CLI command behavior in `src/Cli/Commands`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare the existing solution and CLI wiring for a separate download command path without changing user-visible behavior yet.

- [ ] T001 Update CLI and infrastructure project dependencies for download workflow support in src/Cli/Colorado.BusinessEntityTransactionHistory.Cli.csproj, src/Infrastructure/Colorado.BusinessEntityTransactionHistory.Infrastructure.csproj, and tests/Contract/Cli.ContractTests/Colorado.BusinessEntityTransactionHistory.Cli.ContractTests.csproj
- [ ] T002 Update the CLI entry point to recognize `--download` as a routed command while preserving existing `--validate-config` and default paging behavior in src/Cli/Program.cs

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Create the shared download contracts, models, and wiring that every user story depends on.

**⚠️ CRITICAL**: No user story work can begin until this phase is complete.

- [ ] T003 Create the download workflow abstraction and request model in src/Application/Abstractions/IFileDownloadPort.cs and src/Application/Downloads/DownloadRequest.cs
- [ ] T004 [P] Create shared download result and target models in src/Application/Downloads/DownloadResult.cs and src/Application/Downloads/DownloadTarget.cs
- [ ] T005 [P] Create download orchestration and filename selection support in src/Application/Downloads/DownloadWorkflow.cs and src/Application/Downloads/DownloadFilenamePolicy.cs
- [ ] T006 Create the file-writing adapter boundary in src/Infrastructure/Persistence/FileDownloadAdapter.cs and register it in src/Infrastructure/DependencyInjection.cs
- [ ] T007 [P] Extend the CLI command surface for download execution and output formatting in src/Cli/Commands/DownloadCommand.cs and src/Cli/Commands/DownloadOutputFormatter.cs
- [ ] T008 Update the test support harness to run and assert download command output in tests/Contract/Cli.ContractTests/TestSupport/CliCommandRunner.cs

**Checkpoint**: Foundation ready - user story implementation can now begin.

---

## Phase 3: User Story 1 - Save A Local Download (Priority: P1) 🎯 MVP

**Goal**: Running the CLI with `--download` saves the remote transaction-history response to a local file and reports the file location without entering paging mode.

**Independent Test**: Set `Socrata__AppToken`, run `dotnet run --project src/Cli -- --download`, and confirm a local file is created, a success message is shown, and no paging prompt appears.

### Tests for User Story 1 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [ ] T009 [P] [US1] Add download success and no-paging contract coverage in tests/Contract/Cli.ContractTests/DownloadWorkflowContractTests.cs
- [ ] T010 [P] [US1] Add download orchestration unit coverage for successful file creation in tests/Unit/Application.UnitTests/Downloads/DownloadWorkflowTests.cs
- [ ] T011 [P] [US1] Add file-writing integration coverage for a successful save in tests/Integration/Infrastructure.IntegrationTests/Persistence/FileDownloadAdapterTests.cs

### Implementation for User Story 1

- [ ] T012 [P] [US1] Implement the download orchestration workflow in src/Application/Downloads/DownloadWorkflow.cs and src/Application/Downloads/DownloadResult.cs
- [ ] T013 [P] [US1] Implement filename and target selection rules in src/Application/Downloads/DownloadFilenamePolicy.cs and src/Application/Downloads/DownloadTarget.cs
- [ ] T014 [US1] Implement the file-writing adapter in src/Infrastructure/Persistence/FileDownloadAdapter.cs
- [ ] T015 [US1] Implement the `--download` CLI command and success output in src/Cli/Commands/DownloadCommand.cs and src/Cli/Commands/DownloadOutputFormatter.cs
- [ ] T016 [US1] Wire the new command into the CLI host and help text in src/Cli/Program.cs

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently.

---

## Phase 4: User Story 2 - Understand Download Failures (Priority: P2)

**Goal**: Download failures are reported clearly when the remote source cannot be reached or the local file cannot be written.

**Independent Test**: Simulate remote retrieval failure, existing-file conflict, and write failure, then confirm the CLI exits cleanly with a clear error message and no partial file.

### Tests for User Story 2 ⚠️

- [ ] T017 [P] [US2] Add failure-path contract coverage for remote and local write errors in tests/Contract/Cli.ContractTests/DownloadFailureContractTests.cs
- [ ] T018 [P] [US2] Add download workflow unit coverage for remote failure, empty payload, and file-exists handling in tests/Unit/Application.UnitTests/Downloads/DownloadWorkflowFailureTests.cs
- [ ] T019 [P] [US2] Add adapter integration coverage for empty or malformed payload handling, existing-file conflict, and write failure in tests/Integration/Infrastructure.IntegrationTests/Persistence/FileDownloadAdapterFailureTests.cs

### Implementation for User Story 2

- [ ] T020 [P] [US2] Implement failure mapping and result messaging for remote failure, empty payload, and file-exists cases in src/Application/Downloads/DownloadResult.cs and src/Application/Downloads/DownloadWorkflow.cs
- [ ] T021 [P] [US2] Implement empty or malformed payload detection, existing-file detection, and write failure handling in src/Infrastructure/Persistence/FileDownloadAdapter.cs
- [ ] T022 [US2] Implement CLI failure output and non-interactive exit behavior in src/Cli/Commands/DownloadCommand.cs and src/Cli/Program.cs

**Checkpoint**: At this point, User Stories 1 and 2 should both work independently.

---

## Phase 5: User Story 3 - Keep Download Separate From Paging (Priority: P3)

**Goal**: Running the CLI without `--download` continues to use the existing interactive paging experience unchanged.

**Independent Test**: Run the CLI with no arguments and confirm it still starts the paging loop, while `--download` remains isolated to the download workflow.

### Tests for User Story 3 ⚠️

- [ ] T023 [P] [US3] Add no-argument paging regression coverage to confirm the existing interactive path remains unchanged in tests/Contract/Cli.ContractTests/PagingStartupContractTests.cs
- [ ] T024 [P] [US3] Add CLI routing regression coverage to confirm `--download` does not affect paging startup in tests/Contract/Cli.ContractTests/PagingExitAndFailureContractTests.cs
- [ ] T025 [P] [US3] Add orchestration coverage to confirm paging and download remain separate in tests/Unit/Application.UnitTests/Downloads/DownloadWorkflowTests.cs

### Implementation for User Story 3

- [ ] T026 [US3] Keep the existing paging command path unchanged while routing `--download` to the new command in src/Cli/Program.cs
- [ ] T027 [US3] Update CLI help and operator text to describe both the paging and download workflows in src/Cli/Program.cs and src/Cli/Commands/DownloadOutputFormatter.cs

**Checkpoint**: All user stories should now be independently functional.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Improve consistency, documentation, and operator validation across the completed feature.

- [ ] T028 [P] Update download workflow documentation and examples in specs/003-download-workflow/quickstart.md and specs/003-download-workflow/contracts/cli-contract.md
- [ ] T029 Validate the full download workflow against the quickstart steps and align final CLI messaging in src/Cli/Commands/DownloadCommand.cs and specs/003-download-workflow/quickstart.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately.
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories.
- **User Stories (Phase 3+)**: All depend on Foundational phase completion.
  - User stories can then proceed in parallel (if staffed).
  - Or sequentially in priority order (P1 → P2 → P3).
- **Polish (Final Phase)**: Depends on all desired user stories being complete.

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories.
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - May build on the download workflow introduced in User Story 1, but should remain independently testable.
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - Must preserve the existing paging flow and remain independent of download mode.

### Within Each User Story

- Tests MUST be written and MUST fail before implementation.
- Models and ports before orchestration.
- Orchestration before CLI wiring.
- Story complete before moving to the next priority.

### Parallel Opportunities

- `T004`, `T005`, `T007`, and `T008` can run in parallel after `T003` establishes the shared download abstraction.
- `T009`, `T010`, and `T011` can run in parallel for User Story 1.
- `T012`, `T013`, and `T014` can run in parallel after User Story 1 tests are in place.
- `T017`, `T018`, and `T019` can run in parallel for User Story 2.
- `T020` and `T021` can run in parallel before the CLI failure-handling task `T022`.
- `T023`, `T024`, and `T025` can run in parallel for User Story 3.

---

## Parallel Example: User Story 1

```bash
# Launch User Story 1 tests together:
Task: "Add download success and no-paging contract coverage in tests/Contract/Cli.ContractTests/DownloadWorkflowContractTests.cs"
Task: "Add download orchestration unit coverage for successful file creation in tests/Unit/Application.UnitTests/Downloads/DownloadWorkflowTests.cs"
Task: "Add file-writing integration coverage for a successful save in tests/Integration/Infrastructure.IntegrationTests/Persistence/FileDownloadAdapterTests.cs"

# Launch User Story 1 implementation together:
Task: "Implement the download orchestration workflow in src/Application/Downloads/DownloadWorkflow.cs and src/Application/Downloads/DownloadResult.cs"
Task: "Implement filename and target selection rules in src/Application/Downloads/DownloadFilenamePolicy.cs and src/Application/Downloads/DownloadTarget.cs"
Task: "Implement the file-writing adapter in src/Infrastructure/Persistence/FileDownloadAdapter.cs"
```

---

## Parallel Example: User Story 2

```bash
# Launch User Story 2 tests together:
Task: "Add failure-path contract coverage for remote and local write errors in tests/Contract/Cli.ContractTests/DownloadFailureContractTests.cs"
Task: "Add download workflow unit coverage for remote failure and file-exists handling in tests/Unit/Application.UnitTests/Downloads/DownloadWorkflowFailureTests.cs"
Task: "Add adapter integration coverage for existing-file conflict and write failure handling in tests/Integration/Infrastructure.IntegrationTests/Persistence/FileDownloadAdapterFailureTests.cs"

# Launch User Story 2 implementation together:
Task: "Implement failure mapping and result messaging in src/Application/Downloads/DownloadResult.cs and src/Application/Downloads/DownloadWorkflow.cs"
Task: "Implement existing-file detection and write failure handling in src/Infrastructure/Persistence/FileDownloadAdapter.cs"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup.
2. Complete Phase 2: Foundational.
3. Complete Phase 3: User Story 1.
4. **STOP and VALIDATE**: Test the `--download` success path independently.
5. Demo the first non-interactive product slice before extending failure handling.

### Incremental Delivery

1. Setup and foundational work establish the shared download contracts.
2. User Story 1 delivers successful file creation and reporting.
3. User Story 2 adds explicit failure handling and conflict safety.
4. User Story 3 preserves the paging workflow when download mode is absent.
5. Phase 6 aligns documentation and operator validation.

### Parallel Team Strategy

1. Complete Phases 1 and 2 together.
2. Assign User Story 1 to one developer first because it creates the visible `--download` path.
3. Once User Story 1 stabilizes, split User Story 2 application and Infrastructure work in parallel.
4. Finish User Story 3 with one developer preserving paging behavior and one updating operator text.

---

## Notes

- [P] tasks touch different files and can run in parallel safely.
- Every user story starts with failing tests to preserve constitution-required TDD.
- Existing paging behavior must remain unchanged when `--download` is omitted.
- Stop at each story checkpoint and verify the story independently before proceeding.