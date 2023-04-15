using HarmonyLib;
using System.Collections.Generic;
using Verse;
using static Verse.PawnCapacityUtility;

namespace SimpleCultivation
{
    [HarmonyPatch(typeof(PawnCapacityUtility), "CalculateCapacityLevel")]
    public static class PawnCapacityUtility_CalculateCapacityLevel_Patch
    {
        public static void Postfix(ref float __result, HediffSet diffSet, PawnCapacityDef capacity, List<CapacityImpactor> impactors = null, bool forTradePrice = false)
        {
            var comp = diffSet.pawn.GetComp<CompQi>();
            if (comp != null && comp.bodyRefinement > 0)
            {
                __result += __result * comp.bodyRefinement;
                impactors?.Add(new CapacityImpactorBodyRefinement
                {
                    capacity = capacity
                });
            }
        }
    }

    public class CapacityImpactorBodyRefinement : CapacityImpactorCapacity
    {
        public override string Readable(Pawn pawn)
        {
            var comp = pawn.GetComp<CompQi>();
            return "SC.BodyRefinement".Translate(comp.bodyRefinement.ToStringPercent());
        }
    }
}
