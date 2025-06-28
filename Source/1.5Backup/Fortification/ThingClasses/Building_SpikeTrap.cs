using RimWorld;
using Verse;

namespace Fortification
{
    public class Building_SpikeTrap : Building_TrapDamager
    {
        protected override void SpringSub(Pawn p)
        {
            base.SpringSub(p);
            base.HitPoints -= 50;
            if (base.HitPoints <= 0) base.Destroy(DestroyMode.KillFinalize);
        }
    }
}
