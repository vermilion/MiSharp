using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using DeadDog.Audio;
using MiSharp.Core.Player;
using MiSharp.Player;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class PlayerViewModel : ReactiveObject, IHandle<RawTrack>
    {
        private readonly IEventAggregator _events;
        private readonly NowPlayingViewModel _nowPlayingViewModel;
        private string _currentTime = "00:00";
        private int _maximum;

        private double _positionValue;
        private float _tempVolume;

        private int _tickFrequency;
        private string _totalTime = "00:00";

        [ImportingConstructor]
        public PlayerViewModel(IEventAggregator events, NowPlayingViewModel nowPlayingViewModel)
        {
            _nowPlayingViewModel = nowPlayingViewModel;
            Player = new LocalAudioPlayer();
            Player.PlaybackUpdated += Player_PlaybackUpdated;
            Player.SongFinished += (s, e) => PlayNext();
            _events = events;
            _events.Subscribe(this);
        }

        public RawTrack CurrentlyPlaying
        {
            get { return _currentlyPlaying; }
            set { this.RaiseAndSetIfChanged(ref _currentlyPlaying, value); }
        }

        public LocalAudioPlayer Player { get; set; }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                this.RaiseAndSetIfChanged(ref _isPlaying, value);
                this.RaisePropertyChanged("PlayPauseTooltip");
                this.RaisePropertyChanged("PlayPauseContent");
            }
        }

        public string PlayPauseTooltip
        {
            get { return IsPlaying ? "Pause" : "Play"; }
        }

        public string PlayPauseContent
        {
            get { return IsPlaying ? ";" : "4"; }
        }

        public bool IsMuted
        {
            get { return _isMuted; }
            set
            {
                if (value)
                {
                    _tempVolume = Volume;
                    Volume = 0;
                }
                else Volume = _tempVolume;
                this.RaiseAndSetIfChanged(ref _isMuted, value);
                this.RaisePropertyChanged("Volume");
            }
        }


        public bool RepeatState
        {
            get { return _repeatState; }
            set { this.RaiseAndSetIfChanged(ref _repeatState, value); }
        }

        public bool ShuffleState
        {
            get { return _shuffleState; }
            set { this.RaiseAndSetIfChanged(ref _shuffleState, value); }
        }

        public void Handle(RawTrack song)
        {
            Play(song);
        }

        private void Player_PlaybackUpdated(PlaybackEventArgs args)
        {
            TotalTime = String.Format("{0:00}:{1:00}", (int) args.TotalTime.TotalMinutes, args.TotalTime.Seconds);
            CurrentTime = String.Format("{0:00}:{1:00}", (int) args.CurrentTime.TotalMinutes, args.CurrentTime.Seconds);
            TickFrequency = (int) args.TotalTime.TotalSeconds/30;
            Maximum = (int) args.TotalTime.TotalSeconds;
            PositionValue = (int) args.CurrentTime.TotalSeconds;
        }

        public void Play(RawTrack song)
        {
            if (!Equals(CurrentlyPlaying, song))
            {
                CurrentlyPlaying = song;
                Player.Load(song, Volume);
            }
            Player.Play();
            IsPlaying = true;
            _events.Publish(new TrackState(song, AudioPlayerState.Playing));
        }

        public void ChangePosition(object sender, MouseEventArgs e)
        {
            double x = e.GetPosition((ProgressBar) sender).X;
            double ratio = x/((ProgressBar) sender).ActualWidth;
            double pos = ratio*((ProgressBar) sender).Maximum;
            Player.CurrentTime = TimeSpan.FromSeconds(pos);
        }

        public void PlayPauseClick()
        {
            if (IsPlaying)
            {
                Player.Pause();
                IsPlaying = false;
                _events.Publish(new TrackState(CurrentlyPlaying, AudioPlayerState.Paused));
            }
            else
            {
                RawTrack song;
                if (_nowPlayingViewModel.NowPlayingPlaylist.CurrentEntry != null)
                    song = _nowPlayingViewModel.NowPlayingPlaylist.CurrentEntry.Model;
                else if (_nowPlayingViewModel.NowPlayingPlaylist.Count > 0)
                    song = _nowPlayingViewModel.NowPlayingPlaylist[0].Model;
                else return;

                Play(song);
            }
        }

        public void StopClick()
        {
            Player.Stop();
            IsPlaying = false;
            _events.Publish(new TrackState(CurrentlyPlaying, AudioPlayerState.Stopped));
        }

        public void PlayNext()
        {
            IsPlaying = false;
            _events.Publish(new TrackState(CurrentlyPlaying, AudioPlayerState.None));
            CurrentlyPlaying = null;
            RawTrack song = _nowPlayingViewModel.GetNextSong(RepeatState, ShuffleState);

            if (song != null)
                Play(song);
        }

        public void PlayPrev()
        {
            _events.Publish(new TrackState(CurrentlyPlaying, AudioPlayerState.None));
            RawTrack song = _nowPlayingViewModel.GetPreviousSong(RepeatState, ShuffleState);
            if (song != null)
                Play(song);
        }

        #region Properties

        private RawTrack _currentlyPlaying;
        private bool _isMuted;
        private bool _isPlaying;
        private bool _repeatState;
        private bool _shuffleState;
        private float _volume = 1.0f;

        public int Maximum
        {
            get { return _maximum; }
            set { this.RaiseAndSetIfChanged(ref _maximum, value); }
        }

        public double PositionValue
        {
            get { return _positionValue; }
            set { this.RaiseAndSetIfChanged(ref _positionValue, value); }
        }

        public int TickFrequency
        {
            get { return _tickFrequency; }
            set { this.RaiseAndSetIfChanged(ref _tickFrequency, value); }
        }

        public string TotalTime
        {
            get { return _totalTime; }
            set { this.RaiseAndSetIfChanged(ref _totalTime, value); }
        }


        public string CurrentTime
        {
            get { return _currentTime; }
            set { this.RaiseAndSetIfChanged(ref _currentTime, value); }
        }

        public float Volume
        {
            get { return _volume; }
            set
            {
                this.RaiseAndSetIfChanged(ref _volume, value);
                Player.Volume = value;
                this.RaiseAndSetIfChanged(ref _isMuted, Equals(value, 0.0f), "IsMuted");
            }
        }

        #endregion
    }
}