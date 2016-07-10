using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backtester
{
    class Position
    {

        public enum PositionType
        {
            Long,
            Short
        }
        public long creationIndex;
        public string OpeningTimeStamp;
        public double OpeningCourse;
        public long target;
        public long stopLoss;
        public long duration;
        public bool trailingStop;
        public long trailingStopPips;

        public PositionType type;
    }
}
