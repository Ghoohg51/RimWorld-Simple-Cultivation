using RimWorld;
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
            yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.OnCell);
            var meditate = Toils_General.Wait(MeditationPeriod).WithProgressBar(TargetIndex.A, () => 1f - ((float)pawn.jobs.curDriver.ticksLeftThisToil / (float)MeditationPeriod));
            meditate.socialMode = RandomSocialMode.Off;
            meditate.AddPreTickAction(delegate
            {
                OnMeditationTick();
            });
            yield return meditate;
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

        public virtual void OnMeditationTick()
        {

        }

        public abstract void OnCompleted();

        public abstract void OnCancelled();
    }
}
