using HarmonyLib;
using RimWorld;
using Verse.AI;

namespace SimpleCultivation
{
    [HarmonyPatch(typeof(JobDriver), "DriverTick")]
    public static class JobDriver_DriverTick_Patch
    {
        private static void Postfix(this JobDriver __instance)
        {
            if (__instance is JobDriver_Meditate && __instance.pawn.pather.moving is false)
            {
                if (__instance.pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_CoreFormation) is Hediff_CoreFormation hediff)
                {
                    hediff.AddProgress();
                }
            }
        }
    }
}
