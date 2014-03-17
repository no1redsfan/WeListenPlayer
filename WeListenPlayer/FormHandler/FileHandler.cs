using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeListenPlayer.APIClasses;
using WeListenPlayer.TagLibHandler;

namespace WeListenPlayer.FormHandler
{
    class FileHandler
    {
        private SongData newSong = new SongData();

        public List<SongData> fileDiag()
        {
            //These variables are string array lists to store song locations.
            var newPaths = new ArrayList();
            var songList = new List<SongData>();

            //Open file dialog to select tracks and add them to the play list
            var open = new OpenFileDialog { Filter = "MP3 File (*.mp3)|*.mp3;", DefaultExt = ".mp3", Multiselect = true };

            //Show open
            Nullable<bool> result = open.ShowDialog();

            //Process open file dialog box result
            if (result != false)
            {
                newPaths.AddRange(open.FileNames); //Saves the full paths
            }

            // Try to convert ArrayList to Object List (SongData)
            foreach (string path in newPaths)
            {
                newSong = new TagLibDataAccesser().getSongTags(path);
                //newSong = await dgHandler.populateDataGrid(newSong);
                songList.Add(newSong);
            }

            return songList;
        }
    }
}
