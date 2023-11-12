
using CombatExtended;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace Fortification
{
    [HarmonyPatch(typeof(Building_TurretGunCE), "CanSetForcedTarget", (MethodType)1)]
    internal static class Harmony_TurretGunCE
    {
        public static void Postfix(ref bool __result, Building_TurretGunCE __instance)
        {
            if (ThingCompUtility.TryGetComp<CompForceTargetable>(__instance) != null)
            {
                if (__instance is Building_TurretCapacityCE building_TurretCapacity && building_TurretCapacity.PawnInside != null)
                {
    
                    __result = true;
                }
            }
            else if (!__instance.IsMannable) { __result = true; }
        }
    }
}