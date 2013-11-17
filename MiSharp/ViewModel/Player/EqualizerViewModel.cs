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
        public EqualizerViewModel()
        {
            DisplayName = "Equalizer";
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