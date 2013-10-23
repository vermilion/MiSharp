using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MiSharp.Model.Playlist.Input;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MiSharp.Model.Playlist
{
    public class PlaylistProcessorTemp
    {
        public delegate void PlaybackEventHandler(PlaybackEventArgs args);

        public delegate void TrackbarkEventHandler(TrackbarEventArgs args);

        private readonly Timer _timer;
        public List<IInputFileFormatPlugin> InputFileFormats;

        private WaveStream _fileWaveStream;
        private MiSharp.Playlist _playlist;
        private Song _playlistItem;
        private Action<float> _setVolumeDelegate;
        private IWavePlayer _waveOut;

        public PlaylistProcessorTemp()
        {
            InputFileFormats = new List<IInputFileFormatPlugin>
                {
                    new AiffInputFilePlugin(),
                    new Mp3InputFilePlugin(),
                    new WaveInputFilePlugin()
                };
            _timer = new Timer();
            _timer.Interval = 500;
            _timer.Tick += timer_Tick;
        }


        public event PlaybackEventHandler PlaybackParamsChanged;

        public event TrackbarkEventHandler TrackbarValueChanged;

        //TODO: MEF! Currently not working
        //[ImportMany(typeof (IInputFileFormatPlugin))]
        //public IEnumerable<IInputFileFormatPlugin> InputFileFormats { get; set; }

        private void timer_Tick(object sender, EventArgs e)
        {
            int trackBarPosition = 0;
            string currentTimeText = string.Empty;
            if (_waveOut != null && _fileWaveStream != null)
            {
                TimeSpan currentTime = (_waveOut.PlaybackState == PlaybackState.Stopped)
                                           ? TimeSpan.Zero
                                           : _fileWaveStream.CurrentTime;
                trackBarPosition = (int) currentTime.TotalSeconds;
                currentTimeText = String.Format("{0:00}:{1:00}", (int) currentTime.TotalMinutes, currentTime.Seconds);
            }
            TrackbarValueChanged(new TrackbarEventArgs
                {
                    Position = trackBarPosition,
                    CurrentTime = currentTimeText
                });
        }

        public void SetPlaylist(MiSharp.Playlist other)
        {
            _playlist = other;
        }

        public void Start()
        {
            PlayNextFile();
        }

        public void PlaybackStart(string fileName)
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

            CreateWaveOut();


            ISampleProvider sampleProvider;
            try
            {
                sampleProvider = CreateInputStream(fileName);
            }
            catch (Exception createException)
            {
                MessageBox.Show(String.Format("{0}", createException.Message), "Error Loading File");
                return;
            }

            PlaybackParamsChanged(new PlaybackEventArgs
                {
                    Maximum = (int) _fileWaveStream.TotalTime.TotalSeconds,
                    TickFrequency = (int) _fileWaveStream.TotalTime.TotalSeconds/30,
                    TotalTime = String.Format("{0:00}:{1:00}", (int) _fileWaveStream.TotalTime.TotalMinutes,
                                              _fileWaveStream.TotalTime.Seconds)
                });

            try
            {
                _waveOut.Init(new SampleToWaveProvider(sampleProvider));
            }
            catch (Exception initException)
            {
                MessageBox.Show(String.Format("{0}", initException.Message), "Error Initializing Output");
                return;
            }

            _waveOut.Play();
            _timer.Start();
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
            _timer.Stop();
        }

        public void SetVolumeLevel(float level)
        {
            if (_setVolumeDelegate != null)
                _setVolumeDelegate(level);
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
            var postVolumeMeter = new MeteringSampleProvider(waveChannel);

            return postVolumeMeter;
        }

        private IInputFileFormatPlugin GetPluginForFile(string fileName)
        {
            return (InputFileFormats
                .Where(f => fileName.EndsWith(f.Extension, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault();
        }


        private void CreateWaveOut()
        {
            CloseWaveOut();
            int latency = Settings.Instance.RequestedLatency;
            try
            {
                _waveOut = Settings.Instance.SelectedOutputDriver.CreateDevice(latency);
            }
            catch (Exception driverCreateException)
            {
                MessageBox.Show(String.Format("{0}", driverCreateException.Message));
                return;
            }
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

        public void SetPosition(double pos)
        {
            if (_fileWaveStream != null)
                _fileWaveStream.CurrentTime = TimeSpan.FromSeconds(pos);
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                MessageBox.Show(e.Exception.Message, "Playback Device Error");
                return;
            }
            PlaybackStop();
            //stopped - goto next file in list
            PlayNextFile();
        }

        private void PlayNextFile()
        {
            //if (_playlist.TagPlaylist.Count < _playlist.CurrentIndex)
            //    MessageBox.Show("count < index+1. Possible?");
            //else if (_playlist.TagPlaylist.Count == _playlist.CurrentIndex)
            //{
            //    //MessageBox.Show("Last one");
            //    //TODO: repeat option by default
            //    _playlist.CurrentIndex = 0;
            //    string path = _playlist.TagPlaylist.ElementAt(_playlist.CurrentIndex).OriginalPath;
            //    PlaybackStart(path);
            //}
            //else
            //{
            //    string path = _playlist.TagPlaylist.ElementAt(_playlist.CurrentIndex).OriginalPath;
            //    PlaybackStart(path);
            //    _playlist.CurrentIndex++;
            //}
        }
    }
}