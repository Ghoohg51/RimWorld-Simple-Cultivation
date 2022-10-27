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
        public bool PassedChecks => currentCheckStage == 7;
        public Hediff_Qi Hediff => pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_QiResource) as Hediff_Qi;
        public bool PerformingChecks => pawn.CurJobDef == SC_DefOf.SC_DeepMeditationChecks;

        public bool CanPerformChecks
        {
            get
            {
                if (PassedChecks is false && pawn.health.hediffSet.hediffs.OfType<Hediff_Core>()
                    .Where(x => x.hitpoints == Hediff_Core.MaxHitpoints).Count() == 7)
                {
                    return true;
                }
                return false;
            }
        }

        public BodyPartRecord partBeingAssigned;
        public Hediff_Core coreBeingMoved;
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

                if (CanPerformChecks && PerformingChecks is false) 
                {
                    pawn.StartChecksJob();
                }
            }
        }

        public void CheckCompleted()
        {
            if (Rand.Chance(0.65f) && SimpleCultivationSettings.devMode is false)
            {
                CheckFailed();
            }
            else
            {
                currentCheckStage++;
                if (CanPerformChecks)
                {
                    pawn.StartChecksJob();
                }
            }
        }



        public void CheckFailed()
        {
            currentCheckStage = 0;
            var core = pawn.health.hediffSet.hediffs.OfType<Hediff_Core>().RandomElement();
            core.ShatterCore(10);
            if (core.ShatteredFully)
            {
                var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_CoreAlignmentDrain);
                if (hediff != null)
                {
                    pawn.health.RemoveHediff(hediff);
                }
                SC_DefOf.Catatonic.Worker.TryStart(pawn, "SC.ShatteredCore".Translate(), false);
            }
            else
            {
                var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_CoreAlignmentDrain);
                if (hediff is null)
                {
                    hediff = HediffMaker.MakeHediff(SC_DefOf.SC_CoreAlignmentDrain, pawn);
                    hediff.Severity = 1;
                    pawn.health.AddHediff(hediff);
                }
                else
                {
                    hediff.Severity += 1;
                }
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
            Scribe_References.Look(ref coreBeingMoved, "coreBeingMoved");
            Scribe_BodyParts.Look(ref partBeingAssigned, "partBeingAssigned");

        }
    }
}
