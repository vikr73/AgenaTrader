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

/// <summary>
/// Version: 1.2.8
/// -------------------------------------------------------------------------
/// Simon Pucher 2016
/// -------------------------------------------------------------------------
/// ****** Important ******
/// To compile this script without any error you also need access to the utility indicator to use global source code elements.
/// You will find this script on GitHub: https://github.com/simonpucher/AgenaTrader/blob/master/Utility/GlobalUtilities_Utility.cs
/// -------------------------------------------------------------------------
/// Namespace holds all indicators and is required. Do not change it.
/// </summary>
namespace AgenaTrader.UserCode
{
    public enum FindHighLowTimeFrame_Type
    {
        Session = 1,
        Candle = 2,
        BarsAvailable = 3
    }

	[Description("This indicator finds the high, middle and low value in a dedicated timeframe or the current session.")]
	public class FindHighLowTimeFrame_Indicator : UserIndicator
	{
        //input
        private FindHighLowTimeFrame_Type _FindHighLowTimeFrame_Type = FindHighLowTimeFrame_Type.Session;
        private int _opacity = Const.DefaultOpacity;
        private Color _currentsessionlinecolor = Const.DefaultIndicatorColor;
        private Color _col_timespan = Const.DefaultIndicatorColor;
        private int _currentsessionlinewidth = Const.DefaultLineWidth_small;
        private DashStyle _currentsessionlinestyle = Const.DefaultIndicatorDashStyle;
        private TimeSpan _tim_start = new TimeSpan(9, 0, 0);
        private TimeSpan _tim_end = new TimeSpan(16, 0, 0);
        private bool _UseDedicatedTimeSpan = false;
        private int _sessionsago = 0;
        private int _CandlesAgo = 1;

        private bool _IsDrawHighLineEnabled = true;
        private bool _IsDrawMiddleLineEnabled = true;
        private bool _IsDrawLowLineEnabled = true;
        private bool _IsDrawAreaplotEnabled = true;

        //output
        private double _lastlow = Double.NaN;
        private double _lasthigh = Double.NaN;
        private double _lastmiddle = Double.NaN;

        //internal
 

		protected override void Initialize()
		{
            CalculateOnBarClose = false;
            Overlay = true;
		}

		protected override void OnBarUpdate()
		{
            if (Bars != null && Bars.Count > 0 && this.IsCurrentBarLast)
            {
                this.calculateanddrawhighlowlines();
            }
		}


        /// <summary>
        /// Calculate and draw the high & low lines.
        /// </summary>
        private void calculateanddrawhighlowlines()
        {

            switch (FindHighLowTimeFrame_Type)
            {
                case FindHighLowTimeFrame_Type.Session:
                    //Draw it for the session
                    DateTime thesessiondatetime = Time[0];

                    if (this.Sessionsago != 0)
                    {
                        //todo subtract the date
                        IEnumerable<DateTime> datelist = Bars.Select(x => x.Time.Date).Distinct();
                        if (datelist.Count() - 1 < this.Sessionsago)
                        {
                            //do nothing
                            return;
                        }
                        else
                        {
                            thesessiondatetime = datelist.GetByIndex(datelist.Count() - 1 - this.Sessionsago);
                        }
                    }

                    //Default timeframe is this Session
                    DateTime start = new DateTime(thesessiondatetime.Year, thesessiondatetime.Month, thesessiondatetime.Day, 0, 0, 0);
                    DateTime end = new DateTime(thesessiondatetime.Year, thesessiondatetime.Month, thesessiondatetime.Day, 23, 59, 59);

                    //Override if we want this
                    if (this.UseDedicatedTimeSpan)
                    {
                        start = new DateTime(thesessiondatetime.Year, thesessiondatetime.Month, thesessiondatetime.Day, this.Time_Start.Hours, this.Time_Start.Minutes, this.Time_Start.Seconds);
                        end = new DateTime(thesessiondatetime.Year, thesessiondatetime.Month, thesessiondatetime.Day, this.Time_End.Hours, this.Time_End.Minutes, this.Time_End.Seconds);
                    }


                    //Select all data and find high & low.
                    IEnumerable<IBar> list = Bars.Where(x => x.Time >= start).Where(x => x.Time <= end);

                    //We save the high and low values in public variables to get access from other scripts
                    this.LastLow = list.Where(x => x.Low == list.Min(y => y.Low)).LastOrDefault().Low;
                    this.LastHigh = list.Where(x => x.High == list.Max(y => y.High)).LastOrDefault().High;
                    this.LastMiddle = this.LastLow + ((this.LastHigh - this.LastLow) / 2);


                    //Draw current lines for this day session
                    if (Time[0].Date == DateTime.Now.Date)
                    {
                        if (this.IsDrawLowLineEnabled)
                        {
                            DrawHorizontalLine("LowLine" + start.Ticks, this.AutoScale, this.LastLow, this.CurrentSessionLineColor, this.CurrentSessionLineStyle, this.CurrentSessionLineWidth);
                        }
                        if (this.IsDrawHighLineEnabled)
                        {
                            DrawHorizontalLine("HighLine" + start.Ticks, this.AutoScale, this.LastHigh, this.CurrentSessionLineColor, this.CurrentSessionLineStyle, this.CurrentSessionLineWidth);
                        }
                        if (this.IsDrawMiddleLineEnabled)
                        {
                            DrawHorizontalLine("MiddleLine" + start.Ticks, this.AutoScale, this.LastMiddle, this.CurrentSessionLineColor, this.CurrentSessionLineStyle, this.CurrentSessionLineWidth);
                        }
                    }

                    //Draw a rectangle at the dedicated time frame
                    if (this.IsDrawAreaplotEnabled)
                    {
                        DrawRectangle("HighLowRect" + start.Ticks, this.AutoScale, start, this.LastLow, end, this.LastHigh, this.Color_TimeSpan, this.Color_TimeSpan, this.Opacity);
                    }
                    break;
                case FindHighLowTimeFrame_Type.Candle:
                    //Draw it for the candles
                    if (Bars.Count() - 1 >= this.CandlesAgo)
                    {
                        DateTime startcandle = Bars[this.CandlesAgo].Time.Date;
                        double lastmiddle = Bars[this.CandlesAgo].Low + (Bars[this.CandlesAgo].Range / 2);
                        if (this.IsDrawMiddleLineEnabled)
                        {
                            DrawHorizontalLine("MiddleLineOnCandle" + startcandle.Ticks, this.AutoScale, lastmiddle, this.CurrentSessionLineColor, this.CurrentSessionLineStyle, this.CurrentSessionLineWidth);
                        } 
                    }
                    break;
                case FindHighLowTimeFrame_Type.BarsAvailable:
                    //Draw it for the candles
                    if (Bars.Count() >= 1)
                    {
                        //We save the high and low values in public variables to get access from other scripts
                        this.LastLow = Bars.Where(x => x.Low == Bars.Min(y => y.Low)).LastOrDefault().Low;
                        this.LastHigh = Bars.Where(x => x.High == Bars.Max(y => y.High)).LastOrDefault().High;
                        this.LastMiddle = this.LastLow + ((this.LastHigh - this.LastLow) / 2);

                        string datenow = DateTime.Now.ToString();
                        if (this.IsDrawLowLineEnabled)
                        {
                            DrawHorizontalLine("LowLine" + datenow, this.AutoScale, this.LastLow, this.CurrentSessionLineColor, this.CurrentSessionLineStyle, this.CurrentSessionLineWidth);
                        }
                        if (this.IsDrawHighLineEnabled)
                        {
                            DrawHorizontalLine("HighLine" + datenow, this.AutoScale, this.LastHigh, this.CurrentSessionLineColor, this.CurrentSessionLineStyle, this.CurrentSessionLineWidth);
                        }
                        if (this.IsDrawMiddleLineEnabled)
                        {
                            DrawHorizontalLine("MiddleLine" + datenow, this.AutoScale, this.LastMiddle, this.CurrentSessionLineColor, this.CurrentSessionLineStyle, this.CurrentSessionLineWidth);
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException("FindHighLowTimeFrame: enum FindHighLowTimeFrame_Type is not defined", null);
            }

        }


        public override string ToString()
        {
            return "FindHighMiddleLowTimeFrame";
        }

        public override string DisplayName
        {
            get
            {
                return "FindHighMiddleLowTimeFrame";
            }
        }



		#region Properties


        #region Input Parameters


        
        /// <summary>
        /// </summary>
        [Description("Candle will do measurement on candles. Session will to measurement on f.e. the last day.")]
        [Category("Parameters")]
        [DisplayName("0.0 Type")]
        public FindHighLowTimeFrame_Type FindHighLowTimeFrame_Type
        {
            get { return _FindHighLowTimeFrame_Type; }
            set { _FindHighLowTimeFrame_Type = value; }
        }


        /// <summary>
        /// </summary>
        [Description("0 is for the current session. 1 is the session yesterday. And so on.")]
        [Category("Parameters")]
        [DisplayName("1.0 Sessions ago")]
        public int Sessionsago
        {
            get { return _sessionsago; }
            set { _sessionsago = value; }
        }

        /// <summary>
        /// </summary>
        [Description("If true the you are able to specify a dedicated time span.")]
        [Category("Parameters")]
        [DisplayName("1.1 Use dedicated time span")]
        public bool UseDedicatedTimeSpan
        {
            get { return _UseDedicatedTimeSpan; }
            set { _UseDedicatedTimeSpan = value; }
        }
        
        /// <summary>
        /// </summary>
        [Description("The start time of the time frame.")]
        [Category("Parameters")]
        [DisplayName("1.2 Start")]
        public TimeSpan Time_Start
        {
            get { return _tim_start; }
            set { _tim_start = value; }
        }
        [Browsable(false)]
        public long Time_StartSerialize
        {
            get { return _tim_start.Ticks; }
            set { _tim_start = new TimeSpan(value); }
        }

        /// <summary>
        /// </summary>
        [Description("The end time of the time frame.")]
        [Category("Parameters")]
        [DisplayName("1.3 End")]
        public TimeSpan Time_End
        {
            get { return _tim_end; }
            set { _tim_end = value; }
        }
        [Browsable(false)]
        public long Time_EndSerialize
        {
            get { return _tim_end.Ticks; }
            set { _tim_end = new TimeSpan(value); }
        }
        

        /// <summary>
        /// </summary>
        [Description("0 is for the current candle. 1 is the candle yesterday. And so on.")]
        [Category("Parameters")]
        [DisplayName("2.0 Candles ago")]
        public int CandlesAgo
        {
            get { return _CandlesAgo; }
            set { _CandlesAgo = value; }
        }




        #endregion


        #region Input Drawings
        
        /// <summary>
        /// </summary>
        [Description("Opacity for Drawing")]
        [Category("Drawing")]
        [DisplayName("Opacity")]
        public int Opacity
        {
            get { return _opacity; }
            set
            {
                if (value >= 1 && value <= 100)
                {
                    _opacity = value;
                }
                else
                {
                    _opacity = Const.DefaultOpacity;
                }
            }
        }



        [XmlIgnore()]
        [Description("Select color for the current session")]
        [Category("Drawing")]
        [DisplayName("Current session color")]
        public Color CurrentSessionLineColor
        {
            get { return _currentsessionlinecolor; }
            set { _currentsessionlinecolor = value; }
        }

        [Browsable(false)]
        public string CurrentSessionLineColorSerialize
        {
            get { return SerializableColor.ToString(_currentsessionlinecolor); }
            set { _currentsessionlinecolor = SerializableColor.FromString(value); }
        }

        /// <summary>
        /// </summary>
        [Description("Width for the line of the current session.")]
        [Category("Drawing")]
        [DisplayName("Line Width current session")]
        public int CurrentSessionLineWidth
        {
            get { return _currentsessionlinewidth; }
            set { _currentsessionlinewidth = Math.Max(1, value); }
        }


        /// <summary>
        /// </summary>
        [Description("DashStyle for line of the current session.")]
        [Category("Drawing")]
        [DisplayName("Dash Style current session")]
        public DashStyle CurrentSessionLineStyle
        {
            get { return _currentsessionlinestyle; }
            set { _currentsessionlinestyle = value; }
        }


        /// <summary>
        /// </summary>
        [Description("Time Span Color")]
        [Category("Drawing")]
        [DisplayName("Time Span")]
        public Color Color_TimeSpan
        {
            get { return _col_timespan; }
            set { _col_timespan = value; }
        }

        [Browsable(false)]
        public string Color_TimeSpanSerialize
        {
            get { return SerializableColor.ToString(_col_timespan); }
            set { _col_timespan = SerializableColor.FromString(value); }
        }



        /// <summary>
        /// </summary>
        [Description("If true the you will see the high line.")]
        [Category("Drawing")]
        [DisplayName("Show high line")]
        public bool IsDrawHighLineEnabled
        {
            get { return _IsDrawHighLineEnabled; }
            set { _IsDrawHighLineEnabled = value; }
        }

        /// <summary>
        /// </summary>
        [Description("If true the you will see the middle line.")]
        [Category("Drawing")]
        [DisplayName("Show middle line")]
        public bool IsDrawMiddleLineEnabled
        {
            get { return _IsDrawMiddleLineEnabled; }
            set { _IsDrawMiddleLineEnabled = value; }
        }

        /// <summary>
        /// </summary>
        [Description("If true the you will see the low line.")]
        [Category("Drawing")]
        [DisplayName("Show low line")]
        public bool IsDrawLowLineEnabled
        {
            get { return _IsDrawLowLineEnabled; }
            set { _IsDrawLowLineEnabled = value; }
        }

        /// <summary>
        /// </summary>
        [Description("If true the you will see an area plot on the chart.")]
        [Category("Drawing")]
        [DisplayName("Show area plot")]
        public bool IsDrawAreaplotEnabled
        {
            get { return _IsDrawAreaplotEnabled; }
            set { _IsDrawAreaplotEnabled = value; }
        }

        #endregion



        #region Output





        /// <summary>
        /// Last middle value in dedicated time frame.
        /// </summary>
        [Browsable(false)]
        [XmlIgnore()]
        public double LastMiddle
        {
            get { return _lastmiddle; }
            set { _lastmiddle = value; }
        }

            /// <summary>
        /// Last low value in dedicated time frame.
            /// </summary>
            [Browsable(false)]
            [XmlIgnore()]
            public double LastLow
            {
                get { return _lastlow; }
                set { _lastlow = value; }
            }

            /// <summary>
            /// Last high value in dedicated time frame.
            /// </summary>
            [Browsable(false)]
            [XmlIgnore()]
            public double LastHigh
            {
                get { return _lasthigh; }
                set { _lasthigh = value; }
            }

        #endregion

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
		/// This indicator finds the high, middle and low value in a dedicated timeframe or the current session.
		/// </summary>
		public FindHighLowTimeFrame_Indicator FindHighLowTimeFrame_Indicator(FindHighLowTimeFrame_Type findHighLowTimeFrame_Type, System.Int32 sessionsago, System.Boolean useDedicatedTimeSpan, TimeSpan time_Start, TimeSpan time_End, System.Int32 candlesAgo)
        {
			return FindHighLowTimeFrame_Indicator(Input, findHighLowTimeFrame_Type, sessionsago, useDedicatedTimeSpan, time_Start, time_End, candlesAgo);
		}

		/// <summary>
		/// This indicator finds the high, middle and low value in a dedicated timeframe or the current session.
		/// </summary>
		public FindHighLowTimeFrame_Indicator FindHighLowTimeFrame_Indicator(IDataSeries input, FindHighLowTimeFrame_Type findHighLowTimeFrame_Type, System.Int32 sessionsago, System.Boolean useDedicatedTimeSpan, TimeSpan time_Start, TimeSpan time_End, System.Int32 candlesAgo)
		{
			var indicator = CachedCalculationUnits.GetCachedIndicator<FindHighLowTimeFrame_Indicator>(input, i => i.FindHighLowTimeFrame_Type == findHighLowTimeFrame_Type && i.Sessionsago == sessionsago && i.UseDedicatedTimeSpan == useDedicatedTimeSpan && i.Time_Start == time_Start && i.Time_End == time_End && i.CandlesAgo == candlesAgo);

			if (indicator != null)
				return indicator;

			indicator = new FindHighLowTimeFrame_Indicator
						{
							BarsRequired = BarsRequired,
							CalculateOnBarClose = CalculateOnBarClose,
							Input = input,
							FindHighLowTimeFrame_Type = findHighLowTimeFrame_Type,
							Sessionsago = sessionsago,
							UseDedicatedTimeSpan = useDedicatedTimeSpan,
							Time_Start = time_Start,
							Time_End = time_End,
							CandlesAgo = candlesAgo
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
		/// This indicator finds the high, middle and low value in a dedicated timeframe or the current session.
		/// </summary>
		public FindHighLowTimeFrame_Indicator FindHighLowTimeFrame_Indicator(FindHighLowTimeFrame_Type findHighLowTimeFrame_Type, System.Int32 sessionsago, System.Boolean useDedicatedTimeSpan, TimeSpan time_Start, TimeSpan time_End, System.Int32 candlesAgo)
		{
			return LeadIndicator.FindHighLowTimeFrame_Indicator(Input, findHighLowTimeFrame_Type, sessionsago, useDedicatedTimeSpan, time_Start, time_End, candlesAgo);
		}

		/// <summary>
		/// This indicator finds the high, middle and low value in a dedicated timeframe or the current session.
		/// </summary>
		public FindHighLowTimeFrame_Indicator FindHighLowTimeFrame_Indicator(IDataSeries input, FindHighLowTimeFrame_Type findHighLowTimeFrame_Type, System.Int32 sessionsago, System.Boolean useDedicatedTimeSpan, TimeSpan time_Start, TimeSpan time_End, System.Int32 candlesAgo)
		{
			if (InInitialize && input == null)
				throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

			return LeadIndicator.FindHighLowTimeFrame_Indicator(input, findHighLowTimeFrame_Type, sessionsago, useDedicatedTimeSpan, time_Start, time_End, candlesAgo);
		}
	}

	#endregion

	#region Column

	public partial class UserColumn
	{
		/// <summary>
		/// This indicator finds the high, middle and low value in a dedicated timeframe or the current session.
		/// </summary>
		public FindHighLowTimeFrame_Indicator FindHighLowTimeFrame_Indicator(FindHighLowTimeFrame_Type findHighLowTimeFrame_Type, System.Int32 sessionsago, System.Boolean useDedicatedTimeSpan, TimeSpan time_Start, TimeSpan time_End, System.Int32 candlesAgo)
		{
			return LeadIndicator.FindHighLowTimeFrame_Indicator(Input, findHighLowTimeFrame_Type, sessionsago, useDedicatedTimeSpan, time_Start, time_End, candlesAgo);
		}

		/// <summary>
		/// This indicator finds the high, middle and low value in a dedicated timeframe or the current session.
		/// </summary>
		public FindHighLowTimeFrame_Indicator FindHighLowTimeFrame_Indicator(IDataSeries input, FindHighLowTimeFrame_Type findHighLowTimeFrame_Type, System.Int32 sessionsago, System.Boolean useDedicatedTimeSpan, TimeSpan time_Start, TimeSpan time_End, System.Int32 candlesAgo)
		{
			return LeadIndicator.FindHighLowTimeFrame_Indicator(input, findHighLowTimeFrame_Type, sessionsago, useDedicatedTimeSpan, time_Start, time_End, candlesAgo);
		}
	}

	#endregion

	#region Scripted Condition

	public partial class UserScriptedCondition
	{
		/// <summary>
		/// This indicator finds the high, middle and low value in a dedicated timeframe or the current session.
		/// </summary>
		public FindHighLowTimeFrame_Indicator FindHighLowTimeFrame_Indicator(FindHighLowTimeFrame_Type findHighLowTimeFrame_Type, System.Int32 sessionsago, System.Boolean useDedicatedTimeSpan, TimeSpan time_Start, TimeSpan time_End, System.Int32 candlesAgo)
		{
			return LeadIndicator.FindHighLowTimeFrame_Indicator(Input, findHighLowTimeFrame_Type, sessionsago, useDedicatedTimeSpan, time_Start, time_End, candlesAgo);
		}

		/// <summary>
		/// This indicator finds the high, middle and low value in a dedicated timeframe or the current session.
		/// </summary>
		public FindHighLowTimeFrame_Indicator FindHighLowTimeFrame_Indicator(IDataSeries input, FindHighLowTimeFrame_Type findHighLowTimeFrame_Type, System.Int32 sessionsago, System.Boolean useDedicatedTimeSpan, TimeSpan time_Start, TimeSpan time_End, System.Int32 candlesAgo)
		{
			return LeadIndicator.FindHighLowTimeFrame_Indicator(input, findHighLowTimeFrame_Type, sessionsago, useDedicatedTimeSpan, time_Start, time_End, candlesAgo);
		}
	}

	#endregion

}

#endregion