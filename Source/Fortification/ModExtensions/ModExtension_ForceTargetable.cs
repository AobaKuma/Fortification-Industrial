using Verse;

namespace Fortification
{
    public class CompForceTargetable : ThingComp
    {

    }
    public class CompProperties_ForceTargetable : CompProperties
    {
        public CompProperties_ForceTargetable()
        {
            compClass = typeof(CompForceTargetable);
        }
    }

    public class ModExtension_ForceTargetable : DefModExtension
    {

    }
}
