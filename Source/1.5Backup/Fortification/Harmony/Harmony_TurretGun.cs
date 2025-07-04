using HarmonyLib;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.AI;

namespace Fortification
{
    [HarmonyPatch(typeof(Building_TurretGun), "CanSetForcedTarget", (MethodType)1)]
    internal static class Harmony_TurretGun
    {
        public static void Postfix(ref bool __result, Building_TurretGun __instance)
        {
            if (__instance.def.HasModExtension<ModExtension_ForceTargetable>())
            {
                if (__instance is Building_TurretCapacity building_TurretCapacity && building_TurretCapacity.PawnInside != null)
                {
                    __result = true;
                }
                if (!__instance.IsMannable) { __result = true; }
            }
        }
    }
}