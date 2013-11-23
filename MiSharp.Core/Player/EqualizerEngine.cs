using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Linsft.FmodSharp.Dsp;
using Linsft.FmodSharp.SoundSystem;
using WPFSoundVisualizationLib;

namespace MiSharp.Core.Player
{
    public class EqualizerEngine
    {
        private ObservableCollection<EqualizerBand> _bands;
        private SoundSystem _soundSystem;

        public EqualizerEngine(SoundSystem system)
        {
            _soundSystem = system;

            if (Settings.Instance.EqualizerEnabled)
                InitEqualizer(Settings.Instance.EqualizerValues);
        }

        public void InitEqualizer(List<EqualizerParam> values)
        {
            _bands = new ObservableCollection<EqualizerBand>();

            _soundSystem.LockDSP();

            foreach (EqualizerParam value in values)
            {
                _bands.Add(new EqualizerBand(_soundSystem, value));
            }

            _soundSystem.UnlockDSP();
        }

        public void DeInitEqualizer()
        {
            _soundSystem.LockDSP();

            foreach (EqualizerBand band in _bands)
            {
                band.Remove();
            }

            _soundSystem.UnlockDSP();
        }

        public void SetEqualizerValues(List<EqualizerParam> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                _bands[i].Gain = values[i].Gain;
            }
        }

        public void Reset()
        {
            DeInitEqualizer();
            _bands.Clear();
            _soundSystem = null;
        }

        public class EqualizerBand
        {
            private DSP _dsp;

            public EqualizerBand(SoundSystem system, EqualizerParam param)
            {
                _dsp = system.CreateDspByType(Type.ParameQ);
                system.AddDSP(_dsp);

                _dsp.SetParameter((int) DSPParameq.Center, param.Center);
                _dsp.SetParameter((int) DSPParameq.Bandwidth, param.Bandwidth);
                _dsp.SetParameter((int) DSPParameq.Gain, param.Gain);

                _dsp.Active = true;
            }

            public float Gain
            {
                get
                {
                    float value = 0;
                    var sb = new StringBuilder(16);
                    _dsp.GetParameter((int) DSPParameq.Gain, ref value, sb);
                    return value;
                }
                set
                {
                    _dsp.Active = false;
                    _dsp.SetParameter((int) DSPParameq.Gain, value);
                    _dsp.Active = true;
                }
            }

            public bool Active { get; set; }

            public void Remove()
            {
                if (_dsp != null)
                {
                    _dsp.Remove();
                    _dsp = null;
                }
                Active = false;
            }

        }
    }
}