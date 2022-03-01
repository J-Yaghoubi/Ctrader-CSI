using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, ScalePrecision = 1)]
    public class CSI : Indicator
    {

        #region parameters

        [Parameter(DefaultValue = 14, Group = "Key Settings")]
        public int Period { get; set; }


        [Parameter("AUD Enable", DefaultValue = false, Group = "Enable/Disable")]
        public bool audEnable { get; set; }

        [Parameter("NZD Enable", DefaultValue = false, Group = "Enable/Disable")]
        public bool nzdEnable { get; set; }

        [Parameter("JPY Enable", DefaultValue = false, Group = "Enable/Disable")]
        public bool jpyEnable { get; set; }

        [Parameter("GBP Enable", DefaultValue = false, Group = "Enable/Disable")]
        public bool gbpEnable { get; set; }

        [Parameter("CHF Enable", DefaultValue = false, Group = "Enable/Disable")]
        public bool chfEnable { get; set; }

        [Parameter("EUR Enable", DefaultValue = true, Group = "Enable/Disable")]
        public bool eurEnable { get; set; }

        [Parameter("USD Enable", DefaultValue = true, Group = "Enable/Disable")]
        public bool usdEnable { get; set; }

        [Parameter("CAD Enable", DefaultValue = false, Group = "Enable/Disable")]
        public bool cadEnable { get; set; }


        [Output("AUD", LineColor = "FF33C1F3")]
        public IndicatorDataSeries ResultAUD { get; set; }

        [Output("NZD", LineColor = "FF0071C1")]
        public IndicatorDataSeries ResultNZD { get; set; }

        [Output("JPY", LineColor = "FF7030A0")]
        public IndicatorDataSeries ResultJPY { get; set; }

        [Output("GBP", LineColor = "FF805F00")]
        public IndicatorDataSeries ResultGBP { get; set; }

        [Output("CHF", LineColor = "FF008000")]
        public IndicatorDataSeries ResultCHF { get; set; }

        [Output("EUR", LineColor = "FFFFFF01")]
        public IndicatorDataSeries ResultEUR { get; set; }

        [Output("USD", LineColor = "FFFFFFFF")]
        public IndicatorDataSeries ResultUSD { get; set; }

        [Output("CAD", LineColor = "FFFF3334")]
        public IndicatorDataSeries ResultCAD { get; set; }

        #endregion


        #region Global variables

        private Bars EURAUD, EURNZD, EURJPY, EURGBP, EURCHF, EURCAD, EURUSD, AUDUSD, NZDUSD, USDJPY,
        GBPUSD, USDCHF, USDCAD, AUDJPY, NZDJPY, GBPJPY, CHFJPY, CADJPY, GBPAUD, GBPNZD,
        GBPCHF, GBPCAD, AUDNZD, NZDCHF, NZDCAD, AUDCHF, AUDCAD, CADCHF;

        #endregion


        #region Ctrader Events
        protected override void Initialize()
        {
            /* Loading Pairs Data*/

            if (audEnable)
            {
                //aud pairs
                AUDNZD = MarketData.GetBars(Chart.TimeFrame, "AUDNZD");
                AUDJPY = MarketData.GetBars(Chart.TimeFrame, "AUDJPY");
                GBPAUD = MarketData.GetBars(Chart.TimeFrame, "GBPAUD");
                AUDCHF = MarketData.GetBars(Chart.TimeFrame, "AUDCHF");
                EURAUD = MarketData.GetBars(Chart.TimeFrame, "EURAUD");
                AUDUSD = MarketData.GetBars(Chart.TimeFrame, "AUDUSD");
                AUDCAD = MarketData.GetBars(Chart.TimeFrame, "AUDCAD");
            }

            if (nzdEnable)
            {
                //nzd pairs
                AUDNZD = MarketData.GetBars(Chart.TimeFrame, "AUDNZD");
                NZDJPY = MarketData.GetBars(Chart.TimeFrame, "NZDJPY");
                GBPNZD = MarketData.GetBars(Chart.TimeFrame, "GBPNZD");
                NZDCHF = MarketData.GetBars(Chart.TimeFrame, "NZDCHF");
                EURNZD = MarketData.GetBars(Chart.TimeFrame, "EURNZD");
                NZDUSD = MarketData.GetBars(Chart.TimeFrame, "NZDUSD");
                NZDCAD = MarketData.GetBars(Chart.TimeFrame, "NZDCAD");
            }

            if (jpyEnable)
            {
                //jpy pairs
                AUDJPY = MarketData.GetBars(Chart.TimeFrame, "AUDJPY");
                NZDJPY = MarketData.GetBars(Chart.TimeFrame, "NZDJPY");
                GBPJPY = MarketData.GetBars(Chart.TimeFrame, "GBPJPY");
                CHFJPY = MarketData.GetBars(Chart.TimeFrame, "CHFJPY");
                EURJPY = MarketData.GetBars(Chart.TimeFrame, "EURJPY");
                USDJPY = MarketData.GetBars(Chart.TimeFrame, "USDJPY");
                CADJPY = MarketData.GetBars(Chart.TimeFrame, "CADJPY");
            }

            if (gbpEnable)
            {
                //gbp pairs
                GBPAUD = MarketData.GetBars(Chart.TimeFrame, "GBPAUD");
                GBPNZD = MarketData.GetBars(Chart.TimeFrame, "GBPNZD");
                GBPJPY = MarketData.GetBars(Chart.TimeFrame, "GBPJPY");
                EURGBP = MarketData.GetBars(Chart.TimeFrame, "EURGBP");
                GBPCHF = MarketData.GetBars(Chart.TimeFrame, "GBPCHF");
                GBPUSD = MarketData.GetBars(Chart.TimeFrame, "GBPUSD");
                GBPCAD = MarketData.GetBars(Chart.TimeFrame, "GBPCAD");
            }

            if (chfEnable)
            {
                //chf pairs
                AUDCHF = MarketData.GetBars(Chart.TimeFrame, "AUDCHF");
                NZDCHF = MarketData.GetBars(Chart.TimeFrame, "NZDCHF");
                CHFJPY = MarketData.GetBars(Chart.TimeFrame, "CHFJPY");
                GBPCHF = MarketData.GetBars(Chart.TimeFrame, "GBPCHF");
                EURCHF = MarketData.GetBars(Chart.TimeFrame, "EURCHF");
                USDCHF = MarketData.GetBars(Chart.TimeFrame, "USDCHF");
                CADCHF = MarketData.GetBars(Chart.TimeFrame, "CADCHF");
            }

            if (eurEnable)
            {
                //eur pairs
                EURAUD = MarketData.GetBars(Chart.TimeFrame, "EURAUD");
                EURNZD = MarketData.GetBars(Chart.TimeFrame, "EURNZD");
                EURJPY = MarketData.GetBars(Chart.TimeFrame, "EURJPY");
                EURGBP = MarketData.GetBars(Chart.TimeFrame, "EURGBP");
                EURCHF = MarketData.GetBars(Chart.TimeFrame, "EURCHF");
                EURUSD = MarketData.GetBars(Chart.TimeFrame, "EURUSD");
                EURCAD = MarketData.GetBars(Chart.TimeFrame, "EURCAD");
            }

            if (usdEnable)
            {
                //usd pairs
                AUDUSD = MarketData.GetBars(Chart.TimeFrame, "AUDUSD");
                NZDUSD = MarketData.GetBars(Chart.TimeFrame, "NZDUSD");
                USDJPY = MarketData.GetBars(Chart.TimeFrame, "USDJPY");
                GBPUSD = MarketData.GetBars(Chart.TimeFrame, "GBPUSD");
                USDCHF = MarketData.GetBars(Chart.TimeFrame, "USDCHF");
                EURUSD = MarketData.GetBars(Chart.TimeFrame, "EURUSD");
                USDCAD = MarketData.GetBars(Chart.TimeFrame, "USDCAD");
            }

            if (cadEnable)
            {
                //cad pairs
                AUDCAD = MarketData.GetBars(Chart.TimeFrame, "AUDCAD");
                NZDCAD = MarketData.GetBars(Chart.TimeFrame, "NZDCAD");
                CADJPY = MarketData.GetBars(Chart.TimeFrame, "CADJPY");
                USDCAD = MarketData.GetBars(Chart.TimeFrame, "USDCAD");
                GBPCAD = MarketData.GetBars(Chart.TimeFrame, "GBPCAD");
                CADCHF = MarketData.GetBars(Chart.TimeFrame, "CADCHF");
                EURCAD = MarketData.GetBars(Chart.TimeFrame, "EURCAD");
            }

        }

        public override void Calculate(int index)
        {
            /* Calculate value at specified index and return the values */


            if (index < Period)
                return;

            if (audEnable)
            {
                Bars[] pairs = 
                {
                    AUDNZD,
                    AUDJPY,
                    GBPAUD,
                    AUDCHF,
                    EURAUD,
                    AUDUSD,
                    AUDCAD
                };

                var currencyIndex = Math.Round(CalculateStrength(index, pairs, "AUD"), 0);
                ResultAUD[index] = currencyIndex;
            }

            if (nzdEnable)
            {
                Bars[] pairs = 
                {
                    AUDNZD,
                    NZDJPY,
                    GBPNZD,
                    NZDCHF,
                    EURNZD,
                    NZDUSD,
                    NZDCAD
                };

                var currencyIndex = Math.Round(CalculateStrength(index, pairs, "NZD"), 0);
                ResultNZD[index] = currencyIndex;
            }

            if (jpyEnable)
            {
                Bars[] pairs = 
                {
                    AUDJPY,
                    NZDJPY,
                    GBPJPY,
                    CHFJPY,
                    EURJPY,
                    USDJPY,
                    CADJPY
                };

                var currencyIndex = Math.Round(CalculateStrength(index, pairs, "JPY"), 0);
                ResultJPY[index] = currencyIndex;
            }

            if (gbpEnable)
            {
                Bars[] pairs = 
                {
                    GBPAUD,
                    GBPNZD,
                    GBPJPY,
                    EURGBP,
                    GBPCHF,
                    GBPUSD,
                    GBPCAD
                };

                var currencyIndex = Math.Round(CalculateStrength(index, pairs, "GBP"), 0);
                ResultGBP[index] = currencyIndex;
            }

            if (chfEnable)
            {
                Bars[] pairs = 
                {
                    AUDCHF,
                    NZDCHF,
                    CHFJPY,
                    GBPCHF,
                    EURCHF,
                    USDCHF,
                    CADCHF
                };

                var currencyIndex = Math.Round(CalculateStrength(index, pairs, "CHF"), 0);
                ResultCHF[index] = currencyIndex;
            }

            if (eurEnable)
            {
                Bars[] pairs = 
                {
                    EURAUD,
                    EURNZD,
                    EURJPY,
                    EURGBP,
                    EURCHF,
                    EURCAD,
                    EURUSD
                };

                var currencyIndex = Math.Round(CalculateStrength(index, pairs, "EUR"), 0);
                ResultEUR[index] = currencyIndex;
            }

            if (usdEnable)
            {
                Bars[] pairs = 
                {
                    AUDUSD,
                    NZDUSD,
                    USDJPY,
                    GBPUSD,
                    USDCHF,
                    USDCAD,
                    EURUSD
                };

                var currencyIndex = Math.Round(CalculateStrength(index, pairs, "USD"), 0);
                ResultUSD[index] = currencyIndex;
            }

            if (cadEnable)
            {
                Bars[] pairs = 
                {
                    AUDCAD,
                    NZDCAD,
                    CADJPY,
                    USDCAD,
                    GBPCAD,
                    CADCHF,
                    EURCAD
                };

                var currencyIndex = Math.Round(CalculateStrength(index, pairs, "CAD"), 0);
                ResultCAD[index] = currencyIndex;
            }

        }

        #endregion


        #region Indicator Functions

        private double CalculateStrength(int index, Bars[] currency, string baseCurrency)
        {

                        /* Strength calculation over specified period with this formula: (ClosePrice - OpenPrice) / (HighPrice - LowPrice) */

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

            // Calculate strength of pairs separately

            for (int i = 0; i < currency.Length; i++)
            {
                // sync data with chart index and return as new index

                int index2 = GetIndexByDate(currency[i], Bars.OpenTimes[index]);

                // calculate the strength

                for (int j = 0; j < Period; j++)
                {
                    pairStrength[i] += (currency[i].ClosePrices[index2 - j] - currency[i].OpenPrices[index2 - j]) / (currency[i].HighPrices[index2 - j] - currency[i].LowPrices[index2 - j]);
                }
            }

            // Calculate overall strength:
            // If the Base currency is our favorite then sum pair-value with the overall, otherwise minus the pair-value from the overall

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

            // Return result

            return total;
        }

        private int GetIndexByDate(Bars series, DateTime time)
        {
            /* This function will use to sync the loaded data with chart time */

            for (int i = series.Count - 1; i >= 0; i--)
            {
                if (time == series.OpenTimes[i])
                    return i;
            }
            return -1;
        }

        #endregion


    }
}
