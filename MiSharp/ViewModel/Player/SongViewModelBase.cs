using System;
using Caliburn.Micro;
using MiSharp.Core;
using Rareform.Validation;

namespace MiSharp
{
    public abstract class SongViewModelBase : PropertyChangedBase
    {
        protected SongViewModelBase(Song model)
        {
            if (model == null)
                Throw.ArgumentNullException(() => model);

            Model = model;
        }

        public string Album
        {
            get { return Model.Album; }
        }

        public string Artist
        {
            get { return Model.Artist; }
        }

        public TimeSpan Duration
        {
            get { return Model.Duration; }
        }

        public string Genre
        {
            get { return Model.Genre; }
        }

        public Song Model { get; private set; }

        public string Path
        {
            get { return Model.OriginalPath; }
        }

        public string Title
        {
            get { return Model.Title; }
        }

        public int TrackNumber
        {
            get { return Model.TrackNumber; }
        }
    }
}