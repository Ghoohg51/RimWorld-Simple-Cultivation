﻿using HarmonyLib;
using Verse;

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
                if (core.def == SC_DefOf.SC_BronzeGradeCore)
                {
                    __result *= 1.1f;
                }
                else if (core.def == SC_DefOf.SC_SilverGradeCore)
                {
                    __result *= 1.2f;
                }
                else if (core.def == SC_DefOf.SC_GoldGradeCore)
                {
                    __result *= 1.3f;
                }
                else if (core.def == SC_DefOf.SC_CrystalGradeCore)
                {
                    __result *= 1.5f;
                }
            }
        }
    }
}
