using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using DeadDog.Audio;

namespace MiSharp
{
    [Export(typeof(SongTagEditorViewModel))]
    internal class SongTagEditorViewModel : AlbumTagEditorViewModel
    {
        private string _songComposer;
        private int _songId;
        private string _songTitle;

        [ImportingConstructor]
        public SongTagEditorViewModel(List<RawTrack> mediaToModify)
            : base(mediaToModify)
        {
            if ((from m in mediaToModify select m.TrackTitle).Distinct().Count() > 1)
                SongTitle = "Multiple Titles";
            else
                SongTitle = (from m in mediaToModify select m.TrackTitle).ToArray()[0];

            if ((from m in mediaToModify select m.TrackNumber).Distinct().Count() > 1)
                SongId = -1;
            else
                SongId = (from m in mediaToModify select m.TrackNumber).ToArray()[0];

            //if (mediaToModify.Count > 1)
            //    SongComposer += "Multiple Composers";
            //else
            //{
            //    foreach (var composers in (from m in mediaToModify select m.Composers))
            //    {
            //        if (composers == null)
            //            continue;

            //        foreach (string composer in composers)
            //            SongComposer += composer.Trim() + ";";
            //    }
            //}
        }

        public string SongTitle
        {
            get { return _songTitle; }
            set
            {
                _songTitle = value;
                NotifyOfPropertyChange(() => SongTitle);
            }
        }

        public int SongId
        {
            get { return _songId; }
            set
            {
                _songId = value;
                NotifyOfPropertyChange(() => SongId);
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

        public override IEnumerable<IResult> SaveChanges()
        {
            if (SongTitle.Trim() == string.Empty)
            {
                MessageBox.Show("Please provide an Song Title.", "Error", MessageBoxButtons.OK);
            }

            foreach (RawTrack media in MediaList)
            {
                try
                {
                    if (SongTitle.Trim() != "Multiple Titles")
                        media.TrackTitle = SongTitle.Trim();

                    if (SongAlbum.Trim() != "Multiple Albums")
                        media.AlbumTitle = SongAlbum.Trim();

                    //if (SongComposer.Trim() != "Multiple Composers")
                    //{
                    //    var composers = new List<string>();

                    //    foreach (string composer in SongComposer.Trim().Split(';'))
                    //    {
                    //        if (composer != null)
                    //        {
                    //            if (composer.Trim() != string.Empty)
                    //                composers.Add(composer.Trim());
                    //        }
                    //    }

                    //    media.Composers = composers.ToArray();
                    //}

                    if (SongAlbumArtist.Trim() != "Multiple Artists")
                        media.ArtistName = SongAlbumArtist.Trim();

                    if (SongGenre.Trim() != "Multiple Genres")
                        media.Genre = SongGenre.Trim();

                    if (SongYear != -1)
                        media.Year = SongYear;

                    if (SongId !=-1)
                        media.TrackNumber = SongId;

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