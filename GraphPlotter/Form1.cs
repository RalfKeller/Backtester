using Backtester;
using GraphLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphPlotter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            initGraph();
        }

        private void initGraph()
        {
            CowabungaStrategy cowabunga = Backtester.Program.testCowabunga(@"C:\Users\Ralf\Desktop\talib_test\DAT_ASCII_EURGBP_M1_2015.csv");

            display.SetDisplayRangeX(0, 10);
            display.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;

            DataSource closingPrices = new DataSource();
            closingPrices.Length = cowabunga.ticks.Length;
            //closingPrices.Samples = new cPoint[cowabunga.ticks.Length];
            closingPrices.Name = "Closing Prices";
            closingPrices.AutoScaleY = true;
            closingPrices.SetDisplayRangeY(0.6f, 0.8f);
            closingPrices.SetGridDistanceY(1);

            closingPrices.OnRenderXAxisLabel = RenderXLabel;
            closingPrices.OnRenderYAxisLabel = RenderYLabel;

            for (int i = 0; i < cowabunga.ticks.Length; i++)
            {
                Tick tick = cowabunga.ticks[i];

                closingPrices.Samples[i].x = i;
                closingPrices.Samples[i].y = (float)tick.closePrice;
            }

            DataSource ema75source = new DataSource();
            ema75source.Name = "EMA 75";

            //setupDataSource(ema75source, cowabunga.ema75);

            display.DataSources.Add(closingPrices);
            //display.DataSources.Add(ema75source);

            ResumeLayout();
            display.Refresh();
        }

        private void setupDataSource(DataSource source, double[] data)
        {
            source.Samples = new cPoint[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                double d = data[i];

                source.Samples[i].x = i;
                source.Samples[i].y = (float)data[i];
                source.OnRenderYAxisLabel = RenderYLabel;
                source.OnRenderXAxisLabel = RenderXLabel;
            }

            source.AutoScaleY = true;

        }

        private string RenderXLabel(DataSource s, int idx)
        {
            if (s.AutoScaleX)
            {
                //if (idx % 2 == 0)
                {
                    int Value = (int)(s.Samples[idx].x);
                    return "" + Value;
                }
                return "";
            }
            else
            {
                int Value = (int)(s.Samples[idx].x / 200);
                String Label = "" + Value + "\"";
                return Label;
            }
        }

        private string RenderYLabel(DataSource src, float value)
        {
            return String.Format("{0:0.0}", value);
        }

        private void ApplyColorSchema()
        {
            string CurColorSchema = "WHITE";
            int NumGraphs = display.DataSources.Count;
            switch (CurColorSchema)
            {
                case "DARK_GREEN":
                    {
                        Color[] cols = { Color.FromArgb(0,255,0),
                                         Color.FromArgb(0,255,0),
                                         Color.FromArgb(0,255,0),
                                         Color.FromArgb(0,255,0),
                                         Color.FromArgb(0,255,0) ,
                                         Color.FromArgb(0,255,0),
                                         Color.FromArgb(0,255,0) };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            display.DataSources[j].GraphColor = cols[j % 7];
                        }

                        display.BackgroundColorTop = Color.FromArgb(0, 64, 0);
                        display.BackgroundColorBot = Color.FromArgb(0, 64, 0);
                        display.SolidGridColor = Color.FromArgb(0, 128, 0);
                        display.DashedGridColor = Color.FromArgb(0, 128, 0);
                    }
                    break;
                case "WHITE":
                    {
                        Color[] cols = { Color.DarkRed,
                                         Color.DarkSlateGray,
                                         Color.DarkCyan,
                                         Color.DarkGreen,
                                         Color.DarkBlue ,
                                         Color.DarkMagenta,
                                         Color.DeepPink };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            display.DataSources[j].GraphColor = cols[j % 7];
                        }

                        display.BackgroundColorTop = Color.White;
                        display.BackgroundColorBot = Color.White;
                        display.SolidGridColor = Color.LightGray;
                        display.DashedGridColor = Color.LightGray;
                    }
                    break;

                case "BLUE":
                    {
                        Color[] cols = { Color.Red,
                                         Color.Orange,
                                         Color.Yellow,
                                         Color.LightGreen,
                                         Color.Blue ,
                                         Color.DarkSalmon,
                                         Color.LightPink };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            display.DataSources[j].GraphColor = cols[j % 7];
                        }

                        display.BackgroundColorTop = Color.Navy;
                        display.BackgroundColorBot = Color.FromArgb(0, 0, 64);
                        display.SolidGridColor = Color.Blue;
                        display.DashedGridColor = Color.Blue;
                    }
                    break;

                case "GRAY":
                    {
                        Color[] cols = { Color.DarkRed,
                                         Color.DarkSlateGray,
                                         Color.DarkCyan,
                                         Color.DarkGreen,
                                         Color.DarkBlue ,
                                         Color.DarkMagenta,
                                         Color.DeepPink };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            display.DataSources[j].GraphColor = cols[j % 7];
                        }

                        display.BackgroundColorTop = Color.White;
                        display.BackgroundColorBot = Color.LightGray;
                        display.SolidGridColor = Color.LightGray;
                        display.DashedGridColor = Color.LightGray;
                    }
                    break;

                case "RED":
                    {
                        Color[] cols = { Color.DarkCyan,
                                         Color.Yellow,
                                         Color.DarkCyan,
                                         Color.DarkGreen,
                                         Color.DarkBlue ,
                                         Color.DarkMagenta,
                                         Color.DeepPink };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            display.DataSources[j].GraphColor = cols[j % 7];
                        }

                        display.BackgroundColorTop = Color.DarkRed;
                        display.BackgroundColorBot = Color.Black;
                        display.SolidGridColor = Color.Red;
                        display.DashedGridColor = Color.Red;
                    }
                    break;

                case "LIGHT_BLUE":
                    {
                        Color[] cols = { Color.DarkRed,
                                         Color.DarkSlateGray,
                                         Color.DarkCyan,
                                         Color.DarkGreen,
                                         Color.DarkBlue ,
                                         Color.DarkMagenta,
                                         Color.DeepPink };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            display.DataSources[j].GraphColor = cols[j % 7];
                        }

                        display.BackgroundColorTop = Color.White;
                        display.BackgroundColorBot = Color.FromArgb(183, 183, 255);
                        display.SolidGridColor = Color.Blue;
                        display.DashedGridColor = Color.Blue;
                    }
                    break;

                case "BLACK":
                    {
                        Color[] cols = { Color.FromArgb(255,0,0),
                                         Color.FromArgb(0,255,0),
                                         Color.FromArgb(255,255,0),
                                         Color.FromArgb(64,64,255),
                                         Color.FromArgb(0,255,255) ,
                                         Color.FromArgb(255,0,255),
                                         Color.FromArgb(255,128,0) };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            display.DataSources[j].GraphColor = cols[j % 7];
                        }

                        display.BackgroundColorTop = Color.Black;
                        display.BackgroundColorBot = Color.Black;
                        display.SolidGridColor = Color.DarkGray;
                        display.DashedGridColor = Color.DarkGray;
                    }
                    break;
            }

        }

    }
}
