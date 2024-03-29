﻿using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace SimpleCultivation
{
    public class Hediff_Qi : HediffWithComps
    {
        private float resourceInt;
        public CompQi CompQi => pawn.GetComp<CompQi>();
        public float Resource
        {
            get => resourceInt;
            set
            {
                resourceInt = value;
                resourceInt = Mathf.Min(Mathf.Max(resourceInt, 0), pawn.GetStatValue(SC_DefOf.SC_MaxQi));
                if (resourceInt == 0 && CompQi.PerformingChecks)
                {
                    pawn.Kill(null);
                }
            }
        }

        public override bool ShouldRemove => pawn.health.hediffSet.hediffs.OfType<Hediff_Core>().Any() is false;
        public override string Label => base.Label + ": " + (int)resourceInt;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref resourceInt, "resourceInt");
        }
    }
}
