using HarmonyLib;
using RimWorld;
using Verse;

namespace SimpleCultivation
{
    [HarmonyPatch(typeof(StatWorker), "ShouldShowFor")]
    public static class StatWorker_ShouldShowFor
    {
        public static bool Prefix(StatWorker __instance, StatRequest req, ref bool __result)
        {
            if (__instance.stat.category == SC_DefOf.SC_SimpleCultivation)
            {
                if (req.Thing is Pawn)
                {
                    __result = true;
                    return false;
                }
                else
                {
                    __result = false;
                    return false;
                }
            }
            return true;
        }
    }
}
