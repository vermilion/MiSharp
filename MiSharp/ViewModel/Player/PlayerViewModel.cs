using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core.Player;

namespace MiSharp
{
    [Export(typeof (PlayerViewModel))]
    public class PlayerViewModel : Screen, IHandle<PlaybackEventArgs>
    {
        private readonly PlaylistViewModel _playlistViewModel;
        private string _currentTime = "00:00";
        private int _maximum = 1;

        private double _positionValue;

        private int _tickFrequency;
        private string _totalTime = "00:00";

        [ImportingConstructor]
        public PlayerViewModel(IEventAggregator events, PlaylistViewModel playlistViewModel)
        {
            _playlistViewModel = playlistViewModel;
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

        public void Handle(PlaybackEventArgs args)
        {
            TotalTime = String.Format("{0:00}:{1:00}", (int) args.TotalTime.TotalMinutes, args.TotalTime.Seconds);
            CurrentTime = String.Format("{0:00}:{1:00}", (int) args.CurrentTime.TotalMinutes, args.CurrentTime.Seconds);
            TickFrequency = (int) args.TotalTime.TotalSeconds/30;
            Maximum = (int) args.TotalTime.TotalSeconds;
            PositionValue = (int) args.CurrentTime.TotalSeconds;
        }

        public void ChangePosition(double pos)
        {
            _playlistViewModel.Player.CurrentTime = TimeSpan.FromSeconds(pos);
        }

        public void StartClick()
        {
            _playlistViewModel.Player.Play();
        }

        public void PauseClick()
        {
            _playlistViewModel.Player.Pause();
        }

        public void StopClick()
        {
            _playlistViewModel.Player.Stop();
        }

        public void VolumeValueChanged(float value)
        {
            _playlistViewModel.Player.Volume = value;
        }
    }
}