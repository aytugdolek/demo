# Research: Download Workflow

## Decision 1: Add a dedicated download use case instead of reusing the paging session

- Decision: Introduce a dedicated Application use case for `--download` rather than routing download behavior through the interactive paging controller.
- Rationale: The spec requires a non-interactive path with different success and failure semantics from paging. A separate use case keeps the paging session unchanged and makes the download workflow independently testable.
- Alternatives considered:
  - Reusing the paging controller: rejected because it would mix interactive navigation concerns with a file-output operation.
  - Adding download logic directly in `Program.cs`: rejected because it would put business flow and file I/O in the CLI composition root.

## Decision 2: Keep local file creation in Infrastructure behind a file-writing port

- Decision: Model file writing as an Infrastructure concern and call it through an Application abstraction.
- Rationale: The constitution requires external concerns to stay out of Core and Application logic. A file-writing adapter keeps overwrite checks, path selection, and write failures isolated from CLI code.
- Alternatives considered:
  - Writing files directly from the CLI command: rejected because it would bypass the architecture boundary and make command behavior harder to unit test.
  - Overloading the placeholder persistence port: rejected because the feature is file output, not database persistence.

## Decision 3: Treat an existing target file as a hard failure

- Decision: Fail the download if the target file already exists and preserve the existing file unchanged.
- Rationale: The clarification answer explicitly requires a clear error and no overwrite. That keeps the behavior safe and predictable for operators.
- Alternatives considered:
  - Overwrite the file: rejected by clarification.
  - Auto-generate a new filename: rejected because it would make the success path less deterministic and harder to validate.

## Decision 4: Make the CLI command explicitly non-interactive and exit-code driven

- Decision: Add a distinct `--download` command path that completes without prompting for paging input and returns a success or failure exit code.
- Rationale: The feature goal is non-interactive delivery. Exit codes and a clear status message keep the command suitable for scripted use and demos.
- Alternatives considered:
  - Reusing the paging prompt loop with a special case: rejected because the spec says the download path must not enter the interactive paging flow.
  - Forcing user confirmation for overwrite: rejected because the clarified behavior is to fail immediately instead of prompting.

## Decision 5: Split tests by concern and keep the download workflow test-first

- Decision: Add Application unit tests for orchestration and file-target rules, Infrastructure integration tests for the file writer and remote seam, and CLI contract tests for `--download` behavior and exit codes.
- Rationale: The constitution requires xUnit TDD and targeted tests at the correct layer. This split keeps the download logic fast to verify while still covering the CLI contract.
- Alternatives considered:
  - End-to-end live-only tests: rejected because they would be slow and fragile.
  - Unit tests only: rejected because the file-writing behavior and user-visible CLI contract need dedicated coverage.

## Decision 6: Reuse the existing Socrata configuration path

- Decision: Continue using the existing Socrata configuration binding for the download workflow rather than adding a new configuration system.
- Rationale: The current app already binds Socrata settings through the host and the download workflow depends on the same upstream dataset.
- Alternatives considered:
  - Introduce a second download-specific config path: rejected because it would duplicate an existing seam.
  - Hard-code the remote endpoint and file path: rejected because it would violate configuration and boundary rules.