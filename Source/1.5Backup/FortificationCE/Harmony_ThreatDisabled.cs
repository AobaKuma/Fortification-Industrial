using HarmonyLib;
using RimWorld;

namespace Fortification
{
    [HarmonyPatch(typeof(Building_Turret))]
    internal static class Harmony_ThreatDisabled
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Building_Turret), "ThreatDisabled")]
        public static void PostBuilding_Turret_ThreatDisabled(ref bool __result, Building_Turret __instance)
        {
            if ((__instance as Building_TurretCapacityCE)?.CanEnter ?? false)
            {
                __result = true;
            }
        }
    }
}