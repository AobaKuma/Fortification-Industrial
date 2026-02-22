# Changelog

## release-20260223 (2026-02-23)
> All additions and changes since release-20251123

---

### New Additions

- **Added VFE Security patch `CustomVerb.xml`**: New custom firing-verb patch for Vanilla Furniture Expanded: Security turrets. (PR #13, [@Rosnok](https://github.com/Rosnok))
- **Added Nuclear Dawn integration patch `FortificationIndustrialNuclearDawn.xml`**: When *Fortification Industrial - Nuclear Dawn* is detected, the heavy howitzer description is updated to mention that nuclear shells can be loaded.
- **Added VFE Classical integration patch `VanillaFactionsExpandedClassical.xml`**: Renames the concrete blocks from *Vanilla Factions Expanded - Classical* to "Roman Concrete" to reduce confusion with Fortification Industrial's own concrete.
- **Added Vanilla Quests Expanded - Ancients integration patch `VanillaQuestsExpandedAncients.xml`**: The ENIAC can now link up to the Archogen Injector from *Vanilla Quests Expanded - Ancients* for stat bonuses; description updated to reflect this.
- **Added blast marks on artillery shell impact**: Shells (105mm, 155mm howitzers, etc.) now leave scorch marks on the ground upon impact.

---

### Changes & Fixes

#### Artillery

- **15cm Nebelwerfer multiple tweaks**:
  - Set projectile `flyOverhead = true`, making it a parabolic-arc weapon that can no longer be blocked by mountains.
  - Reduced projectile speed and increased gravity factor to prevent shells from flying into the stratosphere.
  - Reduced fire rate.
  - Added a reloading sound (`soundInteract`).
  - The Nebelwerfer is now properly classified as a true artillery weapon.
- **105mm and 155mm howitzer rebalance**: Stats adjusted; descriptions corrected to more accurately reflect actual function.
- **Relaxed fire arcs on heavy turrets**: Firing angles widened.
- **37×223mmR shrapnel shell**: Migrated to D-lib integration.

#### Recipes & Crafting

- **`FT_Make_ConcreteWood` recipe fix**: The description mentioned requiring cloth; the recipe now actually consumes 5 cloth as implied.
- **Reinforced Barrel crafting cost increased**: Slightly more expensive to craft, reflecting its value as a high-tier component.
- **Added x4 bulk component crafting recipe**: Allows crafting four components at once in a single job.
- **Multiple recipe label, description, and jobString text corrections**.

#### Textures

- **Added missing outlines to turret bases**: All directional sprites for `TurretArtillery_Base_AA` (anti-air) and `TurretArtillery_Base` (standard) now have a 4px black outline matching RimWorld's art style.
- **Updated `TurretArtillery_Base_Heavy` textures**: Also given 4px black outlines.

#### VFE Security Compatibility Update (PR #13)

- Updated `FT_VFESecurity.xml`: Rewrote compatibility definitions for VFE Security turrets to stay in sync with the latest VFE Security version.
- Removed the obsolete `VanillaFurnitureExpandedSecurity.xml` patch file (superseded by the new patch).
- Minor tweaks to auto-turret settings in `FT_Security_AutoTurrets.xml`.
- Fixed CE compatibility values in `FT_Security_Heavy.xml` and `FT_Security_Light.xml`.
- Updated `LoadFolders.xml` to reflect file structure changes.

#### Language & Text

- **Broad text improvements**: Names, descriptions, and job strings across numerous Defs revised for better typography, punctuation, and accuracy.
- **Fixed Radio Terminal description error**.
- Removed redundant English translation entries from the `DefInjected` language folder (superseded by the `Keyed` system).
- Updated `AOBAUtilities.xml` keyed translations; standardised capitalisation of several terms.
- Miscellaneous text fixes in `FT_Misc.xml`.

#### Miscellaneous

- Code cleanup: removed unnecessary redundant definitions from `FT_Security_Heavy_CE.xml`.
- Cleaned up stray whitespace.

---

*Full commit history: [release-20251123...release-20260223](https://github.com/AobaKuma/Fortification-Industrial/compare/release-20251123...release-20260223)*
