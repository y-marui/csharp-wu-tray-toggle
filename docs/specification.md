# Specification

## Overview

Windows Update の自動更新をシステムトレイから停止・再開する C# 製トレイアプリ。

## Windows Update control

| 操作 | レジストリキー | 値 | サービス操作 |
|---|---|---|---|
| 停止 | `HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU` | `NoAutoUpdate = 1` | `wuauserv` を停止 |
| 再開 | 同上 | `NoAutoUpdate = 0` | 起動種別を Manual に設定し `wuauserv` を起動 |

管理者権限が必要なため、トレイ本体は非昇格で常駐し、停止・再開クリック時のみ自exeを `--elevated-stop` / `--elevated-start` 引数・`Verb=runas` で起動して昇格プロセスに処理させる（`src/WuTrayToggle/WindowsUpdateController.cs`）。

## Icon display

| 状態 | アイコン | トレイテキスト |
|---|---|---|
| 稼働中 | RoyalBlue の弧 + 矢印 | `WU: 稼働中 (通常モード)` |
| 停止中 | Red の × | `WU: 停止中 (制御モード)` |

アイコンは `src/WuTrayToggle/Assets/` の静的 `.ico`（盾モチーフ、稼働中=更新矢印/停止中=赤X）を埋め込みリソースとして持ち、`IconFactory.Create` がリソースストリームから `Icon` を読み込む。exe自体のファイルアイコン（`Assets/app.ico`、盾+歯車）は `<ApplicationIcon>` で設定。

## Installation

配布はMSIインストーラーに一本化している（exeを直接配る経路や、アプリ自身がデスクトップのショートカットを作る経路は持たない）。

### MSI installer

`installer/`（WiX 6）が `WuTrayToggle-vX.Y.Z-win-x64.msi` をビルドする（`make msi`）。publishした自己完結のフォルダ（`dotnet publish`、単一ファイルにはしない）を `Files` 要素で丸ごと取り込む。バージョンは `Directory.Build.props` の `<Version>` から渡される。

- マシン単位（per-machine）インストール。`%ProgramFiles%\WuTrayToggle\WuTrayToggle.exe` に配置するため、インストール自体にUAC昇格が必要
- スタートメニュー・デスクトップショートカットはMSIネイティブの機能で作成・追跡し、アンインストール時に自動削除される
- Add/Remove Programs（アプリと機能）に登録される（MSI標準機能）
- アンインストール時、deferred custom action として `WuTrayToggle.exe --disable-startup` を実行し、ユーザーが後から有効化していた「ログイン時に自動起動」（スタートアップフォルダ、MSIの管理外）も解除する
- `UpgradeCode` は固定GUID（`059381dc-b129-4c96-b6ef-9644338a7330`）。将来のバージョンアップ時に上書きインストールできるよう、変更しない（本体 exe の Component の Guid も同様）
- 以前のソースビルド用 `--install`（`make install`）で作った、デスクトップの `WU_TrayIcon.lnk` は、MSIでは管理しない。残っていれば手動で削除する

## Auto-start at login

設定ウィンドウの「ログイン時に自動起動」（チェックボックス。「保存」で反映）で、スタートアップフォルダ（`Environment.SpecialFolder.Startup`、`%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup`）への自exeショートカット登録/解除をトグルする（`ShortcutManager.EnableStartup`/`DisableStartup`）。
レジストリの `Run` キー等は使用せず、ユーザー単位のスタートアップフォルダのみを使う（永続的なレジストリインストールを避ける方針に従う）。
MSIのアンインストール時は、このスタートアップ登録も合わせて解除される（上記の `--disable-startup`）。

## Localization

`docs/dev-charter/LOCALIZATION_POLICY.md` に従い、メニュー・トレイツールチップ・メッセージボックス・バルーン通知のテキストを日本語/英語/中国語/ヒンディー語/スペイン語/フランス語/ポルトガル語の7言語に対応する。

- 言語決定の優先順位: ユーザー設定（設定ウィンドウの「言語」で選択、`%APPDATA%\WuTrayToggle\settings.json` の `language` に永続化。旧版の `language.txt` が残っていれば、初回の読み込みでJSONへ移行して削除する）＞システムのUI言語（`GetUserDefaultUILanguage` で判定）＞英語
- 設定ファイルへの書き込みに失敗した場合（権限不足等）は言語を切り替えず、バルーン通知でエラーを表示する（`AppSettings`/`Localization.SetOverride` が `bool` で成否を返す）
- 文字列は `src/WuTrayToggle/Strings.cs` に集約。中国語/ヒンディー語/スペイン語/フランス語/ポルトガル語は機械翻訳のままで、ネイティブレビュー未実施（文言の不備は Issue で報告する）
- `.resx` には移行しない。理由: 言語を `CultureInfo` ではなく `Localization` で解決し（下記）、ユーザーの明示指定も `settings.json` で持つため、`ResourceManager` の文化フォールバックをそのまま使えない。言語ごとのサテライトアセンブリでMSIのファイルも増える。文字列が少数のため、全7言語を1ファイルで一覧できる現状の表を維持する（新しい文字列は `L(...)` に7言語を `ja, en, zh, hi, es, fr, pt` の順で渡す）
- `<InvariantGlobalization>true</InvariantGlobalization>` のため `CultureInfo` はシステム言語を反映しない。そのため `Localization.cs` は Win32 API を直接 P/Invoke してシステムのUI言語を取得する

## Known limitations

- Windows 専用（System.Windows.Forms が必要）
- レジストリ操作とサービス制御に管理者権限が必要
