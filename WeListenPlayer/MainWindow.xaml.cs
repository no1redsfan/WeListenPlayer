using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VisualizationLib;
using WeListenPlayer.AmazonHandler;
using WeListenPlayer.APIClasses;
using WeListenPlayer.ButtonHandler;
using WeListenPlayer.FormHandler;
using WeListenPlayer.LastFmHandler;
using WeListenPlayer.NAudioHandler;
using WeListenPlayer.TagLibHandler;



namespace WeListenPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //HttpClient for WeListen API
        HttpClient client = new HttpClient();
        RequestCollection _requests = new RequestCollection();
        LoginClass login = new LoginClass();

        public MainWindow()
        {

            InitializeComponent();
            var initializer = new AmazonAccesser();
            initializer.setMain(this); // Declare MainWindow and pass as parameter
            initializer.getAmazonItems("test", "test", "test", "");
            PopulateCboDevices();

            //Load WeListen API
            client.BaseAddress = new Uri("http://welistenmusic.com/api/location/{locationid}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ///////
            // NAUDIO INIT
            ///////////////////////////
            ///////////////////////////

            NAudioEngine soundEngine = NAudioEngine.Instance;
            soundEngine.PropertyChanged += NAudioEngine_PropertyChanged;

            UIHelper.Bind(soundEngine, "CanStop", StopButton, Button.IsEnabledProperty);
            UIHelper.Bind(soundEngine, "CanPlay", PlayButton, Button.IsEnabledProperty);
            UIHelper.Bind(soundEngine, "CanPause", PauseButton, Button.IsEnabledProperty);
            UIHelper.Bind(soundEngine, "CanStop", btnSkipBck, Button.IsEnabledProperty);
            UIHelper.Bind(soundEngine, "CanStop", btnSkipFwd, Button.IsEnabledProperty);

            spectrumAnalyzer.RegisterSoundPlayer(soundEngine);
            waveformTimeline.RegisterSoundPlayer(soundEngine);
            
            NAudioEngine.Instance.volumeValue = (float)sldrVolume.Value;

            LoadExpressionDarkTheme();
        }

        //Declare Variables
        private bool receiving = false;
        private bool random;

        //variable for login status
        private bool loggedIn = false;

        ///////////////////
        //Control Methods//
        ///////////////////

        //////////////////////////////
        //Audio play control methods//
        //////////////////////////////

        //Method for back button  -  Stop button becomes the back button.

        //Method for forward button
        private void OnForwardClick(object sender, RoutedEventArgs e)
        {
                NAudioEngine.Instance.continuousPlay = true;
                NAudioEngine.Instance.Stop();
        }


        //Method for volume slider change
        private void OnSldrVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (NAudioEngine.Instance.IsPlaying)
            {
                NAudioEngine.Instance.Volume((float) sldrVolume.Value);  
            }
        }



        ////////////////////////////
        //Playlist Control Methods//
        ////////////////////////////

        //method for starting to receive the requests
        private void OnRecieveRequestClick(object sender, RoutedEventArgs e)
        {
            
            var dueTime = TimeSpan.FromSeconds(10);
            var interval = TimeSpan.FromSeconds(10);
            CancellationToken stopReceiving;
            var recieving = false;
            if (receiving == false)
            {
                receiving = true;
                DoPeriodicRequestCall(dueTime, interval, stopReceiving);
            }
            else
            {
                stopReceiving.IsCancellationRequested.Equals(true);
                receiving = false;
                DoPeriodicRequestCall(dueTime, interval, stopReceiving);
            }

            // TODO: Add a CancellationTokenSource and supply the token here instead of None. 
           
        }

        //Method for moving items up in playlist
        private void OnMoveUpClick(object sender, RoutedEventArgs e)
        {
            const int movement = -1;
            if (NAudioEngine.Instance.IsPlaying)
            {
                //If its playing or paused you can not move the item into the playing position of the list.
                if (dgvPlayList.SelectedIndex > 1 && dgvPlayList.SelectedIndex != -1)
                {
                    //Move the item up the playlist
                    MoveItemInPlaylist(movement);
                }

            }
            else
            {
                //If the player is stopped you can move the item into the play position of the list.
                if (dgvPlayList.SelectedIndex > 0 && dgvPlayList.SelectedIndex != -1)
                {
                    MoveItemInPlaylist(movement);
                }
            }
        }

        //Method for moving items down in playlist
        private void OnMoveDownClick(object sender, RoutedEventArgs e)
        {
            const int movement = 2;
            //If its playing or paused you can not move the item in the play position of the list.
            if (NAudioEngine.Instance.IsPlaying)
            {
                if (dgvPlayList.SelectedIndex > 0 && dgvPlayList.SelectedIndex != dgvPlayList.Items.Count -1)
                {
                    MoveItemInPlaylist(movement);
                }
            }
            else if (dgvPlayList.SelectedIndex != -1 && dgvPlayList.SelectedIndex != dgvPlayList.Items.Count -1)
            {
                MoveItemInPlaylist(movement);
            }
        }

        //Method for removing song from playlist
        private void OnRemoveSongClick(object sender, RoutedEventArgs e)
        {
            var index = dgvPlayList.SelectedIndex;
            //If song is playing or paused you can not remove the item in the play position of the list.
            if (NAudioEngine.Instance.IsPlaying)
            {
                if (dgvPlayList.SelectedIndex > 0 && dgvPlayList.SelectedIndex != dgvPlayList.Items.Count)
                {
                    RemoveSongFromPlayList(index);
                    dgvPlayList.SelectedIndex = index;
                }
            }
            else if (dgvPlayList.SelectedIndex != -1 && dgvPlayList.SelectedIndex != dgvPlayList.Items.Count)
            {
                RemoveSongFromPlayList(index);
                dgvPlayList.SelectedIndex = index;
            }
        }


        /////////////////////////////////////////////
        //Methods for handling requests from the web
        /////////////////////////////////////////////
        
        //call the get request query on intervals
        private async void DoPeriodicRequestCall(TimeSpan dueTime, TimeSpan interval, CancellationToken token)
        {
            // Declare new object
            var k = new WeListenXmlParser();
            await k.GetTrackInfo(random);

            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                // TODO: call for requests from database
                
                //await k.GetTrackInfo();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }

        ////////////////////////////////////////
        //methods for background audio control//
        ////////////////////////////////////////

        //method for adding song to playlist
        private void AddSongsToPlaylist(ArrayList newFiles, ArrayList newPaths)
        {
            foreach (string path in newPaths)
            {
                var newSong = new TagLibDataAccesser().getSongTags(path);

                var j = new DataGridHandler();
                j.populateDataGrid(newSong);
               
                if (dgvPlayList.Items.Count == 1)
                {
                    var i = new DefaultSongInfoAccesser();
                    i.RetrieveSongInfo();

                    QueueNextSong();
                }
            }
        }
        //Method for populating Devices combo box
        private void PopulateCboDevices()
        {
            var deviceEnumeratior = new MMDeviceEnumerator();
            var devices = deviceEnumeratior.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            foreach (var device in devices)
            {
                cboDevices.Items.Add(devices);
            }
            //Select default automatically
            cboDevices.SelectedIndex = 0;
        }

        //Method to start playing the selected song
        private void PlaySelectedSong()
        {

            if (!dgvPlayList.Items.IsEmpty)
            {
                //Select the top song in the playlist data grid
                var playItem = (SongData)dgvPlayList.Items[0];

                var i = new DefaultSongInfoAccesser();
                i.RetrieveSongInfo();

                //populate string with the song in the first position of the path arrayList
                
                string path = playItem.Path.Replace("\\\\", "\\");//Add error handling

                    // If path is invalid (on current pc), Set row background as RED (as a warning)
                    var row = dgvPlayList.ItemContainerGenerator.ContainerFromItem(dgvPlayList.Items[0]) as DataGridRow;
                    row.Background = Brushes.Red;
                }
        }


        //Method to remove songs from playlist
        private void RemoveSongFromPlayList(int index)
        {
                // remove the played song from playlist data grid
                dgvPlayList.Items.RemoveAt(index); 
        }

        //method to move items in playlist and arrayLists
        private void MoveItemInPlaylist(int movement)
        {
            //Store the value of the selected song
            var selectedIndex = dgvPlayList.SelectedIndex;

            //Copy the item in the playlist
            var itemToMove = dgvPlayList.SelectedItem;
            
            dgvPlayList.Items.Insert(selectedIndex + movement, itemToMove);
                    
                    if (movement != 2)
                    {
                        selectedIndex++;
                    }

                    //remove the song from the original position
                    RemoveSongFromPlayList(selectedIndex);

                    //Change the selected index to the new position
                    dgvPlayList.SelectedItem = itemToMove;
        }

        // Dispose if the window is closed
        private void Window_Closed(object sender, EventArgs e)
        {
            NAudioEngine.Instance.Stop();
        }


        ////////////////////////////
        // ROBERT - MEDIA MANAGER //
        ////////////////////////////

        //////////////////////////////////////////////////////
        // WORKING - KEEP IN MAIN
        // Browse Directory Button Handler
        // - Requests browser explorer, sets folder path
        ///////////////////////////////////////////////////////
        private void OnBtnImport_Click(object sender, RoutedEventArgs e)
        {

            string path = new DirButton().selectDirectory();

            if (path != null)
            {
                var k = new DirectoryHandler();
                k.processDirectory(path, false);
            }
            else
            {
                MessageBox.Show("There was a problem getting the folder path.");
            }
        }

        //button click for adding music files to the dB
        private void OnUploadFolderToDbClick(object sender, RoutedEventArgs e)
        {
            string path = new DirButton().selectDirectory();
            if (path != null)
            {
                var k = new DirectoryHandler();
                k.processDirectory(path, true);
            }
            else
            {
                MessageBox.Show("There was a problem getting the folder path.");
            }
        }

        ///////////////////////////////////////////////////////
        // WORKING - KEEP IN MAIN
        // Drag Over handler for DataGrid
        // - Allows drag/drop and changes bg color
        ///////////////////////////////////////////////////////
        private void dgvPlayList_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                var grid = sender as DataGrid;
                grid.Background = new SolidColorBrush(Color.FromRgb(155, 155, 155));
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        ///////////////////////////////////////////////////////
        // WORKING - KEEP IN MAIN
        // Drag Exit handler for DataGrid
        // - Changes bg color back when exiting DataGrid
        ///////////////////////////////////////////////////////
        private void dgvPlayList_DragLeave(object sender, DragEventArgs e)
        {
            var grid = sender as DataGrid;
            grid.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
        }

        ///////////////////////////////////////////////////////
        // WORKING - KEEP IN MAIN
        // File Drop handler for DataGrid
        // - Process through each file dropped into DataGrid
        ///////////////////////////////////////////////////////
        private void dgvPlayList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var path in files)
                {
                    SongData newSong = new TagLibDataAccesser().getSongTags(path);

                    var j = new DataGridHandler();
                    j.populateDataGrid(newSong);
                }

                var i = new DefaultSongInfoAccesser();
                i.RetrieveSongInfo();
            }
        }

        //Method for login button submit click
        private void OnSubmitClick(object sender, RoutedEventArgs e)
        {
            if (loggedIn == false)
            {

            }
            else
            {

            }
        }

        //query the WeListen API for a list of requests
        private async void GetLoginStatus(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await client.GetAsync("api/login");
                response.EnsureSuccessStatusCode(); // Throw on error code.

                var requests = await response.Content.ReadAsAsync<LoginClass>();
                //_requests.CopyFrom(requests);
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                // This exception indicates a problem deserializing the request body.
                MessageBox.Show(jEx.Message);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }



        ////////////////////////////////////////////
        ////////////////////////////////////////////
        ////////////////////////////////////////////

        // NAUDIO STUFF

        ////////////////////////////////////////////
        ////////////////////////////////////////////
        ////////////////////////////////////////////





        #region NAudio Engine Events
        private void NAudioEngine_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var engine = NAudioEngine.Instance;
            switch (e.PropertyName)
            {
                case "FileTag":
                    if (engine.FileTag != null)
                    {
                        var tag = engine.FileTag.Tag;
                        if (tag.Pictures.Length > 0)
                        {
                            using (var albumArtworkMemStream = new MemoryStream(tag.Pictures[0].Data.Data))
                            {
                                try
                                {
                                    var albumImage = new BitmapImage();
                                    albumImage.BeginInit();
                                    albumImage.CacheOption = BitmapCacheOption.OnLoad;
                                    albumImage.StreamSource = albumArtworkMemStream;
                                    albumImage.EndInit();
                                    albumArtPanel.AlbumArtImage = albumImage;
                                }
                                catch (NotSupportedException)
                                {
                                    albumArtPanel.AlbumArtImage = null;
                                    // System.NotSupportedException:
                                    // No imaging component suitable to complete this operation was found.
                                }
                                albumArtworkMemStream.Close();
                            }
                        }
                        //////////////////////////////////////////////////////////////////////////////
                        // add an else if statement here to pull the album image from the Amazon API
                        //////////////////////////////////////////////////////////////////////////////
                        else
                        {
                            albumArtPanel.AlbumArtImage = null;
                        }
                    }
                    else
                    {
                        albumArtPanel.AlbumArtImage = null;
                    }
                    break;
                case "ChannelPosition":
                    clockDisplay.Time = TimeSpan.FromSeconds(engine.ChannelPosition);
                    break;
                default:
                    // Do Nothing
                    break;
            }

        }
        #endregion

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (NAudioEngine.Instance.CanPlay)
            {
                //Mark continuous play
                NAudioEngine.Instance.continuousPlay = true;
                NAudioEngine.Instance.Play();
            }
            else
            {
                if (dgvPlayList.Items.Count == 0) return;
                //Mark continuous play
                NAudioEngine.Instance.continuousPlay = true;
                    
                QueueNextSong();
                NAudioEngine.Instance.Play();
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (NAudioEngine.Instance.CanPause)
                NAudioEngine.Instance.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (NAudioEngine.Instance.CanStop)
                NAudioEngine.Instance.continuousPlay = false;
                clockDisplay.Time = TimeSpan.FromSeconds(0);
                NAudioEngine.Instance.Stop();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void LoadDefaultTheme()
        {
            DefaultThemeMenuItem.IsChecked = true;
            DefaultThemeMenuItem.IsEnabled = false;
            ExpressionDarkMenuItem.IsChecked = false;
            ExpressionDarkMenuItem.IsEnabled = true;
            ExpressionLightMenuItem.IsChecked = false;
            ExpressionLightMenuItem.IsEnabled = true;

            Resources.MergedDictionaries.Clear();
        }

        private void LoadDarkBlueTheme()
        {
            DefaultThemeMenuItem.IsChecked = false;
            DefaultThemeMenuItem.IsEnabled = true;
            ExpressionDarkMenuItem.IsChecked = false;
            ExpressionDarkMenuItem.IsEnabled = true;
            ExpressionLightMenuItem.IsChecked = false;
            ExpressionLightMenuItem.IsEnabled = true;

            Resources.MergedDictionaries.Clear();
            var themeResources = Application.LoadComponent(new Uri("DarkBlue.xaml", UriKind.Relative)) as ResourceDictionary;
            Resources.MergedDictionaries.Add(themeResources);
        }

        private void LoadExpressionDarkTheme()
        {
            DefaultThemeMenuItem.IsChecked = false;
            DefaultThemeMenuItem.IsEnabled = true;
            ExpressionDarkMenuItem.IsChecked = true;
            ExpressionDarkMenuItem.IsEnabled = false;
            ExpressionLightMenuItem.IsChecked = false;
            ExpressionLightMenuItem.IsEnabled = true;

            Resources.MergedDictionaries.Clear();
            var themeResources = Application.LoadComponent(new Uri("Themes/ExpressionDark.xaml", UriKind.Relative)) as ResourceDictionary;
            Resources.MergedDictionaries.Add(themeResources);
        }

        private void LoadExpressionLightTheme()
        {
            DefaultThemeMenuItem.IsChecked = false;
            DefaultThemeMenuItem.IsEnabled = true;
            ExpressionDarkMenuItem.IsChecked = false;
            ExpressionDarkMenuItem.IsEnabled = true;
            ExpressionLightMenuItem.IsChecked = true;
            ExpressionLightMenuItem.IsEnabled = false;

            Resources.MergedDictionaries.Clear();
            var themeResources = Application.LoadComponent(new Uri("Themes/ExpressionLight.xaml", UriKind.Relative)) as ResourceDictionary;
            Resources.MergedDictionaries.Add(themeResources);
        }

        private void DefaultThemeMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            LoadDefaultTheme();
        }

        private void ExpressionDarkMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            LoadExpressionDarkTheme();
        }

        private void ExpressionLightMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            LoadExpressionLightTheme();
        }

        //////////////////////////////////////////////////////
        // WORKING - KEEP IN MAIN
        // Browse File Button Handler
        // - Requests browser explorer, sets item path
        ///////////////////////////////////////////////////////
        private void OnAddSongClick(object sender, RoutedEventArgs e)
        {

            //Create arrayLists for new files to add
            //These variables are string array lists to store song locations.
            var newFiles = new ArrayList();
            var newPaths = new ArrayList();

            //Open file dialog to select tracks and add them to the play list
            var open = new OpenFileDialog {Filter = "MP3 File (*.mp3)|*.mp3;", DefaultExt = ".mp3", Multiselect = true};

            //Show open
            Nullable<bool> result = open.ShowDialog();

            //Process open file dialog box result
            if (result == true)
            {
                //add files to arrays
                newFiles.AddRange(open.SafeFileNames); //Saves only the names
                newPaths.AddRange(open.FileNames); //Saves the full paths
            }
            //Call addSong Method
            AddSongsToPlaylist(newFiles, newPaths);
        }
        private void OpenFile()
        {
            //Create arrayLists for new files to add
            //These variables are string array lists to store song locations.
            var newFiles = new ArrayList();
            var newPaths = new ArrayList();

            var openDialog = new OpenFileDialog
            {
                Filter = "(*.mp3)|*.mp3",
                DefaultExt = ".mp3",
                Multiselect = true
            };

            if (openDialog.ShowDialog() == true)
            {
                //add files to arrays
                newFiles.AddRange(openDialog.SafeFileNames); //Saves only the names
                newPaths.AddRange(openDialog.FileNames); //Saves the full paths

                //Call addSong Method
                AddSongsToPlaylist(newFiles, newPaths);
                if (!NAudioEngine.Instance.IsPlaying)
                    QueueNextSong();
                
            }
        }

        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            NAudioEngine.Instance.Dispose();
        }

        public void SongStopped(int index)
        {
                //If continuous is checked play next song.
            if (NAudioEngine.Instance.continuousPlay)
                
                RemoveSongFromPlayList(index);
                dgvPlayList.SelectedIndex = 0;
                QueueNextSong();
            
        }

        public async void QueueNextSong()
        {
            var k = new WeListenXmlParser();
            if (dgvPlayList.Items.IsEmpty)
            {
                random = false;
                await k.GetTrackInfo(random);
            }
            if (dgvPlayList.Items.IsEmpty)
            {
                random = true;
                
                await k.GetTrackInfo(random);
            }
 

            var playItem = (SongData)dgvPlayList.Items[0];
            var path = playItem.Path.Replace("\\\\", "\\");
            NAudioEngine.Instance.OpenFile(path);
            FileText.Text = path;
            random = false;
        }

        private void cboDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NAudioEngine.Instance.selectedSoundCard =cboDevices.SelectedIndex;
        }
    }
}

