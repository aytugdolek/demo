# Code Metrics and Test Coverage

Generated: 2026-03-16

## Scope and Method

- Code metrics were computed from `*.cs` files under `src/` and `tests/`.
- `bin/` and `obj/` folders were excluded.
- Test execution totals were taken from `dotnet test Colorado.BusinessEntityTransactionHistory.sln`.
- Coverage was collected with the built-in `.NET` `Code Coverage` collector, exported to Cobertura XML, and normalized to production assemblies only:
  - `Colorado.BusinessEntityTransactionHistory.Application`
  - `Colorado.BusinessEntityTransactionHistory.Cli`
  - `Colorado.BusinessEntityTransactionHistory.Core`
  - `Colorado.BusinessEntityTransactionHistory.Infrastructure`
- Test assemblies and third-party libraries were excluded from the final percentage because they distort production-code coverage.

## Code Metrics Summary

| Area | Files | Lines |
| --- | ---: | ---: |
| Production code (`src/`) | 33 | 747 |
| Test code (`tests/`) | 21 | 695 |
| Total | 54 | 1442 |

### Production Projects

| Project | Files | Lines |
| --- | ---: | ---: |
| `src/Application` | 20 | 317 |
| `src/Cli` | 7 | 236 |
| `src/Core` | 1 | 14 |
| `src/Infrastructure` | 5 | 180 |

### Test Projects

| Project | Files | Lines |
| --- | ---: | ---: |
| `tests/Application.UnitTests` | 7 | 148 |
| `tests/Cli.ContractTests` | 6 | 270 |
| `tests/Core.UnitTests` | 2 | 54 |
| `tests/Infrastructure.IntegrationTests` | 6 | 223 |

## Test Execution Summary

Command used:

```powershell
dotnet test Colorado.BusinessEntityTransactionHistory.sln --nologo --verbosity minimal
```

Results:

- Total tests: 24
- Passed: 24
- Failed: 0
- Skipped: 0
- Duration: 22.4s

## Production Code Coverage Summary

Normalized production line coverage:

- Covered lines: 391
- Valid lines: 508
- Line coverage: 76.97%

### Coverage by Production Assembly

| Assembly | Covered Lines | Valid Lines | Line Coverage |
| --- | ---: | ---: | ---: |
| `Colorado.BusinessEntityTransactionHistory.Application` | 159 | 198 | 80.30% |
| `Colorado.BusinessEntityTransactionHistory.Cli` | 122 | 169 | 72.19% |
| `Colorado.BusinessEntityTransactionHistory.Core` | 8 | 10 | 80.00% |
| `Colorado.BusinessEntityTransactionHistory.Infrastructure` | 102 | 131 | 77.86% |

## File-Level Production Coverage

Sorted from lowest to highest coverage.

| File | Covered | Valid | Coverage |
| --- | ---: | ---: | ---: |
| `src/Cli/Commands/BaselineCommand.cs` | 0 | 11 | 0.00% |
| `src/Infrastructure/Persistence/PlaceholderPersistenceAdapter.cs` | 0 | 3 | 0.00% |
| `src/Application/Paging/PagingSessionController.cs` | 22 | 42 | 52.38% |
| `src/Application/Paging/RemoteTransactionHistoryQuery.cs` | 9 | 15 | 60.00% |
| `src/Cli/Program.cs` | 16 | 26 | 61.54% |
| `src/Application/Configuration/ValidationOutcome.cs` | 7 | 10 | 70.00% |
| `src/Cli/Commands/InteractivePagingCommand.cs` | 48 | 66 | 72.73% |
| `src/Infrastructure/Remote/SocrataTransactionHistoryAdapter.cs` | 73 | 99 | 73.74% |
| `src/Application/Paging/RemoteTransactionHistoryRecord.cs` | 6 | 8 | 75.00% |
| `src/Cli/Commands/TransactionHistoryTableRenderer.cs` | 21 | 27 | 77.78% |
| `src/Core/Architecture/SolutionProject.cs` | 8 | 10 | 80.00% |
| `src/Application/Paging/PagingCommandResult.cs` | 19 | 23 | 82.61% |
| `src/Application/Paging/PagingSessionState.cs` | 11 | 13 | 84.62% |
| `src/Cli/Commands/TransactionHistoryTableRowMapper.cs` | 12 | 14 | 85.71% |
| `src/Application/Paging/NavigationCommandParser.cs` | 9 | 10 | 90.00% |
| `src/Application/Paging/TransactionHistoryPage.cs` | 11 | 12 | 91.67% |
| `src/Application/Configuration/DemoDbOptions.cs` | 1 | 1 | 100.00% |
| `src/Application/Configuration/RuntimeConfigurationValidator.cs` | 28 | 28 | 100.00% |
| `src/Application/Configuration/SocrataOptions.cs` | 3 | 3 | 100.00% |
| `src/Application/DependencyInjection.cs` | 9 | 9 | 100.00% |
| `src/Application/Paging/GetTransactionHistoryPage.cs` | 12 | 12 | 100.00% |
| `src/Application/Paging/NavigationCommand.cs` | 1 | 1 | 100.00% |
| `src/Application/Paging/TransactionHistoryPageFailure.cs` | 1 | 1 | 100.00% |
| `src/Application/Paging/TransactionHistoryPageMapper.cs` | 3 | 3 | 100.00% |
| `src/Application/Startup/BaselineStartupSummary.cs` | 1 | 1 | 100.00% |
| `src/Application/Startup/GetBaselineStartupSummary.cs` | 6 | 6 | 100.00% |
| `src/Cli/Commands/CliTableRow.cs` | 6 | 6 | 100.00% |
| `src/Cli/Commands/ValidateConfigCommand.cs` | 19 | 19 | 100.00% |
| `src/Infrastructure/DependencyInjection.cs` | 14 | 14 | 100.00% |
| `src/Infrastructure/Remote/SocrataQueryRequest.cs` | 7 | 7 | 100.00% |
| `src/Infrastructure/Remote/SocrataTransactionHistoryRecordDto.cs` | 8 | 8 | 100.00% |

## Coverage Hotspots

The weakest-covered production files are:

1. `src/Cli/Commands/BaselineCommand.cs` at 0.00%
2. `src/Infrastructure/Persistence/PlaceholderPersistenceAdapter.cs` at 0.00%
3. `src/Application/Paging/PagingSessionController.cs` at 52.38%
4. `src/Application/Paging/RemoteTransactionHistoryQuery.cs` at 60.00%
5. `src/Cli/Program.cs` at 61.54%
6. `src/Application/Configuration/ValidationOutcome.cs` at 70.00%
7. `src/Cli/Commands/InteractivePagingCommand.cs` at 72.73%
8. `src/Infrastructure/Remote/SocrataTransactionHistoryAdapter.cs` at 73.74%

These files are the best candidates if the next goal is to raise overall confidence or enforce a coverage gate.