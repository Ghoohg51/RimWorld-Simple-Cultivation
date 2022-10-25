using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace SimpleCultivation
{
    public class Hediff_CoreFormation : HediffWithComps
    {
        private float progress;
        public override string Label => base.Label + " (" + progress.ToStringPercent() + ")";
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            progress = 0.01f;
        }

        public void AddProgress(float? progressValue = null, bool instantCheck = false)
        {
            progress += progressValue ?? 1f / (GenDate.TicksPerHour * 210f);
            if (progress >= 1f)
            {
                if (instantCheck is false)
                {
                    pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(SC_DefOf.SC_DeepMeditation));
                }
                else
                {
                    CheckCompleted();
                }
            }
        }

        public void CheckCancelled()
        {
            progress = 0.75f;
        }
        public void CheckCompleted()
        {
            float chance = Rand.Value;
            if (chance > 0.99f)
            {
                Utils.AssignCore(SC_DefOf.SC_CrystalGradeCore, pawn);
                pawn.health.RemoveHediff(this);
            }
            else if (chance > 0.75f)
            {
                Utils.AssignCore(SC_DefOf.SC_GoldGradeCore, pawn);
                pawn.health.RemoveHediff(this);
            }
            else if (chance > 0.5f)
            {
                Utils.AssignCore(SC_DefOf.SC_SilverGradeCore, pawn);
                pawn.health.RemoveHediff(this);
            }
            else if (chance > 0.25)
            {
                Utils.AssignCore(SC_DefOf.SC_BronzeGradeCore, pawn);
                pawn.health.RemoveHediff(this);
            }
            else
            {
                progress = 0.75f;
            }

            List<Hediff_Core> cores = pawn.health.hediffSet.hediffs.OfType<Hediff_Core>()
                .Where(x => x.hitpoints == Hediff_Core.MaxHitpoints).ToList();
            if (cores.Count == 7)
            {
                pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(SC_DefOf.SC_DeepMeditationChecks));
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref progress, "progress");
        }
    }
}
