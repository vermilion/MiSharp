using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using MiSharp.Model;
using Screen = Caliburn.Micro.Screen;

namespace MiSharp
{
    [Export(typeof (ArtistTagEditorViewModel))]
    internal class ArtistTagEditorViewModel : Screen
    {
        protected List<Tag> MediaList;
        private string _songAlbumArtist;

        [ImportingConstructor]
        public ArtistTagEditorViewModel(List<Tag> mediaToModify)
        {
            DisplayName = "Mi# Tag Editor";
            MediaList = mediaToModify;
            if (mediaToModify.Count == 0)
            {
                return;
            }

            if ((from m in mediaToModify select m.AlbumArtist).Distinct().Count() > 1)
                SongAlbumArtist = "Multiple Artists";
            else
                SongAlbumArtist = (from m in mediaToModify select m.AlbumArtist).ToArray()[0];
        }

        public string SongAlbumArtist
        {
            get { return _songAlbumArtist; }
            set
            {
                _songAlbumArtist = value;
                NotifyOfPropertyChange(() => SongAlbumArtist);
            }
        }


        public virtual IEnumerable<IResult> SaveChanges()
        {
            if (SongAlbumArtist.Trim() == string.Empty)
            {
                MessageBox.Show("Please provide an Artist Name.", "Error", MessageBoxButtons.OK);
                yield return null;
            }

            if (SongAlbumArtist.Trim() == "Multiple Artists")
            {
                yield return new CloseResult();
            }

            foreach (Tag media in MediaList)
            {
                try
                {
                    media.AlbumArtist = SongAlbumArtist;
                    media.WriteTag();
                }
                catch
                {
                    MessageBox.Show("Could not update " + media, "Error", MessageBoxButtons.OK);
                }

                yield return new ChangesSaved();
            }
        }


        public virtual IEnumerable<IResult> Cancel()
        {
            yield return new CloseResult();
        }
    }
}