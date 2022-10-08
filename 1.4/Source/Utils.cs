using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace SimpleCultivation
{
    [StaticConstructorOnStartup]
    public static class Utils
    {
        static Utils()
        {
            Startup();
        }

        public static void AssignCore(HediffDef core, Pawn pawn)
        {
            if (pawn.health.hediffSet.GetFirstHediffOfDef(core) is Hediff_Core hediff)
            {
                hediff.Severity++;
            }
            else
            {
                hediff = HediffMaker.MakeHediff(core, pawn, pawn.def.race.body.corePart) as Hediff_Core;
                hediff.Severity = 1;
                pawn.health.AddHediff(hediff, pawn.def.race.body.corePart);
            }
        }
        private static void Startup()
        {
            foreach (var biomeDef in DefDatabase<BiomeDef>.AllDefs)
            {
                var regularGrass = biomeDef.wildPlants.FirstOrDefault(x => x.plant == ThingDefOf.Plant_Grass);
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

            foreach (var thingDef in DefDatabase<ThingDef>.AllDefs)
            {
                if (thingDef.IsMeleeWeapon)
                {
                    var stuff = GenStuff.DefaultStuffFor(thingDef);
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
                    if (meleeDpsGetter(thingDef) > 4.5f)
                    {
                        thingDef.comps ??= new List<CompProperties>();
                        var comp = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
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
                                    value = 0.0f
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
