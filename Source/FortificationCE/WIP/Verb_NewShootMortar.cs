using RimWorld;
using Verse;
using CombatExtended;
using Verse.AI;

namespace Fortification
{
    //public class Verb_NewShootMortar : Verb_ShootMortarCE
    //{
    //    private bool IsBunker => Caster is Building_TurretCapacity && Caster as Building_TurretCapacity != null;
    //    public override bool TryCastShot()
    //    {
    //        bool result;

    //        //多重射擊
    //        CompChangeableProjectile compChangeableProjectile = EquipmentSource.GetComp<CompChangeableProjectile>();
    //        if (compChangeableProjectile != null)
    //        {
    //            ThingDef thingDef = null;
    //            if (burstShotsLeft > 1)
    //            {
    //                thingDef = compChangeableProjectile.LoadedShell;
    //            }
    //            result = base.TryCastShot();
    //            if (thingDef != null)
    //            {
    //                compChangeableProjectile.LoadShell(thingDef, 1);
    //            }
    //        }
    //        else
    //        {
    //            result = base.TryCastShot();
    //        }

    //        if (result)//發射會成功的狀況
    //        {
    //            if (IsBunker)//給碉堡裡的操作者增加射擊經驗與紀錄
    //            {
    //                Pawn castPawn = (Caster as Building_TurretCapacity).PawnInside;
    //                Pawn pawn = currentTarget.Thing as Pawn;
    //                if (pawn != null && !pawn.Downed && !pawn.IsColonyMech && CasterIsPawn && CasterPawn.skills != null)
    //                {
    //                    float num = (pawn.HostileTo(caster) ? 170f : 20f);
    //                    float num2 = verbProps.AdjustedFullCycleTime(this, CasterPawn);
    //                    castPawn.skills.Learn(SkillDefOf.Shooting, num * num2);
    //                }
    //                castPawn.records.Increment(RecordDefOf.ShotsFired);
    //            }
    //            //Log.Message((Caster.DrawPos, CurrentTarget.Cell.ToVector3Shifted()).ToString());
    //            if (EquipmentSource.GetComp<CompCastPushHeat>() is CompCastPushHeat compCastPushHeat)            //射擊加溫
    //            {
    //                if (compCastPushHeat.EnergyPerCast != 0)
    //                {
    //                    GenTemperature.PushHeat(Caster.Position, Caster.Map, compCastPushHeat.EnergyPerCast);
    //                }
    //            }
    //            if (EquipmentSource.GetComps<CompCastEffecter>().EnumerableCount() != 0)
    //            {
    //                foreach (var comp in EquipmentSource.GetComps<CompCastEffecter>())
    //                {
    //                    comp.DoBursting(Vector3Utility.AngleToFlat(Caster.DrawPos, CurrentTarget.Cell.ToVector3Shifted()), Caster);
    //                }
    //            }
    //            if (EquipmentSource.GetComps<CompCastFlecker>().EnumerableCount() != 0)            //發射特效
    //            {
    //                foreach (var comp in EquipmentSource.GetComps<CompCastFlecker>())
    //                {
    //                    comp.DoBursting(Vector3Utility.AngleToFlat(Caster.DrawPos, CurrentTarget.Cell.ToVector3Shifted()), Caster);
    //                }
    //            }

    //        }

    //        return result;
    //    }
    //}
}
