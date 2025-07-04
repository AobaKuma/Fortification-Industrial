using HarmonyLib;
using RimWorld;
using Verse;

namespace FortificationMedieval
{
    [HarmonyPatch(typeof(Verb_Shoot), "TryCastShot", MethodType.Normal)]
    internal static class Harmony_Verb_Shoot
    {
        public static void Postfix(ref bool __result, Verb_Shoot __instance)
        {
            if (__result)//發射會成功的狀況
            {
                if (__instance.EquipmentSource?.def.GetModExtension<ModExtension_ShootWithFleck>() !=null)            //發射特效
                {
                    var comp = __instance.EquipmentSource.def.GetModExtension<ModExtension_ShootWithFleck>();

                    if (!comp.SpawnCheck(__instance.Caster)) return;
                        comp.DoBursting(Vector3Utility.AngleToFlat(__instance.Caster.DrawPos, __instance.CurrentTarget.Cell.ToVector3Shifted()));
                }
                }
            }
        }
    }
