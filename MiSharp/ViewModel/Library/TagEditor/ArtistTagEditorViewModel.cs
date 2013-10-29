using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using DeadDog.Audio;

namespace MiSharp
{
    [Export(typeof(ArtistTagEditorViewModel))]
    internal class ArtistTagEditorViewModel : ReactiveScreen
    {
        protected List<RawTrack> MediaList;
        private string _songAlbumArtist;

        [ImportingConstructor]
        public ArtistTagEditorViewModel(List<RawTrack> mediaToModify)
        {
            DisplayName = "Mi# Tag Editor";
            MediaList = mediaToModify;
            if (mediaToModify.Count == 0)
            {
                return;
            }

            if ((from m in mediaToModify select m.ArtistName).Distinct().Count() > 1)
                SongAlbumArtist = "Multiple Artists";
            else
                SongAlbumArtist = (from m in mediaToModify select m.ArtistName).ToArray()[0];
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

            foreach (RawTrack media in MediaList)
            {
                try
                {
                    //media.Artist = SongAlbumArtist;
                    //media.WriteTag();
                }
                catch
                {
                    MessageBox.Show("Could not update " + media, "Error", MessageBoxButtons.OK);
                }

                yield return new CloseResult();
            }
        }


        public virtual IEnumerable<IResult> Cancel()
        {
            yield return new CloseResult();
        }
    }
}