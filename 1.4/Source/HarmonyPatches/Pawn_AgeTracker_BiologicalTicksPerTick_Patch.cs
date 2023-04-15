using HarmonyLib;
using RimWorld;
using Verse;

namespace SimpleCultivation
{
    [HarmonyPatch(typeof(Pawn_AgeTracker), "BiologicalTicksPerTick", MethodType.Getter)]
    public static class Pawn_AgeTracker_BiologicalTicksPerTick_Patch
    {
        private static void Postfix(ref float __result, Pawn_AgeTracker __instance)
        {
            var core = __instance.pawn.GetCoreFor(BodyPartDefOf.Heart);
            if (core != null)
            {
                if (core.def == SC_DefOf.SC_BronzeGradeCore)
                {
                    __result *= 0.5f;
                }
                else if (core.def == SC_DefOf.SC_SilverGradeCore)
                {
                    __result *= 0.333f;
                }
                else if (core.def == SC_DefOf.SC_GoldGradeCore)
                {
                    __result *= 0.25f;
                }
                else if (core.def == SC_DefOf.SC_CrystalGradeCore)
                {
                    __result *= 0.166f;
                }
            }
        }
    }
}
