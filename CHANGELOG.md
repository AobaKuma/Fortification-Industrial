# Changelog

## Unreleased (since release-20260223)
> All additions and changes since release-20260223

---

### Maintenance

- **1.3-1.5 builds frozen**: the RimWorld 1.3, 1.4 and 1.5 folders are kept as-is and no longer updated. All new work targets 1.6; `About.xml` now says so.
- **Removed duplicate `FT_EnterBunkerFacility` JobDef**: Fortified Feature Framework already provides `FFF_EnterBunkerFacility` with the same job driver.
- **Load order**: `DwS.FunctionalAmmunition.Library` added to `loadAfter`, since the CE howitzer and 37mm shells already use its programmable fuzes via `MayRequire`.
- **Recipe inheritance switched to the framework**: the lathe now uses `Fortified.ModExtension_RecipeInheritance` instead of the VEF extension.
- Removed the `Source/1.5Backup` project and its reference DLLs from the repository.
- Removed the obsolete VFE Security `CustomVerb.xml` patch.
- **Vanilla and CE turret defs share their common parts**: nodes identical in both versions now live in `1.6/Defs/FT_Security_*_Common.xml`, so they are changed in one place. Checked with the new `_Tools/DefDiff` script to resolve to exactly the same defs as before.

---

### Turret roles (vanilla unless noted)

- **Field howitzer vs. field cannon now match their descriptions**: the field howitzer is the indirect gun (minimum range 4.9 to 19.9); the field cannon is a direct-fire AP cannon (range 500 to 59.9, needs line of sight, no mortar scatter, fires a 250-damage armor-piercing shell with a small blast instead of mortar shells). The cannon no longer stores shells, is no longer used as a siege mortar and loses VFE Security world artillery. With Nuclear Dawn, nuclear shells are noted on the field howitzer. CE is unchanged.
- **Auto-turret nests merged from four into two**: the *sandbag nest* (fabric or leather, 350 HP, quick and cheap, tinted by its material) and the *bunker* (stone or metal, 500 HP, masonry or armored texture depending on the material). The old four remain as "(legacy)" so existing buildings survive, but can no longer be built. Both modes.
- **Suppression**: machine-gun hits and heavy-shell blasts now inflict shell shock on every pawn they hurt, friend or foe; mechanoids are immune. Vanilla: auto-turret and quad machine guns, field cannon shell. CE: FI's 105mm, 155mm, 15cm Nebelwerfer and 128mm HE shells. An XML Extensions option (off by default) extends it to vanilla HE mortar shells.
- **Build times**: recoilless gun 3300 to 1500 (both modes), anti-tank gun 1000 to 3000.
- **Quad machine gun toned down**: it keeps its 20-round, 1800 rpm bursts but waits 4.2 s instead of 1 s between them, cutting single-target damage from about 113 to about 45 per second. It stays the suppression gun rather than the highest damage turret on the map.

---

### Framework features

- **Shell types (vanilla)**: the anti-tank gun and recoilless gun switch between armor-piercing and high-explosive, the anti-air gun between AP tracer and airburst (the quad AA gun defaults to airburst), and the field cannon between armor-piercing, heavy high-explosive and smoke. Switching uses the turret's ammo gizmo, is instant apart from a short reload pause, uses no ammo items and does not change barrel wear. The field cannon's AP shell blast is reduced (75 dmg / radius 4.4 to 50 / 2.4) so the new HE shell has a role. CE keeps its real ammo.
- **Crewed machine gun bunker**: a colonist climbs in and works the gun from inside, protected by the structure. No power, no breakdowns and one component, but it does nothing without a crew. The automatic bunker stays as the hands-off option. Both modes.
- **Packable light guns (vanilla)**: the quad machine gun and recoilless gun can be picked up into a colonist's inventory and redeployed, like the infantry mortar. In CE they are too heavy (80 / 210 kg) and stay fixed.
- **Info cards**: new "Special mechanics" entries explain barrel wear, hand-loaded shells, shell types, suppression, concrete grades and sandbag cover.
- **Sandbag cover**: sandbags and sandbag walls take half damage from bullets and a quarter from arrows; explosives are unchanged.

---

### Changes & Fixes

#### Turrets (vanilla)

- **Shielded and cement-base cannons gain damage resistances**: anti-air, quad anti-air and anti-tank guns take reduced damage from bullets and explosives and ignore burn, frostbite and similar damage. CE-only damage types were removed from the vanilla list.
- **Anti-air guns rebalanced**: shorter warmup and cooldown, smaller bursts (8 to 4 shots, quad 16 to 8), higher medium-range and lower long-range accuracy.
- **Anti-tank gun**: warmup raised from 1 to 2 seconds.
- **Quad machine gun**: 0.5 s burst warmup added.
- **Infantry mortar**: aim time cut from 6 to 2 seconds, cooldown raised from 1 to 2 seconds. It can no longer load nuclear or antigrain shells.
- **Heavy artillery**: multi-barrel mortar warmup raised from 2 to 3 seconds and field howitzer from 3 to 4 seconds. The multi-barrel mortar now shows its 30-second cooldown on the info card.
- **Base cannon emplacements can be manned** (PR #16, [@Rosnok](https://github.com/Rosnok)).
- Fixed the field howitzer's turret top being drawn off-center.
- Displayed stats now match the values the turrets actually use.

#### Combat Extended

- **Medium turrets gain matching damage resistances**, including entries for Odyssey, Milira and Monolyn damage types. The turret heads are no longer drawn off-center.
- **155mm howitzer**: new antigrain shell. The shells no longer drop casings and fly slightly faster.
- **Heavy artillery retuned**: multi-barrel mortar range is now 29.9 to 1000 with a longer warmup; heavy gun cooldowns shortened.
- Multi-barrel mortar no longer uses CE propellant charges.
- **15cm Nebelwerfer**: spread changed per warhead (HE no extra spread, EMP 1.25, toxic 1.5).

#### Language & Text

- **Chinese translations complete for 1.6**: every 1.6 def string now has Simplified and Traditional Chinese text, including the 128mm heavy flak / heavy anti-tank guns and their shells, airburst / antigrain / nuclear howitzer shells, the remaining Nebelwerfer rockets and the VFE Architect concrete floors.
- Fixed the 37mm AP-HE recipe translation key and the Simplified Chinese hydrant use label.
- Spelling and wording fixes in research, artillery and turret descriptions.

---

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
