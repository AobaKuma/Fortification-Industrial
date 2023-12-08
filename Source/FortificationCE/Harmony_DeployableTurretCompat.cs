using CombatExtended;
using Fortification;
using HarmonyLib;
using RimWorld;
using Verse;
using static UnityEngine.GraphicsBuffer;

namespace Fortification
{

    [HarmonyPatch(typeof(MinifyUtility), "MakeMinified")]
    internal static class Harmony_MinifyUtility
    {
        [HarmonyPostfix]
        public static void PostFix(MinifiedThing __result)
        {
            CompAmmoUser compAmmo = __result.TryGetComp<CompAmmoUser>();

            if (__result.InnerThing is Building_TurretGunCE turretCE && compAmmo != null)
            {
                CompAmmoUser turretAmmo = turretCE.CompAmmo;
                Harmony_DeployableTurretCompat.SwapAmmo(turretAmmo, compAmmo);
            }
        }
    }


    [HarmonyPatch(typeof(MinifiedThingDeployable))]
    internal static class Harmony_DeployableTurretCompat
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MinifiedThingDeployable), "DeployCECompatHook")]
        public static void PostFix(MinifiedThingDeployable minified, Thing turret)
        {
            if (turret == null || minified == null || minified == turret) return;

            CompAmmoUser compAmmo = minified.TryGetComp<CompAmmoUser>();

            if (turret is Building_TurretGunCE turretCE && compAmmo != null)
            {
                CompAmmoUser turretAmmo = turretCE.CompAmmo;
                SwapAmmo(compAmmo, turretAmmo);
            }
        }

        public static void SwapAmmo(CompAmmoUser from, CompAmmoUser to)
        {
            if (from != null && to != null && from.HasMagazine && to.HasMagazine && (from.Props.ammoSet == to.Props.ammoSet))
            {
                if (from.MagSize != to.MagSize)
                {
                    Log.Warning(from.parent.def.defName + "'s mag size doesn't match " + to.parent.def.defName + "'s mag size. This shouldn't happen");
                }

                to.CurrentAmmo = from.CurrentAmmo;
                to.CurMagCount += from.CurMagCount;
                from.CurMagCount = 0;
            }
        }
    }
}