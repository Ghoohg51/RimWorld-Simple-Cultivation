using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace SimpleCultivation
{
    [HarmonyPatch(typeof(BodyPartDef), "GetMaxHealth")]
    public static class BodyPartDef_GetMaxHealth_Patch
    {
        private static void Postfix(ref float __result, BodyPartDef __instance, Pawn pawn)
        {
            var core = pawn.GetCoreFor(__instance);
            if (core != null)
            {

            }
        }
    }

    [HarmonyPatch(typeof(Pawn_AgeTracker), "GetMaxHealth")]
    public static class Pawn_AgeTracker_GetMaxHealth_Patch
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

    [HarmonyPatch(typeof(Need_Rest), "RestFallPerTick", MethodType.Getter)]
    public static class Need_Rest_RestFallPerTick_Patch
    {
        private static void Postfix(ref float __result, Need_Rest __instance)
        {
            var core = __instance.pawn.GetCoreFor(BodyPartDefOf.Head);
            if (core != null)
            {
                // For eating and sleep, bronze at % 20 silver at % 15, gold at % 10, crystal at % 2.
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

    [HarmonyPatch(typeof(Need_Food), "FoodFallPerTick", MethodType.Getter)]
    public static class Need_Food_FoodFallPerTick_Patch
    {
        private static void Postfix(ref float __result, Need_Food __instance)
        {
            var core = __instance.pawn.GetCoreFor(BodyPartDefOf.Stomach);
            if (core != null)
            {
                // For eating and sleep, bronze at % 20 silver at % 15, gold at % 10, crystal at % 2.
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
