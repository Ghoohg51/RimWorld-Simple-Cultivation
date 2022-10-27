using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace SimpleCultivation
{
    public class JobDriver_CoreAlignment : JobDriver_DeepMeditationBase
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(TargetA, job);
        }
        public override int MeditationPeriod => SimpleCultivationSettings.devMode ? 1000 : GenDate.TicksPerHour * 48;
        public override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);
            foreach (var toil in base.MakeNewToils())
            {
                yield return toil;
            }
        }
        public override void OnCompleted()
        {
            var comp = pawn.GetComp<CompQi>();
            comp.coreBeingMoved.Part = comp.partBeingAssigned;
            comp.coreBeingMoved = null;
            comp.partBeingAssigned = null;
        }

        public override void OnCancelled()
        {
            var comp = pawn.GetComp<CompQi>();
            comp.coreBeingMoved.ShatterCore(10);
            comp.coreBeingMoved = null;
            comp.partBeingAssigned = null;
        }
    }
}
