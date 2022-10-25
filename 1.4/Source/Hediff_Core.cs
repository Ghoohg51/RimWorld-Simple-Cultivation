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
                if (hitpoints == 0)
                {
                    label += " (" + "SC.Shattered".Translate() + ")";
                }
                else if (hitpoints < MaxHitpoints)
                {
                    label += " (" + (hitpoints / MaxHitpoints).ToStringPercent() + ")";
                }
                return label;
            }
        }
        public override bool ShouldRemove => false;

        public bool CanHeal => Shattered is false;
        public bool Shattered => hitpoints == 0;
        public void ShatterCore(float damage)
        {
            hitpoints = Mathf.Max(hitpoints - damage, 0);
        }

        public void HealCore(float heal)
        {
            hitpoints = Mathf.Min(hitpoints + heal, 100);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref hitpoints, "hitpoints", MaxHitpoints);
        }
    }
}
