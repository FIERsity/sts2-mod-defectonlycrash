# DefectOnlyCrash

`DefectOnlyCrash` is a Slay the Spire 2 mod focused on one joke rule:
only Defect is allowed.

`DefectOnlyCrash` 是一个《Slay the Spire 2》模组，核心规则只有一条：
只允许使用故障机器人。

## Note

This mod was mainly made through vibe coding.

The author only has access to macOS hardware and has not yet been able to do a
real Windows playtest. If you try it on Windows, feedback is very welcome.

## 说明

这个模组主要是通过 vibe coding 完成的。

作者目前只有 Mac 电脑，还没有做过真正的 Windows 实机测试。如果你在
Windows 上试用了它，非常欢迎反馈结果。

## Overview

- Automatically unlocks Defect when the mod loads
- Allows normal runs as Defect
- Intentionally terminates the game if the local player starts a run as any
  other character

## 功能概览

- 模组加载时自动解锁故障机器人
- 使用故障机器人时可以正常游玩
- 本地玩家如果用其他角色开局，游戏会闪退

## Installation

1. Open the Slay the Spire 2 game directory.
2. Create a folder named `mods` if it does not already exist.
3. Download the latest release asset `DefectOnlyCrash-windows-dropin.zip`.
4. Extract it.
5. Copy all extracted files into `mods`.
6. Launch the game.

## 安装方法

1. 打开《Slay the Spire 2》的游戏目录。
2. 如果还没有 `mods` 文件夹，就手动创建一个。
3. 下载最新发布附件 `DefectOnlyCrash-windows-dropin.zip`。
4. 解压压缩包。
5. 将解压后的全部文件复制到 `mods` 文件夹中。
6. 启动游戏。

## Usage

- After the mod loads, Defect should become selectable automatically.
- Starting a run as Defect behaves normally.
- Starting a run as Ironclad, Silent, Regent, Necrobinder, or any other
  non-Defect character will intentionally close the game.

## 使用说明

- 模组加载后，故障机器人应当会自动变为可选角色。
- 选择故障机器人开局时，一切正常。
- 如果选择其他任何非故障机器人角色开局，游戏闪退。

## Save Warning

- The game may show a one-time mod warning when mods are enabled.
- Modded play may use a separate modded save path.
- If Steam reports a cloud conflict after switching between modded and normal
  play, review the local files carefully before choosing which side to keep.

## 存档提醒

- 首次启用模组时，游戏可能会弹出一次 mod 警告。
- 启用模组后，游戏可能会使用单独的 modded 存档路径。
- 如果你在普通模式和模组模式之间切换，Steam 可能会提示云存档冲突，选择前请先确认本地文件状态，推荐做好备份。

## Package Contents

- `DefectOnlyCrash.dll`
- `DefectOnlyCrash.pck`
- `DefectOnlyCrash.json`
- `mod_image.png`
- `INSTALL.txt`

## Build

```bash
~/.dotnet/dotnet build DefectOnlyCrash.csproj
~/.dotnet/dotnet publish DefectOnlyCrash.csproj
```

The packaged output is written to:

`dist/windows-dropin/`
