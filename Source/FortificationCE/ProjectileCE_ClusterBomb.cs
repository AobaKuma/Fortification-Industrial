using CombatExtended;
using CombatExtended.Utilities;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Fortification
{
    public class ProjectileCE_ClusterBomb : ProjectileCE
    {
        public override void Tick()
        {
            base.Tick();
            var Props = GetComp<CompClusterBomb>().Props;
            //如果目前飛行距離與目標距離小於一個特定距離
            IntRange tDDistance = Props.TDDistance;
            float a = Vector3.Distance(intendedTarget.CenterVector3, ExactPosition);
            float b = Vector3.Distance(origin, ExactPosition);
            if (a < tDDistance.min && b > Props.safetyDistance)
            {
                if (this.GetComp<CompFragments>() != null)
                {
                    GetComp<CompFragments>().Throw(ExactPosition, base.Map, launcher);
                }
                if (Props.doDismantlExplosion)
                {
                    GenExplosion.DoExplosion(ExactPosition.ToIntVec3(), Map, Props.dismantlExplosionRadius, Props.dismantlExplosionDam, (Thing)this, Props.dismantlExplosionDamAmount, Props.dismantlExplosionArmorPenetration, Props.dismantlExplosionSound, (ThingDef)null, (ThingDef)null, (Thing)null, (ThingDef)null, 1f, 1, null, false, (ThingDef)null, 0f, 1, 0f, false, (float?)null, (List<Thing>)null);
                }
                Dismantling();
            }
        }
        private void GoodwillCorrection(List<IntVec3> cells)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                List<Thing> thingList = cells[i].GetThingList(base.Map);
                foreach (Thing item in thingList)
                {
                    if (item is Pawn pawn && !pawn.Dead && pawn.Faction != null && !pawn.Faction.IsPlayer && !pawn.Faction.HostileTo(launcher.Faction))
                    {
                        FactionRelation factionRelation = pawn.Faction.RelationWith(launcher.Faction);
                        pawn.Faction.TryAffectGoodwillWith(launcher.Faction, -10 - factionRelation.baseGoodwill + pawn.Faction.GoodwillWith(launcher.Faction));
                    }
                }
            }
        }

        private void Dismantling()
        {
            var Props = GetComp<CompClusterBomb>().Props;
            //Vector3 vector = ExactPosition - new Vector3(origin.x, 0f, origin.y);
            //IntVec3 intVec = (ExactPosition + vector.normalized * 5).ToIntVec3();
            float baseOffsetAngle = Props.forceMissingRange;
            ThingDef projectile = Props.projectile;
            for (int i = 0; i < Props.clusterBurstCount; i++)
            {
                ProjectileCE projectile2 = (ProjectileCE)GenSpawn.Spawn(projectile, ExactPosition.ToIntVec3(), Map);
                float errorRange = Props.forceMissingRange;
                List<IntVec3> list = GenRadial.RadialCellsAround(destinationInt.ToIntVec3(), errorRange, useCenter: true).ToList();
                IntVec3 intVec2 = list.RandomElement();
                this.TryGetComp<CompFragments>()?.Throw(intVec2.ToVector3(), Map, this);
                projectile2.Launch(launcher, new Vector2(ExactPosition.x, ExactPosition.z), shotAngle + Rand.Range(-baseOffsetAngle, baseOffsetAngle), shotAngle + Rand.Range(-baseOffsetAngle, baseOffsetAngle), Height, shotSpeed, ThingMaker.MakeThing(equipmentDef));
                GoodwillCorrection(list);
            }
            Destroy();
        }
    }
}
