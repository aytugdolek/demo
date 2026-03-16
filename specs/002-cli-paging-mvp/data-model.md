# Data Model: CLI Paging MVP

## Overview

This feature introduces live paging concepts, a remote transaction-history query contract, and CLI-oriented display models. It does not add local persistence entities or schema changes.

## Entities

### PagingSessionState

- Purpose: Represents the current interactive paging state for one CLI session.
- Fields:
  - `currentPageNumber`: integer, starts at `1`
  - `pageSize`: integer, fixed at `20`
  - `canMovePrevious`: boolean derived from `currentPageNumber > 1`
  - `canMoveNext`: boolean derived from the most recent page result
  - `lastCommand`: `Next`, `Previous`, `Quit`, or `Unknown`
  - `statusMessage`: optional user-facing message for empty pages, boundary feedback, or failures
- Validation rules:
  - `currentPageNumber` must never be less than `1`.
  - `pageSize` is fixed for this feature and must remain `20`.
- State transitions:
  - `Idle` → `ViewingPage`
  - `ViewingPage` → `ViewingPage` on successful next or previous navigation
  - `ViewingPage` → `BoundaryNotice` when `<` is requested on page `1`
  - `ViewingPage` → `Failure` when a page request fails
  - `ViewingPage` → `Exited` when `Q` is entered

### NavigationCommand

- Purpose: Represents one user navigation input in the paging loop.
- Fields:
  - `rawInput`: original terminal text
  - `kind`: `Next`, `Previous`, `Quit`, or `Unknown`
- Validation rules:
  - `>` maps to `Next`.
  - `<` maps to `Previous`.
  - `Q` maps to `Quit` case-insensitively.
  - Any other input maps to `Unknown` and produces guidance without changing the current page.

### RemoteTransactionHistoryQuery

- Purpose: Represents the application-facing request sent through the remote transaction history port.
- Fields:
  - `pageNumber`: integer, minimum `1`
  - `pageSize`: integer, fixed at `20`
  - `sortField`: `receiveddate`
  - `sortDirection`: `Descending`
  - `secondarySortField`: `transactionid`
  - `secondarySortDirection`: `Descending`
- Validation rules:
  - `pageNumber` must be positive.
  - `pageSize` must remain aligned with the CLI contract.
  - Sorting fields must be fields documented by the Socrata dataset.

### RemoteTransactionHistoryRecord

- Purpose: Represents one remote row as returned by the Socrata dataset before CLI-specific formatting.
- Fields:
  - `entityId`: string
  - `transactionId`: string
  - `name`: string
  - `historyDescription`: string
  - `comment`: string?
  - `receivedDate`: date/time?
  - `effectiveDate`: date/time?
- Validation rules:
  - `transactionId` should be retained even when other fields are blank because it supports stable sorting and operator identification.
  - Missing optional text fields must map to safe empty or placeholder values rather than breaking the page.

### TransactionHistoryPage

- Purpose: Represents one page of transaction-history results returned to Application orchestration.
- Fields:
  - `pageNumber`: integer
  - `pageSize`: integer
  - `records`: ordered list of `RemoteTransactionHistoryRecord`
  - `isEmpty`: boolean
  - `hasPreviousPage`: boolean
  - `hasNextPage`: boolean inferred from whether the requested next page returned records
- Validation rules:
  - `records.Count` must not exceed `pageSize`.
  - Record order must match the query sort order.

### CliTableRow

- Purpose: Represents the stable terminal row rendered for one transaction-history record in the `src/Cli` layer.
- Fields:
  - `transactionId`: string
  - `entityId`: string
  - `name`: string
  - `historyDescription`: string
  - `receivedDateText`: string
- Validation rules:
  - All rendered rows must use the same column set and order.
  - Long values must be truncated or width-limited intentionally for terminal readability.

## Relationships

- `PagingSessionState` uses `NavigationCommand` to decide whether to load a new `RemoteTransactionHistoryQuery`, stay on the current page, or exit.
- `RemoteTransactionHistoryQuery` returns a `TransactionHistoryPage` through the Application remote port.
- Each `RemoteTransactionHistoryRecord` maps to one `CliTableRow` inside the `src/Cli` rendering path.
- `TransactionHistoryPage` updates `PagingSessionState` after each successful request.

## Notes

- Local persistence entities are intentionally out of scope.
- The stable CLI column set for this feature is `Transaction ID`, `Entity ID`, `Name`, `History`, and `Received`.