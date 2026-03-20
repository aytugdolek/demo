# CLI Contract: Download Workflow

## Purpose

Define the user-visible behavior of the non-interactive `--download` path.

## Command

- `--download` selects the non-interactive download workflow.
- The command must not prompt for paging input when this option is present.
- The existing no-argument paging workflow remains unchanged.

## Success Behavior

- The CLI must save the download to a local file.
- The CLI must report the output path or another equally actionable success message.
- The command must exit with a success code after the file is written.

## Failure Behavior

- If the target file already exists, the CLI must fail and leave the existing file unchanged.
- If the file cannot be written, the CLI must fail with a clear message.
- If the remote source cannot be retrieved, the CLI must fail with a clear message.
- Failure cases must not enter the paging flow.

## Output Expectations

- Success output should clearly state that the download completed and where the file was written.
- Failure output should clearly state whether the problem was remote retrieval, file existence, or local write failure.