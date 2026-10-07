# File Map

_最終更新: 2026-10-08_

## Core

| ファイル | 役割 | 主な依存先 |
|---|---|---|
| `global.json` | SDK（10.0.100, `latestFeature`）・テストランナー・WiX SDK のバージョン | — |
| `Directory.Build.props` | 共通プロパティ（`Version`・`Nullable`・解析設定。`CI=true` で警告をエラーにする） | — |
| `Directory.Packages.props` | NuGet のバージョン（Central Package Management） | — |
| `.editorconfig` | 整形・コードスタイル・アナライザの重大度 | — |
| `WuTrayToggle.slnx` | ソリューション（`installer/` は含めない） | `src/WuTrayToggle`, `tests/WuTrayToggle.Tests` |
| `tests/WuTrayToggle.Tests/WuTrayToggle.Tests.csproj` | テストプロジェクト（xUnit v3, net10.0）。本体は自己完結の WinExe で参照できないため、`AppSettings.cs` をリンクで取り込む | `xunit.v3` (NuGet) |
| `tests/WuTrayToggle.Tests/AppSettingsTests.cs` | `AppSettings` のテスト（保存・取得、`language.txt` の移行、破損・空ファイル、書き込み失敗）。一時フォルダを保存先にする | `AppSettings` |
| `src/WuTrayToggle/WuTrayToggle.csproj` | プロジェクト定義（net10.0-windows, 自己完結の複数ファイル発行, 高DPI `PerMonitorV2`） | `System.ServiceProcess.ServiceController` (NuGet) |
| `src/WuTrayToggle/Program.cs` | エントリポイント。引数解析（`--disable-startup`/`--elevated-stop`/`--elevated-start`）、多重起動防止、`ApplicationConfiguration.Initialize()` | `TrayApplicationContext`, `ShortcutManager`, `WindowsUpdateController` |
| `src/WuTrayToggle/TrayApplicationContext.cs` | トレイアイコン常駐・メニュー操作（状態確認・停止・再開・設定…・終了）。設定ウィンドウを開く | `System.Windows.Forms`, `IconFactory`, `WindowsUpdateController`, `SettingsForm` |
| `src/WuTrayToggle/IconFactory.cs` | 埋め込み`.ico`リソース(`Assets/`)からのアイコン読み込み | `System.Drawing` |
| `src/WuTrayToggle/Assets/app.ico` | exeファイルアイコン(盾+歯車、`ApplicationIcon`) | — |
| `src/WuTrayToggle/Assets/tray-running.ico` | トレイアイコン(稼働中、盾+更新矢印) | — |
| `src/WuTrayToggle/Assets/tray-stopped.ico` | トレイアイコン(停止中、盾+赤X) | — |
| `src/WuTrayToggle/WindowsUpdateController.cs` | レジストリ確認・サービス状態確認・WU 停止/再開 | `Microsoft.Win32.Registry`, `System.ServiceProcess` |
| `src/WuTrayToggle/ShortcutManager.cs` | ログイン時自動起動(スタートアップフォルダ)の登録・解除 | `IShellLinkW`/`IPersistFile` (COM interop) |
| `src/WuTrayToggle/TrayState.cs` | トレイ状態(Running/Stopped)の列挙型 | — |
| `src/WuTrayToggle/AppLanguage.cs` | 対応7言語の列挙型 | — |
| `src/WuTrayToggle/Localization.cs` | 表示言語の解決(ユーザー設定＞システム言語＞英語)。`GetUserDefaultUILanguage` をP/Invoke | `AppSettings` |
| `src/WuTrayToggle/AppSettings.cs` | 設定の永続化(`%APPDATA%\WuTrayToggle\settings.json`、型付きJSON)。旧 `language.txt` が残っていれば初回の読み込みでJSONへ移行して削除する。読み込み失敗（欠損・破損）は既定値で続行する。保存先のフォルダを受け取る内部オーバーロードはテスト用 | `System.Text.Json` |
| `src/WuTrayToggle/SettingsForm.cs` | 独立した設定ウィンドウ（モーダル）。言語の選択と「ログイン時に自動起動」を設定する | `Localization`, `ShortcutManager`, `Strings` |
| `src/WuTrayToggle/Strings.cs` | UI文字列テーブル(7言語) | `Localization` |
| `archives/icons/*.png` | `Assets/*.ico` の元絵(透過PNG)。差し替え時はここから正方形化・多重解像度化して再生成する | — |
| `installer/WuTrayToggle.Installer.wixproj` | MSIビルド定義(WiX 6 SDK、`global.json` で固定)。`Version`(`Directory.Build.props`)/`PublishDir` を `DefineConstants` で `.wxs` に渡す | `WixToolset.Sdk`, `WixToolset.UI.wixext` (NuGet) |
| `installer/Product.wxs` | MSIパッケージ定義。per-machineインストール・publishフォルダを `Files` で取り込み・ショートカット・`--disable-startup` custom action | `publish/`（`dotnet publish` の出力。ビルド前提） |
| `.github/workflows/release.yml` | タグ `vX.Y.Z` のpushでMSIをビルドしGitHub Releasesに添付する | `installer/` |
| `.github/dependabot.yml` | `nuget`・`dotnet-sdk`・`github-actions` の更新 | — |
