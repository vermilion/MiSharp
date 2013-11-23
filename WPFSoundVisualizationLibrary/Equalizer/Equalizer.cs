using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WPFSoundVisualizationLib
{
    /// <summary>
    ///     A control that displays and edits banded frequency amplification.
    /// </summary>
    [DisplayName("Equalizer")]
    [Description("Displays and edits banded frequency amplification.")]
    [ToolboxItem(true)]
    [TemplatePart(Name = "PART_EqualizerGrid", Type = typeof (Grid))]
    public class Equalizer : Control
    {
        #region Fields

        private readonly List<Slider> _sliders = new List<Slider>();
        private readonly List<TextBlock> _captions = new List<TextBlock>();
        private Grid _equalizerGrid;

        #endregion

        #region DependencyProperties

        #region EqualizerData

        /// <summary>
        ///     Identifies the <see cref="EqualizerData" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty EqualizerDataProperty =
            DependencyProperty.Register("EqualizerData", typeof(List<EqualizerParam>), typeof(Equalizer),
                                        new FrameworkPropertyMetadata(new List<EqualizerParam>(),
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnEqualizerDataChanged,
                                                                      OnCoerceEqualizerData));

        /// <summary>
        ///     Gets or sets the captions of each equalizer band.
        /// </summary>
        /// <remarks>
        ///     The number of elements in the EqualizerValues array must be equal to the number of bands. If not, all values
        ///     will be set to zero.
        /// </remarks>
        [Category("Common")]
        public List<EqualizerParam> EqualizerData
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get { return (List<EqualizerParam>)GetValue(EqualizerDataProperty); }
            set { SetValue(EqualizerDataProperty, value); }
        }

        private static object OnCoerceEqualizerData(DependencyObject o, object value)
        {
            var equalizer = o as Equalizer;
            if (equalizer != null)
                return equalizer.OnCoerceEqualizerData((List<EqualizerParam>)value);
            return value;
        }

        private static void OnEqualizerDataChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var equalizer = o as Equalizer;
            if (equalizer != null)
                equalizer.OnEqualizerDataChanged((List<EqualizerParam>)e.OldValue, (List<EqualizerParam>)e.NewValue);
        }

        /// <summary>
        ///     Coerces the value of <see cref="EqualizerData" /> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="EqualizerData" /></param>
        /// <returns>The adjusted value of <see cref="EqualizerData" /></returns>
        protected virtual List<EqualizerParam> OnCoerceEqualizerData(List<EqualizerParam> value)
        {
            if (value == null)
                return new List<EqualizerParam>(EqualizerData);
            return value;
        }

        /// <summary>
        ///     Called after the <see cref="EqualizerData" /> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="EqualizerData" /></param>
        /// <param name="newValue">The new value of <see cref="EqualizerData" /></param>
        protected virtual void OnEqualizerDataChanged(List<EqualizerParam> oldValue, List<EqualizerParam> newValue)
        {
            CreateSliders();
            if (newValue == null || newValue.Count != EqualizerData.Count)
                SetEqualizerData(new List<EqualizerParam>(EqualizerData));
            else
                SetEqualizerData(newValue);
        }

        #endregion

        #endregion

        #region Constructors

        static Equalizer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (Equalizer),
                new FrameworkPropertyMetadata(typeof (Equalizer)));
        }

        /// <summary>
        ///     Creates an instance of Equalizer.
        /// </summary>
        public Equalizer()
        {
            CreateSliders();
        }

        #endregion

        #region Template Overrides

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code
        ///     or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_equalizerGrid != null)
                _equalizerGrid.Children.Clear();

            _equalizerGrid = GetTemplateChild("PART_EqualizerGrid") as Grid;

            CreateSliders();
        }

        /// <summary>
        ///     Called whenever the control's template changes.
        /// </summary>
        /// <param name="oldTemplate">The old template</param>
        /// <param name="newTemplate">The new template</param>
        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
        }

        #endregion

        #region Private Utility Methods

        private void CreateSliders()
        {
            if (_equalizerGrid == null)
                return;

            ClearSliders();
            _equalizerGrid.ColumnDefinitions.Clear();
            _equalizerGrid.RowDefinitions.Clear();

            var channelRow0 = new RowDefinition
            {
                Height = new GridLength(5.0d, GridUnitType.Star)
            };
            var channelRow = new RowDefinition
            {
                Height = new GridLength(14.0d, GridUnitType.Pixel)
            };
            _equalizerGrid.RowDefinitions.Add(channelRow0);
            _equalizerGrid.RowDefinitions.Add(channelRow);
            for (int i = 0; i < EqualizerData.Count; i++)
            {
                var channelColumn = new ColumnDefinition
                {
                    Width = new GridLength(10.0d, GridUnitType.Star)
                };

                _equalizerGrid.ColumnDefinitions.Add(channelColumn);

                var slider = new Slider
                    {
                        //Style = (Style) FindResource("EqualizerSlider"), //TODO: DynamicResource as Metro in styles
                        Minimum = 0,
                        Maximum = 2,
                        TickFrequency = 0.1,
                        LargeChange = 0.2,
                        SmallChange = 0.1,
                        Ticks = new DoubleCollection {0, 0.2, 0.4, 0.6, 0.8, 1, 1.2, 1.4, 1.6, 1.8, 2},
                        TickPlacement = TickPlacement.BottomRight,
                        Orientation = Orientation.Vertical,
                        SnapsToDevicePixels = true,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        IsMoveToPointEnabled = true
                    };

                var caption = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = EqualizerData[i].Center < 1000
                                   ? EqualizerData[i].Center.ToString(CultureInfo.InvariantCulture)
                                   : string.Format("{0}K", (EqualizerData[i].Center/1000))
                    };

                Grid.SetColumn(slider, i);
                Grid.SetRow(slider, 0);
                Grid.SetColumn(caption, i);
                Grid.SetRow(caption, 1);
                _sliders.Add(slider);
                _captions.Add(caption);
                _equalizerGrid.Children.Add(slider);
                _equalizerGrid.Children.Add(caption);
                slider.ValueChanged += SliderValueChanged;
            }
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = (Slider) sender;
            var sliderIndex = _sliders.IndexOf(slider);
            EqualizerData[sliderIndex].Gain = (float) slider.Value;
            EqualizerData = EqualizerData;
        }

        private void ClearSliders()
        {
            foreach (Slider slider in _sliders)
            {
                slider.ValueChanged -= SliderValueChanged;
                _equalizerGrid.Children.Remove(slider);
            }
            _sliders.Clear();

            foreach (TextBlock caption in _captions)
            {
                _equalizerGrid.Children.Remove(caption);
            }
            _captions.Clear();
        }

        private void SetEqualizerData(List<EqualizerParam> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                _sliders[i].Value = values[i].Gain;
                _captions[i].Text = values[i].Center < 1000
                                        ? values[i].Center.ToString(CultureInfo.InvariantCulture)
                                        : string.Format("{0}K", (values[i].Center/1000));
            }
        }

        #endregion
    }
}