using Microsoft.Win32;
using NAudio.CoreAudioApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


/////////////////////////////////////////
//// Noted BUGS to fix
//// ------------------------------------
//// - Song ends with no songs left in que (null reference error)
//// - Playlist recieving requests does not send updated que id (endless que'd songs)
//// - Removing song does not update Artwork / Wave form (-probably add queueNextSong();)
//// - Hitting next song with no next song available (null reference error)
/////////////////////////////////////////

namespace WeListenPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SongData newSong = new SongData();
        private DirectoryHandler dirHandler = new DirectoryHandler();
        private FileHandler fileHandler = new FileHandler();
        private TagLibDataAccesser tagAccesser = new TagLibDataAccesser();
        private AmazonAccesser amazonAccesser = new AmazonAccesser();
        private WeListenXmlParser xmlParser = new WeListenXmlParser();
        private UserXmlParser userParser = new UserXmlParser();
        private DuplicateCheck dupCheck = new DuplicateCheck();

        //HttpClient for WeListen API
        HttpClient client = new HttpClient();
        RequestCollection _requests = new RequestCollection();
        LoginClass login = new LoginClass();

        public MainWindow()
        {

            InitializeComponent();
            PopulateCboDevices();

            //Load WeListen API
            client.BaseAddress = new Uri("http://welistenmusic.com/api/location/{locationid}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ///////
            // NAUDIO INIT
            ///////////////////////////
            ///////////////////////////

            //create timer to get requests.

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10000);
            timer.Tick += OnTimerTick;
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
        private DispatcherTimer timer;

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

        //// Player UI Management
        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        ////////////////////////////
        //Playlist Control Methods//
        ////////////////////////////

        //method for receiving requests on a timer.
        private void OnTimerTick(object sender, EventArgs e)
        {
            DoPeriodicRequestCall();
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
        private async void DoPeriodicRequestCall()
        {
            // Declare new object
            // await xmlParser.GetTrackInfo(random);

            // TODO: call for requests from database
            var playlistSongs = getPlaylistSongs();

            try
            {
                var addList = await Task.Run(() => xmlParser.GetTrackInfo(playlistSongs, false));

                foreach (SongData song in addList)
                {
                    dgvPlayList.Items.Add(song);
                    QueueNextSong();
                }
                if (dgvPlayList.Items.Count < 2)
                {
                    
                    var addRandom = await Task.Run(() => xmlParser.GetTrackInfo(playlistSongs, true));
                    foreach (SongData song in addRandom)
                    {
                        dgvPlayList.Items.Add(song);
                        QueueNextSong();
                    }
                }

            }
            catch
            {
                
                MessageBox.Show("No songs in request que!");
            }

        }

        ////////////////////////////////////////
        //methods for background audio control//
        ////////////////////////////////////////

        
        //Method for populating Devices combo box
        private void PopulateCboDevices()
        {
            ////
            // Add sound cards to main selection menu at top
            ////

            // Create arbitrary integer for numbering
            var num = 0;

            var deviceEnumeratior = new MMDeviceEnumerator();
            var devices = deviceEnumeratior.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            foreach (var device in devices)
            {

                MenuItem soundCardSelect = new MenuItem();
                MenuItem newMenuItem = (MenuItem)this.MainMenu.Items[2];
                soundCardSelect.Header = device;
                soundCardSelect.Click += cboDevices_SelectionChanged;
                soundCardSelect.Tag = num;
                soundCardSelect.IsCheckable = true;
                soundCardSelect.Name = "SoundCard" + num;
                newMenuItem.Items.Add(soundCardSelect);

                // increase num value for .Tags
                num++;
            }

            // Select / Check & Disable first selected soundcard
            ((MenuItem)SoundSelect.Items[0]).IsChecked = true;
            ((MenuItem)SoundSelect.Items[0]).IsEnabled = false;
        }

        //Method to remove songs from playlist
        private void RemoveSongFromPlayList(int index)
        {
            // remove the played song from playlist data grid
            dgvPlayList.Items.RemoveAt(index);
            QueueNextSong();
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

        private async void fileSelector (object sender, RoutedEventArgs e)
        {
            // Return from file dialog
            var files = fileHandler.fileDiag();

            // Get songs in playlist
            var playlistSongs = getPlaylistSongs();

            if (files != null)
            {
                // Get amazon data on each SongData obj in files
                foreach (SongData song in files)
                {
                    // Assign path to variable
                    var path = song.FilePath;

                    SongData amazonSong = await Task.Run(() => amazonAccesser.getAmazonInfo(song));

                    // Set amazonSong Path
                    amazonSong.FilePath = path;

                    // Check for duplicate values
                    bool isDup = dupCheck.checkDup(playlistSongs, amazonSong);

                    if (!isDup)
                    {
                        // Add AmazonSong to playlist
                        dgvPlayList.Items.Add(amazonSong);
                    }
                }
                QueueNextSong();
            }
            else
            {
                MessageBox.Show("There was a problem getting the selected files.");
            }
        }

        private async void dirSelector(object sender, RoutedEventArgs e)
        {
            // Set targetDirectory string
            string targetDiredctory = new DirButton().selectDirectory();

            // Set master List
            List<SongData> songList = new List<SongData>();

            try
            {
                // Return from directory dialog
                var files = await dirHandler.dirDiag(targetDiredctory, songList);

                // Get songs in playlist
                var playlistSongs = getPlaylistSongs();

                if (files != null)
                {
                    foreach (SongData song in files)
                    {
                        // Assign path to variable
                        var path = song.FilePath;

                        // Get full request
                        SongData amazonSong = await Task.Run(() => amazonAccesser.getAmazonInfo(song));

                        // Set amazonSong Path
                        amazonSong.FilePath = path;

                        // Check for duplicate values
                        bool isDup = dupCheck.checkDup(playlistSongs, amazonSong);

                        if (!isDup)
                        {
                            // Add AmazonSong to playlist
                            dgvPlayList.Items.Add(amazonSong);
                        }
                    }
                    QueueNextSong();
                }
                else
                {
                    MessageBox.Show("There was a problem getting the folder path.");
                }
            }
            catch
            {
                MessageBox.Show("There was a problem getting the folder path.");
            }
        }

        public List<SongData> getPlaylistSongs()
        {
            var playlist = new List<SongData>();
            var songCount = dgvPlayList.Items.Count;

            if (songCount != 0)
            {
                foreach(SongData song in dgvPlayList.Items)
                {
                    playlist.Add(song);
                }
                return playlist;
            }
            return null;
        }

        public void QueueNextSong()
        {
            // Add method call to upload to database
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://welistenmusic.com/api/locations/3");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //bool random;

            var playlist = getPlaylistSongs();

            //var k = new WeListenXmlParser();
            //if (dgvPlayList.Items.IsEmpty)
            //{
            //    random = false;
            //}
            //if (dgvPlayList.Items.Count == 1)
            //{
            //    random = true;

            //}

            if (playlist != null)
            {
                var playItem = (SongData)dgvPlayList.Items[0];
                var path = playItem.FilePath.Replace("\\\\", "\\");

                //// If path is invalid (on current pc), Set row background as RED (as a warning)
                //var row = dgvPlayList.ItemContainerGenerator.ContainerFromItem(dgvPlayList.Items[0]) as DataGridRow;
                //row.Background = Brushes.Red;

                //NAudioEngine.Instance.Volume((float)sldrVolume.Value);  

                NAudioEngine.Instance.OpenFile(path);
                var playListId = playItem.PlaylistId;
                
                //report a song as played to the DB
                var response = Task.Run(() => client.PostAsJsonAsync("api/locations", playListId).Result);
                
                //FileText.Text = path;
                //random = false;

                setLabels(playlist[0]);
            }
        }

        //method for starting to receive the requests
        private void OnRecieveRequestClick(object sender, RoutedEventArgs e)
        {
            
            if (!receiving)
            {
                timer.Start();
                receiving = true;
                btnStartReceiving.Content = "Stop Receiving Requests";
            }
            else if (receiving)
            {
                timer.Stop();
                receiving = false;
                btnStartReceiving.Content = "Start Receiving Requests";
            }
        }

        private void btnRegisterLink_Click(object sender, RoutedEventArgs e)
        {
            string target = "http://www.welistenmusic.com";
            try
            {
                System.Diagnostics.Process.Start(target);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        //button click for adding music files to the dB
        private async void OnUploadFolderToDbClick(object sender, RoutedEventArgs e)
        {
            // Add method call to upload to database
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://welistenmusic.com/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Set targetDirectory string
            string targetDiredctory = new DirButton().selectDirectory();

            // Set master List
            List<SongData> songList = new List<SongData>();
            List<SongData> files;

            try
            {
                // Return from directory dialog
                files = await dirHandler.dirDiag(targetDiredctory, songList);
            }
            catch
            {
                files = null;
            }

            if (files != null)
            {
                var processCount = 0;
                var addedCount = 0;
                var notAddedCount = 0;
                lblProgressCount.Content = processCount;
                lblAddedCount.Content = addedCount;
                lblRejectedCount.Content = notAddedCount;

                foreach (SongData song in files)
                {
                    // Assign path to variable
                    var path = song.FilePath;

                    processCount++;
                    lblProgressCount.Content = processCount;
                    ProcessEvent();
                    // Get full request
                    SongData amazonSong = await amazonAccesser.getAmazonInfo(song);

                    // Set location / user ID
                    amazonSong.UserID = 3;
                    amazonSong.LocationID = 3;

                    // Set and adjust file paths for DB storage
                    amazonSong.FilePath = path;

                    var response = client.PostAsJsonAsync("api/song", amazonSong).Result;
                    

                    if (response.IsSuccessStatusCode)
                    {
                        //MessageBox.Show("Song Added");
                        addedCount++;
                        lblAddedCount.Content = addedCount;
                        ProcessEvent();
                    }
                    else
                    {
                        notAddedCount++;
                        lblRejectedCount.Content = notAddedCount;
                        ProcessEvent();
                        //MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    }
                }
                MessageBox.Show("Songs Added:  " + addedCount + "  Songs Rejected:  " + notAddedCount);
            }
        }

        public void ProcessEvent()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrames), frame);
            Dispatcher.PushFrame(frame);
        }

        public object ExitFrames(object f)
        {
            ((DispatcherFrame)f).Continue = false;

            return null;
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
        private async void dgvPlayList_Drop(object sender, DragEventArgs e)
        {
            // Get songs in playlist
            var playlistSongs = getPlaylistSongs();

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //These variables are string array lists to store song locations.
                var songList = new List<SongData>();

                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var path in files)
                {
                    newSong = new TagLibDataAccesser().getSongTags(path);
                    songList.Add(newSong);
                }

                if (songList != null)
                {
                    foreach (SongData song in songList)
                    {
                        // Assign path to variable
                        var path = song.FilePath;

                        // Get full request
                        SongData amazonSong = await amazonAccesser.getAmazonInfo(song);

                        // Set amazonSong Path
                        amazonSong.FilePath = path;

                        // Add song to master List
                        //songList.Add(amazonSong);

                        // Check for duplicate values
                        bool isDup = dupCheck.checkDup(playlistSongs, amazonSong);

                        if (!isDup)
                        {
                            // Add AmazonSong to playlist
                            dgvPlayList.Items.Add(amazonSong);
                        }
                    }
                }
                QueueNextSong();
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
                        var foundArt = true;
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
                                catch
                                {
                                    foundArt = false;
                                }

                                if (foundArt == false)
                                {
                                    var playItem = (SongData)dgvPlayList.Items[0];

                                    try
                                    {
                                        albumArtPanel.AlbumArtImage = BitmapFrame.Create(new Uri(playItem.Artwork));
                                    }
                                    catch
                                    {
                                        albumArtPanel.AlbumArtImage = null;
                                        // System.NotSupportedException:
                                        // No imaging component suitable to complete this operation was found.
                                    }
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
                    
                    try
                    {
                        var playItem2 = (SongData)dgvPlayList.Items[0];
                        albumArtPanel.AlbumArtImage = BitmapFrame.Create(new Uri(playItem2.Artwork));
                    }
                    catch
                    {
                        albumArtPanel.AlbumArtImage = null;
                        // System.NotSupportedException:
                        // No imaging component suitable to complete this operation was found.
                    }
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
                NAudioEngine.Instance.Volume((float)sldrVolume.Value);
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
                //AddSongsToPlaylist(newFiles, newPaths);
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

        private void cboDevices_SelectionChanged(object sender, System.EventArgs e)
        {
            // Set selected item from menu
            var selectedItem = ((MenuItem)sender);

            // Reset checkmarks and enable all options
            foreach (MenuItem item in SoundSelect.Items)
            {
                item.IsChecked = false;
                item.IsEnabled = true;
            }

            // Assign selected Sound Card to selected menu item
            NAudioEngine.Instance.selectedSoundCard = (int)selectedItem.Tag;
            selectedItem.IsChecked = true;
            selectedItem.IsEnabled = false;
        }

        private void setLabels(SongData localObj)
        {
           
            // Assign Song labels
            tbAmazonArtistInfo.Text = localObj.Artist;
            tbAmazonAlbumInfo.Text = localObj.Album;
            tbAmazonTitleInfo.Text = localObj.Title;
            tbAmazonYearInfo.Text = localObj.Year.ToString();
            tbAmazonAsinInfo.Text = localObj.ASIN;
            tbAmazonPriceInfo.Text = localObj.Price;
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            User returnedUser;

            var usr = tbUserName.Text;
            var pas = pwbPassword.Password.ToString();

            try
            {
                returnedUser = await userParser.GetUserInfo(usr, pas);
            }
            catch
            {
                returnedUser = null;
            }

            if (returnedUser != null)
            {
                MessageBox.Show("Username Confirmed! Logging in...");
                loggedIn = true;
            }
            else
            {
                MessageBox.Show("Username and/or Password Invalid!");
                loggedIn = false;
            }
        }
    }
}

