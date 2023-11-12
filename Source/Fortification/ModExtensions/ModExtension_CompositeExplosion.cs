using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace Fortification
{
    public class CompCompositeExplosion : ThingComp
    {
        public CompProperties_CompositeExplosion Props => (CompProperties_CompositeExplosion)this.props;
        public virtual List<CompositeExplosion> CompositeExplosions => Props.compositeExplosions;
    }

    public class CompProperties_CompositeExplosion : CompProperties
    {
        public CompProperties_CompositeExplosion()
        {
            compClass = typeof(CompCompositeExplosion);
        }
        public List<CompositeExplosion> compositeExplosions = new List<CompositeExplosion>();
    }

    public class ModExtension_CompositeExplosion : DefModExtension
    {
        public List<CompositeExplosion> compositeExplosions = new List<CompositeExplosion>();
    }

    public class CompositeExplosion
    {
        public int countdown;
        public float radius;
        public DamageDef damamgeDef;
        public int amount;
        public float? armorPenetration = null;
        public SoundDef explosionSound = null;

        public ThingDef preExplosionSpawnThingDef = null;
        public float preExplosionSpawnChance = 0;
        public int preExplosionSpawnThingCount = 0;

        public ThingDef postExplosionSpawnThingDef = null;
        public ThingDef postExplosionSpawnThingDefWater = null;
        public GasType? postExplosionGasType = null;
        public float postExplosionSpawnChance = 0;
        public int postExplosionSpawnThingCount = 0;
        public float chanceToStartFire = 0;
    }

}
