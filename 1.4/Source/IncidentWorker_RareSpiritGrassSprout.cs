using RimWorld;
using Verse;

namespace SimpleCultivation
{
    public class IncidentWorker_RareSpiritGrassSprout : IncidentWorker
    {
        private static readonly IntRange CountRange = new IntRange(10, 20);
        public override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
            {
                return false;
            }
            var map = (Map)parms.target;
            if (!map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNow)
            {
                return false;
            }
            return TryFindRootCell(map, out _);
        }

        public override bool TryExecuteWorker(IncidentParms parms)
        {
            var map = (Map)parms.target;
            if (!TryFindRootCell(map, out var cell))
            {
                return false;
            }
            Thing thing = null;
            int randomInRange = CountRange.RandomInRange;
            for (int i = 0; i < randomInRange; i++)
            {
                if (!CellFinder.TryRandomClosewalkCellNear(cell, map, 6, out var result, (IntVec3 x) => CanSpawnAt(x, map)))
                {
                    break;
                }
                result.GetPlant(map)?.Destroy();
                var thing2 = GenSpawn.Spawn(SC_DefOf.SC_RareSpiritGrass, result, map);
                if (thing == null)
                {
                    thing = thing2;
                }
            }
            if (thing == null)
            {
                return false;
            }
            SendStandardLetter(parms, thing);
            return true;
        }

        private bool TryFindRootCell(Map map, out IntVec3 cell)
        {
            return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => CanSpawnAt(x, map)
            && x.GetRoom(map).CellCount >= 64, map, out cell);
        }

        private bool CanSpawnAt(IntVec3 c, Map map)
        {
            if (!c.Standable(map) || c.Fogged(map) || map.fertilityGrid.FertilityAt(c) < SC_DefOf.SC_RareSpiritGrass.plant.fertilityMin
                || !c.GetRoom(map).PsychologicallyOutdoors || c.GetEdifice(map) != null || !PlantUtility.GrowthSeasonNow(c, map))
            {
                return false;
            }
            var plant = c.GetPlant(map);
            if (plant != null && plant.def.plant.growDays > 10f)
            {
                return false;
            }
            var thingList = c.GetThingList(map);
            for (int i = 0; i < thingList.Count; i++)
            {
                if (thingList[i].def == SC_DefOf.SC_RareSpiritGrass)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
