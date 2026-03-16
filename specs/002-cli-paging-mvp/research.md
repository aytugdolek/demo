# Research: CLI Paging MVP

## Decision 1: Use the Socrata `query.json` endpoint with POST and an `X-App-Token` header

- Decision: Call `https://data.colorado.gov/api/v3/views/casm-dbbj/query.json` using POST, pass the configured app token via the `X-App-Token` header, and send paging and ordering instructions in the request body.
- Rationale: The official dataset documentation shows `query.json` as the paging endpoint, supports structured page requests, and recommends POST to unlock the full range of query options. Using the app token in a header keeps it out of URLs and aligns with the documented token guidance.
- Alternatives considered:
  - GET with query-string parameters: rejected because it exposes the token in the URL and is less flexible for deterministic query text.
  - Running without an app token: rejected because the docs warn about lower throttling limits and the spec requires using the configured token.

## Decision 2: Order pages by `receiveddate` descending with `transactionid` as a stable secondary sort key

- Decision: Request records newest first by `receiveddate`, with `transactionid` as a deterministic tie-breaker.
- Rationale: The feature clarification requires newest-first paging. The dataset documents both `receiveddate` and `transactionid`, which together provide a stable order that prevents duplicate or skipped records across pages.
- Alternatives considered:
  - Preserve the dataset default order: rejected because page stability would depend on undocumented upstream behavior.
  - Sort by `effectivedate`: rejected because the clarified feature requirement is based on transaction recency and `receiveddate` is the more direct operational ordering field for this MVP.

## Decision 3: Map remote dataset rows into application models before rendering in Cli

- Decision: Introduce application-facing query/result models and perform terminal row mapping in `src/Cli` instead of letting the CLI render raw remote payloads.
- Rationale: The constitution requires explicit contracts and separation between external DTOs and CLI behavior, with terminal rendering owned by `Cli`. Mapping before rendering keeps remote shape changes isolated from terminal formatting and makes paging logic independently testable.
- Alternatives considered:
  - Render raw JSON or dictionaries directly in the CLI: rejected because it couples terminal behavior to the remote response format and weakens testability.
  - Use the remote DTO as the CLI row model: rejected because it leaks Infrastructure concerns into the presentation workflow.

## Decision 4: Render results with a Spectre.Console table using fixed columns and intentional width rules

- Decision: Use a Spectre.Console `Table` for the record grid, define a fixed column set for the MVP, and configure widths, alignment, and truncation behavior intentionally for long fields.
- Rationale: Spectre.Console’s table guidance is designed for structured multi-column data and supports column width, alignment, and wrapping controls needed for a readable terminal-first workflow. This matches the constitution’s requirement that long fields be handled intentionally.
- Alternatives considered:
  - Plain line-by-line text output: rejected because record comparison across pages would be harder and column stability would be weaker.
  - Live-updating widgets or progress displays for page navigation: rejected because the feature only needs discrete page rendering and Spectre’s progress docs caution against mixing progress with other interactive components.

## Decision 5: Reuse the existing host-based options configuration for Socrata settings

- Decision: Continue binding `SocrataOptions` through the existing host configuration pipeline and inject those options into the remote adapter or application orchestration services.
- Rationale: Microsoft’s .NET options guidance supports strongly typed configuration bound through dependency injection, which matches the existing Feature 1 setup and keeps secrets out of business logic.
- Alternatives considered:
  - Read environment variables directly in commands: rejected because it would couple CLI control flow to configuration retrieval.
  - Add a second ad hoc configuration path for paging: rejected because it would duplicate the configuration seam already established in the solution baseline.

## Decision 6: Keep the feature test-first with split coverage by concern

- Decision: Cover page navigation and page-state rules with Application unit tests, remote field mapping and adapter behavior with Infrastructure integration tests using controlled HTTP responses, and user-visible CLI flows with contract tests.
- Rationale: The constitution requires xUnit TDD and targeted tests at the appropriate layer. Splitting tests by concern keeps the navigation logic fast to verify while still covering remote-contract and CLI behavior without relying on live network calls.
- Alternatives considered:
  - One end-to-end live API test suite only: rejected because it would be slow, flaky, and poor evidence for red-green-refactor.
  - Unit-test only coverage: rejected because the remote contract and CLI surface are central risks in this slice.

## Decision 7: Let `src/Cli` act as the composition root for Infrastructure wiring

- Decision: Allow `src/Cli` to reference `src/Infrastructure` only to register adapters and host configuration at application startup.
- Rationale: The feature needs a runnable host that composes Application ports with Infrastructure adapters. This keeps terminal rendering in `Cli`, remote concerns in `Infrastructure`, and business logic in `Application` without introducing a separate bootstrap project for this small solution.
- Alternatives considered:
  - Keep `src/Cli` free of any Infrastructure reference: rejected because the current solution has no separate composition host and the feature would otherwise lack a concrete place to wire the remote adapter.
  - Introduce a new bootstrap assembly: rejected because it adds structural weight without delivering additional product value in this slice.