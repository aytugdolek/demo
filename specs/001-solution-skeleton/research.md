# Research: Solution Skeleton

## Decision 1: Use a four-project Clean Architecture solution with layer-aligned tests

- Decision: Create production projects for `src/Core`, `src/Application`, `src/Infrastructure`, and `src/Cli`, plus test projects aligned to unit, integration, and CLI contract coverage.
- Rationale: This matches the constitution and technical standards, keeps dependency direction explicit, and gives later slices stable extension points without restructuring the solution.
- Alternatives considered:
  - Three-project solution combining Application and Infrastructure: rejected because it weakens boundary enforcement around API, SQL, and configuration side effects.
  - Single test project for the whole solution: rejected because it obscures TDD ownership and makes fast unit coverage harder to preserve.

## Decision 2: Centralize shared .NET settings at the repository root

- Decision: Put target framework and compiler defaults in `Directory.Build.props`, and centralize package versions in `Directory.Packages.props` so project files stay minimal.
- Rationale: The feature requires shared SDK settings for `.NET 10` and latest C# features. Root-level build settings reduce duplication and make later project additions consistent by default.
- Alternatives considered:
  - Repeating SDK settings in every project file: rejected because it increases drift risk across layers and test projects.
  - Using only per-project package references with inline versions: rejected because the solution baseline is specifically meant to establish consistency for future slices.

## Decision 3: Use a host-based configuration pipeline with strongly typed options

- Decision: Build the CLI startup around `Microsoft.Extensions.Hosting` and `Microsoft.Extensions.Configuration`, binding required settings into strongly typed options for remote access and local persistence.
- Rationale: This keeps configuration outside business logic, allows environment-aware overrides, and makes the explicit validation path testable without invoking live integrations.
- Alternatives considered:
  - Reading environment variables directly in commands: rejected because it couples CLI behavior to configuration retrieval and weakens testability.
  - Hard-coding the local connection string in Infrastructure: rejected because it violates the constitution and repository hygiene rules.

## Decision 4: Expose configuration verification through a dedicated CLI validation path

- Decision: Reserve `--validate-config` as the explicit validation path for Feature 1, separate from normal no-argument startup.
- Rationale: The clarified spec requires configuration validation to be intentional rather than mandatory on every normal startup. A dedicated switch keeps the baseline deterministic and easy to test.
- Alternatives considered:
  - Validate configuration on every application launch: rejected by clarification because it would block normal baseline startup on machines not yet configured.
  - Add a subcommand such as `validate-config`: rejected because the product already reserves switch-style behavior such as `--download`, so a switch keeps the CLI shape consistent.

## Decision 5: Define integration seams in Application, implement placeholders in Infrastructure

- Decision: Place remote-data and persistence ports in `src/Application` and keep placeholder implementations or adapters in `src/Infrastructure`.
- Rationale: This preserves Clean Architecture direction, keeps side effects out of the core, and lets later slices add API and SQL behavior without changing CLI orchestration contracts.
- Alternatives considered:
  - Define ports in `src/Core`: rejected because these are application orchestration concerns, not domain rules.
  - Allow `src/Cli` to reference Infrastructure directly for early convenience: rejected because it would create the exact coupling this baseline is intended to prevent.

## Decision 6: Keep baseline tests isolated from live dependencies

- Decision: Establish xUnit projects so unit tests cover Core and Application behavior first, with separate integration and CLI contract test projects reserved for wiring and entry-point validation.
- Rationale: The feature’s success criteria require tests to pass without live network or database access. Segregated projects make that rule visible and enforceable.
- Alternatives considered:
  - Add only unit tests now and invent integration structure later: rejected because the baseline slice should establish the full testing topology used by later work.
  - Run baseline integration tests against a live local database: rejected because Feature 1 is for skeleton readiness, not environment-dependent persistence behavior.