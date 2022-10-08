using HarmonyLib;
using RimWorld;
using Verse;

namespace SimpleCultivation
{
    [HarmonyPatch(typeof(MeditationFocusTypeAvailabilityCache), "PawnCanUseInt")]
    public static class MeditationFocusTypeAvailabilityCache_PawnCanUseInt_Patch
    {
        public static void Postfix(Pawn p, MeditationFocusDef type, ref bool __result)
        {
            if (type == SC_DefOf.SC_Martial && (p.WorkTagIsDisabled(WorkTags.Violent) || p.skills.GetSkill(SkillDefOf.Melee).TotallyDisabled))
            {
                __result = false;
            }
        }
    }
}
