using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, ScalePrecision = 2)]
    public class CSI : Indicator
    {

        [Parameter(DefaultValue = 14)]
        public int Period { get; set; }

        [Output("USD SI", LineColor = "FF805F00")]
        public IndicatorDataSeries ResultUSD { get; set; }

        [Output("EUR SI")]
        public IndicatorDataSeries ResultEUR { get; set; }


        private Bars EURAUD, EURNZD, EURJPY, EURGBP, EURCHF, EURCAD, EURUSD, AUDUSD, NZDUSD, USDJPY,
        GBPUSD, USDCHF, USDCAD;

        protected override void Initialize()
        {
            //eur pairs
            EURAUD = MarketData.GetBars(Chart.TimeFrame, "EURAUD");
            EURNZD = MarketData.GetBars(Chart.TimeFrame, "EURNZD");
            EURJPY = MarketData.GetBars(Chart.TimeFrame, "EURJPY");
            EURGBP = MarketData.GetBars(Chart.TimeFrame, "EURGBP");
            EURCHF = MarketData.GetBars(Chart.TimeFrame, "EURCHF");
            EURCAD = MarketData.GetBars(Chart.TimeFrame, "EURCAD");
            EURUSD = MarketData.GetBars(Chart.TimeFrame, "EURUSD");

            //usd pairs
            AUDUSD = MarketData.GetBars(Chart.TimeFrame, "AUDUSD");
            NZDUSD = MarketData.GetBars(Chart.TimeFrame, "NZDUSD");
            USDJPY = MarketData.GetBars(Chart.TimeFrame, "USDJPY");
            GBPUSD = MarketData.GetBars(Chart.TimeFrame, "GBPUSD");
            USDCHF = MarketData.GetBars(Chart.TimeFrame, "USDCHF");
            USDCAD = MarketData.GetBars(Chart.TimeFrame, "USDCAD");
        }

        public override void Calculate(int index)
        {

            if (index < Period)
                return;

            Bars[] usdPair = 
            {
                AUDUSD,
                NZDUSD,
                USDJPY,
                GBPUSD,
                USDCHF,
                USDCAD,
                EURUSD
            };

            Bars[] eurPair = 
            {
                EURAUD,
                EURNZD,
                EURJPY,
                EURGBP,
                EURCHF,
                EURCAD,
                EURUSD
            };


            var usdIndex = Math.Round(CalculateStrength(index, usdPair, "USD"), 2);
            var eurIndex = Math.Round(CalculateStrength(index, eurPair, "EUR"), 2);


            Chart.DrawStaticText("USD Strength index", "USD: " + usdIndex.ToString(), VerticalAlignment.Top, HorizontalAlignment.Left, Color.Gray);
            Chart.DrawStaticText("EUR Strength index", "\nEUR: " + eurIndex.ToString(), VerticalAlignment.Top, HorizontalAlignment.Left, Color.Gray);

            // ChartObjects.DrawText("USD Strength index", "USD: " + usdIndex.ToString(), StaticPosition.TopRight, Colors.DarkOrange);
            // ChartObjects.DrawText("EUR Strength index", "\nEUR: " + eurIndex.ToString(), StaticPosition.TopRight, Colors.DarkOrange);


            ResultUSD[index] = usdIndex;
            ResultEUR[index] = eurIndex;

        }

        private double CalculateStrength(int index, Bars[] currency, string baseCurrency)
        {
            // Pairs strength separately calculation

            double[] pairStrength = 
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0
            };


            for (int i = 0; i < currency.Length; i++)
            {
                int index2 = GetIndexByDate(currency[i], Bars.OpenTimes[index]);


                for (int j = 1; j < Period; j++)
                {
                    pairStrength[i] += (currency[i].ClosePrices[index2 - j] - currency[i].OpenPrices[index2 - j]) / (currency[i].HighPrices[index2 - j] - currency[i].LowPrices[index2 - j]);
                }
            }

            // Overall strength calculation

            double total = 0;

            for (int i = 0; i < currency.Length; i++)
            {
                if (currency[i].ToString().Substring(0, 3) == baseCurrency)
                {
                    total += pairStrength[i];
                }
                else
                {
                    total -= pairStrength[i];
                }

            }

            return total / Period;
        }

        private int GetIndexByDate(Bars series, DateTime time)
        {
            for (int i = series.Count - 1; i >= 0; i--)
            {
                if (time == series.OpenTimes[i])
                    return i;
            }
            return -1;
        }

    }
}
