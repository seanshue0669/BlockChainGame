# 區塊鏈作業

## Git 分支與推送規則

### Remote 倉庫規則（origin）
#### `origin/develop` 分支
- 禁止任何人直接 push 至 `develop`！
- 所有變更必須透過 Pull Request（PR）進行。
- Pull Request 必須經過@seanshue0669審核通過後才能合併😋。

#### `origin/master` 分支
- 嚴禁從 feature、fixbug 或其他任意分支直接合併進 `master`。
- 任何 push 至 `master` 一律禁止。
- **僅由 `@seanshue0669` 負責審查與定期手動合併 `develop` ➜ `master`**，並視情況打 release tag😋。


###  Local 分支與開發規則
#### `develop` 分支（本地）

- 禁止在本地的 `develop` 分支上直接開發、commit、push
- `develop` 僅作為整合用途，請從此分支拉出個人開發分支。
- 若 Pull Request 來源分支的 commit 紀錄顯示來自 `develop`，將一律退回，請重新拉出 feature 分支進行修改。

#### `feature/xxx` 分支（本地功能開發）
- 從 `develop` 分支拉出，進行各自功能的開發。

#### Commit 規則（前綴格式與內容）

- 每次 commit 必須加上 **前綴類型標籤**，說明此筆變更的目的。
- commit message 打點有意義的東西就好
- 格式如下(Version 1)：
| 前綴      | 用途                       |
|-----------|----------------------------|
| `feat:`   | 新功能（新增 feature）     |
| `fixbug:` | 修復 bug                    |
| `refactor:`| 重構程式，不影響功能邏輯   |
| `docs:`   | 文件變更（如 README）      |
| `test:`   | 測試相關（單元測試等）      |
| `style:`  | 格式調整（如排版、命名）    |
| `up:`     | 升級依賴或套件版本         |
