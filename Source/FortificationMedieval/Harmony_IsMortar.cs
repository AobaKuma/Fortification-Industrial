using HarmonyLib;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.AI;

namespace FortificationMedieval
{
    [HarmonyPatch(typeof(Building_TurretGun), "IsMortar", (MethodType)1)]
    internal static class Harmony_IsMortar
    {
        public static void Postfix(ref bool __result, Building_TurretGun __instance)
        {
            if (!__result && __instance.def.defName == "FT_Gun_SwivelGun") __result = true;

        }
    }
}