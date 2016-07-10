using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacTec.TA.Library;

namespace Backtester
{
    public class CowabungaStrategy : Strategy
    {

        public double[] macdHisogrammOneMinute;
        public double[] ema75;
        public double[] ema150;
        public double[] ema1200;
        public double[] ema2400;
        public double[] stoch;
        public double[] rsi;

        private bool initialized;
        private bool ema5AboveEma10;

        public override void handleTick(int currentIndex)
        {
            if (currentIndex > 2400) //Vor Minute 2400 sind nicht alle Indikatoren berechnet
            {
                if (!initialized) //Erste Minute, EMA 5 und EMA 10 müssen haben sich noch nicht gekreuzt, sind aber über / untereinander
                {
                    ema5AboveEma10 = ema75[currentIndex] > ema150[currentIndex];
                    initialized = true;
                    return;
                }

                if((ema75[currentIndex] > ema150[currentIndex]) && !ema5AboveEma10) //EMA5 ist über EMA10 getreten
                {
                    ema5AboveEma10 = true;

                    if (rsi[currentIndex] < 50) //RSI ist nicht über 50, nicht kaufen
                        return;

                    if (ema1200[currentIndex] < ema2400[currentIndex]) //EMA 1200 ist nicht über EMA 2400, nicht kaufen
                        return;

                    //if ((macdHisogrammOneMinute[currentIndex - 1] > macdHisogrammOneMinute[currentIndex])) //MACD - Histogramm steigt nicht, nicht kaufen
                    //    return;

                    if (stoch[currentIndex] > 80) //Überkauft, nicht kaufen
                        return;

                    //if (stoch[currentIndex - 1] > stoch[currentIndex]) //Stoch sinkt, nicht kaufen
                    //    return;

                    long closingInPips = closingPrices[currentIndex].toPips();
                    long closingInPipsShortend = (long)(closingInPips / 100) * 100;
                    long targetInPips;

                    if (closingInPips % 100 < 30)
                        targetInPips = closingInPipsShortend + 50;

                    else
                        targetInPips = closingInPipsShortend + 80;

                    triggerOnBuy(targetInPips, closingInPips - 100, currentIndex);

                    Console.WriteLine("Buy " + ticks[currentIndex].Timestamp);
                }

                if (ema75[currentIndex] < ema150[currentIndex] && ema5AboveEma10)
                {
                    ema5AboveEma10 = false;
                    //triggerCloseAll(currentIndex);
                }
            }
        }

        protected override void setupIndicators()
        {
            Core.RetCode ret;
            int outBegIndex;
            int outNBElement;

            macdHisogrammOneMinute = new double[closingPrices.Length];
            ema75 = new double[closingPrices.Length];
            ema150 = new double[closingPrices.Length];
            ema1200 = new double[closingPrices.Length];
            ema2400 = new double[closingPrices.Length];
            rsi = new double[closingPrices.Length];
            stoch = new double[closingPrices.Length];

            double[] macdHisogrammOneMinutetemp     = new double[closingPrices.Length];
            double[] ema75temp                      = new double[closingPrices.Length];
            double[] ema150temp                     = new double[closingPrices.Length];
            double[] ema1200temp                    = new double[closingPrices.Length];
            double[] ema2400temp                    = new double[closingPrices.Length];
            double[] rsitemp                        = new double[closingPrices.Length];
            double[] stochtemp                      = new double[closingPrices.Length];

            double[] outMacd = new double[closingPrices.Length];
            double[] outSignal = new double[closingPrices.Length];

            ret = Core.Macd(0, closingPrices.Length - 1, closingPrices, 180, 390, 135, out outBegIndex, out outNBElement, outMacd, outSignal, macdHisogrammOneMinute);

            ret = Core.Ema(0, closingPrices.Length - 1, closingPrices, 75, out outBegIndex, out outNBElement, ema75temp);
            ret = Core.Ema(0, closingPrices.Length - 1, closingPrices, 150, out outBegIndex, out outNBElement, ema150temp);
            ret = Core.Ema(0, closingPrices.Length - 1, closingPrices, 1200, out outBegIndex, out outNBElement, ema1200temp);
            ret = Core.Ema(0, closingPrices.Length - 1, closingPrices, 2400, out outBegIndex, out outNBElement, ema2400temp);

            ret = Core.Rsi(0, closingPrices.Length - 1, closingPrices, 135, out outBegIndex, out outNBElement, rsitemp);

            
            double[] outD = new double[closingPrices.Length];
            ret = Core.Stoch(0, closingPrices.Length - 1, ticks.getOneMinuteHighPrices(), ticks.getOneMinuteLowPrices(), closingPrices, 150, 45, Core.MAType.Sma, 3, Core.MAType.Sma, out outBegIndex, out outNBElement, stochtemp, outD);


            Array.Copy(ema75temp, 0, ema75, 74, ema75.Length - 74);
            Array.Copy(ema150temp, 0, ema150, 149, ema150.Length - 149);
            Array.Copy(ema1200temp, 0, ema1200, 1199, ema1200.Length - 1199);
            Array.Copy(ema2400temp, 0, ema2400, 2399, ema2400.Length - 2399);

            Array.Copy(rsitemp, 0, rsi, 134, rsi.Length - 134); //370473
            Array.Copy(stochtemp, 0, stoch, 195, rsi.Length - 195);

            //File.Create(@"C:\Users\Ralf\Desktop\Tests\Indicators.txt");
            using (StreamWriter writer = new StreamWriter(@"C:\Users\Ralf\Desktop\Tests\Indicators.txt"))
            {
                for (int i = 0; i < ema75.Length; i++)
                {
                    writer.WriteLine(i + ": " + stoch[i]);
                }
            }
        }
    }
}
