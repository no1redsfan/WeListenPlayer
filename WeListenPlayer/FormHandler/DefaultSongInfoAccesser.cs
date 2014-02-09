using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using WeListenPlayer.AmazonHandler;
using WeListenPlayer.APIClasses;

namespace WeListenPlayer.FormHandler
{
    class DefaultSongInfoAccesser
    {
        ///////////////////////////////////////////////////////
        // Row Selection Handler
        // - Assigns text variables on row select in DataGrid (Currently set as first row)
        //
        // - Uses       DefaultSongInfoAccesser i = new DefaultSongInfoAccesser();
        //              i.RetrieveSongInfo();
        // - Output     Sets Default values based on selected row
        ///////////////////////////////////////////////////////
        public void RetrieveSongInfo()
        {
            // Define new MainWindow object (for reference)
            var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

            // Show loading label..
            mainWindow.tbAlbumArtInfo.Text = "Loading Album Art...";

            var Title = "Unknown";
            var Artist = "Unknown";
            var Album = "Unknown";
            var Year = "Unknown";
            var Genre = "Unknown";
            var Path = "Unknown";

            // Cast 'item' as songData object from selected row
            SongData item = (SongData)mainWindow.dgvPlayList.Items[0];

            Title = item.Title;
            Artist = item.Artist;
            Album = item.Album;
            Year = item.Year.ToString();
            Genre = item.Genre;
            Path = item.Path;

            // Assign Text values to labels for song selected in dataGridView1
            mainWindow.tbLocalTitleInfo.Text = Title;
            mainWindow.tbLocalArtistInfo.Text = Artist;
            mainWindow.tbLocalAlbumInfo.Text = Album;
            mainWindow.tbLocalYearInfo.Text = Year;
            mainWindow.tbLocalGenreInfo.Text = Genre;
            mainWindow.tbLocalFilePathInfo.Text = Path;

            // Call Asynchronous data gathering from LastFM API (XML Parser)
            AmazonAccesser handler = new AmazonAccesser();
            handler.setMain(mainWindow); // Declare MainWindow and pass as parameter
            handler.getAmazonItems(Artist, Album, Title, "");

        }
    }
}
