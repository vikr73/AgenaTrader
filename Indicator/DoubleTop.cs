using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using AgenaTrader.API;
using AgenaTrader.Custom;
using AgenaTrader.Plugins;
using AgenaTrader.Helper;

namespace AgenaTrader.UserCode
{
	[Description("DoubleTop")]
	public class DoubleTop : UserIndicator
	{
        private Boolean SetSuccessFromEcho = false;

        //input
        private double _tolerancePercentage = 0.6;
        private int _candles = 8;
        private bool _drawTolerance;
        private int _barsAgo = 20;


        protected override void OnInit()
        {
            Add(new Plot(Color.Red, "DoubleTop_DS"));
            IsOverlay = false;
            CalculateOnClosedBar = false;

            //Inhalt des OutputWindow l�schen
            ClearOutputWindow();
        }

        protected override void OnCalculate()
        {

            DoubleTop_DS.Set(0);

            double HighestHighFromEchoBars;
            double HighestHighFromEchoBarsIndex;
            DateTime HighestHighFromEchoBarsDate;

            //Get the highest Price/Index from our Echo-Period
            if (ProcessingBarIndex >= (Bars.Count - 1))
            {
                HighestHighFromEchoBars = HighestHighPrice(this.Candles)[0];
                HighestHighFromEchoBarsIndex = HighestHighIndex(this.Candles)[0];
                HighestHighFromEchoBarsDate = Bars[(int)HighestHighFromEchoBarsIndex].Time;
            }
            else
            {
                return;  //Just the Last Bar plus the Echo-Bars
            }


            //check if datafeed is providing appropriate data
            if (Bars[BarsAgo + (int)HighestHighFromEchoBarsIndex] == null)
            {
                return;
            }

            //Calculate the minimum distance from current low to the next low
            DateTime MinBarsAgoDateTime = Bars[BarsAgo + (int)HighestHighFromEchoBarsIndex].Time;

            //Calculate Tolerance
            double tolerance = HighestHighFromEchoBars * (TolerancePercentage / 100);
            double tolerance_min = HighestHighFromEchoBars - tolerance;
            double tolerance_max = HighestHighFromEchoBars + tolerance;


            Print("  Bar {0}, Tol+{1}, Tol-{2}",
            Bars[0].Time.ToString(), Math.Round(tolerance_max, 2), Math.Round(tolerance_min, 2));


            //Check, when the chart was the last time above our current high. That period becomes irrelevant for us and gets ignored
            IEnumerable<IBar> aboveHigh = Bars.Where(y => y.High >= tolerance_max)
                                              .Where(x => x.Time <  HighestHighFromEchoBarsDate)
                                              .OrderByDescending(x => x.Time);

            //if there is no other High and the chart is coming all the way from a lower price, than just leave this indicator
            if (!aboveHigh.Any())
            {
                return;
            }


            DateTime IgnoreFromHereOn = aboveHigh.FirstOrDefault().Time;

            //Draw ToleranceArea for the respected timeperiod
            if (DrawTolerance)
            {
                AddChartRectangle("ToleranceRectangle", true, Bars.GetBarsAgo(IgnoreFromHereOn), tolerance_max, 0, tolerance_min, Color.Yellow, Color.Yellow, 50);
            }


            //find previous highs
            //Select all data and find highs.
            IEnumerable<IBar> lastTops = Bars.Where(x => x.Time <= MinBarsAgoDateTime           //older than x Bars, so we have a arch in between the two low points 
                                                         && x.Time >= IgnoreFromHereOn)           //but younger than the timeperiod when the chart was below our low     
                                                .Where(y => y.High <= tolerance_max                 // Low <= current Low + Tolerance
                                                         && y.High >= tolerance_min                 // Low >= current Low + Tolerance    
                                                         )
                                                .OrderBy(x => x.High)
                                                         ;

            int HighestHighBarsBefore = 5;

            foreach (IBar bar in lastTops)
            {
                double HighestHigh       = HighestHighPrice(Bars.GetBarsAgo(bar.Time))[0];                         //calculate the HighestHigh between current bar and potential bottom
                double HighestHighBefore = HighestHighPrice(Bars.GetBarsAgo(bar.Time) + HighestHighBarsBefore)[0]; //calculate the HighestHigh before the potential top. this is to make sure that there is no higher price leading up to the top

                //now check, if the current bar is on the same price level as the potential top. just to make sure, there is no higher price in that period.
                if (HighestHigh       <= (tolerance_max)                  //check if that HighestHigh is inside tolerance levels   
                 && HighestHigh       >= (tolerance_min)
                 && HighestHighBefore <= (tolerance_max)                  //check if the HighestHighBefore is inside tolerance levels 
                 && HighestHighBefore >= (tolerance_min)
                && (HighestHigh       == HighestHighBefore                //HighestHigh has to be either current bar or the current bottom from loop
                 || HighestHigh       == HighestHighFromEchoBars)
                    )
                {
                    Print("DoubleTop  High: {0}, Time: {1}, HighestHigh: {2}, HighestHighBefore: {3}",
                          bar.High, bar.Time.ToString(), HighestHigh, HighestHighBefore);

                    //Drawings
                    //Red Connection Line of the Bottoms
                    string strdoubleTopConnecter = "DoubleTopConnecter_" + Bars[0].Time.ToString() + "_" + bar.Time.ToString();
                    AddChartLine(strdoubleTopConnecter, Bars.GetBarsAgo(bar.Time), bar.High, (int)HighestHighFromEchoBarsIndex, HighestHighFromEchoBars, Color.Red);

                    //High and Breakthrough
                    double BreakThrough    = LowestLowPrice(Bars.GetBarsAgo(bar.Time))[0];
                    double BreakThroughAgo = LowestLowIndex(Bars.GetBarsAgo(bar.Time))[0];

                    string strBreakThrough = strdoubleTopConnecter + "BreakThrough";
                    string strBreakThroughVert = strdoubleTopConnecter + "BreakThroughVert";
                    AddChartLine(strBreakThrough,     (int)BreakThroughAgo, BreakThrough, 0,                    BreakThrough, Color.Aquamarine, DashStyle.Solid, 2);
                    AddChartLine(strBreakThroughVert, (int)BreakThroughAgo, bar.High,     (int)BreakThroughAgo, BreakThrough, Color.Aquamarine, DashStyle.Solid, 2);

                    //Mark current High
                    DoubleTop_DS.Set((int)HighestHighFromEchoBarsIndex, 1);
                    //Mark previous High(s)
                    DoubleTop_DS.Set(Bars.GetBarsAgo(bar.Time), 0.5);
                    SetSuccessFromEcho = true;
                }
            }
            if (SetSuccessFromEcho)
            {
                DoubleTop_DS.Set(1);
            }
            else
            {
                DoubleTop_DS.Set(0);
            }
        }

        #region Properties

        [Browsable(false)]
		[XmlIgnore()]
		public DataSeries DoubleTop_DS
        {
			get { return Outputs[0]; }
		}

        [Description("Tolerance level in percent.")]
        [Category("Parameters")]
        [DisplayName("Tolerance")]
        public double TolerancePercentage
        {
            get
            {
                return _tolerancePercentage;
            }

            set
            {
                _tolerancePercentage = value;
            }
        }


        [Description("The script shows a signal if the double bottom was reached within the last x candles.")]
        [Category("Parameters")]
        [DisplayName("Candles")]
        public int Candles
        {
            get { return _candles; }
            set { _candles = value; }
        }

        [Description("Draw the ToleranceLevel")]
        [Category("Parameters")]
        [DisplayName("Draw Tolerance")]
        public bool DrawTolerance
        {
            get
            {
                return _drawTolerance;
            }

            set
            {
                _drawTolerance = value;
            }
        }


        [Description("Determines, how many bars the other bottom(s) should be at least away from the current low")]
        [Category("Parameters")]
        [DisplayName("Min Bars ago for last bottom")]
        public int BarsAgo
        {
            get
            {
                return _barsAgo;
            }

            set
            {
                _barsAgo = value;
            }
        }

        #endregion
    }
}
#region AgenaTrader Automaticaly Generated Code. Do not change it manualy

namespace AgenaTrader.UserCode
{
	#region Indicator

	public partial class UserIndicator
	{
		/// <summary>
		/// DoubleTop
		/// </summary>
		public DoubleTop DoubleTop(System.Double tolerancePercentage, System.Int32 candles, System.Boolean drawTolerance, System.Int32 barsAgo)
        {
			return DoubleTop(InSeries, tolerancePercentage, candles, drawTolerance, barsAgo);
		}

		/// <summary>
		/// DoubleTop
		/// </summary>
		public DoubleTop DoubleTop(IDataSeries input, System.Double tolerancePercentage, System.Int32 candles, System.Boolean drawTolerance, System.Int32 barsAgo)
		{
			var indicator = CachedCalculationUnits.GetCachedIndicator<DoubleTop>(input, i => Math.Abs(i.TolerancePercentage - tolerancePercentage) <= Double.Epsilon && i.Candles == candles && i.DrawTolerance == drawTolerance && i.BarsAgo == barsAgo);

			if (indicator != null)
				return indicator;

			indicator = new DoubleTop
						{
							RequiredBarsCount = RequiredBarsCount,
							CalculateOnClosedBar = CalculateOnClosedBar,
							InSeries = input,
							TolerancePercentage = tolerancePercentage,
							Candles = candles,
							DrawTolerance = drawTolerance,
							BarsAgo = barsAgo
						};
			indicator.SetUp();

			CachedCalculationUnits.AddIndicator2Cache(indicator);

			return indicator;
		}
	}

	#endregion

	#region Strategy

	public partial class UserStrategy
	{
		/// <summary>
		/// DoubleTop
		/// </summary>
		public DoubleTop DoubleTop(System.Double tolerancePercentage, System.Int32 candles, System.Boolean drawTolerance, System.Int32 barsAgo)
		{
			return LeadIndicator.DoubleTop(InSeries, tolerancePercentage, candles, drawTolerance, barsAgo);
		}

		/// <summary>
		/// DoubleTop
		/// </summary>
		public DoubleTop DoubleTop(IDataSeries input, System.Double tolerancePercentage, System.Int32 candles, System.Boolean drawTolerance, System.Int32 barsAgo)
		{
			if (IsInInit && input == null)
				throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'OnInit()' method");

			return LeadIndicator.DoubleTop(input, tolerancePercentage, candles, drawTolerance, barsAgo);
		}
	}

	#endregion

	#region Column

	public partial class UserColumn
	{
		/// <summary>
		/// DoubleTop
		/// </summary>
		public DoubleTop DoubleTop(System.Double tolerancePercentage, System.Int32 candles, System.Boolean drawTolerance, System.Int32 barsAgo)
		{
			return LeadIndicator.DoubleTop(InSeries, tolerancePercentage, candles, drawTolerance, barsAgo);
		}

		/// <summary>
		/// DoubleTop
		/// </summary>
		public DoubleTop DoubleTop(IDataSeries input, System.Double tolerancePercentage, System.Int32 candles, System.Boolean drawTolerance, System.Int32 barsAgo)
		{
			return LeadIndicator.DoubleTop(input, tolerancePercentage, candles, drawTolerance, barsAgo);
		}
	}

	#endregion

	#region Scripted Condition

	public partial class UserScriptedCondition
	{
		/// <summary>
		/// DoubleTop
		/// </summary>
		public DoubleTop DoubleTop(System.Double tolerancePercentage, System.Int32 candles, System.Boolean drawTolerance, System.Int32 barsAgo)
		{
			return LeadIndicator.DoubleTop(InSeries, tolerancePercentage, candles, drawTolerance, barsAgo);
		}

		/// <summary>
		/// DoubleTop
		/// </summary>
		public DoubleTop DoubleTop(IDataSeries input, System.Double tolerancePercentage, System.Int32 candles, System.Boolean drawTolerance, System.Int32 barsAgo)
		{
			return LeadIndicator.DoubleTop(input, tolerancePercentage, candles, drawTolerance, barsAgo);
		}
	}

	#endregion

}

#endregion