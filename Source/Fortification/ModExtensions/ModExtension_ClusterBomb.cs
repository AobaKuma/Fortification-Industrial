using RimWorld;
using Verse;

namespace Fortification
{
    public class CompClusterBomb : ThingComp
    {
        public CompProperties_ClusterBomb Props => (CompProperties_ClusterBomb)props;
    }
    public class CompProperties_ClusterBomb : CompProperties
    {
        public int clusterBurstCount = 1;

        public ThingDef projectile = null;

        public float forceMissingRange = 0f;

        public bool doDismantlExplosion = false;

        public DamageDef dismantlExplosionDam;

        public float dismantlExplosionRadius = 1f;

        public int dismantlExplosionDamAmount = -1;

        public float dismantlExplosionArmorPenetration = -1f;

        public SoundDef dismantlExplosionSound = null;

        public float safetyDistance = 5f;

        public IntRange TDDistance = new IntRange(0, 0);

        public CompProperties_ClusterBomb()
        {
            compClass = typeof(CompClusterBomb);
        }

        public override void ResolveReferences(ThingDef parentDef)
        {
            base.ResolveReferences(parentDef);
            if (dismantlExplosionDam == null)
            {
                dismantlExplosionDam = DamageDefOf.Bomb;
            }
        }
    }

    public class ModExtension_ClusterBomb : DefModExtension
    {
        public int clusterBurstCount = 1;

        public ThingDef projectile = null;

        public float forceMissingRange = 0f;

        public bool doDismantlExplosion = false;

        public DamageDef dismantlExplosionDam;

        public float dismantlExplosionRadius = 1f;

        public int dismantlExplosionDamAmount = -1;

        public float dismantlExplosionArmorPenetration = -1f;

        public SoundDef dismantlExplosionSound = null;

        public float safetyDistance = 5f;

        public IntRange TDDistance = new IntRange(0, 0);
    }
}
