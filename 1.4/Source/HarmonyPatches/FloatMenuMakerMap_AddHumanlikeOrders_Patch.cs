using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace SimpleCultivation
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddHumanlikeOrders")]
    public static class FloatMenuMakerMap_AddHumanlikeOrders_Patch
    {
        public static void Postfix(Vector3 clickPos, Pawn pawn, ref List<FloatMenuOption> opts)
        {
            IntVec3 c = IntVec3.FromVector3(clickPos);
            List<Thing> thingList = c.GetThingList(pawn.Map);
            for (int i = 0; i < thingList.Count; i++)
            {
                Thing t = thingList[i];
                if (t.def == SC_DefOf.SC_CoreIgnitionPill)
                {
                    ProcessCoreIgnitionOptions(pawn, opts, t);
                }
                else if (t.def == ThingDefOf.MeditationSpot)
                {
                    var comp = pawn.GetComp<CompQi>();
                    if (comp != null && comp.PassedChecks)
                    {
                        if (pawn.health.hediffSet.hediffs.OfType<Hediff_Core>()
                            .Where(x => x.Moved is false).TryRandomElement(out var core))
                        {
                            var list = pawn.AvailableBodyPartsForCore();
                            opts.Add(new FloatMenuOption("SC.BeginCoreAlignmentStage".Translate(), delegate
                            {
                                comp.coreBeingMoved = core;
                                pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(SC_DefOf.SC_CoreAlignmentStage, t));
                            }));
                        }
                    }
                }
            }
        }

        private static void ProcessCoreIgnitionOptions(Pawn pawn, List<FloatMenuOption> opts, Thing t)
        {
            string text = (!t.def.ingestible.ingestCommandString.NullOrEmpty()) ? string.Format(t.def.ingestible.ingestCommandString, t.LabelShort) : ((string)"ConsumeThing".Translate(t.LabelShort, t));
            FloatMenuOption floatMenuOption = opts.FirstOrDefault((FloatMenuOption x) => x.Label.Contains(text));
            if (floatMenuOption != null)
            {
                if (pawn.health.hediffSet.hediffs.Any(x => x is Hediff_CoreFormation))
                {
                    floatMenuOption.Label += ": " + "SC.AlreadyHasCoreFormation".Translate();
                    floatMenuOption.action = null;
                }
                List<Hediff_Core> cores = pawn.health.hediffSet.hediffs.OfType<Hediff_Core>().ToList();
                if (cores.Any(x => x.ShatteredFully))
                {
                    floatMenuOption.Label += ": " + "SC.ShatteredCoreWarning".Translate();
                    floatMenuOption.action = null;
                }
                else if (pawn.health.hediffSet.hediffs.Count >= 7)
                {
                    floatMenuOption.Label += ": " + "SC.MaxCores".Translate();
                    floatMenuOption.action = null;
                }
            }
        }
    }
}
