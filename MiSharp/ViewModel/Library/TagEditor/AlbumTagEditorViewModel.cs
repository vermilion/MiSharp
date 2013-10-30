using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using DeadDog.Audio;
using MiSharp.DialogResults;

namespace MiSharp
{
    [Export(typeof (AlbumTagEditorViewModel))]
    internal class AlbumTagEditorViewModel : ArtistTagEditorViewModel
    {
        private string _songAlbum;
        private string _songGenre;
        private int _songYear;

        [ImportingConstructor]
        public AlbumTagEditorViewModel(List<RawTrack> mediaToModify)
            : base(mediaToModify)
        {
            if ((from m in mediaToModify select m.AlbumTitle).Distinct().Count() > 1)
                SongAlbum = "Multiple Albums";
            else
                SongAlbum = (from m in mediaToModify select m.AlbumTitle).ToArray()[0];

            if ((from m in mediaToModify select m.Genre).Distinct().Count() > 1)
                SongGenre = "Multiple Genres";
            else
                SongGenre = (from m in mediaToModify select m.Genre).ToArray()[0];

            if ((from m in mediaToModify select m.Year).Distinct().Count() > 1)
                SongYear = -1;
            else
                SongYear = (from m in mediaToModify select m.Year).ToArray()[0];
        }

        public string SongAlbum
        {
            get { return _songAlbum; }
            set
            {
                _songAlbum = value;
                NotifyOfPropertyChange(() => SongAlbum);
            }
        }

        public string SongGenre
        {
            get { return _songGenre; }
            set
            {
                _songGenre = value;
                NotifyOfPropertyChange(() => SongGenre);
            }
        }

        public int SongYear
        {
            get { return _songYear; }
            set
            {
                _songYear = value;
                NotifyOfPropertyChange(() => SongYear);
            }
        }

        public override IEnumerable<IResult> SaveChanges()
        {
            if (string.IsNullOrEmpty(SongAlbum))
            {
                MessageBox.Show("Please provide an Album Name.", "Error", MessageBoxButtons.OK);
                yield return null;
            }

            if (string.IsNullOrEmpty(SongAlbumArtist))
            {
                MessageBox.Show("Please provide an Artist Name.", "Error", MessageBoxButtons.OK);
                yield return null;
            }

            foreach (RawTrack media in MediaList)
            {
                try
                {
                    bool hasChanged = false;

                    if (SongAlbum.Trim() != "Multiple Album Titles")
                    {
                        media.AlbumTitle = SongAlbum.Trim();
                        hasChanged = true;
                    }

                    if (SongAlbumArtist.Trim() != "Multiple Album Artists")
                    {
                        media.ArtistName = SongAlbumArtist.Trim();
                        hasChanged = true;
                    }

                    if (SongGenre != "Multiple Genres")
                    {
                        if (media.Genre != SongGenre.Trim())
                        {
                            media.Genre = SongGenre.Trim();
                            hasChanged = true;
                        }
                    }

                    if (SongYear != -1)
                    {
                        if (media.Year != SongYear)
                        {
                            media.Year = SongYear;
                            hasChanged = true;
                        }
                    }

                    //TODO:media factory
                    if (hasChanged)
                        if (media is Mp3Track)
                            (media as Mp3Track).WriteTag();
                }
                catch
                {
                    MessageBox.Show("Could not update " + media, "Error", MessageBoxButtons.OK);
                }
            }
            yield return new CloseResult();
        }
    }
}