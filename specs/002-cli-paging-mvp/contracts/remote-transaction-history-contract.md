# Remote Transaction History Contract: CLI Paging MVP

## Purpose

Define the application-facing contract between paging orchestration and the remote Socrata adapter for Feature 2.

## Request Contract

### Query Shape

- Endpoint: `https://data.colorado.gov/api/v3/views/casm-dbbj/query.json`
- Method: `POST`
- Authentication:
  - Required app token supplied through the configured `Socrata:AppToken` value.
  - Token sent via `X-App-Token` header.
- Paging:
  - `pageNumber`: requested page number, minimum `1`
  - `pageSize`: fixed to `20`
- Ordering:
  - Primary sort: `receiveddate` descending
  - Secondary sort: `transactionid` descending

## Response Contract

### Required Fields For MVP Mapping

- `transactionid`
- `entityid`
- `name`
- `historydes`
- `receiveddate`

### Optional Fields Retained For Future Use

- `comment`
- `effectivedate`

## Application Port Expectations

- The Application layer requests one page at a time and receives an ordered page result.
- The port must not expose raw HTTP concerns to the caller.
- The result must preserve record order as returned by the upstream query.
- Remote errors must be translated into a failure the CLI can report clearly.

## Empty And Failure Semantics

- Empty result on page 1: return a successful empty page result so the CLI can show an empty-state message.
- Empty result on a later next-page request: return a successful empty page result so the session can inform the user that no further results are available.
- Transport or upstream error: return a failure result or throw a domain-appropriate exception that the CLI path can convert into a clear user-facing message.