# Architecture

## Overview

C# (.NET 10, WinForms) 製システムトレイアプリ。`NotifyIcon` でトレイアイコンを常駐させ、
右クリックメニューから Windows Update の停止・再開を操作する。
レジストリポリシーとサービス制御で WU を制御し、操作には管理者昇格が必要。

## Entry Points

- `src/WuTrayToggle/Program.cs` — エントリポイント。引数なしならトレイ常駐（`Mutex` で多重起動防止）、`--disable-startup`（MSIのアンインストール用）/`--elevated-stop`/`--elevated-start` を解釈する
- `src/WuTrayToggle/TrayApplicationContext.cs` — トレイアイコン・コンテキストメニューの常駐処理
- `src/WuTrayToggle/SettingsForm.cs` — 独立した設定ウィンドウ（言語・ログイン時の自動起動）。メニューの「設定…」からモーダルで開く

## Directory Structure

| ディレクトリ | 役割 |
|---|---|
| `src/WuTrayToggle/` | C# プロジェクト本体（.csproj + ソース） |
| `installer/` | MSIインストーラー定義（WiX 6）。`.slnx` には含めず、publish のあとに単独でビルドする |
| `docs/dev-charter/` | 開発憲章（git subtree） |
| `docs/` | プロジェクトドキュメント |

## Key Dependencies

| ライブラリ / モジュール | 用途 |
|---|---|
| `System.Windows.Forms` | トレイアイコン・コンテキストメニュー・メッセージボックス |
| `System.Drawing` | 埋め込み`.ico`リソースからの `Icon` 読み込み |
| `Microsoft.Win32.Registry` | WU ポリシー(`NoAutoUpdate`)・サービス起動種別の読み書き |
| `System.ServiceProcess.ServiceController` (NuGet) | `wuauserv` サービスの状態取得・起動・停止 |
| `IShellLinkW`/`IPersistFile` (COM interop) | ログイン時自動起動のショートカット（.lnk）の作成・削除 |
| WiX 6 (`WixToolset.Sdk`, `global.json` の `msbuild-sdks`) | `installer/` のMSIビルド（`dotnet build` で完結、Visual Studio拡張不要） |
