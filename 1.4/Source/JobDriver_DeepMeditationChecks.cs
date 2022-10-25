using RimWorld;
using System.Collections.Generic;
using Verse.AI;

namespace SimpleCultivation
{
    public class JobDriver_DeepMeditationChecks : JobDriver
    {
        public int MeditationPeriod => GenDate.TicksPerHour * 10;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
        public bool isCompletedSuccessfully;
        public override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_General.Wait(MeditationPeriod).WithProgressBarToilDelay(TargetIndex.A);
            yield return new Toil
            {
                initAction = delegate
                {
                    isCompletedSuccessfully = true;
                    pawn.GetComp<CompQi>().CheckCompleted();
                }
            };
            AddFinishAction(delegate
            {
                if (!isCompletedSuccessfully)
                {
                    pawn.GetComp<CompQi>().CheckFailed();
                }
            });
        }
    }
}
