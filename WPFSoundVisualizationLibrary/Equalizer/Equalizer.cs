using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

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

        #region EqualizerValues

        /// <summary>
        ///     Identifies the <see cref="EqualizerValues" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty EqualizerValuesProperty =
            DependencyProperty.Register("EqualizerValues", typeof (float[]), typeof (Equalizer),
                                        new FrameworkPropertyMetadata(new[] {0f, 0f, 0f, 0f, 0f, 0f, 0f},
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnEqualizerValuesChanged,
                                                                      OnCoerceEqualizerValues));

        /// <summary>
        ///     Gets or sets the values of each equalizer band.
        /// </summary>
        /// <remarks>
        ///     The number of elements in the EqualizerValues array must be equal to the number of bands. If not, all values
        ///     will be set to zero.
        /// </remarks>
        [Category("Common")]
        public float[] EqualizerValues
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get { return (float[]) GetValue(EqualizerValuesProperty); }
            set { SetValue(EqualizerValuesProperty, value); }
        }

        private static object OnCoerceEqualizerValues(DependencyObject o, object value)
        {
            var equalizer = o as Equalizer;
            if (equalizer != null)
                return equalizer.OnCoerceEqualizerValues((float[]) value);
            return value;
        }

        private static void OnEqualizerValuesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var equalizer = o as Equalizer;
            if (equalizer != null)
                equalizer.OnEqualizerValuesChanged((float[]) e.OldValue, (float[]) e.NewValue);
        }

        /// <summary>
        ///     Coerces the value of <see cref="EqualizerValues" /> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="EqualizerValues" /></param>
        /// <returns>The adjusted value of <see cref="EqualizerValues" /></returns>
        protected virtual float[] OnCoerceEqualizerValues(float[] value)
        {
            if (value == null || value.Length != EqualizerCaptions.Length)
                return new float[EqualizerCaptions.Length];
            return value;
        }

        /// <summary>
        ///     Called after the <see cref="EqualizerValues" /> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="EqualizerValues" /></param>
        /// <param name="newValue">The new value of <see cref="EqualizerValues" /></param>
        protected virtual void OnEqualizerValuesChanged(float[] oldValue, float[] newValue)
        {
            if (newValue == null || newValue.Length != EqualizerCaptions.Length)
                SetEqualizerValues(new float[EqualizerCaptions.Length]);
            else
                SetEqualizerValues(newValue);
        }

        #endregion

        #region EqualizerCaptions

        /// <summary>
        ///     Identifies the <see cref="EqualizerValues" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty EqualizerCaptionsProperty =
            DependencyProperty.Register("EqualizerCaptions", typeof (float[]), typeof (Equalizer),
                                        new UIPropertyMetadata(new[] {0f, 0f, 0f, 0f, 0f, 0f, 0f},
                                                               OnEqualizerCaptionsChanged,
                                                               OnCoerceEqualizerCaptions));

        /// <summary>
        ///     Gets or sets the captions of each equalizer band.
        /// </summary>
        /// <remarks>
        ///     The number of elements in the EqualizerValues array must be equal to the number of bands. If not, all values
        ///     will be set to zero.
        /// </remarks>
        [Category("Common")]
        public float[] EqualizerCaptions
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get { return (float[])GetValue(EqualizerCaptionsProperty); }
            set { SetValue(EqualizerCaptionsProperty, value); }
        }

        private static object OnCoerceEqualizerCaptions(DependencyObject o, object value)
        {
            var equalizer = o as Equalizer;
            if (equalizer != null)
                return equalizer.OnCoerceEqualizerCaptions((float[])value);
            return value;
        }

        private static void OnEqualizerCaptionsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var equalizer = o as Equalizer;
            if (equalizer != null)
                equalizer.OnEqualizerCaptionsChanged((float[])e.OldValue, (float[])e.NewValue);
        }

        /// <summary>
        ///     Coerces the value of <see cref="EqualizerValues" /> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="EqualizerValues" /></param>
        /// <returns>The adjusted value of <see cref="EqualizerValues" /></returns>
        protected virtual float[] OnCoerceEqualizerCaptions(float[] value)
        {
            if (value == null)
                return new float[EqualizerCaptions.Length];
            return value;
        }

        /// <summary>
        ///     Called after the <see cref="EqualizerValues" /> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="EqualizerValues" /></param>
        /// <param name="newValue">The new value of <see cref="EqualizerValues" /></param>
        protected virtual void OnEqualizerCaptionsChanged(float[] oldValue, float[] newValue)
        {
            CreateSliders();
            if (newValue == null || newValue.Length != EqualizerCaptions.Length)
                SetEqualizerCaptions(new float[EqualizerCaptions.Length]);
            else
                SetEqualizerCaptions(newValue);
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
            for (int i = 0; i < EqualizerCaptions.Length; i++)
            {
                var channelColumn = new ColumnDefinition
                {
                    Width = new GridLength(10.0d, GridUnitType.Star)
                };

                _equalizerGrid.ColumnDefinitions.Add(channelColumn);
                
                var slider = new Slider
                {
                    //Style = (Style) FindResource("EqualizerSlider"), //TODO: DynamicResource as Metro in styles
                    Maximum = 1.0,
                    Minimum = -1.0,
                    SmallChange = 0.01,
                    LargeChange = 0.1,
                    Orientation = Orientation.Vertical,
                    SnapsToDevicePixels = true,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    IsMoveToPointEnabled = true
                };

                var caption = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = EqualizerCaptions[i] < 1000
                                   ? EqualizerCaptions[i].ToString(CultureInfo.InvariantCulture)
                                   : string.Format("{0}K", (EqualizerCaptions[i]/1000))
                    };

                Grid.SetColumn(slider, i);
                Grid.SetRow(slider, 0);
                Grid.SetColumn(caption, i);
                Grid.SetRow(caption, 1);
                _sliders.Add(slider);
                _captions.Add(caption);
                _equalizerGrid.Children.Add(slider);
                _equalizerGrid.Children.Add(caption);
                slider.ValueChanged += slider_ValueChanged;
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EqualizerValues = GetEqualizerValues();
        }

        private void ClearSliders()
        {
            foreach (Slider slider in _sliders)
            {
                slider.ValueChanged -= slider_ValueChanged;
                _equalizerGrid.Children.Remove(slider);
            }
            _sliders.Clear();

            foreach (TextBlock caption in _captions)
            {
                _equalizerGrid.Children.Remove(caption);
            }
            _captions.Clear();
        }

        private float[] GetEqualizerValues()
        {
            var sliderValues = new float[EqualizerCaptions.Length];
            for (int i = 0; i < EqualizerCaptions.Length; i++)
                sliderValues[i] = (float) _sliders[i].Value;
            return sliderValues;
        }

        private void SetEqualizerValues(float[] values)
        {
            for (int i = 0; i < EqualizerCaptions.Length; i++)
                _sliders[i].Value = values[i];
        }

        private void SetEqualizerCaptions(float[] values)
        {
            for (int i = 0; i < values.Length; i++)
                _captions[i].Text = values[i] < 1000
                                        ? values[i].ToString(CultureInfo.InvariantCulture)
                                        : string.Format("{0}K", (values[i]/1000));
        }

        #endregion
    }
}