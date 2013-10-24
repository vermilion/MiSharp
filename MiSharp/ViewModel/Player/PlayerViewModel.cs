using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core.Player;

namespace MiSharp
{
    [Export(typeof (PlayerViewModel))]
    public class PlayerViewModel : Screen
    {
        private string _currentTime;
        private int _maximum;

        private double _positionValue;

        private int _tickFrequency;
        private string _totalTime;

        [ImportingConstructor]
        public PlayerViewModel(IEventAggregator events)
        {
            //_library = new Library();
            //_library.PlaybackParamsChanged += ProcessorPlaybackParamsChanged;
            //_library.TrackbarValueChanged += _processor_TrackbarValueChanged;
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
            get { return _positionValue; }
            set
            {
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

        private void _processor_TrackbarValueChanged(TrackbarEventArgs args)
        {
            PositionValue = args.Position;
            CurrentTime = args.CurrentTime;
        }

        private void ProcessorPlaybackParamsChanged(PlaybackEventArgs args)
        {
            Maximum = args.Maximum;
            TickFrequency = args.TickFrequency;
            TotalTime = args.TotalTime;
        }

        public void ChangePosition(double pos)
        {
            // _library.SetPosition(pos);
        }

        public void StartClick()
        {
            //if (!SelectedOutputDriver.IsAvailable)
            //{
            //    MessageBox.Show("The selected output driver is not available on this system");
            //    return;
            //}

            // _library.PlayNextSong();
        }


        public void PauseClick()
        {
            //_library.PauseSong();
        }

        public void StopClick()
        {
            //_library.PauseSong();
        }

        public void VolumeValueChanged(float value)
        {
            //_library.SetVolumeLevel(value);
        }
    }
}