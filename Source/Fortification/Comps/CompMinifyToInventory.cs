using RimWorld;
using Verse;
using Verse.Sound;

namespace Fortification
{
    public class CompMinifyToInventory : CompUseEffect
    {
        private CompProperties_UseEffect Props => (CompProperties_UseEffect)props;
        public override void PrepareTick()
        {
        }
        public override void DoEffect(Pawn usedBy)
        {
            Thing thing = parent;
            if (parent is Building building && building.def.Minifiable)
            {
                thing = building.MakeMinified();
            }
            if (usedBy.equipment.Primary == null && thing.TryGetComp<CompEquippable>() != null && !usedBy.WorkTagIsDisabled(WorkTags.Violent))
            {
                ThingWithComps thingWithComps2 = null;
                if (thing.def.stackLimit > 1 && thing.stackCount > 1)
                {
                    thingWithComps2 = (ThingWithComps)thing.SplitOff(1);
                }
                else
                {
                    thingWithComps2 = (ThingWithComps)thing;
                }
                usedBy.equipment.MakeRoomFor(thingWithComps2);
                usedBy.equipment.AddEquipment(thingWithComps2);
                if (thing.def.soundInteract != null)
                {
                    thing.def.soundInteract.PlayOneShot(new TargetInfo(usedBy.Position, usedBy.Map));
                }
            }
            else
            {
                if (thing.Spawned)
                {
                    thing.DeSpawn();
                }
                usedBy.inventory.innerContainer.TryAddOrTransfer(thing);
            }
        }
    }
}
