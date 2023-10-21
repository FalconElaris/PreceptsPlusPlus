using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace PreceptsPlusPlus {
    public class Settings : ModSettings {

        public static int dayForSpokenThreshold = 5;

        public override void ExposeData() {
            Scribe_Values.Look(ref dayForSpokenThreshold, "DaysForTradeThershold", 5);
            base.ExposeData();
        }
    }
}
