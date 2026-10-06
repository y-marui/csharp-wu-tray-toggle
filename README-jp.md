# csharp-wu-tray-toggle

> **このファイルは正本（日本語版）です。**
> 英語版（参照）は [README.md](README.md) を参照してください。

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![CI](https://github.com/y-marui/csharp-wu-tray-toggle/actions/workflows/ci.yml/badge.svg)](https://github.com/y-marui/csharp-wu-tray-toggle/actions/workflows/ci.yml)
[![Charter Check](https://github.com/y-marui/csharp-wu-tray-toggle/actions/workflows/dev-charter-check.yml/badge.svg)](https://github.com/y-marui/csharp-wu-tray-toggle/actions/workflows/dev-charter-check.yml)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/y-marui?style=social)](https://github.com/sponsors/y-marui)
[![Buy Me a Coffee](https://img.shields.io/badge/Buy%20Me%20a%20Coffee-donate-yellow.svg)](https://www.buymeacoffee.com/y.marui)

Windows Update の自動更新を通知領域のトレイアイコンから停止・再開できる C# 製トレイアプリ。

## Setup

Windows と管理者権限が必要です。

### From the installer (recommended)

[Releases](https://github.com/y-marui/csharp-wu-tray-toggle/releases) から最新の `WuTrayToggle-vX.Y.Z-win-x64.msi` をダウンロードして実行します。UAC昇格後、スタートメニューとデスクトップにショートカットが作成されます。アンインストールは「アプリと機能」から行えます。ソースからビルドする場合は [Building from source](#building-from-source) を参照してください。

### Building from source

.NET 10 SDK が必要です（`global.json` で固定）。配布はインストーラー（MSI）に一本化しているため、ソースから直接インストールする方法はありません。開発時の起動は `make run` です。

```powershell
make run
```

## Usage

| コマンド | 説明 |
|---|---|
| `make run` | アプリを起動する（開発用） |
| `make lint` | `dotnet format` と、警告をエラーにしたビルド |
| `make msi` | publish して MSI を `installer/bin/x64/Release/` にビルド |

トレイアイコンを右クリックしてメニューを操作します：

- **現在の状態を確認** — アプリバージョン、ポリシー、サービスの状態をポップアップ表示
- **停止 (制御開始)** — グループポリシー経由で Windows Update を停止
- **再開 (通常)** — Windows Update を通常モードに戻す

## License

[MIT](LICENSE)

---
*この文書には英語版 [README.md](README.md) があります。編集時は同一コミットで更新してください。*
