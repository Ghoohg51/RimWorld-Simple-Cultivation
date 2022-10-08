using System.Collections.Generic;
using Verse;

namespace SimpleCultivation
{
    public class Hediff_Qi : HediffWithComps
    {
        public float resource;

        public override string Label => base.Label + ": " + (int)resource;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref resource, "resource");
        }
    }
}
