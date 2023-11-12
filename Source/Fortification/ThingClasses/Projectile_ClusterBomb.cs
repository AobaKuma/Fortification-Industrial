using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Fortification
{
    public class Projectile_ClusterBomb : Projectile
    {
        private int tick = 0;

        private int DismantlL = 0;

        public override void Tick()
        {
            if (landed)
            {
                return;
            }
            ticksToImpact--;
            if (!ExactPosition.InBounds(base.Map))
            {
                ticksToImpact++;
                base.Position = ExactPosition.ToIntVec3();
                Destroy();
                return;
            }
            if (ticksToImpact == 60 && Find.TickManager.CurTimeSpeed == TimeSpeed.Normal && def.projectile.soundImpactAnticipate != null)
            {
                def.projectile.soundImpactAnticipate.PlayOneShot(this);
            }
            if (ticksToImpact <= 0)
            {
                if (base.DestinationCell.InBounds(base.Map))
                {
                    base.Position = base.DestinationCell;
                }
                Destroy();
                return;
            }
            tick++;
            var Props = GetComp<CompClusterBomb>().Props;

            if (Vector2.Distance(origin, ExactPosition) > Props.safetyDistance)
            {
                IntRange tDDistance = Props.TDDistance;
                if ((tDDistance.min == 0 && tDDistance.max == 0) ? Dismantl(0, 0, DefaultDismantl: true) : Dismantl(tDDistance.min, tDDistance.max))
                {
                    if (GetComp<CompClusterBomb>().Props.doDismantlExplosion)
                    {
                        GenExplosion.DoExplosion(ExactPosition.ToIntVec3(), base.Map, GetComp<CompClusterBomb>().Props.dismantlExplosionRadius, GetComp<CompClusterBomb>().Props.dismantlExplosionDam, (Thing)this, GetComp<CompClusterBomb>().Props.dismantlExplosionDamAmount, GetComp<CompClusterBomb>().Props.dismantlExplosionArmorPenetration, GetComp<CompClusterBomb>().Props.dismantlExplosionSound, (ThingDef)null, (ThingDef)null, (Thing)null, (ThingDef)null, 1f, 1, null, false, (ThingDef)null, 0f, 1, 0f, false, (float?)null, (List<Thing>)null);
                    }
                    Dismantling();
                }
            }
        }

        private bool Dismantl(int DismantlDistanceMin = 0, int DismantlDistanceMax = 1, bool DefaultDismantl = false)
        {
            Vector3 exactPosition = ExactPosition;
            Vector3 end = base.DestinationCell.ToVector3();
            int num = Distance(exactPosition, end);
            Vector3 exactPosition2 = ExactPosition;
            Vector3 end2 = launcher.Position.ToVector3();
            int num2 = Distance(exactPosition2, end2);
            Vector3 sta = launcher.Position.ToVector3();
            Vector3 end3 = base.DestinationCell.ToVector3();
            int num3 = Distance(sta, end3);
            if (DismantlL == 0)
            {
                DismantlL = num3 / 2;
            }
            if (DefaultDismantl)
            {
                if (tick >= 10 && num2 >= DismantlL)
                {
                    tick = 0;
                    return true;
                }
            }
            else if (num <= DismantlDistanceMax && num2 >= DismantlDistanceMin)
            {
                tick = 0;
                return true;
            }
            return false;
        }

        private int Distance(Vector3 Sta, Vector3 End)
        {
            int x = End.ToIntVec3().x;
            int z = End.ToIntVec3().z;
            int x2 = Sta.ToIntVec3().x;
            int z2 = Sta.ToIntVec3().z;
            return Math.Abs((int)Math.Round(Math.Sqrt(Math.Pow(x - x2, 2.0) + Math.Pow(z - z2, 2.0))));
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
            ThingDef projectile = GetComp<CompClusterBomb>().Props.projectile;
            for (int i = 0; i < GetComp<CompClusterBomb>().Props.clusterBurstCount; i++)
            {
                ProjectileHitFlags hitFlags = ProjectileHitFlags.All;
                Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, ExactPosition.ToIntVec3(), base.Map);
                float errorRange = GetComp<CompClusterBomb>().Props.forceMissingRange;
                List<IntVec3> list = GenRadial.RadialCellsAround(base.DestinationCell, errorRange, useCenter: true).ToList();
                IntVec3 intVec = list.RandomElement();
                projectile2.Launch(this, ExactPosition, intVec, intVec, hitFlags, preventFriendlyFire: false, ThingMaker.MakeThing(base.EquipmentDef));
                GoodwillCorrection(list);
            }
            Destroy();
        }
    }
}
