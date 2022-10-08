using RimWorld;
using System.Collections.Generic;
using Verse.AI;

namespace SimpleCultivation
{
    public class JobDriver_DeepMeditation : JobDriver
    {
        public int MeditationPeriod => GenDate.TicksPerHour * 10;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
        public bool isCompletedSuccessfully;
        public Hediff_CoreFormation Hediff => pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_CoreFormation) as Hediff_CoreFormation;
        public override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_General.Wait(MeditationPeriod).WithProgressBarToilDelay(TargetIndex.A);
            yield return new Toil
            {
                initAction = delegate
                {
                    isCompletedSuccessfully = true;
                    Hediff.CheckCompleted();
                }
            };
            AddFinishAction(delegate
            {
                if (!isCompletedSuccessfully)
                {
                    Hediff.CheckCancelled();
                }
            });
        }
    }
}
