using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using DeadDog.Audio.Libraries;
using MiSharp.Core.Player;
using MiSharp.Core.Repository.FileStorage;
using MiSharp.Player;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class PlayerViewModel : ReactiveObject, IHandle<Track>
    {
        private readonly BitmapImage _defaultCover =
            new BitmapImage(new Uri(@"pack://application:,,,/MiSharp;component/MusicAndCatalog.ico"));

        private readonly IEventAggregator _events;
        private readonly ObservableAsPropertyHelper<bool> _isMuted;
        private readonly NowPlayingViewModel _nowPlayingViewModel;
        private readonly ObservableAsPropertyHelper<string> _playPauseContent;

        private float _tempVolume;

        [ImportingConstructor]
        public PlayerViewModel(IEventAggregator events, NowPlayingViewModel nowPlayingViewModel)
        {
            _nowPlayingViewModel = nowPlayingViewModel;
            Player = new LocalAudioPlayer();
            Player.SongFinished += (s, e) => PlayNextCommand.Execute(null);

            _events = events;
            _events.Subscribe(this);

            PlayNextCommand = new ReactiveCommand();
            PlayNextCommand.Subscribe(param =>
                {
                    IsPlaying = false;
                    _events.Publish(new TrackState(CurrentlyPlaying, AudioPlayerState.None));
                    CurrentlyPlaying = null;
                    Track song = _nowPlayingViewModel.GetNextSong(RepeatState, ShuffleState);

                    if (song != null)
                        Play(song);
                });

            PlayPrevCommand = new ReactiveCommand();
            PlayPrevCommand.Subscribe(param =>
                {
                    IsPlaying = false;
                    _events.Publish(new TrackState(CurrentlyPlaying, AudioPlayerState.None));
                    Track song = _nowPlayingViewModel.GetPreviousSong(RepeatState, ShuffleState);
                    if (song != null)
                        Play(song);
                });

            PlayPauseCommand = new ReactiveCommand();
            PlayPauseCommand.Subscribe(param =>
                {
                    if (IsPlaying)
                    {
                        Player.Pause();
                        IsPlaying = false;
                        _events.Publish(new TrackState(CurrentlyPlaying, AudioPlayerState.Paused));
                    }
                    else
                    {
                        Track song;
                        if (_nowPlayingViewModel.NowPlayingPlaylist.CurrentEntry != null)
                            song = _nowPlayingViewModel.NowPlayingPlaylist.CurrentEntry.Model;
                        else if (_nowPlayingViewModel.NowPlayingPlaylist.Count > 0)
                            song = _nowPlayingViewModel.NowPlayingPlaylist[0].Model;
                        else return;

                        Play(song);
                    }
                });

            _playPauseContent = this.WhenAnyValue(x => x.IsPlaying, x => x ? ";" : "4")
                                    .ToProperty(this, x => x.PlayPauseContent);

            _isMuted = this.WhenAnyValue(x => x.Volume, x => Equals(x, 0.0f)).ToProperty(this, x => x.IsMuted);

            Player.TotalTimeChanged.Subscribe(x =>
                {
                    this.RaisePropertyChanged("Maximum");
                    this.RaisePropertyChanged("TickFrequency");
                    this.RaisePropertyChanged("TotalTime");
                });
            Player.CurrentTimeChanged.Subscribe(x =>
                {
                    this.RaisePropertyChanged("PositionValue");
                    this.RaisePropertyChanged("CurrentTime");
                });
        }

        public ReactiveCommand PlayNextCommand { get; private set; }
        public ReactiveCommand PlayPrevCommand { get; private set; }
        public ReactiveCommand PlayPauseCommand { get; private set; }

        public Track CurrentlyPlaying
        {
            get { return _currentlyPlaying; }
            set
            {
                this.RaiseAndSetIfChanged(ref _currentlyPlaying, value);
                this.RaisePropertyChanged("Cover");
            }
        }

        private LocalAudioPlayer Player { get; set; }

        public BitmapSource Cover
        {
            get
            {
                if (CurrentlyPlaying != null)
                {
                    return AlbumCoverRepository.Instance.GetCover(CurrentlyPlaying.Album.Title, CurrentlyPlaying.Artist.Name, CurrentlyPlaying.Album.Identifier);         
                }
                return  _defaultCover;
            }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { this.RaiseAndSetIfChanged(ref _isPlaying, value); }
        }

        public string PlayPauseContent
        {
            get { return _playPauseContent.Value; }
        }

        public bool IsMuted
        {
            get { return _isMuted.Value; }
            set
            {
                if (value)
                {
                    _tempVolume = Volume;
                    Volume = 0;
                }
                else Volume = _tempVolume;
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

        public void Handle(Track song)
        {
            Play(song);
        }

        public void Play(Track song)
        {
            if (!Equals(CurrentlyPlaying, song))
            {
                CurrentlyPlaying = song;
                Player.Load(song.Model, Volume);
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

        #region Properties

        private Track _currentlyPlaying;
        private bool _isPlaying;
        private bool _repeatState;
        private bool _shuffleState;
        private float _volume = 1.0f;

        public int Maximum
        {
            get { return (int) Player.TotalTime.TotalSeconds; }
        }

        public double PositionValue
        {
            get { return (int) Player.CurrentTime.TotalSeconds; }
        }

        public int TickFrequency
        {
            get { return (int) Player.TotalTime.TotalSeconds/30; }
        }

        public string TotalTime
        {
            get { return String.Format("{0:00}:{1:00}", (int) Player.TotalTime.TotalMinutes, Player.TotalTime.Seconds); }
        }

        public string CurrentTime
        {
            get { return String.Format("{0:00}:{1:00}", (int) Player.CurrentTime.TotalMinutes, Player.CurrentTime.Seconds); }
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

        #endregion
    }
}