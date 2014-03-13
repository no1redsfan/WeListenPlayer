using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeListenPlayer.APIClasses;
using WeListenPlayer.TagLibHandler;

namespace WeListenPlayer.FormHandler
{
    class DirectoryHandler
    {
        private SongData newSong = new SongData();

        ///////////////////////////////////////////////////////
        // ProcessDirectory Handler
        // - Processes directory string and handles it (if/then)
        //
        // - Uses       DirectoryHandler k = new DirectoryHandler();
        //              k.processDirectory(path, false);
        // - Output     Handles directory, [Upload to DB {if boolean true}, or add to DataGrid {if boolean false}]
        ///////////////////////////////////////////////////////
        public async Task<List<SongData>> dirDiag(string targetDirectory)
        {
            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                await dirDiag(subdirectory);
            }

            var songList = new List<SongData>();

            // Process the list of files found in the directory. (Only grabs .mp3's)
            string[] fileEntries = Directory.GetFiles(targetDirectory, "*.mp3");
            foreach (string filePath in fileEntries)
            {
                newSong = new TagLibDataAccesser().getSongTags(filePath);
                songList.Add(newSong);
            }

            return songList;
        }
    }
}
