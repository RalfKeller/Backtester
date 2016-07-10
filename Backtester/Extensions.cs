using Backtester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backtester
{
    static class Extensions
    {
        public static double[] getOneMinuteClosingPrices(this Tick[] ticks)
        {
            List<double> values = new List<double>();
            foreach (Tick tick in ticks)
            {
                values.Add(tick.closePrice);
            }

            return values.ToArray();
        }

        public static double[] getOneMinuteLowPrices(this Tick[] ticks)
        {
            List<double> values = new List<double>();
            foreach (Tick tick in ticks)
            {
                values.Add(tick.lowPrice);
            }

            return values.ToArray();
        }

        public static double[] getOneMinuteHighPrices(this Tick[] ticks)
        {
            List<double> values = new List<double>();
            foreach (Tick tick in ticks)
            {
                values.Add(tick.highPrice);
            }

            return values.ToArray();
        }


        public static long toPips(this double price)
        {
            return (long)(price * 10000);
        }
    }
}
