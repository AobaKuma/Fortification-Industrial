using RimWorld;
using Verse;

namespace Fortification
{
    public class ModExtension_AlongWayDamage : DefModExtension
    {
        public DamageDef endExplosionDam = DamageDefOf.Bomb;

        public float endExplosionRadius = 1f;

        public int endExplosionDamAmount = -1;

        public float endExplosionArmorPenetration = -1f;

        public DamageDef alongOnWayDamage = DamageDefOf.Crush;

        public float alongOnWayDamAmount = 1f;

        public float alongOnWayDamArmorPenetration = 0f;

        public HediffDef alongOnWayHediff = null;

        public float alongOnWayHediffSeverity = 1f;
    }
}
