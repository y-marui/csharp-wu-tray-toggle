# UI Design

## Components

### System Tray Icon

- `src/WuTrayToggle/Assets/` の静的 `.ico`（盾モチーフ）を埋め込みリソースとして持ち、`IconFactory.Create` が状態に応じて読み込む
- 状態に応じてアイコンを切り替え（詳細は `docs/specification.md` 参照）
- ダーク/ライトモード切り替え: 未対応

### Context Menu

右クリックで表示するメニュー（`src/WuTrayToggle/TrayApplicationContext.cs`）：

| メニュー項目 | 動作 |
|---|---|
| 現在の状態を確認 | アプリバージョン(`Application.ProductVersion`)・ポリシー・サービス状態をメッセージボックスで表示 |
| 停止 (制御開始) | 管理者昇格して WU 停止 |
| 再開 (通常) | 管理者昇格して WU 再開 |
| 設定… | 設定ウィンドウ（下記）をモーダルで開く。開いている間に再度押しても、2 つ目は開かない |
| 終了 | トレイアイコンを終了 |

メニューは開く直前（`ContextMenuStrip.Opening`）に全項目のテキストを再設定する。設定ウィンドウを閉じた直後にも、メニューとツールチップを再設定するため、言語切り替えがすぐに反映される。

### Settings Window

`src/WuTrayToggle/SettingsForm.cs`。独立したモーダルのウィンドウ（`FixedDialog`、画面中央、`AutoScaleMode.Dpi`）。

| 項目 | 内容 |
|---|---|
| 言語 | ドロップダウン。システム既定/日本語/English/中文/हिन्दी/Español/Français/Português から 1 つ選ぶ |
| ログイン時に自動起動 | チェックボックス。スタートアップフォルダのショートカットの有無を、開いた時点の状態として表示する |
| 保存 | 言語と自動起動を反映して閉じる。言語の保存に失敗した場合は、エラーを表示してウィンドウを閉じない |
| キャンセル | 何も変えずに閉じる（Esc でも同じ） |

## Notes

- UI テキストのローカライズ対応（メニュー・ツールチップ・メッセージボックス・バルーン通知）は `docs/specification.md` の「ローカライゼーション」参照
