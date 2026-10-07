# Developing

## Requirements

- Windows
- .NET 10 SDK (pinned in `global.json`; `rollForward: latestFeature`)
- Administrator privileges (for registry and service access)
- pre-commit

## Setup

```powershell
dotnet build WuTrayToggle.slnx
pre-commit install
pre-commit install --hook-type commit-msg
```

## Running Directly

```powershell
dotnet run --project src/WuTrayToggle
```

## Lint

```powershell
dotnet format WuTrayToggle.slnx --verify-no-changes
$env:CI = "true"; dotnet build WuTrayToggle.slnx --no-incremental
```

With `CI=true`, warnings are errors (see `Directory.Build.props`), matching the CI. Ordinary local builds only show them as warnings.

## Tests

```powershell
dotnet test --solution WuTrayToggle.slnx
```

Tests live in `tests/WuTrayToggle.Tests` (xUnit v3, run by Microsoft.Testing.Platform as set in `global.json`). The app is a self-contained WinExe and cannot be referenced from a test executable, so the test project compiles only the dependency-free sources it needs (currently `AppSettings.cs`) by link. `AppSettings` has internal overloads that take the settings folder, so tests use a temporary folder instead of `%APPDATA%`. CI runs the tests in the `build` job.

## Installer (MSI)

Distribution is MSI-only. The installer is not part of the `.slnx` (its input is the published folder), so it is built on its own after publishing:

```powershell
dotnet publish src/WuTrayToggle -c Release -o publish
dotnet build installer/WuTrayToggle.Installer.wixproj -c Release -p:PublishDir=$PWD\publish
```

The output is `installer/bin/x64/Release/WuTrayToggle-v<Version>-win-x64.msi`.

- The WiX version is pinned in `global.json` (`msbuild-sdks`). WiX v6 and later fall under the Open Source Maintenance Fee (OSMF); it is not required for non-revenue use.
- Do not change the `UpgradeCode` or the `MainExecutable` component GUID in `installer/Product.wxs` (they keep in-place upgrades working).
- To release, push a tag `vX.Y.Z`; `.github/workflows/release.yml` attaches the MSI to a GitHub Release.
- Every release needs a higher `<Version>` than the previous one: the MSI uses `MajorUpgrade` without `AllowSameVersionUpgrades`, so installing a same-version build over an existing install is rejected.
- A locally built MSI with a higher version blocks installing lower versions (including the released one) until it is uninstalled. Uninstall development builds before testing a release build.

## Conventions

- **Naming:** PascalCase for classes/methods (`WindowsUpdateController`, `GetState`), camelCase for local variables/fields
- **Comments:** Explain *why*, not *what* — see `docs/dev-charter/CODE_STYLE.md`
- **Commit messages:** Conventional Commits format (`feat:`, `fix:`, `docs:`, `chore:`)
- **Branching:** One branch per feature/fix; merge to `main` via PR
- **Version:** `Directory.Build.props` `<Version>` is the single source (keep `CHANGELOG.md` in sync)

## Architecture

See [docs/architecture.md](docs/architecture.md).
