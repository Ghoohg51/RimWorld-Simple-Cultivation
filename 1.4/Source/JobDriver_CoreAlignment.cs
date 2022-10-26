using RimWorld;
using Verse;

namespace SimpleCultivation
{
    public class JobDriver_CoreAlignment : JobDriver_DeepMeditationBase
    {
        public override int MeditationPeriod => GenDate.TicksPerHour * 48;
        public override void OnCompleted()
        {
            var comp = pawn.GetComp<CompQi>();
            comp.coreBeingMoved.Part = pawn.AvailableBodyPartsForCore().RandomElement(); 
        }
        public override void OnCancelled()
        {
            var comp = pawn.GetComp<CompQi>();
            comp.coreBeingMoved.ShatterCore(10);
        }
    }
}
