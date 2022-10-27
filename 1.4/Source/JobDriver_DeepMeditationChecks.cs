using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace SimpleCultivation
{
    public class JobDriver_DeepMeditationChecks : JobDriver_DeepMeditationBase
    {
        public override int MeditationPeriod => SimpleCultivationSettings.devMode ? 1000 : GenDate.TicksPerHour * 10;
        public override string GetReport()
        {
            return base.GetReport() + "SC.Stage".Translate(pawn.GetComp<CompQi>().currentCheckStage + 1);
        }

        public override void Notify_Starting()
        {
            base.Notify_Starting();
            pawn.health.AddHediff(SC_DefOf.SC_CoreAlignmentDrainChecks);
        }
        public override void OnCompleted()
        {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_CoreAlignmentDrainChecks);
            if (hediff != null)
            {
                pawn.health.RemoveHediff(hediff);
            }
            pawn.GetComp<CompQi>().CheckCompleted();
        }

        public override void OnCancelled()
        {
            pawn.GetComp<CompQi>().CheckFailed();
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_CoreAlignmentDrainChecks);
            if (hediff != null)
            {
                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}
