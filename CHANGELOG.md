# Changelog

## release-20260223 (2026-02-23)
> 對比 release-20251123 的所有新增與修改

---

### 新增 (New Additions)

- **新增 VFE Security 補丁 `CustomVerb.xml`**：為 Vanilla Furniture Expanded: Security 的炮塔新增自訂射擊行為補丁。(PR #13, [@Rosnok](https://github.com/Rosnok))
- **新增 Nuclear Dawn 整合補丁 `FortificationIndustrialNuclearDawn.xml`**：當偵測到 *Fortification Industrial - Nuclear Dawn* 模組時，重型榴彈砲說明將更新，提示可使用核彈砲彈。
- **新增 VFE Classical 整合補丁 `VanillaFactionsExpandedClassical.xml`**：將 *Vanilla Factions Expanded - Classical* 的混凝土方塊重命名為「羅馬混凝土」(Roman Concrete)，以避免與本模組混凝土混淆。
- **新增 Vanilla Quests Expanded - Ancients 整合補丁 `VanillaQuestsExpandedAncients.xml`**：ENIAC 現在可以連接 *Vanilla Quests Expanded - Ancients* 的 Archogen Injector，並獲得數值加成；說明文字更新以反映此功能。
- **新增砲彈爆炸焦痕特效**：各砲彈（105mm、155mm 榴彈等）命中時現在會在地面留下爆炸焦痕 (blast marks)。

---

### 修改 (Changes & Fixes)

#### 砲兵系統 (Artillery)

- **15cm Nebelwerfer 多項調整**：
  - 砲彈設定 `flyOverhead = true`，使其成為拋物線彈道武器，不再被山脈阻擋。
  - 降低砲彈飛行速度並增加重力係數，防止砲彈飛出大氣層。
  - 降低射速。
  - 新增裝填音效 (soundInteract)。
  - 修正後，Nebelwerfer 現在被正式定位為真正的砲兵武器。
- **105mm 榴彈砲與 155mm 重型榴彈砲重新平衡**：數值調整，說明文字修正以更準確反映實際功能。
- **重型砲塔放寬射擊弧度 (fire arcs)**：調整射擊範圍更為寬鬆。
- **37×223mmR 榴霰彈**：改用 D-lib (Dlib) 系統整合。

#### 材料與製作 (Recipes & Crafting)

- **`FT_Make_ConcreteWood` 配方修正**：說明中標示需要布料，現在配方實際確實消耗 5 布料。
- **強化槍管 (Reinforced Barrel) 製作費用上調**：因其為高價零件，製作成本略微提高。
- **新增 x4 批量組件製作配方**：可以一次批量製作四份組件。
- **多項配方標籤 (label)、說明 (description)、工作字串 (jobString) 文字修正**。

#### 材質 (Textures)

- **砲塔底座貼圖補充黑色輪廓**：`TurretArtillery_Base_AA`（防空型）及 `TurretArtillery_Base`（標準型）所有方向貼圖補上 4px 黑色輪廓，符合 RimWorld 風格。
- **重型砲兵底座 `TurretArtillery_Base_Heavy` 貼圖更新**：同樣加入 4px 黑色輪廓。

#### VFE Security 兼容性更新 (PR #13)

- 更新 `FT_VFESecurity.xml`：重寫 VFE Security 炮塔的相容性定義，與 VFE Security 最新版本保持同步。
- 移除舊版 `VanillaFurnitureExpandedSecurity.xml` 補丁檔（已由新補丁取代）。
- 微調自動砲塔 (`FT_Security_AutoTurrets.xml`) 相關設定。
- 修正重型安全設施 (`FT_Security_Heavy.xml`) 及輕型安全設施 (`FT_Security_Light.xml`) 的 CE 兼容數值。
- 更新 `LoadFolders.xml` 以反映檔案結構變更。

#### 語言 / 文字 (Language & Text)

- **大量文字改進**：對多個 Def 的名稱、說明及工作提示字串進行了全面修正，改善排版、標點符號與措詞準確性。
- **修正無線電終端 (Radio Terminal) 說明文字錯誤**。
- 移除 `DefInjected` 語言資料夾中的冗余英文翻譯條目（已被 `Keyed` 系統取代）。
- 更新 `AOBAUtilities.xml` 鍵值翻譯，部分詞首大小寫統一。
- 修正 `FT_Misc.xml` 中的雜項文字。

#### 其他 (Miscellaneous)

- 程式碼清理：移除 `FT_Security_Heavy_CE.xml` 中不必要的重複定義。
- 清理多餘空白字元。

---

*完整 commit 紀錄請參閱 [release-20251123...release-20260223](https://github.com/AobaKuma/Fortification-Industrial/compare/release-20251123...release-20260223)*
