using RimWorld;
using System;
using Verse;

namespace Fortification
{
    public class CompCastEffecter : ThingComp
    {
        public CompProperties_CastEffecter Props => (CompProperties_CastEffecter)this.props;
        public void DoBursting(float v, Thing caster)
        {
            Props.effecter.SpawnAttached(caster, caster.Map, 1);
        }
    }
    public class CompProperties_CastEffecter : CompProperties
    {
        public CompProperties_CastEffecter()
        {
            compClass = typeof(CompCastEffecter);
        }
        public EffecterDef effecter;
    }
    public class SubEffecter_Dust : SubEffecter_Sprayer
    {
        
        public SubEffecter_Dust(SubEffecterDef def, Effecter parent) : base(def, parent)
        {
        }
        public override void SubEffectTick(TargetInfo A, TargetInfo B)
        {
            base.SubEffectTick(A, B);
        }
        public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
        {
            base.SubTrigger(A, B, overrideSpawnTick, force);
        }
    }
}