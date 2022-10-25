using System.Linq;
using Verse;

namespace SimpleCultivation
{
    public class Hediff_Qi : HediffWithComps
    {
        public float resource;
        public override bool ShouldRemove => pawn.health.hediffSet.hediffs.OfType<Hediff_Core>().Any() is false;
        public override string Label => base.Label + ": " + (int)resource;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref resource, "resource");
        }
    }
}
