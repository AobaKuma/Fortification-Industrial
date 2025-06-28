using HarmonyLib;
using System.Reflection;
using Verse;

namespace Fortification
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            Harmony harmony = new Harmony("FortificationCE");

            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}