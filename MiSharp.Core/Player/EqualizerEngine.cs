using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Caliburn.Micro;
using Linsft.FmodSharp.Dsp;
using Linsft.FmodSharp.SoundSystem;
using Type = Linsft.FmodSharp.Dsp.Type;


namespace MiSharp.Core.Player
{
    public class EqualizerEngine : PropertyChangedBase, IDisposable
    {
        private SoundSystem _soundSystem;

        public EqualizerEngine(SoundSystem system)
        {
            _soundSystem = system;

            Bands = new ObservableCollection<EqualizerBand>();
            foreach (EqualizerParam value in Settings.Instance.EqualizerValues)
            {
                Bands.Add(new EqualizerBand(value));
            }

            if (Settings.Instance.EqualizerEnabled)
                InitEqualizer(Settings.Instance.EqualizerValues);
        }

        public ObservableCollection<EqualizerBand> Bands { get; set; }

        public List<EqualizerParam> BandsValues
        {
            get { return Bands.Select(x => new EqualizerParam(x.Center, x.BandWidth, x.Gain)).ToList(); }
        }

        public void InitEqualizer(List<EqualizerParam> values)
        {
            Bands = new ObservableCollection<EqualizerBand>();

            _soundSystem.LockDSP();

            foreach (EqualizerParam value in values)
            {
                Bands.Add(new EqualizerBand(_soundSystem, value));
            }

            _soundSystem.UnlockDSP();

            NotifyOfPropertyChange(() => Bands);
        }

        public void DeInitEqualizer()
        {
            _soundSystem.LockDSP();

            foreach (EqualizerBand band in Bands)
            {
                band.Remove();
            }

            _soundSystem.UnlockDSP();

            NotifyOfPropertyChange(() => Bands);
        }

        public void Reset()
        {
            DeInitEqualizer();
            Bands.Clear();
            _soundSystem = null;
        }

        public class EqualizerBand : PropertyChangedBase
        {
            private bool _active;
            private float _bandwidth;
            private string _caption;
            private float _center;
            private DSP _dsp;
            private float _gain;

            public EqualizerBand(EqualizerParam param)
            {
                _center = param.Center;
                _bandwidth = param.Bandwidth;
                _gain = param.Gain;

                Caption = param.Center < 1000
                    ? param.Center.ToString(CultureInfo.InvariantCulture)
                    : string.Format("{0}K", (param.Center/1000));
            }

            public EqualizerBand(SoundSystem system, EqualizerParam param)
            {
                _dsp = system.CreateDspByType(Type.ParameQ);
                system.AddDSP(_dsp);

                Center = param.Center;
                BandWidth = param.Bandwidth;
                Gain = param.Gain;

                _dsp.Active = Active = true;

                Caption = param.Center < 1000
                    ? param.Center.ToString(CultureInfo.InvariantCulture)
                    : string.Format("{0}K", (param.Center/1000));
            }

            public float Gain
            {
                get { return _gain; }
                set
                {
                    _dsp.Active = false;
                    _dsp.SetParameter((int) DSPParameq.Gain, value);
                    _gain = value;
                    _dsp.Active = true;
                    NotifyOfPropertyChange(() => Gain);
                }
            }

            public float Center
            {
                get { return _center; }
                set
                {
                    _dsp.Active = false;
                    _dsp.SetParameter((int) DSPParameq.Center, value);
                    _center = value;
                    _dsp.Active = true;
                    NotifyOfPropertyChange(() => Center);
                }
            }

            public float BandWidth
            {
                get { return _bandwidth; }
                set
                {
                    _dsp.Active = false;
                    _dsp.SetParameter((int) DSPParameq.Bandwidth, value);
                    _bandwidth = value;
                    _dsp.Active = true;
                    NotifyOfPropertyChange(() => BandWidth);
                }
            }


            public bool Active
            {
                get { return _active; }
                set
                {
                    _active = value;
                    NotifyOfPropertyChange(() => Active);
                }
            }

            public string Caption
            {
                get { return _caption; }
                set
                {
                    _caption = value;
                    NotifyOfPropertyChange(() => Caption);
                }
            }

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
        #region IDisposable

        public void Dispose()
        {
            Bands.Clear();
            if (_soundSystem != null)
            {
                DeInitEqualizer();                
                _soundSystem.Dispose();
                _soundSystem = null;
            }
        }

        #endregion
    }
}