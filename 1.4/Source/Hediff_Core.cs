using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace SimpleCultivation
{
    public class Hediff_Core : HediffWithComps
    {
        public override bool TryMergeWith(Hediff other)
        {
            return false;
        }
        public override int UIGroupKey => GetHashCode();

        public const float MaxHitpoints = 100;
        public float hitpoints = MaxHitpoints;
        public override string Label
        {
            get
            {
                string label = base.Label;
                if (hitpoints is > 0 and < MaxHitpoints)
                {
                    label += " (" + (hitpoints / MaxHitpoints).ToStringPercent() + ")";
                }
                return label;
            }
        }
        public override bool ShouldRemove => false;
        public bool CanHeal => ShatteredFully is false;
        public bool ShatteredFully => hitpoints == 0;
        public void ShatterCore(float damage)
        {
            hitpoints = Mathf.Max(hitpoints - damage, 0);
            if (ShatteredFully)
            {
                Severity = 1;
            }
        }

        public void HealCore(float heal)
        {
            hitpoints = Mathf.Min(hitpoints + heal, MaxHitpoints);
            if (ShatteredFully is false)
            {
                Severity = 0;
            }
            List<Hediff_Core> cores = pawn.health.hediffSet.hediffs.OfType<Hediff_Core>()
                .Where(x => x.hitpoints == MaxHitpoints).ToList();
            if (cores.Count == 7)
            {
                pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(SC_DefOf.SC_DeepMeditationChecks));
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref hitpoints, "hitpoints", MaxHitpoints);
        }
    }
}
