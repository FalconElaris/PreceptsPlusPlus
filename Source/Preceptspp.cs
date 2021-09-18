using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;
using UnityEngine;

namespace PreceptsPlusPlus
{
    public class Preceptspp : Mod
    {
        PreceptsPlusPlus.Settings settings;
        
        public static int LastTickOralTrad = -9999999;
        public Preceptspp(ModContentPack pack) : base(pack) {
            this.settings = GetSettings<PreceptsPlusPlus.Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect) {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("Max days since last oral revelation precepts: " + Settings.dayForSpokenThreshold + " (After this number of days, it becomes a negative mood. Requires restart to apply)");
            listingStandard.IntAdjuster(ref Settings.dayForSpokenThreshold, 1, 1);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() {
            return "PreceptsPlusPlus";
        }
    }

    [StaticConstructorOnStartup]
    internal static class HarmonyInit {
        static HarmonyInit() {
            new Harmony("FalconElaris.PreceptsPlusPlus").PatchAll(); //Applies all harmony patches.
        }
    }



}