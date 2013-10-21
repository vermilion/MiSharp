using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Screen = Caliburn.Micro.Screen;

namespace MiSharp
{
    [Export(typeof (PlayerViewModel))]
    public class PlayerViewModel : Screen
    {
        private string _currentTime;
        private int _maximum;
        private IOutputDevicePlugin _outSettingsViewModel;
        private double _positionValue;
        private List<int> _requestedLatency = new List<int>();
        private int _selectedLatency;
        private IOutputDevicePlugin _selectedOutputDriver;
        private int _tickFrequency;
        private string _totalTime;
        private string fileName;
        private WaveStream fileWaveStream;
        private Action<float> setVolumeDelegate;
        private IWavePlayer waveOut;

        public PlayerViewModel()
        {
            RequestedLatency.AddRange(new[] {25, 50, 100, 150, 200, 300, 400, 500});
        }

        [ImportMany(typeof (IOutputDevicePlugin))]
        public IEnumerable<IOutputDevicePlugin> OutputDevicePlugins { get; set; }

        [ImportMany(typeof (IInputFileFormatPlugin))]
        public IEnumerable<IInputFileFormatPlugin> InputFileFormats { get; set; }


        public IOutputDevicePlugin OutSettingsViewModel
        {
            get { return _outSettingsViewModel; }
            set
            {
                _outSettingsViewModel = value;
                NotifyOfPropertyChange(() => OutSettingsViewModel);
            }
        }

        public IOutputDevicePlugin SelectedOutputDriver
        {
            get { return _selectedOutputDriver; }
            set
            {
                _selectedOutputDriver = value;
                //TODO: refactor switch
                switch (_selectedOutputDriver.Name)
                {
                    case "AsioOut":
                        OutSettingsViewModel = new AsioOutSettingsViewModel();
                        break;
                    case "WaveOut":
                        OutSettingsViewModel = new WaveOutSettingsViewModel();
                        break;
                    case "WasapiOut":
                        OutSettingsViewModel = new WasapiOutSettingsViewModel();
                        break;
                    case "DirectSound":
                        OutSettingsViewModel = new DirectSoundOutSettingsViewModel();
                        break;
                }
                NotifyOfPropertyChange(() => SelectedOutputDriver);
            }
        }

        public List<int> RequestedLatency
        {
            get { return _requestedLatency; }
            set
            {
                _requestedLatency = value;
                NotifyOfPropertyChange(() => RequestedLatency);
            }
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
                if (fileWaveStream == null) return _positionValue;
                TimeSpan currentTime = fileWaveStream.CurrentTime;
                _positionValue = currentTime.TotalSeconds;
                CurrentTime = String.Format("{0:00}:{1:00}", (int) currentTime.TotalMinutes, currentTime.Seconds);
                return _positionValue;
            }
            set
            {
                fileWaveStream.CurrentTime = TimeSpan.FromSeconds(value);
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

        public int SelectedLatency
        {
            get { return _selectedLatency; }
            set
            {
                _selectedLatency = value;
                NotifyOfPropertyChange(() => SelectedLatency);
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

        public void OpenClick()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                string allExtensions = string.Join(";", (from f in InputFileFormats select "*" + f.Extension).ToArray());
                openFileDialog.Filter = String.Format("All Supported Files|{0}|All Files (*.*)|*.*", allExtensions);
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog.FileName;
                }
            }
        }

        public void StartClick()
        {
            if (!SelectedOutputDriver.IsAvailable)
            {
                MessageBox.Show("The selected output driver is not available on this system");
                return;
            }

            if (waveOut != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    return;
                }
                if (waveOut.PlaybackState == PlaybackState.Paused)
                {
                    waveOut.Play();
                    // groupBoxDriverModel.Enabled = false;
                    return;
                }
            }

            // we are in a stopped state
            // TODO: only re-initialise if necessary
            if (String.IsNullOrEmpty(fileName))
            {
                OpenClick();
            }
            if (String.IsNullOrEmpty(fileName))
            {
                return;
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
                sampleProvider = CreateInputStream(fileName);
            }
            catch (Exception createException)
            {
                MessageBox.Show(String.Format("{0}", createException.Message), "Error Loading File");
                return;
            }


            Maximum = (int) fileWaveStream.TotalTime.TotalSeconds;
            TotalTime = String.Format("{0:00}:{1:00}", (int) fileWaveStream.TotalTime.TotalMinutes,
                fileWaveStream.TotalTime.Seconds);
            TickFrequency = Maximum/30;

            try
            {
                waveOut.Init(new SampleToWaveProvider(sampleProvider));
            }
            catch (Exception initException)
            {
                MessageBox.Show(String.Format("{0}", initException.Message), "Error Initializing Output");
                return;
            }

            // setVolumeDelegate(volumeSlider1.Volume);
            //groupBoxDriverModel.Enabled = false;
            waveOut.Play();
        }


        public void PauseClick()
        {
            if (waveOut != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Pause();
                }
            }
        }

        public void StopClick()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
            }
        }

        private ISampleProvider CreateInputStream(string fileName)
        {
            IInputFileFormatPlugin plugin = GetPluginForFile(fileName);
            if (plugin == null)
            {
                throw new InvalidOperationException("Unsupported file extension");
            }
            fileWaveStream = plugin.CreateWaveStream(fileName);
            var waveChannel = new SampleChannel(fileWaveStream, true);
            setVolumeDelegate = vol => waveChannel.Volume = vol;
            waveChannel.PreVolumeMeter += OnPreVolumeMeter;

            var postVolumeMeter = new MeteringSampleProvider(waveChannel);
            postVolumeMeter.StreamVolume += OnPostVolumeMeter;


            return postVolumeMeter;
        }

        private void OnPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            // we know it is stereo
            //waveformPainter1.AddMax(e.MaxSampleValues[0]);
            //waveformPainter2.AddMax(e.MaxSampleValues[1]);
        }

        private void OnPostVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            // we know it is stereo
            //volumeMeter1.Amplitude = e.MaxSampleValues[0];
            //volumeMeter2.Amplitude = e.MaxSampleValues[1];
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
            int latency = SelectedLatency;
            waveOut = SelectedOutputDriver.CreateDevice(latency);
            waveOut.PlaybackStopped += OnPlaybackStopped;
        }

        private void CloseWaveOut()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
            }
            if (fileWaveStream != null)
            {
                // this one really closes the file and ACM conversion
                fileWaveStream.Dispose();
                setVolumeDelegate = null;
            }
            if (waveOut != null)
            {
                waveOut.Dispose();
                waveOut = null;
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            //groupBoxDriverModel.Enabled = true;
            if (e.Exception != null)
            {
                MessageBox.Show(e.Exception.Message, "Playback Device Error");
            }
        }
    }
}