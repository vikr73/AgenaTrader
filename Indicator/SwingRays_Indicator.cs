using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
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
/// The indicator was taken from: http://ninjatrader.com/support/forum/showthread.php?t=37759
/// Code was generated by AgenaTrader conversion tool and modified by Simon Pucher.
/// -------------------------------------------------------------------------
/// ****** Important ******
/// To compile this script without any error you also need access to the utility indicator to use these global source code elements.
/// You will find this indicator on GitHub: https://github.com/simonpucher/AgenaTrader/blob/master/Utility/GlobalUtilities_Utility.cs
/// -------------------------------------------------------------------------
/// Namespace holds all indicators and is required. Do not change it.
/// </summary>
namespace AgenaTrader.UserCode
{
    /// <summary>
    /// Plots horizontal rays at swing highs and lows and removes them once broken. 
    /// </summary>
    [Description("Plots horizontal rays at swing highs and lows and removes them once broken.")]
    public class SwingRays : UserIndicator
    {
        #region Variables
        // Wizard generated variables
        private int strength = 5; // number of bars required to left and right of the pivot high/low
                                  // User defined variables (add any user defined variables below)
        private Color swingHighColor = Color.DarkCyan;
        private Color swingLowColor = Color.Magenta;

        private ArrayList lastHighCache;
        private ArrayList lastLowCache;
        private double lastSwingHighValue = double.MaxValue; // used when testing for price breaks
        private double lastSwingLowValue = double.MinValue;
        private Stack swingHighRays;    //	last entry contains nearest swing high; removed when swing is broken
        private Stack swingLowRays; // track swing lows in the same manner
        private bool enableAlerts = false;
        private bool keepBrokenLines = true;

        private Soundfile _soundfile = Soundfile.Blip;

        #endregion

        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void Initialize()
        {
            DisplayInDataBox = false;
            CalculateOnBarClose = true;
            Overlay = true;
            PriceTypeSupported = false;

            lastHighCache = new ArrayList(); // used to identify swing points; from default Swing indicator
            lastLowCache = new ArrayList();
            swingHighRays = new Stack(); // LIFO buffer; last entry contains the nearest swing high
            swingLowRays = new Stack();
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        { 
            // build up cache of recent High and Low values
            // code devised from default Swing Indicator by marqui@BMT, 10-NOV-2010 
            lastHighCache.Add(High[0]);
            if (lastHighCache.Count > (2 * strength) + 1)
                lastHighCache.RemoveAt(0); // if cache is filled, drop the oldest value
            lastLowCache.Add(Low[0]);
            if (lastLowCache.Count > (2 * strength) + 1)
                lastLowCache.RemoveAt(0);
            //
            if (lastHighCache.Count == (2 * strength) + 1) // wait for cache of Highs to be filled
            {
                // test for swing high 
                bool isSwingHigh = true;
                double swingHighCandidateValue = (double)lastHighCache[strength];
                for (int i = 0; i < strength; i++)
                    if ((double)lastHighCache[i] >= swingHighCandidateValue - double.Epsilon)
                        isSwingHigh = false; // bar(s) to right of candidate were higher
                for (int i = strength + 1; i < lastHighCache.Count; i++)
                    if ((double)lastHighCache[i] > swingHighCandidateValue - double.Epsilon)
                        isSwingHigh = false; // bar(s) to left of candidate were higher
                                             // end of test

                if (isSwingHigh)
                    lastSwingHighValue = swingHighCandidateValue;

                if (isSwingHigh) // if we have a new swing high then we draw a ray line on the chart
                {
                    IRay newRay = DrawRay("highRay" + (CurrentBar - strength), false, strength, lastSwingHighValue, 0, lastSwingHighValue, swingHighColor, DashStyle.Dot, 2);
                    swingHighRays.Push(newRay); // store a reference so we can remove it from the chart later
                }
                else if (High[0] > lastSwingHighValue) // otherwise, we test to see if price has broken through prior swing high
                {
                    if (swingHighRays.Count > 0) // just to be safe 
                    {
                        IRay currentRay = (IRay)swingHighRays.Pop(); // pull current ray from stack 
                        //if (enableAlerts) Alert("SwHiAlert", AlertPriority.Low, "Swing High at " + currentRay.Y1 + " broken", "Alert2.wav", 5, Color.White, Color.Red);
                        if (enableAlerts)
                        {
                            Alert("Swing High at " + currentRay.Y1 + " broken", GlobalUtilities.GetSoundfile(this.Soundfile));
                        }
                        if (keepBrokenLines) // draw a line between swing point and break bar 
                        {
                           int barsAgo = currentRay.BarsAgo1;
                            ITrendLine newLine = DrawLine("highLine" + (CurrentBar - barsAgo), false, barsAgo, currentRay.Y1, 0, currentRay.Y1, swingHighColor, DashStyle.Solid, 2);
                        }
                        RemoveDrawObject(currentRay.Tag);
                        if (swingHighRays.Count > 0)
                        {
                            IRay priorRay = (IRay)swingHighRays.Peek();
                           lastSwingHighValue = priorRay.Y1; // needed when testing the break of the next swing high
                        }
                        else
                            lastSwingHighValue = double.MaxValue; // there are no higher swings on the chart; reset to default	
                    }
                }
            }

            if (lastLowCache.Count == (2 * strength) + 1) // repeat the above for the swing lows
            {
                // test for swing low 
                bool isSwingLow = true;
                double swingLowCandidateValue = (double)lastLowCache[strength];
                for (int i = 0; i < strength; i++)
                    if ((double)lastLowCache[i] <= swingLowCandidateValue + double.Epsilon)
                        isSwingLow = false; // bar(s) to right of candidate were lower

                for (int i = strength + 1; i < lastLowCache.Count; i++)
                    if ((double)lastLowCache[i] < swingLowCandidateValue + double.Epsilon)
                        isSwingLow = false; // bar(s) to left of candidate were lower
                                            // end of test for low

                if (isSwingLow)
                    lastSwingLowValue = swingLowCandidateValue;

                if (isSwingLow) // found a new swing low; draw it on the chart
                {
                    IRay newRay = DrawRay("lowRay" + (CurrentBar - strength), false, strength, lastSwingLowValue, 0, lastSwingLowValue, swingLowColor, DashStyle.Dot, 2);
                    swingLowRays.Push(newRay);
                }
                else if (Low[0] < lastSwingLowValue) // otherwise test to see if price has broken through prior swing low
                {
                    if (swingLowRays.Count > 0)
                    {
                        IRay currentRay = (IRay)swingLowRays.Pop();
                        //if (enableAlerts) Alert("SwHiAlert", AlertPriority.Low, "Swing Low at " + currentRay.Y1 + " broken", "Alert2.wav", 5, Color.White, Color.Red);
                        if (enableAlerts) {
                            Alert("Swing Low at " + currentRay.Y1 + " broken", GlobalUtilities.GetSoundfile(this.Soundfile));
                        }
                        if (keepBrokenLines) // draw a line between swing point and break bar 
                        {
                            int barsAgo = currentRay.BarsAgo1;
                           ITrendLine newLine = DrawLine("highLine" + (CurrentBar - barsAgo), false, barsAgo, currentRay.Y1, 0, currentRay.Y1, swingLowColor, DashStyle.Solid, 2);
                        }
                        RemoveDrawObject(currentRay.Tag);

                        if (swingLowRays.Count > 0)
                        {
                            IRay priorRay = (IRay)swingLowRays.Peek();
                           lastSwingLowValue = priorRay.Y1; // price level of the prior swing low 
                        }
                        else
                            lastSwingLowValue = double.MinValue; // no swing lows present; set this to default value 
                    }
                }
            }
        }


        public override string ToString()
        {
            return "SwingRays";
        }

        public override string DisplayName
        {
            get
            {
                return "SwingRays";
            }
        }

        
        #region Input properties

        [Description("Number of bars before/after each pivot bar")]
            [Category("Parameters")]
            [DisplayName("Strength")]
            public int Strength
            {
                get { return strength; }
                set { strength = Math.Max(2, value); }
            }

            [Description("Alert when swings are broken")]
            [Category("Parameters")]
            [DisplayName("EnableAlerts")]
            public bool EnableAlerts
            {
                get { return enableAlerts; }
                set { enableAlerts = value; }
            }

            [Description("Show broken swing points")]
            [Category("Parameters")]
            [DisplayName("Keep broken lines")]
            public bool KeepBrokenLines
            {
                get { return keepBrokenLines; }
                set { keepBrokenLines = value; }
            }

            [XmlIgnore()]
            [Description("Color for swing highs")]
            [Category("Parameters")]
            [DisplayName("Swing High Color")]
            public Color SwingHighColor
            {
                get { return swingHighColor; }
                set { swingHighColor = value; }
            }

            // Serialize our Color object
            [Browsable(false)]
            public string SwingHighColorSerialize
            {
                get { return SerializableColor.ToString(swingHighColor); }
                set { swingHighColor = SerializableColor.FromString(value); }
            }

            [XmlIgnore()]
            [Description("Color for swing lows")]
            [Category("Parameters")]
            [DisplayName("Swing Low Color")]
            public Color SwingLowColor
            {
                get { return swingLowColor; }
                set { swingLowColor = value; }
            }

            // Serialize our Color object	
            [Browsable(false)]
            public string SwingLowColorSerialize
            {
                get { return SerializableColor.ToString(swingLowColor); }
                set { swingLowColor = SerializableColor.FromString(value); }
            }

            [XmlIgnore()]
            [Description("Select the soundfile for the alert.")]
            [Category("Parameters")]
            [DisplayName("Soundfile name")]
            public Soundfile Soundfile
            {
                get { return _soundfile; }
                set { _soundfile = value; }
            }

        #endregion

        #region Output properties

        [Browsable(false)]  // this line prevents the data series from being displayed in the indicator properties dialog, do not remove
            [XmlIgnore()]   // this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
            public DataSeries HighRay
            {
                get { return Values[0]; }
            }

            [Browsable(false)]  // this line prevents the data series from being displayed in the indicator properties dialog, do not remove
            [XmlIgnore()]   // this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
            public DataSeries LowRay
            {
                get { return Values[1]; }
            }

            [Browsable(false)]  // this line prevents the data series from being displayed in the indicator properties dialog, do not remove
            [XmlIgnore()]   // this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
            public DataSeries HighLine
            {
                get { return Values[2]; }
            }

            [Browsable(false)]  // this line prevents the data series from being displayed in the indicator properties dialog, do not remove
            [XmlIgnore()]   // this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
            public DataSeries LowLine
            {
                get { return Values[3]; }
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
		/// Plots horizontal rays at swing highs and lows and removes them once broken.
		/// </summary>
		public SwingRays SwingRays(System.Int32 strength, System.Boolean enableAlerts, System.Boolean keepBrokenLines, Color swingHighColor, Color swingLowColor, Soundfile soundfile)
        {
			return SwingRays(Input, strength, enableAlerts, keepBrokenLines, swingHighColor, swingLowColor, soundfile);
		}

		/// <summary>
		/// Plots horizontal rays at swing highs and lows and removes them once broken.
		/// </summary>
		public SwingRays SwingRays(IDataSeries input, System.Int32 strength, System.Boolean enableAlerts, System.Boolean keepBrokenLines, Color swingHighColor, Color swingLowColor, Soundfile soundfile)
		{
			var indicator = CachedCalculationUnits.GetCachedIndicator<SwingRays>(input, i => i.Strength == strength && i.EnableAlerts == enableAlerts && i.KeepBrokenLines == keepBrokenLines && i.SwingHighColor == swingHighColor && i.SwingLowColor == swingLowColor && i.Soundfile == soundfile);

			if (indicator != null)
				return indicator;

			indicator = new SwingRays
						{
							BarsRequired = BarsRequired,
							CalculateOnBarClose = CalculateOnBarClose,
							Input = input,
							Strength = strength,
							EnableAlerts = enableAlerts,
							KeepBrokenLines = keepBrokenLines,
							SwingHighColor = swingHighColor,
							SwingLowColor = swingLowColor,
							Soundfile = soundfile
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
		/// Plots horizontal rays at swing highs and lows and removes them once broken.
		/// </summary>
		public SwingRays SwingRays(System.Int32 strength, System.Boolean enableAlerts, System.Boolean keepBrokenLines, Color swingHighColor, Color swingLowColor, Soundfile soundfile)
		{
			return LeadIndicator.SwingRays(Input, strength, enableAlerts, keepBrokenLines, swingHighColor, swingLowColor, soundfile);
		}

		/// <summary>
		/// Plots horizontal rays at swing highs and lows and removes them once broken.
		/// </summary>
		public SwingRays SwingRays(IDataSeries input, System.Int32 strength, System.Boolean enableAlerts, System.Boolean keepBrokenLines, Color swingHighColor, Color swingLowColor, Soundfile soundfile)
		{
			if (InInitialize && input == null)
				throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

			return LeadIndicator.SwingRays(input, strength, enableAlerts, keepBrokenLines, swingHighColor, swingLowColor, soundfile);
		}
	}

	#endregion

	#region Column

	public partial class UserColumn
	{
		/// <summary>
		/// Plots horizontal rays at swing highs and lows and removes them once broken.
		/// </summary>
		public SwingRays SwingRays(System.Int32 strength, System.Boolean enableAlerts, System.Boolean keepBrokenLines, Color swingHighColor, Color swingLowColor, Soundfile soundfile)
		{
			return LeadIndicator.SwingRays(Input, strength, enableAlerts, keepBrokenLines, swingHighColor, swingLowColor, soundfile);
		}

		/// <summary>
		/// Plots horizontal rays at swing highs and lows and removes them once broken.
		/// </summary>
		public SwingRays SwingRays(IDataSeries input, System.Int32 strength, System.Boolean enableAlerts, System.Boolean keepBrokenLines, Color swingHighColor, Color swingLowColor, Soundfile soundfile)
		{
			return LeadIndicator.SwingRays(input, strength, enableAlerts, keepBrokenLines, swingHighColor, swingLowColor, soundfile);
		}
	}

	#endregion

	#region Scripted Condition

	public partial class UserScriptedCondition
	{
		/// <summary>
		/// Plots horizontal rays at swing highs and lows and removes them once broken.
		/// </summary>
		public SwingRays SwingRays(System.Int32 strength, System.Boolean enableAlerts, System.Boolean keepBrokenLines, Color swingHighColor, Color swingLowColor, Soundfile soundfile)
		{
			return LeadIndicator.SwingRays(Input, strength, enableAlerts, keepBrokenLines, swingHighColor, swingLowColor, soundfile);
		}

		/// <summary>
		/// Plots horizontal rays at swing highs and lows and removes them once broken.
		/// </summary>
		public SwingRays SwingRays(IDataSeries input, System.Int32 strength, System.Boolean enableAlerts, System.Boolean keepBrokenLines, Color swingHighColor, Color swingLowColor, Soundfile soundfile)
		{
			return LeadIndicator.SwingRays(input, strength, enableAlerts, keepBrokenLines, swingHighColor, swingLowColor, soundfile);
		}
	}

	#endregion

}

#endregion
