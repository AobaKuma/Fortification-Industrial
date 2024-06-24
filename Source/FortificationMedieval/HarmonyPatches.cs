using HarmonyLib;
using System.Reflection;
using Verse;

namespace FortificationMedieval
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            Harmony harmony = new Harmony("FortificationMedieval");

            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}