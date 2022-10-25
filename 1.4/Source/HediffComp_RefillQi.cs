using Verse;

namespace SimpleCultivation
{
    public class HediffCompProperties_RefillQi : HediffCompProperties
    {
        public float? qiRefillRate;
        public HediffCompProperties_RefillQi()
        {
            compClass = typeof(HediffComp_RefillQi);
        }
    }
    public class HediffComp_RefillQi : HediffComp
    {
        public HediffCompProperties_RefillQi Props => base.props as HediffCompProperties_RefillQi;

        public float qiRefillRate;
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            Hediff_Qi hediff = Pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_QiResource) as Hediff_Qi;
            hediff ??= Pawn.health.AddHediff(SC_DefOf.SC_QiResource) as Hediff_Qi;
            hediff.Resource += Props.qiRefillRate ?? qiRefillRate;
        }
        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref qiRefillRate, "qiRefillRate");
        }
    }
}
