﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeListenPlayer1._1_WPF.APIClasses;

namespace WeListenPlayer1._1_WPF.FormHandler
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
            mainWindow.tbTitleInfo.Text = Title;
            mainWindow.tbArtistInfo.Text = Artist;
            mainWindow.tbAlbumInfo.Text = Album;
            mainWindow.tbYearInfo.Text = Year;
            mainWindow.tbGenreInfo.Text = Genre;
            mainWindow.tbFilePathInfo.Text = Path;


            // Call Asynchronous data gathering from LastFM API (XML Parser)
            AlbumArtAccesser i = new AlbumArtAccesser();
            i.setAlbumArt(Title, Artist);
        }
    }
}
