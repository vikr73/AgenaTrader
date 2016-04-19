using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using AgenaTrader.API;
using AgenaTrader.Custom;
using AgenaTrader.Plugins;
using AgenaTrader.Helper;

/// <summary>
/// Version: in progress
/// -------------------------------------------------------------------------
/// Simon Pucher 2016
/// -------------------------------------------------------------------------
/// The indicator was taken from: http://www.greattradingsystems.com/Coppock-ninjatraderindicator
/// Code was generated by AgenaTrader conversion tool and modified by Simon Pucher.
/// -------------------------------------------------------------------------
/// Namespace holds all indicators and is required. Do not change it.
/// </summary>
namespace AgenaTrader.UserCode
{
    /// <summary>
    /// The anaMACDBBLines (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
    /// Optimized execution by predefining instances of external indicators (Zondor August 10 2010)    
    /// </summary>
    [Description("The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.")]
	public class Coppock : UserIndicator
	{
		#region Variables

        private int _roclongPeriod = 14;
        private int _rocshortPeriod = 11;
		private int	_wmaperiod	= 10;

		private int		bandPeriod		= 34;
		private double	stdDevNumber	= 1.62;
		private Color	main			= Color.Lime;
		private int plot0Width 			= 1;
		private PlotStyle plot0Style 	= PlotStyle.Line;
		private DashStyle dash0Style 	= DashStyle.Solid;
		private int plot1Width 			= 1;
		private PlotStyle plot1Style 	= PlotStyle.Line;
		private DashStyle dash1Style 	= DashStyle.Solid;
		private int plot2Width 			= 1;
		private PlotStyle plot2Style 	= PlotStyle.Line;
		private DashStyle dash2Style 	= DashStyle.Solid;
		private int plot3Width 			= 1;
		private PlotStyle plot3Style 	= PlotStyle.Line;
		private DashStyle dash3Style 	= DashStyle.Solid;
			
		private ROC _ROCLONG;
		private ROC _ROCSHORT;
        private WMA _WMA;


		#endregion


		/// <summary>
		/// This method is used to configure the indicator and is called once before any bar data is loaded.
		/// </summary>
		protected override void Initialize()
		{
            //Add(new Plot(new Pen(Color.Gray, 1), PlotStyle.Line, "PriceLine"));
            Add(new Plot(new Pen(Color.Gray, 1), PlotStyle.Line, "Signalline"));
            //Add(new Plot(new Pen(Color.Gray, 1), PlotStyle.Line, "Average"));
            //Add(new Plot(new Pen(Color.Gray, 1), PlotStyle.Line, "Upper"));
            //Add(new Plot(new Pen(Color.Gray, 1), PlotStyle.Line, "Lower"));
            //Add(new Plot(new Pen(Color.Gray, 1), PlotStyle.Line, "MidLine"));

        

            CalculateOnBarClose = true;

          
            
		}



		/// <summary>
		/// Calculates the indicator value(s) at the current index.
		/// </summary>
		protected override void OnStartUp()
		{

           _ROCLONG = ROC(Input, ROCLongPeriod);
            _ROCSHORT = ROC(Input, ROCShortPeriod);
            //List<double> WMAEnumerable = new List<double>();

            //for (int i = 0; i < ROCLONG.Count; i++)
            //{
            //    WMAEnumerable.Add(ROCLONG.GetByIndex(i) + ROCSHORT.GetByIndex(i));
            //}


            //double[] WMAEnumerable = new double[Input.Count];

            //for (int i = 0; i < _ROCLONG.Count; i++)
            //{


            //    WMAEnumerable[i] = _ROCLONG.GetByIndex(i) + _ROCSHORT.GetByIndex(i);



            //}
          


            

            //IEnumerable<double> mumumu = WMAEnumerable.AsEnumerable<double>();
            //FloatSeries huhu = new FloatSeries();
            //IDataSeries bubu = (IDataSeries)huhu;

            //_WMA = WMA(huhu, WMAPeriod);
 
            //DYNRSI = RSI(Input,RSIPeriod,1);
            //DYNPrice = SMA(DYNRSI,PricePeriod);
            //DYNSignal = SMA(DYNRSI,SignalPeriod);
            //DYNAverage = SMA(DYNRSI, BandPeriod);
            //SDBB = StdDev(DYNRSI,BandPeriod);
		}



		
		protected override void OnBarUpdate()
		{

            double roclong = _ROCLONG[CurrentBar];
            double rocshort = _ROCSHORT[CurrentBar];

           

            //double newvalue = 0;
            //if (CurrentBar - ROCLongPeriod > 0)
            //{
            //    newvalue = ((ROCLONG[CurrentBar] - ROCLONG[CurrentBar - ROCLongPeriod]) / ROCLONG[CurrentBar - ROCLongPeriod]) * 100;
            //}

            //if (CurrentBar >= ROCLongPeriod)
            //{
            //    newvalue = ((Bars[0].Close - Bars[ROCLongPeriod].Close) / Bars[ROCLongPeriod].Close) * 100;
            //}

            //newvalue = ROC(ROCLongPeriod)[0];



            //PriceLine.Set(ROCLONG.GetByIndex(CurrentBar));
            //SignalLine.Set(ROCSHORT.GetByIndex(CurrentBar));

            Addition.Set(_ROCLONG.GetByIndex(CurrentBar) + _ROCSHORT.GetByIndex(CurrentBar));

            SignalLine.Set(SMA(Addition, WMAPeriod)[0]);

            //Average.Set(_WMA.GetByIndex(CurrentBar));

            //PriceLine.Set(roclong);
           // SignalLine.Set(rocshort);

            //double avg = DYNAverage[countbar];
            //Average.Set(avg);
            //MidLine.Set(50);

            //double stdDevValue = SDBB[countbar];
            //Upper.Set(avg + StdDevNumber * stdDevValue);
            //Lower.Set(avg - StdDevNumber * stdDevValue);

            PlotColors[0][0] = Main;
            //PlotColors[1][0] = Signal;
            //PlotColors[2][0] = BBAverage;
            //PlotColors[3][0] = BBUpper;

            //PlotColors[4][0] = BBLower;

            //if (avg > 50)
            //    PlotColors[5][0] = MidPositive;
            //else
            //    PlotColors[5][0] = MidNegative;
		}

        protected override void OnTermination()
        {
            //Print("OnTermination");
        }


        public override string ToString()
        {
            return "Coppock";
        }

        public override string DisplayName
        {
            get
            {
                return "Coppock";
            }
        }



        #region Properties
        ///// <summary>
        ///// </summary>
        //[Browsable(false)]
        //[XmlIgnore()]
        //public DataSeries PriceLine
        //{
        //    get { return Values[0]; }
        //}

        /// <summary>
        /// </summary>
        [Browsable(false)]
		[XmlIgnore()]
		public DataSeries SignalLine
		{
			get { return Values[0]; }
		}

        /// <summary>
        /// </summary>
        [Browsable(false)]
        [XmlIgnore()]
        public DataSeries Addition
        {
            get { return Values[1]; }
        }

        ///// <summary>
        ///// </summary>
        //[Browsable(false)]
        //[XmlIgnore()]
        //public DataSeries Upper
        //{
        //    get { return Values[3]; }
        //}

        ///// <summary>
        ///// </summary>
        //[Browsable(false)]
        //[XmlIgnore()]
        //public DataSeries Lower
        //{
        //    get { return Values[4]; }
        //}

        ///// <summary>
        ///// </summary>
        //[Browsable(false)]
        //[XmlIgnore()]
        //public DataSeries MidLine
        //{
        //    get { return Values[5]; }
        //}

		/// <summary>
		/// </summary>
		[Description("Period for longer ROC")]
		[Category("Parameters")]
		[DisplayName("Period for longer ROC")]
		public int ROCLongPeriod
		{
            get { return _roclongPeriod; }
            set { _roclongPeriod = Math.Max(1, value); }
		}

		/// <summary>
		/// </summary>
        [Description("Period for shorter ROC")]
        [Category("Parameters")]
        [DisplayName("Period for shorter ROC")]
        public int ROCShortPeriod
		{
			get { return _rocshortPeriod; }
            set { _rocshortPeriod = Math.Max(1, value); }
		}

		/// <summary>
		/// </summary>
		[Description("Period for Signalline")]
		[Category("Parameters")]
		[DisplayName("Period for Signalline")]
		public int WMAPeriod
		{
            get { return _wmaperiod; }
            set { _wmaperiod = Math.Max(1, value); }
		}

        ///// <summary>
        ///// </summary>
        //[Description("Band Period for Bollinger Band")]
        //[Category("Parameters")]
        //[DisplayName("Period for VolaBands")]
        //public int BandPeriod
        //{
        //    get { return bandPeriod; }
        //    set { bandPeriod = Math.Max(1, value); }
        //}

        ///// <summary>
        ///// </summary>
        //[Description("Number of standard deviations")]
        //[Category("Parameters")]
        //[DisplayName("# of Std. Dev.")]
        //public double StdDevNumber
        //{
        //    get { return stdDevNumber; }
        //    set { stdDevNumber = Math.Max(0, value); }
        //}

		/// <summary>
		/// </summary>
		[Description("Select Color")]
		[Category("Colors")]
		[DisplayName("Pricline")]
		public Color Main
		{
			get { return main; }
			set { main = value; }
		}
		
		// Serialize Color object
		[Browsable(false)]
		public string MainSerialize
		{
			get { return SerializableColor.ToString(main); }
			set { main = SerializableColor.FromString(value); }
		}

        ///// <summary>
        ///// </summary>
        //[Description("Select Color")]
        //[Category("Colors")]
        //[DisplayName("Signalline")]
        //public Color Signal
        //{
        //    get { return signal; }
        //    set { signal = value; }
        //}
		
        //// Serialize Color object
        //[Browsable(false)]
        //public string SignalSerialize
        //{
        //    get { return SerializableColor.ToString(signal); }
        //    set { signal = SerializableColor.FromString(value); }
        //}

        ///// <summary>
        ///// </summary>
        //[Description("Select Color")]
        //[Category("Colors")]
        //[DisplayName("Bollinger Average")]
        //public Color BBAverage
        //{
        //    get { return bbAverage; }
        //    set { bbAverage = value; }
        //}
		
        //// Serialize Color object
        //[Browsable(false)]
        //public string BBAverageSerialize
        //{
        //    get { return SerializableColor.ToString(bbAverage); }
        //    set { bbAverage = SerializableColor.FromString(value); }
        //}

        ///// <summary>
        ///// </summary>
        //[Description("Select Color")]
        //[Category("Colors")]
        //[DisplayName("Bollinger Upper Band")]
        //public Color BBUpper
        //{
        //    get { return bbUpper; }
        //    set { bbUpper = value; }
        //}
		
        //// Serialize Color object
        //[Browsable(false)]
        //public string BBUpperSerialize
        //{
        //    get { return SerializableColor.ToString(bbUpper); }
        //    set { bbUpper = SerializableColor.FromString(value); }
        //}

        ///// <summary>
        ///// </summary>
        //[Description("Select Color")]
        //[Category("Colors")]
        //[DisplayName("Bollinger Lower Band")]
        //public Color BBLower
        //{
        //    get { return bbLower; }
        //    set { bbLower = value; }
        //}
		
        //// Serialize Color object
        //[Browsable(false)]
        //public string BBLowerSerialize
        //{
        //    get { return SerializableColor.ToString(bbLower); }
        //    set { bbLower = SerializableColor.FromString(value); }
        //}

        ///// <summary>
        ///// </summary>
        //[Description("Select Color")]
        //[Category("Colors")]
        //[DisplayName("Midline Positive")]
        //public Color MidPositive
        //{
        //    get { return midPositive; }
        //    set { midPositive = value; }
        //}
		
        //// Serialize Color object
        //[Browsable(false)]
        //public string MidPositiveSerialize
        //{
        //    get { return SerializableColor.ToString(midPositive); }
        //    set { midPositive = SerializableColor.FromString(value); }
        //}

        ///// <summary>
        ///// </summary>
        //[Description("Select Color")]
        //[Category("Colors")]
        //[DisplayName("Midline Negative")]
        //public Color MidNegative
        //{
        //    get { return midNegative; }
        //    set { midNegative = value; }
        //}
		
        //// Serialize Color object
        //[Browsable(false)]
        //public string MidNegativeSerialize
        //{
        //    get { return SerializableColor.ToString(midNegative); }
        //    set { midNegative = SerializableColor.FromString(value); }
        //}
		
		/// <summary>
		/// </summary>
		[Description("Width for Priceline.")]
		[Category("Plots")]
		[DisplayName("Line Width Priceline")]
		public int Plot0Width
		{
			get { return plot0Width; }
			set { plot0Width = Math.Max(1, value); }
		}
		
		/// <summary>
		/// </summary>
		[Description("PlotStyle for Priceline.")]
		[Category("Plots")]
		[DisplayName("Plot Style Priceline")]
		public PlotStyle Plot0Style
		{
			get { return plot0Style; }
			set { plot0Style = value; }
		}
		
		/// <summary>
		/// </summary>
		[Description("DashStyle for Priceline.")]
		[Category("Plots")]
		[DisplayName("Dash Style Priceline")]
		public DashStyle Dash0Style
		{
			get { return dash0Style; }
			set { dash0Style = value; }
		} 
		
		/// <summary>
		/// </summary>
		[Description("Width for Signalline.")]
		[Category("Plots")]
		[DisplayName("Line Width Signal")]
		public int Plot1Width
		{
			get { return plot1Width; }
			set { plot1Width = Math.Max(1, value); }
		}
		
		/// <summary>
		/// </summary>
		[Description("PlotStyle for Signalline.")]
		[Category("Plots")]
		[DisplayName("Plot Style Signal")]
		public PlotStyle Plot1Style
		{
			get { return plot1Style; }
			set { plot1Style = value; }
		}
		
		/// <summary>
		/// </summary>
		[Description("DashStyle for Signalline.")]
		[Category("Plots")]
		[DisplayName("Dash Style Signal")]
		public DashStyle Dash1Style
		{
			get { return dash1Style; }
			set { dash1Style = value; }
		} 

		/// <summary>
		/// </summary>
		[Description("Width for Midband.")]
		[Category("Plots")]
		[DisplayName("Line Width Midband")]
		public int Plot2Width
		{
			get { return plot2Width; }
			set { plot2Width = Math.Max(1, value); }
		}
		
		/// <summary>
		/// </summary>
		[Description("PlotStyle for Midband.")]
		[Category("Plots")]
		[DisplayName("Plot Style Midband")]
		public PlotStyle Plot2Style
		{
			get { return plot2Style; }
			set { plot2Style = value; }
		}
		
		/// <summary>
		/// </summary>
		[Description("DashStyle for Bollinger Bands.")]
		[Category("Plots")]
		[DisplayName("Dash Style BBands")]
		public DashStyle Dash2Style
		{
			get { return dash2Style; }
			set { dash2Style = value; }
		} 
		
		/// <summary>
		/// </summary>
		[Description("Width for Bollinger Bands.")]
		[Category("Plots")]
		[DisplayName("Line Width BBAnds")]
		public int Plot3Width
		{
			get { return plot3Width; }
			set { plot3Width = Math.Max(1, value); }
		}
		
		/// <summary>
		/// </summary>
		[Description("PlotStyle for Bollinger Bands.")]
		[Category("Plots")]
		[DisplayName("Plot Style BBAnds")]
		public PlotStyle Plot3Style
		{
			get { return plot3Style; }
			set { plot3Style = value; }
		}
		
		/// <summary>
		/// </summary>
		[Description("DashStyle for Trigger Average Line.")]
		[Category("Plots")]
		[DisplayName("Dash Style Average")]
		public DashStyle Dash3Style
		{
			get { return dash3Style; }
			set { dash3Style = value; }
		} 

		#endregion
	}
}

#region AgenaTrader Automaticaly Generated Code. Do not change it manualy

namespace AgenaTrader.UserCode
{
	#region Indicator

	public partial class UserIndicator : Indicator
	{
		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock Coppock(System.Int32 rOCLongPeriod, System.Int32 rOCShortPeriod, System.Int32 wMAPeriod)
        {
			return Coppock(Input, rOCLongPeriod, rOCShortPeriod, wMAPeriod);
		}

		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock Coppock(IDataSeries input, System.Int32 rOCLongPeriod, System.Int32 rOCShortPeriod, System.Int32 wMAPeriod)
		{
			var indicator = CachedCalculationUnits.GetCachedIndicator<Coppock>(input, i => i.ROCLongPeriod == rOCLongPeriod && i.ROCShortPeriod == rOCShortPeriod && i.WMAPeriod == wMAPeriod);

			if (indicator != null)
				return indicator;

			indicator = new Coppock
						{
							BarsRequired = BarsRequired,
							CalculateOnBarClose = CalculateOnBarClose,
							Input = input,
							ROCLongPeriod = rOCLongPeriod,
							ROCShortPeriod = rOCShortPeriod,
							WMAPeriod = wMAPeriod
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
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock Coppock(System.Int32 rOCLongPeriod, System.Int32 rOCShortPeriod, System.Int32 wMAPeriod)
		{
			return LeadIndicator.Coppock(Input, rOCLongPeriod, rOCShortPeriod, wMAPeriod);
		}

		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock Coppock(IDataSeries input, System.Int32 rOCLongPeriod, System.Int32 rOCShortPeriod, System.Int32 wMAPeriod)
		{
			if (InInitialize && input == null)
				throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

			return LeadIndicator.Coppock(input, rOCLongPeriod, rOCShortPeriod, wMAPeriod);
		}
	}

	#endregion

	#region Column

	public partial class UserColumn
	{
		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock Coppock(System.Int32 rOCLongPeriod, System.Int32 rOCShortPeriod, System.Int32 wMAPeriod)
		{
			return LeadIndicator.Coppock(Input, rOCLongPeriod, rOCShortPeriod, wMAPeriod);
		}

		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock Coppock(IDataSeries input, System.Int32 rOCLongPeriod, System.Int32 rOCShortPeriod, System.Int32 wMAPeriod)
		{
			return LeadIndicator.Coppock(input, rOCLongPeriod, rOCShortPeriod, wMAPeriod);
		}
	}

	#endregion

	#region Scripted Condition

	public partial class UserScriptedCondition
	{
		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock Coppock(System.Int32 rOCLongPeriod, System.Int32 rOCShortPeriod, System.Int32 wMAPeriod)
		{
			return LeadIndicator.Coppock(Input, rOCLongPeriod, rOCShortPeriod, wMAPeriod);
		}

		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock Coppock(IDataSeries input, System.Int32 rOCLongPeriod, System.Int32 rOCShortPeriod, System.Int32 wMAPeriod)
		{
			return LeadIndicator.Coppock(input, rOCLongPeriod, rOCShortPeriod, wMAPeriod);
		}
	}

	#endregion

}

#endregion


