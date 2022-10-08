using RimWorld;

namespace SimpleCultivation
{
    public class StatWorker_MaxQiEnergy : StatWorker
    {
        public override void FinalizeValue(StatRequest req, ref float val, bool applyPostProcess)
        {
            _ = req.Pawn;
            base.FinalizeValue(req, ref val, applyPostProcess);
        }
    }
}
