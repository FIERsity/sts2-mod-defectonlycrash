# Repo Map

## Top Level

- `src/`
  C# source code for the mod.
- `assets/`
  Files copied next to the built mod output, such as `mod_image.png`.
- `docs/`
  Project explanation and onboarding notes.
- `ModTemplate.csproj`
  Main build configuration.
- `project.godot`
  Godot project configuration.
- `mod_manifest.json`
  Runtime mod metadata.

## Source Layout

- `src/Bootstrap/MainFile.cs`
  Mod initializer. Keeps startup glue small and defers real behavior to feature
  code.
- `src/Features/Cheats/CheatOverlay.cs`
  Current runtime cheat behavior:
  - heal from top bar HP
  - refill energy from energy orb
  - delete cards from inspect screen

## Build/Runtime Notes

- Build with `dotnet build`
- Publish with `dotnet publish`
- On macOS, the live mod directory is:
  `SlayTheSpire2.app/Contents/MacOS/mods`

## Suggested Future Expansion

If the project grows beyond this cheat mod, add folders like:

- `src/Features/Cards/`
- `src/Features/Relics/`
- `src/Features/Events/`
- `src/Features/UI/`
- `content/localization/`
- `content/scenes/`
- `content/images/`

Keep feature code separate from bootstrap/setup code.

## Identity Rule

This repo is the development base.

The live mod installed in the game's `mods` directory may be a separately named
build. Do not treat the repository name and the installed mod name as the same
thing by default.
