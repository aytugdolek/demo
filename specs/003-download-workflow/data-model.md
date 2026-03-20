# Data Model: Download Workflow

## Overview

This feature adds a non-interactive download request, the resulting file target, and the outcome reported back to the user. It does not introduce database entities or schema changes.

## Entities

### DownloadRequest

- Purpose: Represents one invocation of the download workflow.
- Fields:
  - `isDownloadMode`: boolean, must be `true` when the `--download` option is used
  - `targetFileName`: string, the deterministic local file name selected for the run
  - `targetDirectory`: string, the local directory used for the download output
- Validation rules:
  - `isDownloadMode` must be true for this feature slice.
  - The target path must resolve to a single local file.

### DownloadTarget

- Purpose: Represents the fully resolved local file path for a download.
- Fields:
  - `fullPath`: string
  - `exists`: boolean
  - `isWritable`: boolean derived from the local environment
- Validation rules:
  - Existing files must be treated as a failure condition.
  - The path must be safe to report back to the user.

### DownloadResult

- Purpose: Represents the outcome of a download attempt.
- Fields:
  - `success`: boolean
  - `message`: string
  - `outputPath`: string?
  - `error`: string?
- Validation rules:
  - Success results must include a usable output path.
  - Failure results must include a clear human-readable message.

### DownloadedFile

- Purpose: Represents the local file produced by a successful run.
- Fields:
  - `path`: string
  - `sizeInBytes`: long
  - `writtenAt`: timestamp
- Validation rules:
  - The file must be written once per successful invocation.
  - The file must not replace an existing file in this feature slice.

## Relationships

- `DownloadRequest` resolves to one `DownloadTarget`.
- The download use case combines the remote transaction-history response with the `DownloadTarget` and produces a `DownloadResult`.
- A successful `DownloadResult` corresponds to one `DownloadedFile`.

## Notes

- The existing interactive paging data model remains unchanged.
- The download workflow is intentionally separate so the file-output path can be tested without paging prompts.