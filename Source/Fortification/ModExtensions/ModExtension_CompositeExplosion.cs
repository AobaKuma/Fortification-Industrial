using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace Fortification
{
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
