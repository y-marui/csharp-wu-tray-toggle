# AI_CONTEXT.md

## Reference Order

AI はタスク開始時に以下の順で参照する:

1. README.md（概要・セットアップ）
2. DEVELOPING.md（ビルド・実装規約・命名規則）

必要に応じて以下を参照する（順不同）:
- CONTRIBUTING.md（PR・Issue ルール）
- docs/architecture.md（モジュール・コンポーネント構造）
- docs/file-map.md（ファイルレベルの依存関係 ※情報が足りない・古い場合は適宜探索し、追記・更新する）
- docs/specification.md（機能仕様・データフロー）
- docs/ui-design.md（UI 設計・コンポーネント仕様）

## Project Overview

Windows Update の自動更新をシステムトレイから停止・再開できる C# 製トレイアプリ。

- **言語:** C# (.NET 10, WinForms)
- **対象OS:** Windows のみ
- **主要ファイル:**
  - `src/WuTrayToggle/TrayApplicationContext.cs` — トレイアイコン本体（System.Windows.Forms を使用）
  - `src/WuTrayToggle/WindowsUpdateController.cs` — レジストリ確認・サービス制御
  - `src/WuTrayToggle/ShortcutManager.cs` — ログイン時の自動起動（スタートアップのショートカット）の作成・削除
  - `src/WuTrayToggle/Program.cs` — エントリポイント（引数解析・多重起動防止）
  - `installer/` — WiX 6 の MSI（配布の入口はこれに一本化）
  - `Makefile` — run / lint / publish / msi コマンド

## Applied Charter Principles

憲章参照: `docs/dev-charter/CHARTER_INDEX.md` でトピックを特定してから該当ファイルのみ読む。

- **言語ポリシー:** README.md（英語・参照）と README-jp.md（日本語・正本）を同一コミットで更新する → `docs/dev-charter/LANGUAGE_POLICY.md`
- **CI:** `.github/workflows/ci.yml` に `dotnet format` + 警告をエラーにしたビルドの Lint、`dotnet publish` + MSI の Build、集約 job → `docs/dev-charter/topics/CI_POLICY.md`・[CSHARP_DEV_ENV.md](https://github.com/y-marui/dev-charter/blob/develop/src/topics/csharp/CSHARP_DEV_ENV.md)（憲章の `main` 反映後に、`docs/dev-charter/topics/csharp/` のローカルパスへ置き換える）
- **コーディング原則:** YAGNI・変更最小限・DRY3回ルール → `docs/dev-charter/PRINCIPLES.md`

## dev-charter Modification Rules

`docs/dev-charter/` 配下のファイルは**直接編集しない**。

- 変更が必要な場合は dev-charter リポジトリ本体に Issue を立て、`git subtree pull` でアップデートを取り込む
- `git subtree pull` によるアップデートのみ許可する
- プロジェクト固有のルールは `AI_CONTEXT.md` または専用ファイルに記載する

## Document Sync Rule

仕様・ルール・構成に変更が生じたとき、変更と同じ作業内で関連ドキュメントを更新する。
対象は docs/ 内のファイルに限らず、AI_CONTEXT.md・README.md・README-jp.md 等のルートファイルも含む。

## Project-Specific Rules

- `WindowsUpdateController.cs` はレジストリ書き込みと Windows サービス操作を行うため、変更時は管理者権限要件を考慮する
- 配布は MSI のみ（perMachine）。exe を直接配る経路や、アプリ自身がデスクトップのショートカットを作る経路は持たない。`installer/Product.wxs` の `UpgradeCode` と `MainExecutable` の Component の Guid は変更しない

## AI Tool Assignments

- **使用ツール**：Claude Code、GitHub Copilot
- **標準担当の正本**：`docs/dev-charter/AI_COLLABORATION_RULES.md` の「AI Tool Responsibilities」と「Rules for Multi-AI Usage」
- **プロジェクト固有の上書き**：なし

## Prohibited Actions

- シークレット・認証情報のコミット
- Windows 以外のプラットフォーム向けコードの追加
- ソースコード以外のビルド成果物のコミット
