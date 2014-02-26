using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeListenPlayer.ButtonHandler
{
    class DirButton
    {
        // MainWindow newMain; // Global Declaration

        //// Run directory processing on a new thread
        //public async Task<string> getDirectory(MainWindow mainWindow)
        //{
        //    newMain = mainWindow;


        //}

        ///////////////////////////////////////////////////////
        // Select Directory Button Handler
        // - Gets full path including sub-directories
        //
        // - Uses       string path = new DirButton().selectDirectory();
        // - Output     returns {string} path (selected in dialog)
        ///////////////////////////////////////////////////////
        public string selectDirectory()
        {
            // Define new MainWindow object (for reference)
            var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

            // Begin Windows Dialog .DLL Extension usage
            var dlg = new CommonOpenFileDialog();
            var currentDirectory = "";
            dlg.Title = "Select Music Directory";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = currentDirectory;
            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = currentDirectory;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string folder = dlg.FileName;

                mainWindow.FileText.Text = folder;

                return folder;
            }
            else
            {
                return null;
            }
        }
    }
}
