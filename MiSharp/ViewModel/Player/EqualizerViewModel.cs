using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core;
using MiSharp.DialogResults;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class EqualizerViewModel : Screen
    {
        private List<float[]> _equalizerCaptions;
        private readonly PlayerViewModel _playerViewModel;

        public EqualizerViewModel()
        {
            DisplayName = "Equalizer";
            _playerViewModel = IoC.Get<PlayerViewModel>();

            //var t = _playerViewModel.PlaybackController;
            //EqualizerValues.ObservableForProperty(x => x).Subscribe(x =>
            //    {
            //        // var tt = x;
            //    });

            // Center: Frequency center. 20.0 to 22000.0. Default = 8000.0
            // Bandwith: Octave range around the center frequency to filter. 0.2 to 5.0. Default = 1.0
            // Gain: Frequency Gain. 0.05 to 3.0. Default = 1.0
            EqualizerCaptions = new List<float[]>
                {
                    new[] {32f, 1f, 1f},
                    new[] {64f, 1f, 1f},
                    new[] {125f, 1f, 1f},
                    new[] {250f, 1f, 1f},
                    new[] {500f, 1f, 1f},
                    new[] {1000f, 1f, 1f},
                    new[] {2000f, 1f, 1f},
                    new[] {4000f, 1f, 1f},
                    new[] {8000f, 1f, 1f},
                    new[] {16000f, 1f, 1f}
                };
        }

        public List<float[]> EqualizerCaptions
        {
            get { return _equalizerCaptions; }
            set
            {
                _equalizerCaptions = value;
                NotifyOfPropertyChange(() => EqualizerCaptions);
            }
        }

        public float[] EqualizerValues
        {
            get { return Settings.Instance.EqualizerValues; }
            set
            {
                Settings.Instance.EqualizerValues = value;
                NotifyOfPropertyChange(() => EqualizerEnabled);
            }
        }

        public bool EqualizerEnabled
        {
            get { return Settings.Instance.EqualizerEnabled; }
            set
            {
                Settings.Instance.EqualizerEnabled = value;
                NotifyOfPropertyChange(() => EqualizerEnabled);
            }
        }

        public IEnumerable<IResult> SaveChanges()
        {
            Settings.Instance.SaveSettings();
            yield return new CloseResult();
        }
    }
}