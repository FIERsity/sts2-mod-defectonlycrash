# ModTemplate-StS2

This repository is a reusable Slay the Spire 2 mod development base.

It currently contains one working example feature set:

- Click the top bar HP display to fully heal.
- Click the combat energy orb to refill energy.
- Open a card in the inspect screen and use the `删牌` button to remove it.

That example is here to prove the toolchain and runtime integration work. It is
not the long-term identity of the repository itself.

## What This Repo Is

- `Godot 4.5.1 + C# + .NET 9` Slay the Spire 2 mod project
- Built against the game's `sts2.dll` and `0Harmony.dll`
- By default, this repository builds `ModTemplate.dll` and `ModTemplate.pck`
- Currently implemented without Harmony runtime patches for gameplay UI
  injection on macOS; instead it uses a Godot overlay node installed at mod
  initialization time

## Important Files

- `src/Bootstrap/MainFile.cs`
  Mod entrypoint. Installs the overlay node from `[ModInitializer]`.
- `src/Features/Cheats/CheatOverlay.cs`
  Main gameplay cheat logic. Handles heal, energy refill, and card deletion UI.
- `ModTemplate.csproj`
  Build and publish logic. Also contains the platform-specific game path
  defaults and the copy-to-mods behavior.
- `ModTemplate.local.props.example`
  Machine-local override template. Copy this to `ModTemplate.local.props`.
- `mod_manifest.json`
  Mod metadata used by the game when loading the `.pck`.
- `export_presets.cfg`
  Godot export presets used for `.pck` generation.
- `project.godot`
  Godot project file.

## Directory Notes

- `src/`
  C# source code for the mod.
- `src/Bootstrap/`
  Mod entrypoints and startup glue.
- `src/Features/Cheats/`
  The current cheat feature implementation.
- `assets/`
  External mod assets copied beside the built mod, currently `mod_image.png`.
- `docs/`
  Repo documentation intended for humans and AI agents.
- `packages/`
  Local NuGet cache because `nuget.config` is configured to restore packages
  inside the repo. This is generated content and should not be committed.
- `.godot/`
  Generated Godot project state. This is generated content and should not be
  committed.

## Local Setup

Copy `ModTemplate.local.props.example` to `ModTemplate.local.props`, then fill
in the machine-specific paths.

Important values:

- `SteamLibraryPath`
- `Sts2Path`
- `Sts2DataDir`
- `Sts2ModsDir`
- `GodotPath`
- `GodotExportPreset`

On macOS, the game executable is inside the app bundle, and the actual scanned
mod directory is:

`SlayTheSpire2.app/Contents/MacOS/mods`

That macOS path matters. The game loads mods from the executable directory's
`mods` folder, not from `Contents/Resources/mods`.

## Build And Publish

Typical workflow:

```bash
dotnet build
dotnet publish
```

What happens:

1. The project builds `ModTemplate.dll`.
2. The build copies the DLL plus `mod_manifest.json` and `mod_image.png` into
   the configured mods directory.
3. `dotnet publish` also runs Godot headless export to generate
   `ModTemplate.pck`.

## Current macOS Notes

- The current project is configured and tested for macOS.
- `BaseLib` package restore still exists in the project, but the local macOS
  setup currently disables copying `BaseLib.dll` and `BaseLib.pck` into the live
  mods directory because Harmony patching was throwing runtime
  `NotImplementedException` on this machine.
- The current cheat functionality does not depend on BaseLib at runtime.

## For Future AI Work

Start with these files in this order:

1. `README.md`
2. `AGENTS.md`
3. `ModTemplate.csproj`
4. `src/Bootstrap/MainFile.cs`
5. `src/Features/Cheats/CheatOverlay.cs`

If you are changing behavior, prefer extending `src/Features/Cheats/CheatOverlay.cs` unless you
have a strong reason to reintroduce Harmony patching.

## Current Tree

```text
.
├── AGENTS.md
├── README.md
├── docs/
├── assets/
│   └── mod_image.png
├── src/
│   ├── Bootstrap/
│   │   └── MainFile.cs
│   └── Features/
│       └── Cheats/
│           └── CheatOverlay.cs
├── ModTemplate.csproj
├── ModTemplate.sln
├── ModTemplate.local.props.example
├── mod_manifest.json
├── project.godot
├── export_presets.cfg
├── icon.svg
└── nuget.config
```

## Important Distinction

There are two separate things:

1. This repository
   It is the reusable development base and should stay generic.
2. The game's live `mods` directory
   That may contain a concrete mod build with its own final name, manifest,
   version, and author information.

Do not assume the identity of the currently installed live mod should be copied
back into this repository.
