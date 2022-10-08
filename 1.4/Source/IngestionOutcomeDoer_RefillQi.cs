using RimWorld;
using Verse;

namespace SimpleCultivation
{
    public class IngestionOutcomeDoer_RefillQi : IngestionOutcomeDoer
    {
        public HediffDef hediffDef;

        public float refillRate;

        public int duration;
        public override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            var hediff = HediffMaker.MakeHediff(hediffDef, pawn);
            var comp = hediff.TryGetComp<HediffComp_RefillQi>();
            comp.qiRefillRate = refillRate;
            var comp2 = hediff.TryGetComp<HediffComp_Disappears>();
            comp2.ticksToDisappear = duration;
            pawn.health.AddHediff(hediff);
        }
    }
}
