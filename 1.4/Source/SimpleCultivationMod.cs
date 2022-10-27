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
        public static bool devMode;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref devMode, "devMode");
        }
        public void DoSettingsWindowContents(Rect inRect)
        {
            Rect rect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height);
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(rect);
            listingStandard.CheckboxLabeled("Enable Simple Cultivation dev mode", ref devMode);
            listingStandard.End();
        }
    }
}
