using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeListenPlayer
{
    class DuplicateCheck
    {
        public bool checkDup(List<SongData> songList, SongData checkSong)
        {
            var isDup = false;

            if (songList != null)
            {
                // Check if there are duplicates detected in DataGrid by Path
                foreach (SongData song in songList)
                {
                    if (checkSong.FilePath == song.FilePath)
                    {
                        //Log the duplicate!
                        isDup = true;
                        return isDup;
                    }

                    isDup = false;
                }
                return isDup;
            }
            return isDup;
        }
    }
}
