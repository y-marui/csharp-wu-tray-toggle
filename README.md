# csharp-wu-tray-toggle

> **This is the reference (English) version.**
> The canonical (Japanese) version is [README-jp.md](README-jp.md).

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![CI](https://github.com/y-marui/csharp-wu-tray-toggle/actions/workflows/ci.yml/badge.svg)](https://github.com/y-marui/csharp-wu-tray-toggle/actions/workflows/ci.yml)
[![Charter Check](https://github.com/y-marui/csharp-wu-tray-toggle/actions/workflows/dev-charter-check.yml/badge.svg)](https://github.com/y-marui/csharp-wu-tray-toggle/actions/workflows/dev-charter-check.yml)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/y-marui?style=social)](https://github.com/sponsors/y-marui)
[![Buy Me a Coffee](https://img.shields.io/badge/Buy%20Me%20a%20Coffee-donate-yellow.svg)](https://www.buymeacoffee.com/y.marui)

A C# system tray app to stop and resume Windows Update from the notification area.

## Setup

Requires Windows and administrator privileges.

### From the installer (recommended)

Download the latest `WuTrayToggle-vX.Y.Z-win-x64.msi` from [Releases](https://github.com/y-marui/csharp-wu-tray-toggle/releases) and run it. After the UAC prompt, Start Menu and desktop shortcuts are created. Uninstall from "Apps & features". To build from source instead, see [Building from source](#building-from-source).

### Building from source

Requires the .NET 8 SDK.

```powershell
make install
```

Builds a self-contained single-file exe and creates a shortcut `WU_TrayIcon.lnk` on the desktop. Double-click it to launch the tray icon.

## Usage

| Command | Description |
|---|---|
| `make install` | Build and install a desktop shortcut |
| `make uninstall` | Remove desktop shortcut |
| `make msi` | Build the MSI installer into `dist/` |

Right-click the tray icon to access the menu:

- **Check current status** — Shows the app version, registry policy, and service state
- **Stop (disable auto-update)** — Stops Windows Update via group policy
- **Resume (normal)** — Re-enables Windows Update

## License

[MIT](LICENSE)

---
*This document has a Japanese canonical version [README-jp.md](README-jp.md). Update both in the same commit when editing.*
