using System.Collections.Generic;
using RimWorld;
using Verse;
using HarmonyLib;

namespace PreceptsPlusPlus {

    [HarmonyPatch(typeof(RitualBehaviorWorker_Speech), "PostExecute")]
    public static class OralTraditionMadePatch {
        public static void Postfix() {
            //Whenever a revelation is triggered, it gets if you actually let the speech finsih, if true then it saves the tick completed.
            Preceptspp.LastTickOralTrad = Find.TickManager.TicksGame;
        }
    }
    public class ThoughtWorker_Precept_OralTradition : ThoughtWorker_Precept, IPreceptCompDescriptionArgs {
        private float DaysSinceLastRevelation {
            //Gets the last tick you traded and divides it into days.
            get {
                return (float)(Find.TickManager.TicksGame - Preceptspp.LastTickOralTrad) / 60000f;
            }
        }
        //This is for the interface, purely aesthetic. 
        public IEnumerable<NamedArgument> GetDescriptionArgs() {
            yield return this.DaysSinceLastSpokenThreshold.Named("DAYSSINCELASTSPOKENTHRESHOLD");
            yield break;
        }

        protected override ThoughtState ShouldHaveThought(Pawn p) {
            if (!p.IsColonist) {
                //only for colonists
                return false;
            }
            if (p.IsSlave) {
                //Slaves cannot get it
                return false;
            }
            //get the last day you had a successful revelation. If it's more than the threshold it activates the thought at stage 1 (negative mood in this case).
            //If it's less it activates it at stage 0.
            int num = (this.DaysSinceLastRevelation > (float)this.DaysSinceLastSpokenThreshold) ? 1 : 0;
            if (num == 1 && (float)Find.TickManager.TicksGame < 600000f) 
                //Gives you 10 days before the negative mood can become active
            {
                return false;
            }
            return ThoughtState.ActiveAtStage(num);
        }

        public override float MoodMultiplier(Pawn p) //Gets the mood multiplier from a curve. Using the day since the last trade as a parameter.
        {
            return ThoughtWorker_Precept_OralTradition.MoodMultiplierCurve.Evaluate(this.DaysSinceLastRevelation);
        }

        //This curve is relatively simple. When days since last trade are 0, the multipier is x1. It then gets reduced linealy to 0 by the fifth day.
        //Once you reach the sixth day, the ShouldHaveThought metod makes the thought stage change, and in this case it now becomes a negative thought.
        //We slowly increase the multiplier linealy, until it's back a x1 at 10 days without trade.
        //After this, it increases linealy to x1.5 in the next five days, and then stops.
        private static readonly SimpleCurve MoodMultiplierCurve = new SimpleCurve
        {
            {
                new CurvePoint(0f, 1f),
                true
            },
            {
                new CurvePoint(Settings.dayForSpokenThreshold, 0f),
                true
            },
            {
                new CurvePoint(Settings.dayForSpokenThreshold*2, 1f),
                true
            },
            {
                new CurvePoint(Settings.dayForSpokenThreshold*3, 1.5f),
                true
            }
        };

        //Threshold of days from last trade in which the thought will be a possitive one.
        private int DaysSinceLastSpokenThreshold = Settings.dayForSpokenThreshold;
    }
   
}
