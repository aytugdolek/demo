# Product Requirements

## Product Name

Colorado Business Entity Transaction History CLI

## Purpose

Build a command-line tool that retrieves and displays Colorado Business Entity Transaction History data from the Colorado Information Marketplace dataset powered by Socrata.

The tool is intended for developer and analyst use on a local workstation, with SQL Server Developer Edition as the local persistence layer.

## Business Goal

- Provide a fast terminal-based experience for browsing transaction-history data.
- Allow local persistence for future enrichment, caching, or offline workflows.
- Support a simple download mode for extracting dataset data to a file.

## Dataset

- Dataset name: `Business Entity Transaction History`
- Dataset identifier: `casm-dbbj`
- Source domain: `data.colorado.gov`
- Documentation: `https://dev.socrata.com/foundry/data.colorado.gov/casm-dbbj`
- Primary query endpoint: `https://data.colorado.gov/api/v3/views/casm-dbbj/query.json`

## Known Dataset Fields

The current public documentation exposes these primary fields:

- `entityid`
- `transactionid`
- `historydes`
- `comment`
- `receiveddate`
- `effectivedate`
- `name`

## External API Configuration

- API provider: `dev.socrata.com`
- App token: `9nXi5FmwW1AwxtyMIaHBZhkuo`

## User Experience

### Default Mode

When the application runs without parameters, it displays paged data from the Colorado Business Entity Transaction History dataset.

### Paging Rules

- Page size is fixed at `20` items.
- The CLI initially loads the first page.
- Users can move forward and backward through pages without restarting the app.

### Interactive Controls

- `>` moves to the next page.
- `<` moves to the previous page.
- `Q` exits the application.

### Download Mode

When the application runs with `--download`, it calls the following endpoint and creates a download file:

- `https://data.colorado.gov/api/v3/views/casm-dbbj/query.json`

The exact file name can be implementation-defined, but the saved content must come from the download endpoint response.

## Functional Requirements

1. The application must run as a CLI application.
2. The default command with no parameters must display dataset results in a paged terminal view.
3. The terminal view must render 20 items per page.
4. The terminal view must expose `>`, `<`, and `Q` navigation controls.
5. The application must use the Socrata app token when calling the remote API.
6. The application must support `--download` for file creation from the query endpoint.
7. The application must be able to connect to a local SQL Server instance for persistence-related operations.
8. The application must be structured so data access, core business logic, and presentation are separated.

## Non-Functional Requirements

1. The CLI output must remain readable in a terminal-first workflow.
2. The application must support local developer execution on Windows.
3. The design must allow unit testing without depending on live API calls or a live database.
4. The solution must follow Clean Architecture boundaries.
5. The solution must use test-first development practices.

## Assumptions

- Local development occurs on Windows.
- SQL Server Developer Edition 17.0 is installed and reachable at `localhost\DEMO`.
- Windows Authentication is used for database access.
- The provided Socrata app token is valid for development use.

## Open Design Decisions

- Whether the default paging mode reads directly from the API every time or can optionally read from a synchronized local database cache.
- Whether `--download` stores raw JSON only or also supports transformed formats later.
- Whether pagination state should preserve total-record metadata or only page-local navigation state.