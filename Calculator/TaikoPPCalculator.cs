﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuRTDataProvider.Listen;
using OsuRTDataProvider.Mods;
using RealTimePPDisplayer.Displayer;
using RealTimePPDisplayer.PP;

namespace RealTimePPDisplayer.Calculator
{
    class TaikoPPCalculator : PPCalculatorBase
    {
        private const OsuPlayMode s_mode = OsuPlayMode.Taiko;

        public override PPTuple GetPP(ModsInfo mods)
        {
            if (Beatmap == null) return PPTuple.Empty;

            PPTuple pp_tuple;
            var result = Beatmap.GetMaxPP(mods, s_mode);
            pp_tuple.MaxPP = result.total;
            pp_tuple.MaxAimPP = result.aim;
            pp_tuple.MaxSpeedPP = result.speed;
            pp_tuple.MaxAccuracyPP = result.acc;

            double acc = Oppai.taiko_acc_calc(Count300, Count100, CountMiss) * 100.0;
            Oppai.taiko_acc_round(acc, Beatmap.ObjectCount, CountMiss, out int n300, out int n150);

            result = Beatmap.GetIfFcPP(mods, n300, n150,0, s_mode);
            pp_tuple.FullComboPP = result.total;
            pp_tuple.FullComboAimPP = result.aim;
            pp_tuple.FullComboSpeedPP = result.speed;
            pp_tuple.FullComboAccuracyPP = result.acc;

            result = Beatmap.GetRealTimePP(Time, mods, Count100, Count50, CountMiss, MaxCombo, s_mode);
            pp_tuple.RealTimePP = result.total;
            pp_tuple.RealTimeAimPP = result.aim;
            pp_tuple.RealTimeSpeedPP = result.speed;
            pp_tuple.RealTimeAccuracyPP = result.acc;

            return pp_tuple;
        }
    }
}
