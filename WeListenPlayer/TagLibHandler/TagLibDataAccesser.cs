using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeListenPlayer
{
    class TagLibDataAccesser
    {

        ///////////////////////////////////////////////////////
        // TagLib File MetaData Accessor (from string URL Path)
        // - Obtains local file meta-data, assigns UNKNOWN if missing
        //
        // - Uses       SongData newSong = new TagLibDataAccesser().getSongTags({string:path})
        // - Output     New SongData object with updated field values
        ///////////////////////////////////////////////////////
        public SongData getSongTags(string path)
        {
            TagLib.File tagFile = TagLib.File.Create(path);

            uint trackNumber = tagFile.Tag.Track;
            string songTitle = tagFile.Tag.Title;
            string artist = tagFile.Tag.AlbumArtists.FirstOrDefault();
            string albumTitle = tagFile.Tag.Album;
            uint year = tagFile.Tag.Year;
            string genre = tagFile.Tag.Genres.FirstOrDefault();

            // Set Song title to file name, else UNKNOWN
            if (songTitle == null)
                try
                {
                    string editedPath = System.IO.Path.GetFileNameWithoutExtension(path);
                    songTitle = Regex.Replace(editedPath, @"^[\d-]*\s*", "");
                }
                catch
                {
                    songTitle = "Unknown";
                }

            // Set Album name to folder name holding file, else UNKNOWN
            if (albumTitle == null)
                try
                {
                    albumTitle = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path));
                }
                catch
                {
                    //
                }

            // Check for Artist in "Contributing Artists" Meta-Data
            if (artist == null || artist == "")
            {
                artist = tagFile.Tag.JoinedPerformers;
            }
            // If still null, set to UNKNOWN
            if (artist == null || artist == "")
                try
                {
                    artist = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(path)));
                }
                catch
                {
                    artist = "Unknown";
                }
            if (genre == null) { genre = "Unknown"; }

            // Year will default to 0 as a UINT if unknown

            // Assign data to new SongData object
            var newSongObject = new SongData { Title = songTitle, Artist = artist, Album = albumTitle, Year = (int)year, Genre = genre, FilePath = path};

            return newSongObject;
        }
    }
}
