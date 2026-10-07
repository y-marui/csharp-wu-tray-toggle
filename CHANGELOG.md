# Changelog

All notable changes to this project will be documented here.

## [Unreleased]

## [1.1.0] - 2026-10-07

### Changed
- The tray menu is shortened to: Check current status, Stop, Resume, Settings…, Exit.
- Settings are stored as typed JSON in `%APPDATA%\WuTrayToggle\settings.json` (`language`). A `language.txt` left by an earlier version is migrated on first read and then deleted. A missing or corrupt settings file falls back to defaults instead of failing.
- Moved from .NET 8 to .NET 10 LTS (.NET 8 reaches end of support on 2026-11-10). The SDK is pinned in `global.json`.
- Adopted the standard C# layout from the dev-charter C# topic: `global.json`, `Directory.Build.props` (single `<Version>`, analyzers at `latest-Recommended`, warnings are errors on CI), `Directory.Packages.props` (Central Package Management), `.editorconfig`, and a `.slnx` solution.
- The MSI is now built with WiX 6 and packages the self-contained published folder (multi-file; no longer a single-file exe). `UpgradeCode` and the main component GUID are unchanged, so existing installs upgrade in place.
- High DPI mode is now `PerMonitorV2` (via `ApplicationHighDpiMode` and `ApplicationConfiguration.Initialize()`), previously `SystemAware`.
- CI uses `actions/setup-dotnet@v5` with `global-json-file` and builds the MSI from the published folder.

### Added
- Automated tests (`tests/WuTrayToggle.Tests`, xUnit v3) for the settings storage and the `language.txt` migration, run in CI.
- A settings window (tray menu "Settings…"): the display language and "Start at login" now live in an independent window with Save/Cancel, instead of tray submenu items. Strings are provided in all 7 supported languages (the new zh/hi/es/fr/pt strings are machine-translated, like the existing ones).
- `release.yml`: pushing a tag `vX.Y.Z` builds the MSI and attaches it to a GitHub Release.
- Dependabot for `nuget`, `dotnet-sdk`, and `github-actions`.

### Removed
- `--install` / `--uninstall` and `make install` / `make uninstall`: distribution is MSI-only. A desktop `WU_TrayIcon.lnk` left over from the old source install is not managed by the MSI and can be deleted manually. Use `make run` during development.

## [1.0.0] - 2026-08-04

### Added
- MSI installer (`installer/`, WiX Toolset v5, `make msi`): per-machine install to `%ProgramFiles%\WuTrayToggle`, Start Menu and desktop shortcuts, Add/Remove Programs registration. Recommended install path for end users; GitHub Releases attach the MSI rather than the raw exe.
- `--disable-startup` CLI argument, used by the MSI's uninstall custom action to clean up the Startup-folder autostart shortcut (not tracked by MSI itself, since it's created later via the tray menu).
- `EnableCompressionInSingleFile` baked into the csproj, shrinking the self-contained publish output from ~154MB to ~72MB.
- Tray menu now has a checkable "ログイン時に自動起動" item that toggles a shortcut in the current user's Startup folder, so the app can launch automatically at login without a persistent registry install.
- Balloon-tip feedback for stop/resume actions (success/failure/UAC-cancelled), and static shield-themed icons (`src/WuTrayToggle/Assets/`) replacing the runtime-drawn shapes for the exe file icon and the two tray states.
- Localized all UI text (menu, tray tooltip, message boxes, balloon tips) into the 7 languages listed in `docs/dev-charter/LOCALIZATION_POLICY.md` (Japanese/English/Chinese/Hindi/Spanish/French/Portuguese), with a tray "Language" submenu (system default / per-language override, priority: user setting > system UI language > English). Closes #3.

### Changed
- Rewrote the tray app from a PowerShell script (`src/tray.ps1`/`src/install.ps1`) to a self-contained C# (.NET 8, WinForms) single-file exe (`src/WuTrayToggle/`), to fix instability in the `powershell.exe`-based launch path (console flash, execution-policy/AMSI delay, no multi-instance guard). The desktop shortcut now launches the exe directly, and the exe installs/uninstalls its own shortcut via `--install`/`--uninstall`.
- Embedded version 1.0.0 in the executable and added it to the status dialog.
- Language-setting file errors no longer terminate the tray application.
