using RimWorld;
using Verse;

namespace SimpleCultivation
{
    public class JobDriver_BodyRefinement : JobDriver_DeepMeditationBase
    {
        public override string GetReport()
        {
            return base.GetReport() + ": " + CompQi.bodyRefinement.ToStringPercent();
        }

        public CompQi CompQi => pawn.GetComp<CompQi>();

        public const float TotalHoursToComplete = 400f;
        public override int MeditationPeriod => (int)((TotalHoursToComplete * GenDate.TicksPerHour) / CompQi.bodyRefinement);
        public override void OnCompleted()
        {

        }

        public override void OnCancelled()
        {

        }

        public override void OnMeditationTick()
        {
            base.OnMeditationTick();
            CompQi.bodyRefinement += 1f / (TotalHoursToComplete * GenDate.TicksPerHour);
            pawn.health.capacities.Notify_CapacityLevelsDirty();
        }
    }
}
