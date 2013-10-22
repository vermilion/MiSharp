using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Screen = Caliburn.Micro.Screen;

namespace MiSharp
{
    public class PlaylistProcessor
    {
        public void PlaybackStart()
        {
            if (_waveOut != null)
            {
                if (_waveOut.PlaybackState == PlaybackState.Playing)
                {
                    return;
                }
                if (_waveOut.PlaybackState == PlaybackState.Paused)
                {
                    _waveOut.Play();
                    return;
                }
            }

            try
            {
                CreateWaveOut();
            }
            catch (Exception driverCreateException)
            {
                MessageBox.Show(String.Format("{0}", driverCreateException.Message));
                return;
            }

            ISampleProvider sampleProvider = null;
            try
            {
                // sampleProvider = CreateInputStream(_fileName);
            }
            catch (Exception createException)
            {
                MessageBox.Show(String.Format("{0}", createException.Message), "Error Loading File");
                return;
            }


            //Maximum = (int)_fileWaveStream.TotalTime.TotalSeconds;
            //TotalTime = String.Format("{0:00}:{1:00}", (int)_fileWaveStream.TotalTime.TotalMinutes,
            //                          _fileWaveStream.TotalTime.Seconds);
            //TickFrequency = Maximum / 30;

            try
            {
                _waveOut.Init(new SampleToWaveProvider(sampleProvider));
            }
            catch (Exception initException)
            {
                MessageBox.Show(String.Format("{0}", initException.Message), "Error Initializing Output");
                return;
            }

            // setVolumeDelegate(volumeSlider1.Volume);
            //groupBoxDriverModel.Enabled = false;
            _waveOut.Play();
        }

        public void PlaybackPause()
        {
            if (_waveOut != null)
            {
                if (_waveOut.PlaybackState == PlaybackState.Playing)
                {
                    _waveOut.Pause();
                }
            }
        }

        public void PlaybackStop()
        {
            if (_waveOut != null)
            {
                _waveOut.Stop();
            }
        }

        private ISampleProvider CreateInputStream(string fileName)
        {
            IInputFileFormatPlugin plugin = GetPluginForFile(fileName);
            if (plugin == null)
            {
                throw new InvalidOperationException("Unsupported file extension");
            }
            _fileWaveStream = plugin.CreateWaveStream(fileName);
            var waveChannel = new SampleChannel(_fileWaveStream, true);
            _setVolumeDelegate = vol => waveChannel.Volume = vol;
            //waveChannel.PreVolumeMeter += OnPreVolumeMeter;

            var postVolumeMeter = new MeteringSampleProvider(waveChannel);
            //postVolumeMeter.StreamVolume += OnPostVolumeMeter;


            return postVolumeMeter;
        }

        private IInputFileFormatPlugin GetPluginForFile(string fileName)
        {
            return
                (from f in InputFileFormats
                 where fileName.EndsWith(f.Extension, StringComparison.OrdinalIgnoreCase)
                 select f).FirstOrDefault();
        }


        private void CreateWaveOut()
        {
            CloseWaveOut();
            //int latency = SelectedLatency;
            int latency = 300;
            // _waveOut = SelectedOutputDriver.CreateDevice(latency);
            _waveOut.PlaybackStopped += OnPlaybackStopped;
        }

        private void CloseWaveOut()
        {
            if (_waveOut != null)
            {
                _waveOut.Stop();
            }
            if (_fileWaveStream != null)
            {
                // this one really closes the file and ACM conversion
                _fileWaveStream.Dispose();
                _setVolumeDelegate = null;
            }
            if (_waveOut != null)
            {
                _waveOut.Dispose();
                _waveOut = null;
            }
        }

        [ImportMany(typeof (IInputFileFormatPlugin))]
        public IEnumerable<IInputFileFormatPlugin> InputFileFormats { get; set; }


        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            //groupBoxDriverModel.Enabled = true;
            if (e.Exception != null)
            {
                MessageBox.Show(e.Exception.Message, "Playback Device Error");
            }
        }

        private WaveStream _fileWaveStream;
        private Action<float> _setVolumeDelegate;
        private IWavePlayer _waveOut;
    }


    [Export(typeof (PlayerViewModel))]
    public class PlayerViewModel : Screen, IHandle<Playlist>
    {
        private string _currentTime;
        private string _fileName;

        private int _maximum;

        private double _positionValue;

        private int _tickFrequency;
        private string _totalTime;
        private readonly PlaylistProcessor _processor;

        [ImportingConstructor]
        public PlayerViewModel(IEventAggregator events)
        {
            _processor = new PlaylistProcessor();
            events.Subscribe(this);
        }




        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                NotifyOfPropertyChange(() => Maximum);
            }
        }

        public double PositionValue
        {
            get
            {
                // if (_fileWaveStream == null) return _positionValue;
                //TimeSpan currentTime = _fileWaveStream.CurrentTime;
                //_positionValue = currentTime.TotalSeconds;
                //CurrentTime = String.Format("{0:00}:{1:00}", (int) currentTime.TotalMinutes, currentTime.Seconds);
                return _positionValue;
            }
            set
            {
                // _fileWaveStream.CurrentTime = TimeSpan.FromSeconds(value);
                _positionValue = value;
                NotifyOfPropertyChange(() => PositionValue);
            }
        }

        public int TickFrequency
        {
            get { return _tickFrequency; }
            set
            {
                _tickFrequency = value;
                NotifyOfPropertyChange(() => TickFrequency);
            }
        }

        public string TotalTime
        {
            get { return _totalTime; }
            set
            {
                _totalTime = value;
                NotifyOfPropertyChange(() => TotalTime);
            }
        }



        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                NotifyOfPropertyChange(() => CurrentTime);
            }
        }

        public void StartClick()
        {
            //if (!SelectedOutputDriver.IsAvailable)
            //{
            //    MessageBox.Show("The selected output driver is not available on this system");
            //    return;
            //}

            _processor.PlaybackStart();
        }


        public void PauseClick()
        {
            _processor.PlaybackPause();
        }

        public void StopClick()
        {
            _processor.PlaybackStop();
        }

        //TODO: another event
        public void VolumeValueChanged(float value)
        {
            //if (_setVolumeDelegate != null)
            //{
            //    _setVolumeDelegate(value);
            //}
        }




        #region IHandle implementation

        public void Handle(Playlist playlist)
        {
            _fileName = playlist.TagPlaylist[playlist.CurrentIndex].MediaPath;
            //TODO: playlist queue
            StartClick();
        }

        #endregion
    }
}