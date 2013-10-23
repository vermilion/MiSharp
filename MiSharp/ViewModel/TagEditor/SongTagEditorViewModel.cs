using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using MiSharp.Model;

namespace MiSharp
{
    [Export(typeof (SongTagEditorViewModel))]
    internal class SongTagEditorViewModel : AlbumTagEditorViewModel
    {
        private string _songId;
        private string _songTitle;

        [ImportingConstructor]
        public SongTagEditorViewModel(List<Song> mediaToModify) : base(mediaToModify)
        {
            if ((from m in mediaToModify select m.Title).Distinct().Count() > 1)
                SongTitle = "Multiple Titles";
            else
                SongTitle = (from m in mediaToModify select m.Title).ToArray()[0];

            if ((from m in mediaToModify select m.Album).Distinct().Count() > 1)
                SongAlbum = "Multiple Albums";
            else
                SongAlbum = (from m in mediaToModify select m.Album).ToArray()[0];

            if (mediaToModify.Count > 1)
                SongComposer += "Multiple Composers";
            else
            {
                foreach (var composers in (from m in mediaToModify select m.Composers))
                {
                    if (composers == null)
                        continue;

                    foreach (string composer in composers)
                        SongComposer += composer.Trim() + ";";
                }
            }

            if ((from m in mediaToModify select m.Conductor).Distinct().Count() > 1)
                SongConductor = "Multiple Conductors";
            else
                SongConductor = (from m in mediaToModify select m.Conductor).ToArray()[0];

            if ((from m in mediaToModify select m.Artist).Distinct().Count() > 1)
                SongAlbumArtist = "Multiple Artists";
            else
                SongAlbumArtist = (from m in mediaToModify select m.Artist).ToArray()[0];

            if ((from m in mediaToModify select m.Genre).Distinct().Count() > 1)
                SongGenre = "Multiple Genres";
            else
                SongGenre = (from m in mediaToModify select m.Genre).ToArray()[0];

            if ((from m in mediaToModify select m.Year).Distinct().Count() > 1)
                SongYear = "Multiple Years";
            else
                SongYear = (from m in mediaToModify select m.Year).ToArray()[0];

            if ((from m in mediaToModify select m.TrackNumber).Distinct().Count() > 1)
                SongId = "Multiple Track Numbers";
            else
                SongId = (from m in mediaToModify select m.TrackNumber).ToArray()[0].ToString();
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

        public string SongId
        {
            get { return _songId; }
            set
            {
                _songId = value;
                NotifyOfPropertyChange(() => SongId);
            }
        }

        public override IEnumerable<IResult> SaveChanges()
        {
            if (SongTitle.Trim() == string.Empty)
            {
                MessageBox.Show("Please provide an Song Title.", "Error", MessageBoxButtons.OK);
            }

            foreach (Song media in MediaList)
            {
                try
                {
                    if (SongTitle.Trim() != "Multiple Titles")
                        media.Title = SongTitle.Trim();

                    if (SongAlbum.Trim() != "Multiple Albums")
                        media.Album = SongAlbum.Trim();

                    if (SongComposer.Trim() != "Multiple Composers")
                    {
                        var composers = new List<string>();

                        foreach (string composer in SongComposer.Trim().Split(';'))
                        {
                            if (composer != null)
                            {
                                if (composer.Trim() != string.Empty)
                                    composers.Add(composer.Trim());
                            }
                        }

                        media.Composers = composers.ToArray();
                    }

                    if (SongConductor.Trim() != "Multiple Conductors")
                        media.Conductor = SongConductor.Trim();

                    if (SongAlbumArtist.Trim() != "Multiple Artists")
                        media.Artist = SongAlbumArtist.Trim();

                    if (SongGenre.Trim() != "Multiple Genres")
                        media.Genre = SongGenre.Trim();

                    if (SongYear.Trim() != "Multiple Years")
                        media.Year = SongYear.Trim();

                    if (SongId.Trim() != "Multiple Track Numbers")
                    {
                        int outTrack = 0;

                        if (int.TryParse(SongId.Trim(), out outTrack))
                            media.TrackNumber = outTrack;
                    }

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