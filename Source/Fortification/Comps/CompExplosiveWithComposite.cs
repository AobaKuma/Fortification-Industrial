﻿using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Fortification
{
    public class CompExplosiveWithComposite : ThingComp
    {
        public bool wickStarted;

        protected int wickTicksLeft;

        private Thing instigator;

        private int countdownTicksLeft = -1;

        public bool destroyedThroughDetonation;

        private List<Thing> thingsIgnoredByExplosion;

        public float? customExplosiveRadius;

        protected Sustainer wickSoundSustainer;

        private OverlayHandle? overlayBurningWick;

        public CompProperties_ExplosiveWithComposite Props => (CompProperties_ExplosiveWithComposite)props;

        public ModExtension_CompositeExplosion compCompositeExplosion;
        public ModExtension_ExpolsionWithEvents compExpolsionWithEvents;
        public int ticksToDetonation_ForComps = -1;

        protected int StartWickThreshold => Mathf.RoundToInt(Props.startWickHitPointsPercent * (float)parent.MaxHitPoints);

        private bool CanEverExplodeFromDamage
        {
            get
            {
                if (Props.chanceNeverExplodeFromDamage < 1E-05f)
                {
                    return true;
                }
                Rand.PushState();
                Rand.Seed = parent.thingIDNumber.GetHashCode();
                bool result = Rand.Value > Props.chanceNeverExplodeFromDamage;
                Rand.PopState();
                return result;
            }
        }
        public void AddThingsIgnoredByExplosion(List<Thing> things)
        {
            if (thingsIgnoredByExplosion == null)
            {
                thingsIgnoredByExplosion = new List<Thing>();
            }
            thingsIgnoredByExplosion.AddRange(things);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look(ref instigator, "instigator");
            Scribe_Collections.Look(ref thingsIgnoredByExplosion, "thingsIgnoredByExplosion", LookMode.Reference);
            Scribe_Values.Look(ref wickStarted, "wickStarted", defaultValue: false);
            Scribe_Values.Look(ref wickTicksLeft, "wickTicksLeft", 0);
            Scribe_Values.Look(ref destroyedThroughDetonation, "destroyedThroughDetonation", defaultValue: false);
            Scribe_Values.Look(ref countdownTicksLeft, "countdownTicksLeft", 0);
            Scribe_Values.Look(ref customExplosiveRadius, "explosiveRadius");
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if (Props.countdownTicks.HasValue)
            {
                countdownTicksLeft = Props.countdownTicks.Value.RandomInRange;
            }
            compCompositeExplosion = parent.def.GetModExtension<ModExtension_CompositeExplosion>();
            compExpolsionWithEvents = parent.def.GetModExtension<ModExtension_ExpolsionWithEvents>();
            UpdateOverlays();
        }

        public override void CompTick()
        {
            if (countdownTicksLeft > 0)
            {
                countdownTicksLeft--;
                if (countdownTicksLeft == 0)
                {
                    StartWick();
                    countdownTicksLeft = -1;
                }
            }
            if (!wickStarted)
            {
                return;
            }
            if (wickSoundSustainer == null)
            {
                StartWickSustainer();
            }
            else
            {
                wickSoundSustainer.Maintain();
            }
            if (Props.wickMessages != null)
            {
                foreach (WickMessage wickMessage in Props.wickMessages)
                {
                    if (wickMessage.ticksLeft == wickTicksLeft && wickMessage.wickMessagekey != null)
                    {
                        Messages.Message(wickMessage.wickMessagekey.Translate(parent, wickTicksLeft.ToStringSecondsFromTicks()), parent, wickMessage.messageType ?? MessageTypeDefOf.NeutralEvent, historical: false);
                    }
                }
            }
            wickTicksLeft--;
            if (wickTicksLeft <= 0)
            {
                ticksToDetonation_ForComps--;
                if (ticksToDetonation_ForComps > 0)
                {
                    ticksToDetonation_ForComps--;
                    CompositeExplosion(parent.MapHeld);
                }
                else
                {
                    Detonate(parent.MapHeld);
                }
            }
        }

        private void StartWickSustainer()
        {
            SoundDefOf.MetalHitImportant.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
            SoundInfo info = SoundInfo.InMap(parent, MaintenanceType.PerTick);
            wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
        }

        private void EndWickSustainer()
        {
            if (wickSoundSustainer != null)
            {
                wickSoundSustainer.End();
                wickSoundSustainer = null;
            }
        }

        private void UpdateOverlays()
        {
            if (parent.Spawned)
            {
                parent.Map.overlayDrawer.Disable(parent, ref overlayBurningWick);
                if (wickStarted)
                {
                    overlayBurningWick = parent.Map.overlayDrawer.Enable(parent, OverlayTypes.BurningWick);
                }
            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            if (mode == DestroyMode.KillFinalize && Props.explodeOnKilled)
            {
                Detonate(previousMap, ignoreUnspawned: true);
            }
        }

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            absorbed = false;
            if (!CanEverExplodeFromDamage)
            {
                return;
            }
            if (dinfo.Def.ExternalViolenceFor(parent) && dinfo.Amount >= (float)parent.HitPoints && CanExplodeFromDamageType(dinfo.Def))
            {
                if (parent.MapHeld != null)
                {
                    instigator = dinfo.Instigator;
                    Detonate(parent.MapHeld);
                    if (parent.Destroyed)
                    {
                        absorbed = true;
                    }
                }
            }
            else if (!wickStarted && Props.startWickOnDamageTaken != null && Props.startWickOnDamageTaken.Contains(dinfo.Def))
            {
                StartWick(dinfo.Instigator);
            }
        }

        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if (CanEverExplodeFromDamage && CanExplodeFromDamageType(dinfo.Def) && !parent.Destroyed)
            {
                if (wickStarted && dinfo.Def == DamageDefOf.Stun)
                {
                    StopWick();
                }
                else if (!wickStarted && parent.HitPoints <= StartWickThreshold && dinfo.Def.ExternalViolenceFor(parent))
                {
                    StartWick(dinfo.Instigator);
                }
            }
        }

        public void StartWick(Thing instigator = null)
        {
            if (!wickStarted && !(ExplosiveRadius() <= 0f))
            {
                this.instigator = instigator;
                wickStarted = true;
                wickTicksLeft = Props.wickTicks.RandomInRange;
                StartWickSustainer();
                GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(parent, Props.explosiveDamageType, null, instigator);
                UpdateOverlays();
            }
        }

        public void StopWick()
        {
            wickStarted = false;
            instigator = null;
            UpdateOverlays();
        }

        public float ExplosiveRadius()
        {
            CompProperties_ExplosiveWithComposite compProperties_Explosive = Props;
            float num = customExplosiveRadius ?? Props.explosiveRadius;
            if (parent.stackCount > 1 && compProperties_Explosive.explosiveExpandPerStackcount > 0f)
            {
                num += Mathf.Sqrt((float)(parent.stackCount - 1) * compProperties_Explosive.explosiveExpandPerStackcount);
            }
            if (compProperties_Explosive.explosiveExpandPerFuel > 0f && parent.GetComp<CompRefuelable>() != null)
            {
                num += Mathf.Sqrt(parent.GetComp<CompRefuelable>().Fuel * compProperties_Explosive.explosiveExpandPerFuel);
            }
            return num;
        }

        protected void Detonate(Map map, bool ignoreUnspawned = false)
        {
            if (!ignoreUnspawned && !parent.SpawnedOrAnyParentSpawned)
            {
                return;
            }
            CompProperties_ExplosiveWithComposite compProperties_Explosive = Props;
            float num = ExplosiveRadius();
            if (compProperties_Explosive.explosiveExpandPerFuel > 0f && parent.GetComp<CompRefuelable>() != null)
            {
                parent.GetComp<CompRefuelable>().ConsumeFuel(parent.GetComp<CompRefuelable>().Fuel);
            }
            if (compProperties_Explosive.destroyThingOnExplosionSize <= num && !parent.Destroyed)
            {
                destroyedThroughDetonation = true;
                parent.Kill();
            }
            EndWickSustainer();
            wickStarted = false;
            if (map == null)
            {
                Log.Warning("Tried to detonate CompExplosive in a null map.");
                return;
            }
            if (compProperties_Explosive.explosionEffect != null)
            {
                Effecter effecter = compProperties_Explosive.explosionEffect.Spawn();
                effecter.Trigger(new TargetInfo(parent.PositionHeld, map), new TargetInfo(parent.PositionHeld, map));
                effecter.Cleanup();
            }
            if (compExpolsionWithEvents != null)
            {
                foreach (Condition condition in compExpolsionWithEvents.conditions)
                {
                    TryStartCondition(map, condition);
                }
            }
            GenExplosion.DoExplosion(instigator: (instigator == null || (instigator.HostileTo(parent.Faction) && parent.Faction != Faction.OfPlayer)) ? parent : instigator, center: parent.PositionHeld, map: map, radius: num, damType: compProperties_Explosive.explosiveDamageType, damAmount: compProperties_Explosive.damageAmountBase, armorPenetration: compProperties_Explosive.armorPenetrationBase, explosionSound: compProperties_Explosive.explosionSound, weapon: null, projectile: null, intendedTarget: null, postExplosionSpawnThingDef: compProperties_Explosive.postExplosionSpawnThingDef, postExplosionSpawnChance: compProperties_Explosive.postExplosionSpawnChance, postExplosionSpawnThingCount: compProperties_Explosive.postExplosionSpawnThingCount, applyDamageToExplosionCellsNeighbors: compProperties_Explosive.applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef: compProperties_Explosive.preExplosionSpawnThingDef, preExplosionSpawnChance: compProperties_Explosive.preExplosionSpawnChance, preExplosionSpawnThingCount: compProperties_Explosive.preExplosionSpawnThingCount, chanceToStartFire: compProperties_Explosive.chanceToStartFire, damageFalloff: compProperties_Explosive.damageFalloff, direction: null, ignoredThings: thingsIgnoredByExplosion);
        }

        private bool CanExplodeFromDamageType(DamageDef damage)
        {
            if (Props.requiredDamageTypeToExplode != null)
            {
                return Props.requiredDamageTypeToExplode == damage;
            }
            return true;
        }
        //複合爆炸
        private void CompositeExplosion(Map map)
        {
            if (compCompositeExplosion != null)
            {
                foreach (CompositeExplosion compositeExplosion in compCompositeExplosion.compositeExplosions)
                {

                    if (compositeExplosion.countdown == ticksToDetonation_ForComps)
                    {
                        GenExplosion.DoExplosion(parent.PositionHeld,
                               map,
                               compositeExplosion.radius,
                               compositeExplosion.damamgeDef,
                               (instigator == null || (instigator.HostileTo(parent.Faction) && parent.Faction != Faction.OfPlayer)) ? parent : instigator,
                               compositeExplosion.amount,
                               compositeExplosion.armorPenetration ?? (-1f),
                               compositeExplosion.explosionSound,
                               weapon: null,
                               projectile: null,
                               intendedTarget: null,
                               compositeExplosion.postExplosionSpawnThingDef,
                               compositeExplosion.postExplosionSpawnChance,
                               compositeExplosion.postExplosionSpawnThingCount,
                               postExplosionGasType: null,
                               applyDamageToExplosionCellsNeighbors: false,
                               compositeExplosion.preExplosionSpawnThingDef,
                               compositeExplosion.preExplosionSpawnChance,
                               compositeExplosion.preExplosionSpawnThingCount,
                               compositeExplosion.chanceToStartFire);
                    }
                }
            }
        }
        //遍歷觸發事件
        private void TryStartCondition(Map map, Condition condition)
        {
            if (Rand.Range(1, 101) > condition.percent)
            {
                return;
            }
            foreach (GameCondition activeCondition in map.gameConditionManager.ActiveConditions)
            {
                if (activeCondition.def == condition.conditionDef)
                {
                    return;
                }
            }
            GameCondition cond = GameConditionMaker.MakeCondition(condition.conditionDef, condition.duration.RandomInRange);
            map.gameConditionManager.RegisterCondition(cond);
        }
        public override string CompInspectStringExtra()
        {
            string text = "";
            if (countdownTicksLeft != -1)
            {
                text += "DetonationCountdown".Translate(countdownTicksLeft.TicksToDays().ToString("0.0"));
            }
            if (Props.extraInspectStringKey != null)
            {
                text += ((text != "") ? "\n" : "") + Props.extraInspectStringKey.Translate();
            }
            return text;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (countdownTicksLeft > 0)
            {
                Command_Action command_Action = new Command_Action
                {
                    defaultLabel = "DEV: Trigger countdown",
                    action = delegate
                    {
                        countdownTicksLeft = 1;
                    }
                };
                yield return command_Action;
            }
        }
    }
    public class CompProperties_ExplosiveWithComposite : CompProperties
    {
        public float explosiveRadius = 1.9f;

        public DamageDef explosiveDamageType;

        public int damageAmountBase = -1;

        public float armorPenetrationBase = -1f;

        public ThingDef postExplosionSpawnThingDef;

        public float postExplosionSpawnChance;

        public int postExplosionSpawnThingCount = 1;

        public bool applyDamageToExplosionCellsNeighbors;

        public ThingDef preExplosionSpawnThingDef;

        public float preExplosionSpawnChance;

        public int preExplosionSpawnThingCount = 1;

        public float chanceToStartFire;

        public bool damageFalloff;

        public bool explodeOnKilled;

        public float explosiveExpandPerStackcount;

        public float explosiveExpandPerFuel;

        public EffecterDef explosionEffect;

        public SoundDef explosionSound;

        public List<DamageDef> startWickOnDamageTaken;

        public float startWickHitPointsPercent = 0.2f;

        public IntRange wickTicks = new IntRange(140, 150);

        public float wickScale = 1f;

        public float chanceNeverExplodeFromDamage;

        public float destroyThingOnExplosionSize;

        public DamageDef requiredDamageTypeToExplode;

        public IntRange? countdownTicks;

        public string extraInspectStringKey;

        public List<WickMessage> wickMessages;

        public CompProperties_ExplosiveWithComposite()
        {
            compClass = typeof(CompExplosiveWithComposite);
        }

        public override void ResolveReferences(ThingDef parentDef)
        {
            base.ResolveReferences(parentDef);
            if (explosiveDamageType == null)
            {
                explosiveDamageType = DamageDefOf.Bomb;
            }
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string item in base.ConfigErrors(parentDef))
            {
                yield return item;
            }
            if (parentDef.tickerType != TickerType.Normal)
            {
                yield return "CompExplosive requires Normal ticker type";
            }
        }
    }

}
