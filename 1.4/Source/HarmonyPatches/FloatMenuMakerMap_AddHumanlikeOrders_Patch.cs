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
            var c = IntVec3.FromVector3(clickPos);
            var thingList = c.GetThingList(pawn.Map);
            for (int i = 0; i < thingList.Count; i++)
            {
                var t = thingList[i];
                if (t.def == SC_DefOf.SC_CoreIgnitionPill)
                {
                    string text = (!t.def.ingestible.ingestCommandString.NullOrEmpty()) ? string.Format(t.def.ingestible.ingestCommandString, t.LabelShort) : ((string)"ConsumeThing".Translate(t.LabelShort, t));
                    var floatMenuOption = opts.FirstOrDefault((FloatMenuOption x) => x.Label.Contains(text));
                    if (floatMenuOption != null)
                    {
                        if (pawn.health.hediffSet.hediffs.Any(x => x is Hediff_CoreFormation))
                        {
                            floatMenuOption.Label += ": " + "SC.AlreadyHasCoreFormation".Translate();
                            floatMenuOption.action = null;
                        }
                        if (pawn.health.hediffSet.hediffs.Sum(x => x.Severity) >= 7)
                        {
                            floatMenuOption.Label += ": " + "SC.MaxCores".Translate();
                            floatMenuOption.action = null;
                        }
                    }
                }
            }
        }
    }
}
