using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core;
using MiSharp.DialogResults;

namespace MiSharp
{
    [Export]
    public class EqualizerViewModel : Screen
    {
        private float[] _equalizerCaptions;

        public EqualizerViewModel()
        {
            DisplayName = "Equalizer";

            EqualizerCaptions = new[] {32f, 64f, 125f, 250f, 500f, 1000f, 2000f, 4000f, 8000f, 16000f};
        }

        public float[] EqualizerCaptions
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