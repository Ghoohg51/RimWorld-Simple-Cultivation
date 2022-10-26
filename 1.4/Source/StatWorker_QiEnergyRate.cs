using RimWorld;
using Verse;

namespace SimpleCultivation
{
    public class StatWorker_QiEnergyRate : StatWorker
    {
        public override bool ShouldShowFor(StatRequest req)
        {
            if (req.Pawn != null)
            {
                if (req.Pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_QiResource) is null)
                {
                    return false;
                }
            }
            return base.ShouldShowFor(req);
        }
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
            base.FinalizeValue(req, ref val, applyPostProcess);
        }
    }
}
