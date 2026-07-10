# DefectOnlyCrash

A small *Slay the Spire 2* joke mod with one rule: play as the Defect—or the game closes.

## Features

- Unlocks the Defect when the mod loads
- Leaves Defect runs unchanged
- Intentionally terminates a run started with any other character

## Installation

1. Download `DefectOnlyCrash-windows-dropin.zip` from the latest release.
2. Extract the archive into the game's `mods` directory.
3. Keep every packaged file together and launch the game.

> Windows has not yet been fully playtested. Back up your save before switching between modded and unmodded play.

## Build

Requires .NET 9, Godot 4.5.1, and a local *Slay the Spire 2* assembly path.

```bash
dotnet build DefectOnlyCrash.csproj
dotnet publish DefectOnlyCrash.csproj
```

Packaged files are written to `dist/windows-dropin/`.

---

## 中文

一个规则简单的《杀戮尖塔 2》趣味模组：只能使用故障机器人，否则游戏会主动关闭。

### 功能

- 加载时自动解锁故障机器人
- 故障机器人可正常游玩
- 使用其他角色开局时主动关闭游戏

### 安装

1. 从最新 Release 下载 `DefectOnlyCrash-windows-dropin.zip`。
2. 解压到游戏的 `mods` 目录。
3. 保留压缩包内全部文件并启动游戏。

> Windows 版本尚未完成充分实机测试；切换模组与原版前请备份存档。
