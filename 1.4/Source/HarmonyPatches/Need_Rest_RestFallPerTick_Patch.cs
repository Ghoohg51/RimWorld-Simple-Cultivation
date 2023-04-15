using HarmonyLib;
using RimWorld;

namespace SimpleCultivation
{
    [HarmonyPatch(typeof(Need_Rest), "RestFallPerTick", MethodType.Getter)]
    public static class Need_Rest_RestFallPerTick_Patch
    {
        private static void Postfix(ref float __result, Need_Rest __instance)
        {
            var core = __instance.pawn.GetCoreFor(BodyPartDefOf.Head);
            if (core != null)
            {
                // For eating, bronze at % 20 silver at % 15, gold at % 10, crystal at % 2.
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
