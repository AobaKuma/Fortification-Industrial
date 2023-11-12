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
            Harmony harmony = new Harmony("Fortification");

            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}