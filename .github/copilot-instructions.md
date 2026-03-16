# Copilot Instructions

## Git Ignore Awareness

- Always inspect the workspace `.gitignore` before suggesting files to commit, creating repository artifacts, or preparing content for a pull request.
- Treat `.gitignore` as the source of truth for what should stay out of version control.
- Do not stage, commit, or recommend committing files and folders that match `.gitignore` rules.
- If a generated file or tool artifact is ignored, keep it local unless the user explicitly asks to change the ignore policy.
- If `.gitignore` does not exist, call that out and propose creating one before adding generated artifacts or local-only files to the repository.
- When new tools or workflows create local state, check whether their outputs should be added to `.gitignore`.

## Repository Hygiene

- Avoid including temporary files, IDE state, tool caches, build outputs, logs, local databases, secrets, downloaded exports, or machine-specific configuration in the repository unless the user explicitly requests it.
- Before recommending a new file for version control, prefer files that are source, configuration, documentation, migrations, or tests rather than generated output.
- If there is uncertainty about whether a file should be tracked, ask or explain the tradeoff before proceeding.

## Current Workspace Guidance

- Be cautious with tool-generated files under hidden folders.
- Review `.specify` outputs before assuming they belong in version control.
- Keep local initialization metadata and transient output out of the repository unless the user wants them tracked.