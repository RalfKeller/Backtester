using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backtester
{
    public abstract class Strategy
    {
        public delegate void OnBuyHandler(long target, long stopLoss, long index);
        public event OnBuyHandler onBuy;

        public delegate void OnCloseAllHandler(long index);
        public event OnCloseAllHandler onCloseAll;

        protected double[] closingPrices;
        public Tick[] ticks;


        public virtual void setup(Tick[] ticks)
        {
            this.ticks = ticks;

            closingPrices = ticks.getOneMinuteClosingPrices();

            setupIndicators();
        }

        protected abstract void setupIndicators();

        public abstract void handleTick(int currentIndex);

        protected void triggerOnBuy(long target, long stopLoss, long index)
        {
            if(onBuy != null)
                onBuy(target, stopLoss, index);
        }

        protected void triggerCloseAll(long index)
        {
            if (onCloseAll != null)
                onCloseAll(index);
        }

    }
}
