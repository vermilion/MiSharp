using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using DeadDog.Audio.Libraries;
using MiSharp.Core.Repository.FileStorage;
using MiSharp;
using ReactiveUI;

namespace MiSharp
{
    [Export]
    public class PlayerViewModel : ReactiveObject
    {
        private readonly BitmapImage _defaultCover =
            new BitmapImage(new Uri(@"pack://application:,,,/MiSharp;component/MusicAndCatalog.ico"));

        private readonly ObservableAsPropertyHelper<string> _playPauseContent;

        [ImportingConstructor]
        public PlayerViewModel(IEventAggregator events)
        {
            PlaybackController = IoC.Get<PlaybackController>();


            PlayNextCommand = new ReactiveCommand();
            PlayNextCommand.Subscribe(param => PlaybackController.PlayNext());

            PlayPrevCommand = new ReactiveCommand();
            PlayPrevCommand.Subscribe(param => PlaybackController.PlayPrev());

            PlayPauseCommand = new ReactiveCommand();
            PlayPauseCommand.Subscribe(param => PlaybackController.PlayPause());

            _playPauseContent = PlaybackController.WhenAnyValue(x => x.IsPlaying, x => x ? ";" : "4")
                .ToProperty(this, x => x.PlayPauseContent);

            PlaybackController.AudioPlayer.TotalTimeChanged.Subscribe(x =>
            {
                this.RaisePropertyChanged("Maximum");
                this.RaisePropertyChanged("TickFrequency");
                this.RaisePropertyChanged("TotalTime");
            });
            PlaybackController.AudioPlayer.CurrentTimeChanged.Subscribe(x =>
            {
                this.RaisePropertyChanged("PositionValue");
                this.RaisePropertyChanged("CurrentTime");
            });

            PlaybackController.CurrentTrackChanged.Subscribe(x =>
            {
                this.RaisePropertyChanged("CurrentlyPlaying");
                this.RaisePropertyChanged("Cover");
            });

            PlaybackController.IsMutedChanged.Subscribe(x => this.RaisePropertyChanged("IsMuted"));
            PlaybackController.VolumeChanged.Subscribe(x => this.RaisePropertyChanged("Volume"));
        }

        public PlaybackController PlaybackController { get; set; }


        public ReactiveCommand PlayNextCommand { get; private set; }
        public ReactiveCommand PlayPrevCommand { get; private set; }
        public ReactiveCommand PlayPauseCommand { get; private set; }


        public void ChangePosition(object sender, MouseEventArgs e)
        {
            double x = e.GetPosition((ProgressBar) sender).X;
            double ratio = x/((ProgressBar) sender).ActualWidth;
            double pos = ratio*((ProgressBar) sender).Maximum;
            PlaybackController.AudioPlayer.CurrentTime = TimeSpan.FromSeconds(pos);
        }

        #region Properties

        public Track CurrentlyPlaying
        {
            get { return PlaybackController.CurrentTrack != null ? PlaybackController.CurrentTrack.Track : null; }
        }

        public BitmapSource Cover
        {
            get
            {
                if (CurrentlyPlaying != null)
                {
                    return IoC.Get<AlbumCoverRepository>().GetCover(CurrentlyPlaying.Album.Title,
                        CurrentlyPlaying.Artist.Name, CurrentlyPlaying.Album.Identifier);
                }
                return _defaultCover;
            }
        }

        public string PlayPauseContent
        {
            get { return _playPauseContent.Value; }
        }

        public bool IsMuted
        {
            get { return PlaybackController.IsMuted; }
            set { PlaybackController.IsMuted = value; }
        }

        public float Volume
        {
            get { return PlaybackController.Volume; }
            set { PlaybackController.Volume = value; }
        }

        public bool RepeatState
        {
            get { return PlaybackController.RepeatState; }
            set { PlaybackController.RepeatState = value; }
        }

        public bool ShuffleState
        {
            get { return PlaybackController.ShuffleState; }
            set { PlaybackController.ShuffleState = value; }
        }

        public int Maximum
        {
            get { return (int) PlaybackController.AudioPlayer.TotalTime.TotalSeconds; }
        }

        public double PositionValue
        {
            get { return (int) PlaybackController.AudioPlayer.CurrentTime.TotalSeconds; }
        }

        public int TickFrequency
        {
            get { return (int) PlaybackController.AudioPlayer.TotalTime.TotalSeconds/30; }
        }

        public string TotalTime
        {
            get
            {
                return String.Format("{0:00}:{1:00}", (int) PlaybackController.AudioPlayer.TotalTime.TotalMinutes,
                    PlaybackController.AudioPlayer.TotalTime.Seconds);
            }
        }

        public string CurrentTime
        {
            get
            {
                return String.Format("{0:00}:{1:00}", (int) PlaybackController.AudioPlayer.CurrentTime.TotalMinutes,
                    PlaybackController.AudioPlayer.CurrentTime.Seconds);
            }
        }

        #endregion
    }
}