using System;
using System.Windows;
using Caliburn.Micro;
using MahApps.Metro.Controls;

namespace MiSharp
{
    public sealed class AppWindowManager : WindowManager
    {
        private static readonly ResourceDictionary[] Resources;

        static AppWindowManager()
        {
            Resources = new[]
                {
                    new ResourceDictionary
                        {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml", UriKind.RelativeOrAbsolute)},
                    new ResourceDictionary
                        {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml", UriKind.RelativeOrAbsolute)},
                    new ResourceDictionary
                        {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml", UriKind.RelativeOrAbsolute)},
                    new ResourceDictionary
                        {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml", UriKind.RelativeOrAbsolute)},
                    new ResourceDictionary
                        {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml", UriKind.RelativeOrAbsolute)},
                    new ResourceDictionary
                        {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.AnimatedTabControl.xaml", UriKind.RelativeOrAbsolute)}
                };
        }

        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var window = view as MetroWindow;
            if (window == null)
            {
                window = new MetroWindow
                    {
                        Content = view,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        ResizeMode = ResizeMode.NoResize
                    };
                window.MinHeight = 150;
                window.MinWidth = 500;
                foreach (ResourceDictionary resourceDictionary in Resources)
                {
                    window.Resources.MergedDictionaries.Add(resourceDictionary);
                }
                //window.SetValue(View.Settings.IsGeneratedProperty, true);
                Window owner = InferOwnerOf(window);
                if (owner != null)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = owner;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            else
            {
                Window owner2 = InferOwnerOf(window);
                if (owner2 != null && isDialog)
                {
                    window.Owner = owner2;
                }
            }
            return window;
        }
    }
}