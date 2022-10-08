using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SimpleCultivation
{
    [HarmonyPatch(typeof(Plant), "PlantCollected")]
    public static class Plant_PlantCollected_Patch
    {
        public static void Prefix(Plant __instance, Pawn by)
        {
            if (__instance.def == SC_DefOf.SC_SpiritGrass)
            {
                int num = by.skills.GetSkill(SkillDefOf.Plants).Level;
                if (num > 0)
                {
                    HarvestPlant(__instance, by, num);
                }
            }
            else if (__instance.def == SC_DefOf.SC_RareSpiritGrass)
            {
                int num = Mathf.CeilToInt(by.skills.GetSkill(SkillDefOf.Plants).Level / 2f);
                if (num > 0)
                {
                    HarvestPlant(__instance, by, num);
                }
            }
        }

        private static void HarvestPlant(Plant __instance, Pawn by, int num)
        {
            var thing = ThingMaker.MakeThing(__instance.def.plant.harvestedThingDef);
            thing.stackCount = num;
            if (by.Faction != Faction.OfPlayer)
            {
                thing.SetForbidden(value: true);
            }
            Find.QuestManager.Notify_PlantHarvested(by, thing);
            GenPlace.TryPlaceThing(thing, by.Position, __instance.Map, ThingPlaceMode.Near);
            by.records.Increment(RecordDefOf.PlantsHarvested);
        }
    }
}
