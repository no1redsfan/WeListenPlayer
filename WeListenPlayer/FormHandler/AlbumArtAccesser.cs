using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeListenPlayer.APIClasses;
using WeListenPlayer.LastFmHandler;

namespace WeListenPlayer.FormHandler
{
    class AlbumArtAccesser
    {

        ///////////////////////////////////////////////////////
        // Album Art Handler
        // - Assigns Album Art url to MainWindow picture view
        //
        // - Uses       AlbumArtAccesser i = new AlbumArtAccesser();
        //              i.setAlbumArt(Title, Artist);
        // - Output     Sets url to MainWindow
        ///////////////////////////////////////////////////////
        public async void setAlbumArt()
        {
            try
            {
                // Define new MainWindow object (for reference)
                var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

                // Pull album art from determined Url
                SongData pulledSong = await new LastFmXmlParser().GetTrackInfo();

                //mainWindow.imgAlbumArt.Source = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri(Url));
            }
            catch
            {
                //MessageBox.Show("Error setting album image!");
            }
        }

    }
}
