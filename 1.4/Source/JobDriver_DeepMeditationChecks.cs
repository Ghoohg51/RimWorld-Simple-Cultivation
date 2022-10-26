using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace SimpleCultivation
{
    public class JobDriver_DeepMeditationChecks : JobDriver_DeepMeditationBase
    {
        public override int MeditationPeriod => GenDate.TicksPerHour * 10;
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
            pawn.GetComp<CompQi>().CheckCompleted();
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_CoreAlignmentDrainChecks);
            if (hediff != null)
            {
                pawn.health.RemoveHediff(hediff);
            }
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
