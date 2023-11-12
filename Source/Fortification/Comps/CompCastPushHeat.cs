using Verse;

namespace Fortification
{
    public class CompCastPushHeat : ThingComp
    {
        public CompProperties_CastPushHeat Props => (CompProperties_CastPushHeat)this.props;
        public virtual float EnergyPerCast => Props.energyPerCast;
    }
    public class CompProperties_CastPushHeat : CompProperties
    {
        public CompProperties_CastPushHeat()
        {
            this.compClass = typeof(CompCastPushHeat);
        }
        public float energyPerCast;
    }
}
