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
/// ****** Important ******
/// To compile this indicator without any error you also need access to the utility indicator to use these global source code elements.
/// You will find this indicator on GitHub: https://github.com/simonpucher/AgenaTrader/blob/master/Utility/GlobalUtilities_Utility.cs
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
	public class Coppock_Indicator : UserIndicator
	{
		#region Variables

        //input
        private int _roclongPeriod = 14;
        private int _rocshortPeriod = 11;
		private int	_wmaperiod	= 10;

        private Color _plot0color = Const.DefaultIndicatorColor;
        private int _plot0width = Const.DefaultLineWidth;
        private DashStyle _plot0dashstyle = Const.DefaultIndicatorDashStyle;

        //internal
        private DataSeries _ROC_Long;
        private DataSeries _ROC_Short;
        private DataSeries _ROC_Combined;


		#endregion


		/// <summary>
		/// This method is used to configure the indicator and is called once before any bar data is loaded.
		/// </summary>
		protected override void Initialize()
		{
            Add(new Plot(new Pen(this.Plot0Color, this.Plot0Width), PlotStyle.Line, "Coppock_Curve"));

            CalculateOnBarClose = true;
		}



		/// <summary>
		/// Calculates the indicator value(s) at the current index.
		/// </summary>
		protected override void OnStartUp()
		{
            this._ROC_Long = new DataSeries(this);
            this._ROC_Short = new DataSeries(this);
            this._ROC_Combined = new DataSeries(this);
		}



		/// <summary>
		/// 
		/// </summary>
		protected override void OnBarUpdate()
		{

            double roc_long_value = ROC(this.ROCLongPeriod)[0];
            this._ROC_Long.Set(roc_long_value);

            double roc_short_value = ROC(this.ROCShortPeriod)[0];
            this._ROC_Short.Set(roc_short_value);

            this._ROC_Combined.Set(roc_long_value + roc_short_value);

            double wma_value = WMA(this._ROC_Combined, this.WMAPeriod)[0];
            this.Coppock_Curve.Set(wma_value);

            
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


            PlotColors[0][0] = Plot0Color;
            Plots[0].PenStyle = this.Dash0Style;
            Plots[0].Pen.Width = this.Plot0Width;


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


            #region Input 

                /// <summary>
                /// </summary>
                [Description("Period for the long ROC")]
                [Category("Values")]
                [DisplayName("Period ROC long")]
                public int ROCLongPeriod
                {
                    get { return _roclongPeriod; }
                    set { _roclongPeriod = Math.Max(1, value); }
                }

                /// <summary>
                /// </summary>
                [Description("Period for the short ROC")]
                [Category("Values")]
                [DisplayName("Period ROC short")]
                public int ROCShortPeriod
                {
                    get { return _rocshortPeriod; }
                    set { _rocshortPeriod = Math.Max(1, value); }
                }

                /// <summary>
                /// </summary>
                [Description("Period for WMA")]
                [Category("Values")]
                [DisplayName("Period for WMA")]
                public int WMAPeriod
                {
                    get { return _wmaperiod; }
                    set { _wmaperiod = Math.Max(1, value); }
                }



                /// <summary>
                /// </summary>
                [Description("Select Color for Coppock Curve")]
                [Category("Colors")]
                [DisplayName("Coppock Curve")]
                public Color Plot0Color
                {
                    get { return _plot0color; }
                    set { _plot0color = value; }
                }

                // Serialize Color object
                [Browsable(false)]
                public string MainSerialize
                {
                    get { return SerializableColor.ToString(_plot0color); }
                    set { _plot0color = SerializableColor.FromString(value); }
                }



                /// <summary>
                /// </summary>
                [Description("Width for Coppock Curve.")]
                [Category("Plots")]
                [DisplayName("Line Width Coppock Curve")]
                public int Plot0Width
                {
                    get { return _plot0width; }
                    set { _plot0width = Math.Max(1, value); }
                }



                /// <summary>
                /// </summary>
                [Description("DashStyle for Coppock Curve.")]
                [Category("Plots")]
                [DisplayName("Dash Style Coppock Curve")]
                public DashStyle Dash0Style
                {
                    get { return _plot0dashstyle; }
                    set { _plot0dashstyle = value; }
                } 
		
            #endregion



            #region Output

                [Browsable(false)]
                [XmlIgnore()]
                public DataSeries Coppock_Curve
                {
                    get { return Values[0]; }
                }

            #endregion

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
		public Coppock_Indicator Coppock_Indicator()
        {
			return Coppock_Indicator(Input);
		}

		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock_Indicator Coppock_Indicator(IDataSeries input)
		{
			var indicator = CachedCalculationUnits.GetCachedIndicator<Coppock_Indicator>(input);

			if (indicator != null)
				return indicator;

			indicator = new Coppock_Indicator
						{
							BarsRequired = BarsRequired,
							CalculateOnBarClose = CalculateOnBarClose,
							Input = input
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
		public Coppock_Indicator Coppock_Indicator()
		{
			return LeadIndicator.Coppock_Indicator(Input);
		}

		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock_Indicator Coppock_Indicator(IDataSeries input)
		{
			if (InInitialize && input == null)
				throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

			return LeadIndicator.Coppock_Indicator(input);
		}
	}

	#endregion

	#region Column

	public partial class UserColumn
	{
		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock_Indicator Coppock_Indicator()
		{
			return LeadIndicator.Coppock_Indicator(Input);
		}

		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock_Indicator Coppock_Indicator(IDataSeries input)
		{
			return LeadIndicator.Coppock_Indicator(input);
		}
	}

	#endregion

	#region Scripted Condition

	public partial class UserScriptedCondition
	{
		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock_Indicator Coppock_Indicator()
		{
			return LeadIndicator.Coppock_Indicator(Input);
		}

		/// <summary>
		/// The Coppock (Moving Average Convergence/Divergence) is a trend following momentum indicator that shows the relationship between two moving averages of prices.
		/// </summary>
		public Coppock_Indicator Coppock_Indicator(IDataSeries input)
		{
			return LeadIndicator.Coppock_Indicator(input);
		}
	}

	#endregion

}

#endregion
