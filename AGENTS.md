# AGENTS.md

This repository is a single-product Slay the Spire 2 mod repository for
`DefectOnlyCrash`.

## Purpose

The mod does exactly two things:

- Auto-unlocks Defect by revealing `Defect1Epoch`.
- Terminates the game if the local player starts a run as any character other
  than Defect.

## Read First

When starting work in this repo, read these files first:

1. `README.md`
2. `DefectOnlyCrash.csproj`
3. `src/MainFile.cs`
4. `src/CharacterCrashMonitor.cs`
5. `mod_manifest.json`

## Current Architecture

- `src/MainFile.cs` is the mod initializer.
- `src/CharacterCrashMonitor.cs` installs a runtime monitor node.
- The monitor unlocks Defect through the game's own save/progress APIs.
- The monitor checks the local run character and calls `FailFast` for
  non-Defect runs.

## Build Assumptions

- Target stack: `Godot 4.5.1`, `.NET 9`, `C#`
- The project builds a drop-in package to `dist/windows-dropin/`
- Local machine overrides belong in `ModTemplate.local.props`

Typical commands:

```bash
~/.dotnet/dotnet build DefectOnlyCrash.csproj
~/.dotnet/dotnet publish DefectOnlyCrash.csproj
```

## Distribution Rule

This repo publishes one mod, not a generic template.

Keep the public-facing repository centered on:

- what the mod does
- how to install it
- where to download the release package

Do not reintroduce unrelated template/demo features into the root README or
main source tree.
