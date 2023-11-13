using CombatExtended;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Fortification
{
    [HarmonyPatch(typeof(Verb_ShootCE), "TryCastShot", MethodType.Normal)]
    internal static class Harmony_Verb_Shoot
    {
        public static void Postfix(ref bool __result, Verb_ShootCE __instance)
        {
            //多重射擊
            CompChangeableProjectile compChangeableProjectile = __instance.EquipmentSource.GetComp<CompChangeableProjectile>();
            if (compChangeableProjectile != null)
            {
                ThingDef thingDef = null;
                if (__instance.Bursting)
                {
                    if (compChangeableProjectile.LoadedShell != null)
                    {
                        compChangeableProjectile.LoadShell(thingDef, 1);
                    }
                }
            }
            if (__result)//發射會成功的狀況
            {
                if (__instance.Caster is Building_TurretCapacityCE turretCapacity)//給碉堡裡的操作者增加射擊經驗與紀錄
                {
                    Pawn castPawn = turretCapacity.PawnInside;
                    Pawn pawn = __instance.CurrentTarget.Thing as Pawn;
                    if (pawn != null && !pawn.Downed && !pawn.IsColonyMech && __instance.CasterIsPawn && __instance.CasterPawn.skills != null)
                    {
                        float num = (pawn.HostileTo(castPawn) ? 170f : 20f);
                        float num2 = __instance.verbProps.AdjustedFullCycleTime(__instance, __instance.CasterPawn);
                        castPawn.skills.Learn(SkillDefOf.Shooting, num * num2);
                    }
                    castPawn.records.Increment(RecordDefOf.ShotsFired);
                }
                //Log.Message((Caster.DrawPos, CurrentTarget.Cell.ToVector3Shifted()).ToString());
                if (__instance.EquipmentSource.GetComp<CompCastPushHeat>() is CompCastPushHeat compCastPushHeat)            //射擊加溫
                {
                    if (compCastPushHeat.EnergyPerCast != 0)
                    {
                        GenTemperature.PushHeat(__instance.Caster.Position, __instance.Caster.Map, compCastPushHeat.EnergyPerCast);
                    }
                }
                //if (__instance.EquipmentSource.GetComps<CompCastEffecter>().EnumerableCount() != 0)
                //{
                //    foreach (var comp in __instance.EquipmentSource.GetComps<CompCastEffecter>())
                //    {
                //        comp.DoBursting(Vector3Utility.AngleToFlat(__instance.Caster.DrawPos, __instance.CurrentTarget.Cell.ToVector3Shifted()), __instance.Caster);
                //    }
                //}
                if (__instance.EquipmentSource.GetComps<CompCastFlecker>().EnumerableCount() != 0)            //發射特效
                {
                    foreach (var comp in __instance.EquipmentSource.GetComps<CompCastFlecker>())
                    {
                        if (!comp.SpawnCheck(__instance.Caster)) break;
                        comp.DoBursting(Vector3Utility.AngleToFlat(__instance.Caster.DrawPos, __instance.CurrentTarget.Cell.ToVector3Shifted()));
                    }
                }
            }
        }
    }
}