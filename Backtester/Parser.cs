using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Backtester
{
    class Parser
    {
        public static Tick[] getTicks(string csvPath)
        {
            List<Tick> ticks = new List<Tick>();
            using (StreamReader stream = new StreamReader(csvPath))
            {
                while (!stream.EndOfStream)
                {
                    string currentLine = stream.ReadLine();

                    string[] data = currentLine.Split(new char[] { ';' });

                    Tick currentTick = new Tick()
                    {
                        Timestamp = data[0],
                        openingPrice = Convert.ToDouble(data[1], CultureInfo.InvariantCulture),
                        highPrice = Convert.ToDouble(data[2], CultureInfo.InvariantCulture),
                        lowPrice = Convert.ToDouble(data[3], CultureInfo.InvariantCulture),
                        closePrice = Convert.ToDouble(data[4], CultureInfo.InvariantCulture),
                        Volume = Convert.ToDouble(data[5], CultureInfo.InvariantCulture)
                    };

                    ticks.Add(currentTick);
                }
            }

            return ticks.ToArray();
        }
    }
}
