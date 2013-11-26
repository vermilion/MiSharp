using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using ReactiveUI;
using IScreen = Caliburn.Micro.IScreen;

namespace MiSharp
{
    [Export]
    public class PlayerPanesViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public PlayerPanesViewModel()
        {
            ActivateItem(IoC.Get<NowPlayingViewModel>());

            EqualizerCommand = new ReactiveCommand();
            EqualizerCommand.Subscribe(param => ActivateItem(IoC.Get<EqualizerViewModel>()));

            NowPlayingCommand = new ReactiveCommand();
            NowPlayingCommand.Subscribe(param => ActivateItem(IoC.Get<NowPlayingViewModel>()));

            LyricsCommand = new ReactiveCommand();
            LyricsCommand.Subscribe(param => ActivateItem(IoC.Get<LyricsViewModel>()));
        }

        public ReactiveCommand EqualizerCommand { get; set; }
        public ReactiveCommand NowPlayingCommand { get; set; }
        public ReactiveCommand LyricsCommand { get; set; }
    }
}