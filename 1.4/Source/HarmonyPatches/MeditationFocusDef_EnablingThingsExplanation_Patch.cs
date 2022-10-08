using HarmonyLib;
using RimWorld;
using Verse;

namespace SimpleCultivation
{
    [HarmonyPatch(typeof(MeditationFocusDef), "EnablingThingsExplanation")]
    public static class MeditationFocusDef_EnablingThingsExplanation_Patch
    {
        public static void Postfix(MeditationFocusDef __instance, Pawn pawn, ref string __result)
        {
            if (__instance == SC_DefOf.SC_Martial)
            {
                __result = "SC.MeleeSkill".Translate();
            }
        }
    }
}
