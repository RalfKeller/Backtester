using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backtester
{
    public struct Tick
    {
        public string Timestamp;
        public double openingPrice;
        public double highPrice;
        public double lowPrice;
        public double closePrice;
        public double Volume;
    }
}
