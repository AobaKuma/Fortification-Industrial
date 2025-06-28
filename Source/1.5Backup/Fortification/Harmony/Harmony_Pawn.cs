using HarmonyLib;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;

namespace Fortification
{
    [HarmonyPatch(typeof(Pawn), "GetGizmos")]
    internal static class Harmony_Pawn
    {
        static List<IntVec3> AcceptedCell(Pawn pawn)
        {
            return new List<IntVec3>() { pawn.Position + IntVec3.South, pawn.Position + IntVec3.North, pawn.Position + IntVec3.East, pawn.Position + IntVec3.West };
        }

        public static TargetingParameters TargetParam(Pawn pawn)
        {
            return new TargetingParameters
            {
                canTargetLocations = true,
                canTargetSelf = false,
                canTargetPawns = false,
                canTargetFires = false,
                canTargetBuildings = false,
                canTargetItems = false,
                validator = (TargetInfo x) => AcceptedCell(pawn).Contains(x.Cell) && x.Cell.GetEdifice(pawn.Map) == null
            };
        }

        public static Gizmo GenGizmoForThing(Thing thing, Pawn pawn)
        {
            if (thing is MinifiedThingDeployable deployable)
            {
                Command_Target command_Target = new Command_Target
                {
                    defaultLabel = deployable.InnerThing.Label,
                    targetingParams = TargetParam(pawn),
                    icon = deployable.InnerThing.def.GetUIIconForStuff(null),
                    action = delegate (LocalTargetInfo target)
                    {
                        deployable.Deploy(target.Cell, pawn);
                    }
                };
                if (!pawn.Drafted)
                {
                    command_Target.Disable("FT_DisabledUndrafted".Translate());
                }
                return command_Target;
            }
            return null;
        }

        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Pawn __instance)
        {
            foreach (Gizmo command in __result) yield return command;
            if (__instance.Spawned && Find.Selector.SingleSelectedThing == __instance && __instance.RaceProps.Humanlike)
            {
                foreach (Thing thing in __instance.inventory.innerContainer)
                {
                    Gizmo g = GenGizmoForThing(thing, __instance);
                    if (g != null) yield return g;
                }
                foreach (Thing thing in __instance.equipment.AllEquipmentListForReading)
                {
                    Gizmo g = GenGizmoForThing(thing, __instance);
                    if (g != null) yield return g;
                }
            }
        }
    }
}