// Copyright (C) 2011 - 2012, Jacob Johnston 
//
// Permission is hereby granted, free of charge, to any person obtaining a 
// copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions: 
//
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software. 
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE. 

// CD Jewel Case Images Copyright LeMarquis
// Used under Creative Commons License.
// http://lemarquis.deviantart.com/art/JEWEL-CASE-Photoshop-file-69316052

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFSoundVisualizationLib
{
    /// <summary>
    ///     Displays album cover artwork
    ///     with a CD Jewel Case overlay.
    /// </summary>
    [DisplayName("Album Art Display")]
    [Description("Displays album cover artwork with a CD Jewel Case overlay.")]
    [ToolboxItem(true)]
    [TemplatePart(Name = "PART_AlbumArt", Type = typeof (Image))]
    public class AlbumArtDisplay : Control
    {
        #region Fields

        private readonly RenderTargetBitmap _albumArtBuffer = new RenderTargetBitmap(433, 379, 96, 96,
            PixelFormats.Pbgra32);

        private readonly DrawingVisual _drawingVisual = new DrawingVisual();

        private readonly BitmapImage _noArtImage =
            new BitmapImage(
                new Uri("pack://application:,,,/WPFSoundVisualizationLib;component/Album Art Display/NoArtwork.png",
                    UriKind.Absolute));

        private readonly BitmapImage _overlayImage =
            new BitmapImage(
                new Uri("pack://application:,,,/WPFSoundVisualizationLib;component/Album Art Display/Overlay.png",
                    UriKind.Absolute));

        private readonly BitmapImage _underlayImage =
            new BitmapImage(
                new Uri("pack://application:,,,/WPFSoundVisualizationLib;component/Album Art Display/Underlay.png",
                    UriKind.Absolute));

        private Image _albumArtImage;

        #endregion

        #region Dependency Properties

        #region AlbumArtImage

        /// <summary>
        ///     Identifies the <see cref="AlbumArtImage" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlbumArtImageProperty = DependencyProperty.Register("AlbumArtImage",
            typeof (ImageSource), typeof (AlbumArtDisplay),
            new UIPropertyMetadata(null, OnAlbumArtImageChanged, OnCoerceAlbumArtImage));

        /// <summary>
        ///     Gets or sets the image to display in the CD jewel case as the album art cover. If this value
        ///     is set to null, the CD sleeve will appear empty and show an unlabeled disc instead.
        /// </summary>
        [Category("Common")]
        public ImageSource AlbumArtImage
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get { return (ImageSource) GetValue(AlbumArtImageProperty); }
            set { SetValue(AlbumArtImageProperty, value); }
        }

        private static object OnCoerceAlbumArtImage(DependencyObject o, object value)
        {
            var albumArtDisplay = o as AlbumArtDisplay;
            if (albumArtDisplay != null)
                return albumArtDisplay.OnCoerceAlbumArtImage((ImageSource) value);
            return value;
        }

        private static void OnAlbumArtImageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var albumArtDisplay = o as AlbumArtDisplay;
            if (albumArtDisplay != null)
                albumArtDisplay.OnAlbumArtImageChanged((ImageSource) e.OldValue, (ImageSource) e.NewValue);
        }

        /// <summary>
        ///     Coerces the value of <see cref="AlbumArtImage" /> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="AlbumArtImage" /></param>
        /// <returns>The adjusted value of <see cref="AlbumArtImage" /></returns>
        protected virtual ImageSource OnCoerceAlbumArtImage(ImageSource value)
        {
            return value;
        }

        /// <summary>
        ///     Called after the <see cref="AlbumArtImage" /> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="AlbumArtImage" /></param>
        /// <param name="newValue">The new value of <see cref="AlbumArtImage" /></param>
        protected virtual void OnAlbumArtImageChanged(ImageSource oldValue, ImageSource newValue)
        {
            DrawAlbumArt();
        }

        #endregion

        #endregion

        #region Template Overrides

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code
        ///     or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _albumArtImage = GetTemplateChild("PART_AlbumArt") as Image;
            _albumArtImage.Source = _albumArtBuffer;
            DrawAlbumArt();
        }

        #endregion

        #region Constructors

        static AlbumArtDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (AlbumArtDisplay),
                new FrameworkPropertyMetadata(typeof (AlbumArtDisplay)));
        }

        #endregion

        #region Drawing

        private void DrawAlbumArt()
        {
            // Clear Canvas
            _albumArtBuffer.Clear();

            using (DrawingContext drawingContext = _drawingVisual.RenderOpen())
            {
                if (AlbumArtImage == null)
                {
                    drawingContext.DrawImage(_noArtImage, new Rect(0, 0, _albumArtBuffer.Width, _albumArtBuffer.Height));
                }
                else
                {
                    drawingContext.DrawImage(_underlayImage, new Rect(0, 0, _albumArtBuffer.Width, _albumArtBuffer.Height));
                    drawingContext.DrawImage(AlbumArtImage,
                        new Rect(54, 5, _albumArtBuffer.Width - 62, _albumArtBuffer.Height - 10));
                    drawingContext.DrawImage(_overlayImage, new Rect(0, 0, _albumArtBuffer.Width, _albumArtBuffer.Height));
                }
            }

            _albumArtBuffer.Render(_drawingVisual);
        }

        #endregion
    }
}