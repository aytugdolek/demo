# CLI Contract: CLI Paging MVP

## Purpose

Define the user-visible CLI behavior for the first live paging workflow.

## Supported Entry Points

### No Arguments

- Intent: Start the interactive transaction-history paging session.
- Behavior:
  - Build the application host and dependency graph.
  - Resolve the configured Socrata app token.
  - Request page 1 from the remote dataset.
  - Render a stable table of results ordered newest first.
  - Prompt for the next navigation input.
- Exit code:
  - `0` when the user exits the session normally.
  - Non-zero when startup cannot retrieve the initial page because configuration or remote access fails.

### `--validate-config`

- Intent: Preserve the explicit configuration validation path introduced in Feature 1.
- Behavior:
  - Validate the required Socrata app token and DemoDb connection string.
  - Do not start the interactive paging workflow.

### `--help`

- Intent: Describe supported CLI behavior.
- Behavior:
  - Explain that running with no arguments starts the interactive paging workflow.
  - List the paging controls `>`, `<`, and `Q`.
  - Preserve future workflow space for `--download` without claiming it is implemented.

## Interactive Controls

### `>`

- Intent: Move to the next page.
- Behavior:
  - Request the next page from the remote port.
  - Replace the rendered table with the new page when records are returned.
  - If no further records are available, keep the current session active and display clear feedback.

### `<`

- Intent: Move to the previous page.
- Behavior:
  - If the current page is greater than 1, request and render the previous page.
  - If the current page is 1, keep the user on page 1 and display boundary feedback.

### `Q`

- Intent: Exit the interactive workflow.
- Behavior:
  - End the session immediately and cleanly.
  - Do not request another page.

## Output Rules

- The session must display the current page number before each rendered table.
- The records view must render the same columns in the same order on every page.
- The MVP column set is `Transaction ID`, `Entity ID`, `Name`, `History`, and `Received`.
- Long text must be truncated or width-limited intentionally rather than overflowing uncontrolled.
- Empty pages, invalid input, configuration problems, and remote failures must be reported with readable terminal messages.