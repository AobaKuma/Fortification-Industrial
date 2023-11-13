using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace Fortification
{
	public class JobDriver_EnterBunkerFacility : JobDriver
	{
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil toil = Toils_General.Wait(job.def.joyDuration, TargetIndex.None);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return toil;
			Toil enter = new Toil();
			enter.initAction = delegate ()
			{
				Pawn actor = enter.actor;
				Building_TurretCapacity pod = (Building_TurretCapacity)actor.CurJob.targetA.Thing;
				void action()
				{
					actor.DeSpawn(DestroyMode.Vanish);
					pod.TryAcceptThing(actor, true);
				}
				action();
			};
			enter.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return enter;
			yield break;
		}
	}
}
