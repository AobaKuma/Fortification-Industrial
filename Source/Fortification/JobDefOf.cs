using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Fortification
{
	[DefOf]
	public static class JobDefOf
	{
		static JobDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
		}
		public static JobDef FT_EnterBunkerFacility;
	}
}
