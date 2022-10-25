using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace SimpleCultivation
{
    public class CompProperties_Qi : CompProperties
    {
        public CompProperties_Qi()
        {
            compClass = typeof(CompQi);
        }
    }
    public class CompQi : ThingComp
    {
        public Pawn pawn => parent as Pawn;

        public int currentCheckStage;
        public Hediff_Qi Hediff => pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_QiResource) as Hediff_Qi;
        public bool PerformingChecks => pawn.CurJobDef == SC_DefOf.SC_DeepMeditationChecks;
        public override void CompTick()
        {
            base.CompTick();
            if (parent.IsHashIntervalTick(60))
            {
                float qiOffset = parent.GetStatValue(SC_DefOf.SC_QiRegenRate);
                if (qiOffset != 0)
                {
                    Utils.OffsetQi(qiOffset, parent as Pawn);
                }
            }
        }

        public void CheckCompleted()
        {
            currentCheckStage++;
            if (currentCheckStage == 6)
            {
                Log.Message("Passed all checks");
            }
            else
            {
                pawn.jobs.StopAll();
                pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(SC_DefOf.SC_DeepMeditationChecks));
            }
        }

        public void CheckFailed()
        {
            currentCheckStage = 0;
            if (pawn.health.hediffSet.hediffs.OfType<Hediff_Core>().TryRandomElement(out Hediff_Core core))
            {
                core.ShatterCore(10);
            }
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }
            foreach (Gizmo gizmo in GetGizmos())
            {
                yield return gizmo;
            }
        }

        private IEnumerable<Gizmo> GetGizmos()
        {
            if ((parent.Faction == Faction.OfPlayer) && Find.Selector.SingleSelectedThing == parent && Hediff != null)
            {
                Gizmo_QiStatus gizmo_QiStatus = new()
                {
                    compQi = this
                };
                yield return gizmo_QiStatus;
            }
            if (Prefs.DevMode)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Add core progress",
                    action = delegate
                    {
                        if (pawn.health.hediffSet.hediffs.OfType<Hediff_CoreFormation>().TryRandomElement(out Hediff_CoreFormation core))
                        {
                            core.AddProgress(0.1f, instantCheck: true);
                        }
                    }
                };
                yield return new Command_Action
                {
                    defaultLabel = "Shatter random core",
                    action = delegate
                    {
                        if (pawn.health.hediffSet.hediffs.OfType<Hediff_Core>().TryRandomElement(out Hediff_Core core))
                        {
                            core.ShatterCore(10);
                        }
                    }
                };
                yield return new Command_Action
                {
                    defaultLabel = "Heal random core",
                    action = delegate
                    {
                        if (pawn.health.hediffSet.hediffs.OfType<Hediff_Core>().Where(x => x.CanHeal)
                            .TryRandomElement(out Hediff_Core core))
                        {
                            core.HealCore(10);
                        }
                    }
                };
            }
        }


        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref currentCheckStage, "currentCheckStage");
        }
    }
}
