﻿using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using MiSharp.Core;
using MiSharp.Core.Player;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class PlayerViewModel : ReactiveObject, IHandle<Song>
    {
        private readonly IEventAggregator _events;
        private readonly PlaylistViewModel _playlistViewModel;
        private string _currentTime = "00:00";
        private bool _isPlaying;
        private int _maximum = 1;

        private double _positionValue;

        private int _tickFrequency;
        private string _totalTime = "00:00";

        [ImportingConstructor]
        public PlayerViewModel(IEventAggregator events, PlaylistViewModel playlistViewModel)
        {
            _playlistViewModel = playlistViewModel;
            Player = new LocalAudioPlayer();
            Player.PlaybackUpdated += Player_PlaybackUpdated;
            Player.SongFinished += Player_SongFinished;
            _events = events;
            _events.Subscribe(this);
        }

        public LocalAudioPlayer Player { get; set; }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                this.RaiseAndSetIfChanged(ref _isPlaying, value);
            }
        }

        public void Handle(Song song)
        {
            Play(song);
        }

        private void Player_PlaybackUpdated(PlaybackEventArgs args)
        {
            TotalTime = String.Format("{0:00}:{1:00}", (int) args.TotalTime.TotalMinutes, args.TotalTime.Seconds);
            CurrentTime = String.Format("{0:00}:{1:00}", (int) args.CurrentTime.TotalMinutes, args.CurrentTime.Seconds);
            TickFrequency = (int) args.TotalTime.TotalSeconds/30;
            Maximum = (int) args.TotalTime.TotalSeconds;
            if (!_dragging)
                PositionValue = (int) args.CurrentTime.TotalSeconds;
        }

        public void Play(Song song)
        {
            Player.Load(song, Volume);
            Player.Play();
            IsPlaying = true;
            song.State = AudioPlayerState.Playing;
            _playlistViewModel.CurrentSong = song;
        }

        public void ChangePosition(double pos)
        {
            Player.CurrentTime = TimeSpan.FromSeconds(pos);
        }

        public void StartClick()
        {
            Song song = _playlistViewModel.CurrentSong;
            if (song == null) return;
            Play(song);
        }

        public void PauseClick()
        {
            Player.Pause();
            IsPlaying = false;
            _playlistViewModel.CurrentSong.State = AudioPlayerState.Paused;
        }

        public void StopClick()
        {
            Player.Stop();
            IsPlaying = false;
            _playlistViewModel.CurrentSong.State = AudioPlayerState.Stopped;
        }

        public void PlayNext()
        {
            Song song = _playlistViewModel.GetNextSong();
            if (song != null)
                Play(song);
        }

        public void PlayPrev()
        {
            Song song = _playlistViewModel.GetPreviousSong();
            if (song != null)
                Play(song);
        }

        #region Playback Events

        private void Player_SongFinished(object sender, EventArgs e)
        {
            Song song = _playlistViewModel.GetNextSong();
            if (song == null) return;
            Play(song);
        }

        #endregion

        #region Properties

        private bool _dragging;
        private float _volume = 1.0f;

        public int Maximum
        {
            get { return _maximum; }
            set
            {
                this.RaiseAndSetIfChanged(ref _maximum, value);
            }
        }

        public double PositionValue
        {
            get { return _positionValue; }
            set
            {
                this.RaiseAndSetIfChanged(ref _positionValue, value);
            }
        }

        public int TickFrequency
        {
            get { return _tickFrequency; }
            set
            {
                this.RaiseAndSetIfChanged(ref _tickFrequency, value);
            }
        }

        public string TotalTime
        {
            get { return _totalTime; }
            set
            {
                this.RaiseAndSetIfChanged(ref _totalTime, value);
            }
        }


        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                this.RaiseAndSetIfChanged(ref _currentTime, value);
            }
        }

        public float Volume
        {
            get { return _volume; }
            set
            {
                this.RaiseAndSetIfChanged(ref _volume, value);
                Player.Volume = value;
            }
        }

        public void Dragging(int flag)
        {
            _dragging = Convert.ToBoolean(flag);
        }

        #endregion
    }
}