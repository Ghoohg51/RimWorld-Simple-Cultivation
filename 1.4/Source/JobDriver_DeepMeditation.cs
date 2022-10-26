using RimWorld;

namespace SimpleCultivation
{
    public class JobDriver_DeepMeditation : JobDriver_DeepMeditationBase
    {
        public override int MeditationPeriod => GenDate.TicksPerHour * 10;
        public Hediff_CoreFormation Hediff => pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_CoreFormation) as Hediff_CoreFormation;
        public override void OnCompleted()
        {
            Hediff.CheckCompleted();
        }
        public override void OnCancelled()
        {
            Hediff.CheckCancelled();
        }
    }
}
