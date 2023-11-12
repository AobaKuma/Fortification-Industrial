using System.Collections.Generic;
using Verse;

namespace Fortification
{
    public class CompExpolsionWithEvents : ThingComp
    {
        public CompProperties_ExpolsionWithEvents Props => (CompProperties_ExpolsionWithEvents)this.props;
    }
    public class CompProperties_ExpolsionWithEvents : CompProperties
    {
        public CompProperties_ExpolsionWithEvents()
        {
            this.compClass = typeof(CompExpolsionWithEvents);
        }
        public List<Condition> conditions;
    }

    public class ModExtension_ExpolsionWithEvents : DefModExtension
    {
        public List<Condition> conditions;
    }

    public class Condition
    {
        public GameConditionDef conditionDef;
        public int percent;
        public IntRange duration;
    }
}
