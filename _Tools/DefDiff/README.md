# DefDiff

Checks that a refactor of the 1.6 defs does not change what the game ends up with.

`defresolve.py` loads the folders RimWorld loads for one mode (`1.6/Defs` plus
`1.6/Vanilla/Defs` or `1.6/CombatExtended/Defs`), resolves FI's own `ParentName`
inheritance the way RimWorld does, and writes every non-abstract def in a canonical form.
External parents (`BuildingBase`, `BaseWeaponTurret`, ...) are recorded, not resolved.
Patches are not applied.

Order is ignored where it has no gameplay effect: field containers (`statBases`,
`building`, ...), `damageMultipliers`, and purely visual comps (`CompProperties_CastFlecker`,
`CompProperties_Flecker`, `MuzzleFlash.MuzzleFlashProps`). Every other `li` list keeps
its order.

```
git worktree add /tmp/fi-before HEAD
python3 _Tools/DefDiff/defresolve.py /tmp/fi-before vanilla before.json
python3 _Tools/DefDiff/defresolve.py . vanilla after.json
python3 _Tools/DefDiff/defdiff.py before.json after.json    # expect "DIFF COUNT 0"
```

Run it for both `vanilla` and `ce`. Requires Python 3.9+.
