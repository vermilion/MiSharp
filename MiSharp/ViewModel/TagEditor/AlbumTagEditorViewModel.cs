using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using MiSharp.Model;

namespace MiSharp
{
    [Export(typeof (AlbumTagEditorViewModel))]
    internal class AlbumTagEditorViewModel : ArtistTagEditorViewModel
    {
        private string _songAlbum;
        private string _songComposer;
        private string _songConductor;
        private string _songGenre;
        private string _songYear;

        [ImportingConstructor]
        public AlbumTagEditorViewModel(List<Tag> mediaToModify) : base(mediaToModify)
        {
            if ((from m in mediaToModify select m.Album).Distinct().Count() > 1)
                SongAlbum = "Multiple Albums";
            else
                SongAlbum = (from m in mediaToModify select m.Album).ToArray()[0];

            if ((from m in mediaToModify select m.Conductor).Distinct().Count() > 1)
                SongConductor = "Multiple Conductors";
            else
                SongConductor = (from m in mediaToModify select m.Conductor).ToArray()[0];

            if ((from m in mediaToModify select m.Genre).Distinct().Count() > 1)
                SongGenre = "Multiple Genres";
            else
                SongGenre = (from m in mediaToModify select m.Genre).ToArray()[0];

            if ((from m in mediaToModify select m.Year).Distinct().Count() > 1)
                SongYear = "Multiple Years";
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

        public string SongComposer
        {
            get { return _songComposer; }
            set
            {
                _songComposer = value;
                NotifyOfPropertyChange(() => SongComposer);
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

        public string SongYear
        {
            get { return _songYear; }
            set
            {
                _songYear = value;
                NotifyOfPropertyChange(() => SongYear);
            }
        }

        public string SongConductor
        {
            get { return _songConductor; }
            set
            {
                _songConductor = value;
                NotifyOfPropertyChange(() => SongConductor);
            }
        }

        public override IEnumerable<IResult> SaveChanges()
        {
            if (SongAlbum.Trim() == string.Empty)
            {
                MessageBox.Show("Please provide an Album Name.", "Error", MessageBoxButtons.OK);
            }

            if (SongAlbumArtist.Trim() == string.Empty)
            {
                MessageBox.Show("Please provide an Album Name.", "Error", MessageBoxButtons.OK);
            }

            foreach (Tag media in MediaList)
            {
                try
                {
                    bool hasChanged = false;

                    if (SongAlbum.Trim() != "Multiple Album Titles")
                    {
                        media.Album = SongAlbum.Trim();
                        hasChanged = true;
                    }

                    if (SongAlbumArtist.Trim() != "Multiple Album Artists")
                    {
                        media.AlbumArtist = SongAlbumArtist.Trim();
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

                    if (SongYear != "Multiple Years")
                    {
                        if (media.Year != SongYear.Trim())
                        {
                            media.Year = SongYear.Trim();
                            hasChanged = true;
                        }
                    }

                    if (SongConductor != "Multiple Conductors")
                    {
                        if (media.Conductor != SongConductor.Trim())
                        {
                            media.Conductor = SongConductor.Trim();
                            hasChanged = true;
                        }
                    }

                    if (hasChanged)
                        media.WriteTag();
                }
                catch
                {
                    MessageBox.Show("Could not update " + media, "Error", MessageBoxButtons.OK);
                }
            }
            yield return new ChangesSaved();
        }
    }
}