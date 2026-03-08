# DefectOnlyCrash

`DefectOnlyCrash` is a Slay the Spire 2 mod.

`DefectOnlyCrash` 是一个《Slay the Spire 2》模组。

## What It Does

- Automatically unlocks Defect
- Allows normal runs as Defect
- Intentionally terminates the game if any other character starts a run

## 功能

- 自动解锁故障机器人
- 使用故障机器人正常游玩
- 使用其他角色开局时，游戏会被故意终止

## Installation

1. Create a `mods` folder in the Slay the Spire 2 game directory.
2. Copy every file from the release package into `mods`.
3. Launch the game.

Release asset:
`DefectOnlyCrash-windows-dropin.zip`

## 安装

1. 在游戏根目录创建 `mods` 文件夹。
2. 将发布包中的所有文件复制进去。
3. 启动游戏。

发布附件：
`DefectOnlyCrash-windows-dropin.zip`

## Included Files

- `DefectOnlyCrash.dll`
- `DefectOnlyCrash.pck`
- `mod_manifest.json`
- `mod_image.png`
- `INSTALL.txt`

## Development

Build:

```bash
~/.dotnet/dotnet build DefectOnlyCrash.csproj
~/.dotnet/dotnet publish DefectOnlyCrash.csproj
```

The packaged output is written to:

`dist/windows-dropin/`
