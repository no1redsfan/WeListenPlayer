using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeListenPlayer.APIClasses;
using WeListenPlayer.TagLibHandler;

namespace WeListenPlayer.FormHandler
{
    class DirectoryHandler
    {

        ///////////////////////////////////////////////////////
        // ProcessDirectory Handler
        // - Processes directory string and handles it (if/then)
        //
        // - Uses       DirectoryHandler k = new DirectoryHandler();
        //              k.processDirectory(path, false);
        // - Output     Handles directory, [Upload to DB {if boolean true}, or add to DataGrid {if boolean false}]
        ///////////////////////////////////////////////////////
        public void processDirectory(string targetDirectory, Boolean export)
        {
            // Process the list of files found in the directory. (Only grabs .mp3's)
            string[] fileEntries = Directory.GetFiles(targetDirectory, "*.mp3");
            foreach (string path in fileEntries)
            {
                // Define new MainWindow object (for reference)
                var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

                SongData newSong = new TagLibDataAccesser().getSongTags(path);

                if (export)
                {
                    DefaultSongInfoAccesser i = new DefaultSongInfoAccesser();
                    i.RetrieveSongInfo();

                    //establish an audio file reader to get the song durration.
                    
                    ////
                    // Add method call to upload to database
                    ////

                    var userID = 1;//this will be changed when the login for the dj is established

                    var Artist = newSong.Artist;
                    var Album = newSong.Album;
                    var Title = newSong.Title;
                    

                    ////
                    // Add method call to upload to database
                    ////

                    //Call the amazon classes to confirm info is populated correctly

                }
                else
                {
                    DataGridHandler j = new DataGridHandler();
                    j.populateDataGrid(newSong);

                    if (mainWindow.dgvPlayList.Items.Count == 1)
                    {
                        DefaultSongInfoAccesser i = new DefaultSongInfoAccesser();
                        i.RetrieveSongInfo();
                    }
                }
            }

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                processDirectory(subdirectory, false);
            }
        }
    }
}
