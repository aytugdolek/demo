---

description: "Task list template for feature implementation"
---

# Tasks: [FEATURE NAME]

**Input**: Design documents from `/specs/[###-feature-name]/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are REQUIRED by the constitution. Every user story phase MUST
start with failing tests before implementation tasks begin.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Primary layout**: `src/Core`, `src/Application`, `src/Infrastructure`,
  `src/Cli`, and `tests/*` at repository root
- **Tests**: Prefer `tests/Unit`, `tests/Integration`, and `tests/Contract`
- Paths shown below assume the project constitution's Clean Architecture layout

<!-- 
  ============================================================================
  IMPORTANT: The tasks below are SAMPLE TASKS for illustration purposes only.
  
  The /speckit.tasks command MUST replace these with actual tasks based on:
  - User stories from spec.md (with their priorities P1, P2, P3...)
  - Feature requirements from plan.md
  - Entities from data-model.md
  - Endpoints from contracts/
  
  Tasks MUST be organized by user story so each story can be:
  - Implemented independently
  - Tested independently
  - Delivered as an MVP increment
  
  DO NOT keep these sample tasks in the generated tasks.md file.
  ============================================================================
-->

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [ ] T001 Create project structure per implementation plan
- [ ] T002 Initialize the .NET solution and project dependencies
- [ ] T003 [P] Configure shared SDK, formatting, and test settings

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

Examples of foundational tasks (adjust based on your project):

- [ ] T004 Setup SQL Server connectivity and FluentMigrator infrastructure
- [ ] T005 [P] Create shared domain and application abstractions in src/Core and src/Application
- [ ] T006 [P] Setup Socrata API client boundaries in src/Infrastructure
- [ ] T007 Create base entities and mapping contracts shared by all stories
- [ ] T008 Configure error handling, logging, and terminal status messaging
- [ ] T009 Setup configuration management for app token, connection string, and runtime options

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - [Title] (Priority: P1) 🎯 MVP

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 1 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [ ] T010 [P] [US1] Add contract test for [external contract] in tests/Contract/[Name]ContractTests.cs
- [ ] T011 [P] [US1] Add integration test for [user journey] in tests/Integration/[Name]IntegrationTests.cs
- [ ] T012 [P] [US1] Add unit test for [use case or paging rule] in tests/Unit/[Name]Tests.cs

### Implementation for User Story 1

- [ ] T013 [P] [US1] Create [Entity1] in src/Core/[Feature]/[Entity1].cs
- [ ] T014 [P] [US1] Create [Entity2] in src/Core/[Feature]/[Entity2].cs
- [ ] T015 [US1] Implement [UseCase] in src/Application/[Feature]/[UseCase].cs (depends on T013, T014)
- [ ] T016 [US1] Implement infrastructure support in src/Infrastructure/[Feature]/[File].cs
- [ ] T017 [US1] Implement CLI workflow in src/Cli/[Feature]/[File].cs
- [ ] T018 [US1] Add validation, error handling, and logging for user story 1

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently

---

## Phase 4: User Story 2 - [Title] (Priority: P2)

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 2 ⚠️

- [ ] T019 [P] [US2] Add contract test for [external contract] in tests/Contract/[Name]ContractTests.cs
- [ ] T020 [P] [US2] Add integration test for [user journey] in tests/Integration/[Name]IntegrationTests.cs
- [ ] T021 [P] [US2] Add unit test for [use case] in tests/Unit/[Name]Tests.cs

### Implementation for User Story 2

- [ ] T022 [P] [US2] Create [Entity] in src/Core/[Feature]/[Entity].cs
- [ ] T023 [US2] Implement [UseCase] in src/Application/[Feature]/[UseCase].cs
- [ ] T024 [US2] Implement infrastructure support in src/Infrastructure/[Feature]/[File].cs
- [ ] T025 [US2] Implement CLI behavior in src/Cli/[Feature]/[File].cs

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently

---

## Phase 5: User Story 3 - [Title] (Priority: P3)

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 3 ⚠️

- [ ] T026 [P] [US3] Add contract test for [external contract] in tests/Contract/[Name]ContractTests.cs
- [ ] T027 [P] [US3] Add integration test for [user journey] in tests/Integration/[Name]IntegrationTests.cs
- [ ] T028 [P] [US3] Add unit test for [use case] in tests/Unit/[Name]Tests.cs

### Implementation for User Story 3

- [ ] T029 [P] [US3] Create [Entity] in src/Core/[Feature]/[Entity].cs
- [ ] T030 [US3] Implement [UseCase] in src/Application/[Feature]/[UseCase].cs
- [ ] T031 [US3] Implement CLI or infrastructure support in src/[Project]/[Feature]/[File].cs

**Checkpoint**: All user stories should now be independently functional

---

[Add more user story phases as needed, following the same pattern]

---

## Phase N: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [ ] TXXX [P] Documentation updates in .doc/ or docs/
- [ ] TXXX Code cleanup and refactoring
- [ ] TXXX Performance optimization across all stories
- [ ] TXXX [P] Additional unit tests in tests/Unit/
- [ ] TXXX Security hardening
- [ ] TXXX Run quickstart.md validation

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User stories can then proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2 → P3)
- **Polish (Final Phase)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - May integrate with US1 but should be independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - May integrate with US1/US2 but should be independently testable

### Within Each User Story

- Tests MUST be written and FAIL before implementation
- Models before services
- Services before endpoints
- Core implementation before integration
- Story complete before moving to next priority

### Parallel Opportunities

- All Setup tasks marked [P] can run in parallel
- All Foundational tasks marked [P] can run in parallel (within Phase 2)
- Once Foundational phase completes, all user stories can start in parallel (if team capacity allows)
- All tests for a user story marked [P] can run in parallel
- Models within a story marked [P] can run in parallel
- Different user stories can be worked on in parallel by different team members

---

## Parallel Example: User Story 1

```bash
# Launch all tests for User Story 1 together:
Task: "Contract test for [external contract] in tests/Contract/[Name]ContractTests.cs"
Task: "Integration test for [user journey] in tests/Integration/[Name]IntegrationTests.cs"

# Launch all models for User Story 1 together:
Task: "Create [Entity1] in src/Core/[Feature]/[Entity1].cs"
Task: "Create [Entity2] in src/Core/[Feature]/[Entity2].cs"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL - blocks all stories)
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Test User Story 1 independently
5. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
3. Add User Story 2 → Test independently → Deploy/Demo
4. Add User Story 3 → Test independently → Deploy/Demo
5. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1
   - Developer B: User Story 2
   - Developer C: User Story 3
3. Stories complete and integrate independently

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Avoid: vague tasks, same file conflicts, cross-story dependencies that break independence
