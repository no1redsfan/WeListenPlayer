using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeListenPlayer.APIClasses;

namespace WeListenPlayer.FormHandler
{
    class DataGridHandler
    {

        ///////////////////////////////////////////////////////
        // DataGrid Population Handler
        // - Adds SongData object to DataGrid
        //
        // - Uses       DataGridHandler i = new DataGridHandler();
        //              i.populateDataGrid({SongData: newSong});
        // - Output     Checks for duplicate entries, then adds SongData to DataGrid
        ///////////////////////////////////////////////////////
        public void populateDataGrid(SongData songDataObject)
        {
            // Define new MainWindow object (for reference)
            var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

            var isDup = false;

            // Check if there are duplicates detected in DataGrid by Path
            if (mainWindow.dgvPlayList.Items.Count != 0)
            {
                foreach (SongData row in mainWindow.dgvPlayList.Items)
                {
                    if (row.Path.Equals(songDataObject.Path))
                    {
                        //Log the duplicate!
                        isDup = true;
                        break;
                    }
                }

                if (isDup == false)
                {
                    mainWindow.dgvPlayList.Items.Add(songDataObject);
                }
            }
            else
            {
                mainWindow.dgvPlayList.Items.Add(songDataObject);
                mainWindow.QueueNextSong();
            }
        }
    }
}
