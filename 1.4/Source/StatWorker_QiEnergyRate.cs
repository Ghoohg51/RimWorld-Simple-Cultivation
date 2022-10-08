using RimWorld;
using Verse;

namespace SimpleCultivation
{
    public class StatWorker_QiEnergyRate : StatWorker
    {
        public override void FinalizeValue(StatRequest req, ref float val, bool applyPostProcess)
        {
            var pawn = req.Thing;
            float value = pawn.GetStatValue(SC_DefOf.SC_QiRegenRatePerMaxQi);
            float maxQi = pawn.GetStatValue(SC_DefOf.SC_MaxQi);
            Log.Message(req + " - " + pawn);
            if (value > 0)
            {
                val += maxQi * value;
            }
            base.FinalizeValue(req, ref val, applyPostProcess);
        }
    }
}
