using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace SimpleCultivation
{
    [StaticConstructorOnStartup]
    public static class Utils
    {
        static Utils()
        {
            AssignDefs();
        }

        public static void AssignCore(HediffDef core, Pawn pawn)
        {
            Hediff_Core hediff = HediffMaker.MakeHediff(core, pawn, pawn.def.race.body.corePart) as Hediff_Core;
            pawn.health.AddHediff(hediff, pawn.def.race.body.corePart);
        }

        public static void OffsetQi(float value, Pawn pawn)
        {
            if (pawn.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_QiResource) is Hediff_Qi hediff)
            {
                hediff.Resource += value;
            }
            else if (value > 0)
            {
                hediff = HediffMaker.MakeHediff(SC_DefOf.SC_QiResource, pawn, pawn.def.race.body.corePart) as Hediff_Qi;
                hediff.Resource = value;
                pawn.health.AddHediff(hediff);
            }
        }

        public static Hediff_Core GetCoreFor(this Pawn pawn, BodyPartDef def)
        {
            return pawn.health.hediffSet.hediffs.OfType<Hediff_Core>().Where(x => x.part?.def == def).FirstOrDefault();
        }
        public static List<BodyPartRecord> AvailableBodyPartsForCore(this Pawn pawn)
        {
            List<BodyPartRecord> list = new List<BodyPartRecord>();
            list.AddRange(pawn.health.hediffSet.GetNotMissingParts().Where(x => x.IsInGroup(SC_DefOf.Arms))
                .GroupBy(x => x.depth).First());
            list.AddRange(pawn.health.hediffSet.GetNotMissingParts().Where(x => x.IsInGroup(BodyPartGroupDefOf.Legs))
                .GroupBy(x => x.depth).First());
            list.Add(pawn.health.hediffSet.GetNotMissingParts().Where(x => x.IsInGroup(SC_DefOf.HeadAttackTool))
                .GroupBy(x => x.depth).First().First());
            list.Add(pawn.health.hediffSet.GetNotMissingParts().Where(x => x.def == BodyPartDefOf.Heart)
                .GroupBy(x => x.depth).First().First());
            list.Add(pawn.health.hediffSet.GetNotMissingParts().Where(x => x.def == BodyPartDefOf.Stomach)
                .GroupBy(x => x.depth).First().First());
            var cores = pawn.health.hediffSet.hediffs.OfType<Hediff_Core>();
            var filteredList = list.Where(x => cores.Any(y => y.Part == x) is false).ToList();
            return filteredList;
        }

        public static void StartChecksJob(this Pawn pawn)
        {
            pawn.jobs.StopAll();
            if (pawn.Drafted)
            {
                pawn.drafter.Drafted = false;
            }
            pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(SC_DefOf.SC_DeepMeditationChecks));
        }
        private static void AssignDefs()
        {
            foreach (BiomeDef biomeDef in DefDatabase<BiomeDef>.AllDefs)
            {
                BiomePlantRecord regularGrass = biomeDef.wildPlants.FirstOrDefault(x => x.plant == ThingDefOf.Plant_Grass);
                if (regularGrass != null)
                {
                    biomeDef.wildPlants.Add(new BiomePlantRecord
                    {
                        plant = SC_DefOf.SC_SpiritGrass,
                        commonality = regularGrass.commonality,
                    });
                }
                else
                {
                    biomeDef.wildPlants.Add(new BiomePlantRecord
                    {
                        plant = SC_DefOf.SC_SpiritGrass,
                        commonality = biomeDef.wildPlants.Any() ? biomeDef.wildPlants.Average(x => x.commonality) : 1,
                    });
                }

                biomeDef.wildPlants.Add(new BiomePlantRecord
                {
                    plant = SC_DefOf.SC_RareSpiritGrass,
                    commonality = biomeDef.wildPlants.Any() ? biomeDef.wildPlants.Average(x => x.commonality) / 10f : 0.01f,
                });
            }

            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
            {
                if (thingDef.race?.Humanlike ?? false)
                {
                    thingDef.comps.Add(new CompProperties_Qi());
                }
                else if (thingDef.IsMeleeWeapon)
                {
                    ThingDef stuff = GenStuff.DefaultStuffFor(thingDef);
                    float meleeCooldownGetter(ThingDef d)
                    {
                        return d.tools.Average(x => x.AdjustedCooldown(thingDef, stuff));
                    }
                    float meleeDamageGetter(ThingDef d)
                    {
                        return d.tools.Average(x => x.power);
                    }
                    float meleeDpsGetter(ThingDef d)
                    {
                        return meleeDamageGetter(d) * 0.82f / meleeCooldownGetter(d);
                    }
                    float meleeDps = meleeDpsGetter(thingDef);
                    if (meleeDps > 4.5f)
                    {
                        thingDef.comps ??= new List<CompProperties>();
                        CompProperties_MeditationFocus comp = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
                        if (comp is null)
                        {
                            comp = new CompProperties_MeditationFocus
                            {
                                statDef = StatDefOf.MeditationFocusStrength,
                                focusTypes = new List<MeditationFocusDef> { SC_DefOf.SC_Martial },
                            };
                            thingDef.comps.Add(comp);
                            thingDef.statBases ??= new List<StatModifier>();
                            if (thingDef.statBases.Any(x => x.stat == StatDefOf.MeditationFocusStrength) is false)
                            {
                                thingDef.statBases.Add(new StatModifier
                                {
                                    stat = StatDefOf.MeditationFocusStrength,
                                    value = Mathf.CeilToInt(meleeDps)
                                });
                            }
                        }
                        else
                        {
                            comp.focusTypes.Add(SC_DefOf.SC_Martial);
                        }
                    }
                }
            }
        }
    }
}
