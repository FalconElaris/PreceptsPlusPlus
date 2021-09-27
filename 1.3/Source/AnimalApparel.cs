using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;
using UnityEngine;

namespace PreceptsPlusPlus {

	public class ThoughtWorker_Precept_AnimalLeatherApparel : ThoughtWorker_Precept {
		protected override ThoughtState ShouldHaveThought(Pawn p) {
			return ThoughtWorker_AnimalLeatherApparel.CurrentThoughtState(p);
		}
	}

	public class ThoughtWorker_AnimalLeatherApparel : ThoughtWorker {
		public static ThoughtState CurrentThoughtState(Pawn p) {
			string text = null;
			int num = 0;

			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++) {
				if (ThoughtWorker_AnimalLeatherApparel.ShouldPawnReactToLeatherType(wornApparel[i].Stuff)) {
					if (ThingCategoryDefOf.Leathers.ContainedInThisOrDescendant(wornApparel[i].Stuff) && wornApparel[i].Stuff != ThingDefOf.Human.race.leatherDef) {
						if (text == null) {
							text = wornApparel[i].def.label;
						}
						num++;
					}
				}
			}
			if (num == 0) {
				return ThoughtState.Inactive;
			}
			if (num >= 5) {
				return ThoughtState.ActiveAtStage(4, text);
			}
			return ThoughtState.ActiveAtStage(num - 1, text);
		}
		protected override ThoughtState CurrentStateInternal(Pawn p) {
			return ThoughtWorker_AnimalLeatherApparel.CurrentThoughtState(p);
		}
		protected static bool ShouldPawnReactToLeatherType(ThingDef stuff) {
			if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId == "sirmashedpotato.escp.dunmer" || x.Name == "ESCP - Dunmer")) {
				if (stuff == DefDatabase<ThingDef>.GetNamedSilentFail("ESCP_LeatherDunmer")) {
					//Log.Message("MorrowRim - Dunmer Race detected");
					return false;
				}
			}
			if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId == "sirmashedpotato.escp.altmer" || x.Name == "ESCP - Altmer")) {
				if (stuff == DefDatabase<ThingDef>.GetNamedSilentFail("ESCP_LeatherAltmer")) {
					//Log.Message("MorrowRim - Altmer Race detected");
					return false;
				}
			}
			if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId == "sirmashedpotato.escp.bosmer" || x.Name == "ESCP - Bosmer")) {
				if (stuff == DefDatabase<ThingDef>.GetNamedSilentFail("ESCP_LeatherBosmer")) {
					//Log.Message("MorrowRim - Bosmer Race detected");
					return false;
				}
			}
			if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId == "sirmashedpotato.escp.orsimer" || x.Name == "ESCP - Orsimer")) {
				if (stuff == DefDatabase<ThingDef>.GetNamedSilentFail("ESCP_LeatherOrsimer")) {
					//Log.Message("MorrowRim - Orsimer Race detected");
					return false;
				}
			}
			if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId == "sirmashedpotato.escp.falmer" || x.Name == "ESCP - Falmer")) {
				if (stuff == DefDatabase<ThingDef>.GetNamedSilentFail("ESCP_LeatherFalmer")) {
					//Log.Message("MorrowRim - Falmer Race detected");
					return false;
				}
			}
			return true;
		}

	}
}
