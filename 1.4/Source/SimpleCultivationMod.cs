using HarmonyLib;
using UnityEngine;
using Verse;

namespace SimpleCultivation
{
    public class SimpleCultivationMod : Mod
    {
        public static SimpleCultivationSettings settings;
        public SimpleCultivationMod(ModContentPack pack) : base(pack)
        {
            settings = GetSettings<SimpleCultivationSettings>();
            new Harmony("SimpleCultivation.Mod").PatchAll();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return this.Content.Name;
        }
    }

    public class SimpleCultivationSettings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
        }
        public void DoSettingsWindowContents(Rect inRect)
        {
            Rect rect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height);
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(rect);

            listingStandard.End();
        }
    }
}
