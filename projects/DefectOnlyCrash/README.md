# DefectOnlyCrash

Standalone Slay the Spire 2 mod package project.

## 简介

- 自动解锁故障机器人
- 使用故障机器人开局时不处理
- 使用其他角色开局时，游戏会被故意终止

## Introduction

- Automatically unlocks Defect
- Does nothing if the local player starts as Defect
- Intentionally terminates the game if the local player starts as any other character

## Build

From this directory:

```bash
~/.dotnet/dotnet build
~/.dotnet/dotnet publish
```

The packaged drop-in files are written to:

`dist/windows-dropin/`

## Installation

For end users on Windows:

1. Create a `mods` folder inside the Slay the Spire 2 game directory.
2. Copy every file from `dist/windows-dropin/` into that `mods` folder.
3. Start the game.

## Package Contents

- `DefectOnlyCrash.dll`
- `DefectOnlyCrash.pck`
- `mod_manifest.json`
- `mod_image.png`
- `INSTALL.txt`
