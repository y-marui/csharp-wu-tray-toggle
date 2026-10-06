# Docs Maintenance

ローカル LLM で `docs/` ファイルをメンテナンスするためのプロンプト集。

---

## Updating docs/architecture.md

```
src/ 配下のファイルを確認し、docs/architecture.md を最新の状態に更新してください。

手順：
1. src/ 配下の全 .cs ファイルを読む
2. docs/architecture.md を読む
3. 以下のフォーマットで上書き保存する：
   - ## Overview（3行以内）
   - ## Entry Points（エントリーポイントのみ）
   - ## Directory Structure（ディレクトリ一覧）
   - ## Key Dependencies（主要依存のみ・全列挙しない）

注意：ファイルレベルの詳細は記載しない（file-map.md に委譲する）。
```

---

## Updating docs/file-map.md

```
作業対象ファイルの依存関係を調査し、docs/file-map.md に追記・更新してください。

手順：
1. 作業対象のファイルを読む
2. docs/file-map.md を読む
3. 変更・参照したファイルのエントリを追加または更新し、最終更新日を今日の日付にして上書き保存する

フォーマット：
| ファイル | 役割 | 主な依存先 |
|---|---|---|
| `src/WuTrayToggle/Foo.cs` | 説明 | `System.Windows.Forms` |

注意：全ファイルを網羅しなくてよい。未探索ファイルは記載しない。
```
