using RimWorld;
using Verse;

namespace SimpleCultivation
{
    public class StatWorker_QiEnergyRate : StatWorker
    {
        public override void FinalizeValue(StatRequest req, ref float val, bool applyPostProcess)
        {
            Thing pawn = req.Thing;
            CompQi comp = pawn.TryGetComp<CompQi>();
            float value = pawn.GetStatValue(SC_DefOf.SC_QiRegenRatePerMaxQi);
            float maxQi = pawn.GetStatValue(SC_DefOf.SC_MaxQi);
            if (value > 0)
            {
                val += maxQi * value;
            }
            if (comp.PerformingChecks)
            {
                val -= 10;
            }
            base.FinalizeValue(req, ref val, applyPostProcess);
        }
    }
}
