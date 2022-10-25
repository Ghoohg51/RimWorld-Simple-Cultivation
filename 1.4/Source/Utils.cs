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
                hediff.resource += value;
            }
            else
            {
                hediff = HediffMaker.MakeHediff(SC_DefOf.SC_QiResource, pawn, pawn.def.race.body.corePart) as Hediff_Qi;
                hediff.resource = value;
                pawn.health.AddHediff(hediff);
            }
            hediff.resource = Mathf.Min(hediff.resource, pawn.GetStatValue(SC_DefOf.SC_MaxQi));
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
