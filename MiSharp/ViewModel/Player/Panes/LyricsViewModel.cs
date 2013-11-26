using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LyricsLibNet;
using LyricsLibNet.Providers;
using MiSharp.Core;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class LyricsViewModel : Screen
    {
        private string _lyricsText;

        public LyricsViewModel()
        {
            var playerViewModel = IoC.Get<PlayerViewModel>();

            GetLyricsCommand = new ReactiveCommand();
            GetLyricsCommand.Subscribe(x =>
                {
                    var current = playerViewModel.CurrentlyPlaying;
                    if (current != null && SelectedLyricsProvider != null)
                    {
                        var finder = new LyricsFinder(current.Model.ArtistName, current.Model.TrackTitle);
                        finder.ClearProviders();
                        finder.AddProvider(SelectedLyricsProvider);
                        finder.ResultAvailable += (sender, e) => LyricsText = e.Result.Text;
                        finder.ProviderError += (sender, e) => LyricsText = e.ProviderName + ": " + e.Exception.Message;
                        finder.Start();
                    }
                });
        }

        public ReactiveCommand GetLyricsCommand { get; set; }

        public List<ILyricsProvider> LyricsProviders
        {
            get
            {
                return new List<ILyricsProvider>
                    {
                        new AzLyricsProvider(),
                        new ChartLyricsProvider(),
                        new LyricsComProvider(),
                        new WikiaLyricsProvider()
                    };
            }
        }

        public ILyricsProvider SelectedLyricsProvider
        {
            get { return Settings.Instance.SelectedLyricsProvider; }
            set
            {
                Settings.Instance.SelectedLyricsProvider = value;
                NotifyOfPropertyChange(() => SelectedLyricsProvider);
            }
        }

        public string LyricsText
        {
            get { return _lyricsText; }
            set
            {
                _lyricsText = value;
                NotifyOfPropertyChange(() => LyricsText);
            }
        }
    }
}