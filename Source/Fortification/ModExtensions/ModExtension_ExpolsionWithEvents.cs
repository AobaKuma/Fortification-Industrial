using System.Collections.Generic;
using Verse;

namespace Fortification
{
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
