using System.Collections.Generic;
using Verse.AI;

namespace SimpleCultivation
{
    public abstract class JobDriver_DeepMeditationBase : JobDriver
    {
        public virtual int MeditationPeriod { get; }
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
                    OnCompleted();
                }
            };
            AddFinishAction(delegate
            {
                if (!isCompletedSuccessfully)
                {
                    OnCancelled();
                }
            });
        }

        public abstract void OnCompleted();

        public abstract void OnCancelled();
    }
}
