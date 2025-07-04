using RimWorld;
using Verse;
using CombatExtended;

namespace Fortification
{
    //public class Verb_NewShoot : Verb_ShootCE
    //{
    //    public override bool TryCastShot()
    //    {
    //        bool result;

    //        多重射擊
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
    //        return result;
    //    }
    //}
}
