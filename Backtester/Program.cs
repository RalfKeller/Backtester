using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backtester
{
    public class Program
    {
        private const int SPREAD = 2;

        private static List<Position> openPositions;
        private static Tick[] ticks;

        private static long pipCounter;
        private static long transactionCounter;
        private static long durationCounter;

        private static long loosingTransactions;
        private static long winningTransactions;

        private static long tradesOnSunday;

        static void Main(string[] args)
        {
            testCowabunga(@"C:\Users\Ralf\Desktop\talib_test\DAT_ASCII_EURGBP_M1_2015.csv");
            //testCowabunga(@"C:\Users\Ralf\Desktop\talib_test")
            //testCowabunga(@"")
        }

        public static CowabungaStrategy testCowabunga(string csvPath)
        {
            pipCounter = 0;
            transactionCounter = 0;
            durationCounter = 0;
            tradesOnSunday = 0;
            openPositions = new List<Position>();

            CowabungaStrategy cowabunga = new CowabungaStrategy();
            cowabunga.onBuy += Cowabunga_onBuy;
            cowabunga.onCloseAll += Cowabunga_onCloseAll;

            ticks = Parser.getTicks(csvPath);

            cowabunga.setup(ticks);

            for (int i = 0; i < ticks.Length; i++)
            {
                if(i % 15 == 0)
                    cowabunga.handleTick(i);
                handleOpenPositions(i);
            }

            Console.WriteLine("Pip Counter:\t" + pipCounter);
            Console.WriteLine("TransactionCounter:\t" + transactionCounter);
            Console.WriteLine("Trades On Sunday:\t" + tradesOnSunday);
            Console.WriteLine("Average Duration:\t" + durationCounter / transactionCounter);
            Console.WriteLine("Winning:\t" + winningTransactions);
            Console.WriteLine("Loosing:\t" + loosingTransactions);

            return cowabunga;
        }

        private static void Cowabunga_onCloseAll(long index)
        {
            while (openPositions.Count != 0)
            {
                Position position = openPositions[0];
                    long diff = (ticks[index].closePrice.toPips() - position.OpeningCourse.toPips() );
                    pipCounter -= diff + SPREAD;
                    durationCounter += position.duration;
                    //Console.WriteLine("Stop Loss!");

                    openPositions.RemoveAt(0);
                
            }
        }

        private static void handleOpenPositions(int currentIndex)
        {
            for (int i = 0; i < openPositions.Count; i++)
            {
                Position position = openPositions[i];

                if (position.creationIndex < currentIndex)
                {
                    position.duration++;
                    if (position.type == Position.PositionType.Long)
                    {
                        if (position.trailingStop)
                        {
                            if ((position.stopLoss < (ticks[currentIndex].closePrice.toPips() - position.trailingStopPips)) && ticks[currentIndex].closePrice > position.OpeningCourse)
                            {
                                position.stopLoss = ticks[currentIndex].closePrice.toPips() - position.trailingStopPips;
                            }
                        }

                        if (position.target <= ticks[currentIndex].highPrice.toPips())
                        {
                            //long wonPips = position.target - position.OpeningCourse.toPips() - SPREAD;
                            //pipCounter += wonPips;
                            //durationCounter += position.duration;

                            //winningTransactions++;

                            ////Console.WriteLine("Success! Won pips:\t" + wonPips);
                            //openPositions.RemoveAt(i);
                            //i--;
                            //continue;
                        }

                        if (position.stopLoss > ticks[currentIndex].closePrice.toPips())
                        {
                            long diff = (position.OpeningCourse.toPips() - position.stopLoss);
                            pipCounter -= diff  + SPREAD;
                            durationCounter += position.duration;

                            if (diff < 0)
                                loosingTransactions++;

                            else
                                winningTransactions++;
                            //Console.WriteLine("Stop Loss!");
                            
                            openPositions.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }
                }
            }
        }

        private static void Cowabunga_onBuy(long target, long stopLoss, long index)
        {
            Position pos = new Position()
            {
                creationIndex = index,
                OpeningCourse = ticks[index].closePrice,
                OpeningTimeStamp = ticks[index].Timestamp,
                stopLoss = stopLoss,
                target = target,
                type = Position.PositionType.Long,
                trailingStop = true,
                trailingStopPips = 30
            };

            DateTime date = DateTime.ParseExact(ticks[index].Timestamp, "yyyyMMdd HHmmss", CultureInfo.InvariantCulture);
            if (date.DayOfWeek == DayOfWeek.Sunday)
                tradesOnSunday++;

            openPositions.Add(pos);
            transactionCounter++;
        }


    }
}
