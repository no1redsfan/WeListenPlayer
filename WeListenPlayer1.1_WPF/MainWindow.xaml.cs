using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Web;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Data;
using System.ComponentModel;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using WeListenPlayer1._1_WPF.APIClasses;
using Newtonsoft.Json.Linq;
using System.Xml;
using Newtonsoft.Json;

namespace WeListenPlayer1._1_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Initialize Global Variable
        // Get your own API_KEY and API_SECRET from http://www.last.fm/api/account
        string API_KEY = "69473ce24376ae029d0b35211016700c";
        string API_SECRET = "305fef5dd50570f5da565ffb863a65ef";

        //HttpClient for WeListen API
        HttpClient client = new HttpClient();
        RequestCollection _requests = new RequestCollection();
        LoginClass login = new LoginClass();

        //HttpWebRequest myWebRequest = HttpWebRequest.Create(http://welistenmusic.com/api/locations/1);
          //  myWebRequest.ContentType
        
        public MainWindow()
        {
            InitializeComponent();

            //create timer to track song position
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += OnTimerTick;
            EnableControls();
            PopulateCboDevices();

            //Load WeListen API
            client.BaseAddress = new Uri("http://welistenmusic.com/api/location/{locationid}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.TestRequestList.ItemsSource = _requests;
        }

        //Declare Variables
        //for wasapiOut
        private WasapiOut wasapiOut;
        private AudioFileReader reader;
        private DispatcherTimer timer;
        private bool receiving = false;

        //variable for login status
        private bool loggedIn = false;

        ///////////////////
        //Control Methods//
        ///////////////////

        //////////////////////////////
        //Audio play control methods//
        //////////////////////////////

        //Method for when the play button is clicked
        private void OnPlayClick(object sender, RoutedEventArgs e)
        {
            //Check to see if the buffer is loaded.
            if (wasapiOut != null)
            {
                //If buffer is loaded
                //Check to see if playback is paused.
                if (wasapiOut.PlaybackState == PlaybackState.Paused)
                {
                    //Mark continuous play
                    chkContinuousPlay.IsChecked = true;
                    //Play the song from current position
                    wasapiOut.Play();
                    //enable the appropriate controls
                    EnableControls();
                }
            }
            else
            {
                //Check continuous
                chkContinuousPlay.IsChecked = true;
                PlaySelectedSong();
            }
        }

        //Method for when the stop button is clicked
        private void OnStopClicked(object sender, RoutedEventArgs e)
        {
            //if stop button is pushed, uncheck continuous 
            chkContinuousPlay.IsChecked = false;
            wasapiOut.Stop();
        }

        //Method for when the pause button is clicked
        private void OnPauseClicked(object sender, RoutedEventArgs e)
        {
            wasapiOut.Pause();
            EnableControls();
        }

        //Method for back button
        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            // call stop and then call play.
            chkContinuousPlay.IsChecked = false;
            wasapiOut.Stop();
            PlaySelectedSong();
        }

        //Method for forward button
        private void OnForwardClick(object sender, RoutedEventArgs e)
        {
            //If list is not empty, stop and delete the playing song and start the next.
            if (!dgvPlayList.Items.IsEmpty)
            {
                wasapiOut.Stop();
            }
            else
            {
                //A new method needs to be called to grab a random song.
            }
        }

        //Method for position slider drag complete
        private void SldrPositionOnDragCompleted(object sender, EventArgs e)
        {
            //If the buffer is not null allow the position to be moved
            if (wasapiOut != null)
            {
                reader.CurrentTime = TimeSpan.FromSeconds(sldrPosition.Value + 2.5);
            }
        }

        //Method for volume slider change
        private void OnSldrVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //for wasapiOut
            if (wasapiOut != null)
            {
                //simple way of controling volume - Glitchy
                //var device = (MMDevice)cboDevices.SelectedItem;
                //device.AudioEndpointVolume.MasterVolumeLevelScalar = (float)sldrVolume.Value;
                //wasapiOut.Volume = sldrVolume.Value

                reader.Volume = (float)sldrVolume.Value;
            }
        }



        ////////////////////////////
        //Playlist Control Methods//
        ////////////////////////////

        //Method for Adding songs to playList
        private void OnAddSongClick(object sender, RoutedEventArgs e)
        {

            /////////////////////////////////////////////////
            //Modify this method to accomidate Roberts change
            /////////////////////////////////////////////////

            //Create arrayLists for new files to add
            //These variables are string array lists to store song locations.
            ArrayList newFiles = new ArrayList();
            ArrayList newPaths = new ArrayList();

            //Open file dialog to select tracks and add them to the play list
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "MP3 File (*.mp3)|*.mp3;";
            open.DefaultExt = ".mp3";
            open.Multiselect = true;

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

        //method for starting to receive the requests
        private void OnRecieveRequestClick(object sender, RoutedEventArgs e)
        {
            
            var dueTime = TimeSpan.FromSeconds(10);
            var interval = TimeSpan.FromSeconds(10);
            CancellationToken stopReceiving;
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
            var movement = -1;
            if (wasapiOut != null)
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
            var movement = 2;
            //If its playing or paused you can not move the item in the play position of the list.
            if (wasapiOut != null)
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
            if (wasapiOut != null)
            {
                if (dgvPlayList.SelectedIndex > 0 && dgvPlayList.SelectedIndex != dgvPlayList.Items.Count)
                {
                    removeSongFromPlayList(index);
                    dgvPlayList.SelectedIndex = index;
                }
            }
            else if (dgvPlayList.SelectedIndex != -1 && dgvPlayList.SelectedIndex != dgvPlayList.Items.Count)
            {
                removeSongFromPlayList(index);
                dgvPlayList.SelectedIndex = index;
            }
        }


        /////////////////////////////////////////////
        //Methods for handling requests from the web
        /////////////////////////////////////////////
        
        //call the get request query on intervals
        private async void DoPeriodicRequestCall(TimeSpan dueTime, TimeSpan interval, CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            //test count
            int testCount = 0;
            while (!token.IsCancellationRequested)
            {
                // TODO: call for requests from database
                testCount++;
                testLabel.Text = "test count" + testCount.ToString();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }


        //query the WeListen API for a list of requests
        private async void GetRequests(object sender, RoutedEventArgs e)
        {
            try
            {
                btnGetProducts.IsEnabled = false;

                var response = await client.GetAsync("http://welistenmusic.com/api/locations/1");
                response.EnsureSuccessStatusCode(); // Throw on error code.
                if (response.IsSuccessStatusCode)
                {
                    //JObject requests = JObject.Parse(response.Content.ReadAsAsync<IEnumerable<SongData>>().ToString());

                   // var requests = response.Content.ReadAsAsync<IEnumerable<SongData>>().Result;
                    //_requests.CopyFrom(requests);

                    //WebClient webClient = new WebClient();
                    //dynamic result = JsonValue.Parse(webClient.DownloadString("https://api.foursquare.com/v2/users/self?oauth_token=XXXXXXX"));
                    //Console.WriteLine(result.response.user.firstName);

                    var json = new WebClient().DownloadString("http://welistenmusic.com/api/locations/1");
                    XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(json);

                  


                    //dynamic result = json.Parse
                    Console.WriteLine(doc);

                    //foreach (var r in _requests)
                    //{
                    //    TestRequestList.Items.Add(r.RequestTitle);
                    //}
                    TestRequestList.ItemsSource = _requests;
                }



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
                btnGetProducts.IsEnabled = true;
            }
        }


        ////////////////////////////////////////
        //methods for background audio control//
        ////////////////////////////////////////

        //method for adding song to playlist
        private void AddSongsToPlaylist(ArrayList newFiles, ArrayList newPaths)
        {
            foreach (string n in newPaths)
            {
                SongData newSong = getSongTags(n);
                populateDataGrid(newSong);

                if (dgvPlayList.Items.Count == 1)
                {
                    RetrieveSongInfo();
                }
            }
        }

        //Method for when stop is called
        void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            timer.Stop();
            sldrPosition.Value = 0;
            txtPosition.Text = "0:0";

            DisposeStream();

            EnableControls();
            if (chkContinuousPlay.IsChecked == true)
            {
                //If continuous is checked play next song.
                removeSongFromPlayList(0);
                PlaySelectedSong();
                dgvPlayList.SelectedIndex = 0;
            }

            if (e.Exception != null)
            {
                MessageBox.Show(e.Exception.Message);
            }
        }

        //Method for enabling and disabling controls
        private void EnableControls()
        {
            if (wasapiOut == null)
            {
                btnPlay.IsEnabled = true;
                btnPause.IsEnabled = false;
                btnStop.IsEnabled = false;
                sldrPosition.IsEnabled = false;
            }
            else
            {
                if (wasapiOut.PlaybackState == PlaybackState.Playing)
                {
                    btnPlay.IsEnabled = false;
                    btnPause.IsEnabled = true;
                    btnStop.IsEnabled = true;
                    sldrPosition.IsEnabled = true;
                }
                else if (wasapiOut.PlaybackState == PlaybackState.Paused)
                {
                    btnPlay.IsEnabled = true;
                    btnPause.IsEnabled = false;
                    btnStop.IsEnabled = true;
                    sldrPosition.IsEnabled = true;
                }
            }


            //note that volume is always enabled
        }

        //Method for each tick of the timer
        void OnTimerTick(object sender, EventArgs e)
        {
            if (wasapiOut != null)
            {
                TimeSpan currentTime = (wasapiOut.PlaybackState == PlaybackState.Stopped) ? TimeSpan.Zero : reader.CurrentTime;
                sldrPosition.Value = reader.CurrentTime.TotalSeconds;
                //sldrPosition.Value = Math.Min(sldrPosition.Maximum, (int)(100 * currentTime.TotalSeconds / reader.TotalTime.TotalSeconds));
                txtPosition.Text = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);
            }
            else
            {
                sldrPosition.Value = 0;
            }
        }

        //Method for populating Devices combo box
        private void PopulateCboDevices()
        {
            //for wasapiOut
            var deviceEnumerator = new MMDeviceEnumerator();
            var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            foreach (var device in devices)
            {
                cboDevices.Items.Add(device);
            }
            //Select default automatically
            cboDevices.SelectedIndex = 0;
        }

        // Clear the output - Remove playing song
        private void DisposeStream()
        {
            if (wasapiOut != null)
            {
                if (wasapiOut.PlaybackState == NAudio.Wave.PlaybackState.Playing) wasapiOut.Stop();
                wasapiOut.Dispose();
                wasapiOut = null;
            }
            if (wasapiOut != null)
            {
                reader.Dispose();
                reader = null;
            }
        }

        //Method to start playing the selected song
        private void PlaySelectedSong()
        {

            if (!dgvPlayList.Items.IsEmpty)//if (!lbxPlayList.Items.IsEmpty)
            {
                //Select the top song in the playlist data grid
                SongData playItem = (SongData)dgvPlayList.Items[0];
                RetrieveSongInfo();
                //populate string with the song in the first position of the path arrayList
                
                string path = playItem.Path;

                //For WasapiOut
                var device = (MMDevice)cboDevices.SelectedItem;
                AudioClientShareMode shareMode = AudioClientShareMode.Shared;
                int latency = 20;
                bool useEventSync = false;
                wasapiOut = new WasapiOut(device, shareMode, useEventSync, latency);
                device.AudioEndpointVolume.MasterVolumeLevelScalar = (float)sldrVolume.Value;

                wasapiOut.PlaybackStopped += OnPlaybackStopped;

                reader = new AudioFileReader(path);

                txtDurration.Text = String.Format("{0:00}:{1:00}", (int)reader.TotalTime.TotalMinutes, reader.TotalTime.Seconds);
                txtPosition.Text = reader.CurrentTime.ToString();
                sldrPosition.Maximum = reader.TotalTime.TotalSeconds;
                sldrPosition.Value = 0;
                timer.Start();

                reader.Volume = (float)sldrVolume.Value;

                //for wasapiOut
                wasapiOut.Init(reader);
                wasapiOut.Play();

                //enable controls
                EnableControls();
            }
            else
            {
                EnableControls();
                //Method to call random song needs to be called
            }
        }


        //Method to remove songs from playlist
        private void removeSongFromPlayList(int index)
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
                    removeSongFromPlayList(selectedIndex);

                    //Change the selected index to the new position
                    dgvPlayList.SelectedItem = itemToMove;
        }

        // Dispose if the window is closed
        private void Window_Closed(object sender, EventArgs e)
        {
            DisposeStream();
        }




        ////////////////////////////
        // ROBERT - MEDIA MANAGER //
        ////////////////////////////


        ///////////////////////////////////////////////////////
        // WORKING
        // Base URL Handler
        // - Set baseUrl for LastFM.API reference (XML page base URL)
        ///////////////////////////////////////////////////////
        private string GetBaseRequestUrl()
        {
            string baseUrl = "http://ws.audioscrobbler.com/2.0/?api_key=" + API_KEY;
            return baseUrl;
        }

        ///////////////////////////////////////////////////////
        // WORKING
        // Row Selection Handler
        // - Assigns text variables on row select in DataGrid
        ///////////////////////////////////////////////////////
        private void RetrieveSongInfo()
        {
            // Show loading label..
            tbAlbumArtInfo.Text = "Loading Album Art...";

            var Title = "Unknown";
            var Artist = "Unknown";
            var Album = "Unknown";
            var Year = "Unknown";
            var Genre = "Unknown";
            var Path = "Unknown";

            // Cast 'item' as songData object from selected row
            SongData item = (SongData)dgvPlayList.Items[0];

            Title = item.Title;
            Artist = item.Artist;
            Album = item.Album;
            Year = item.Year.ToString();
            Genre = item.Genre;
            Path = item.Path;

            // Assign Text values to labels for song selected in dataGridView1
            tbTitleInfo.Text = Title;
            tbArtistInfo.Text = Artist;
            tbAlbumInfo.Text = Album;
            tbYearInfo.Text = Year;
            tbGenreInfo.Text = Genre;
            tbFilePathInfo.Text = Path;


            // Call Asynchronous data gathering from LastFM API (XML Parser)
            aSyncFileData(Title, Artist);
        }

        ///////////////////////////////////////////////////////
        // WORKING
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
        // WORKING
        // Drag Exit handler for DataGrid
        // - Changes bg color back when exiting DataGrid
        ///////////////////////////////////////////////////////
        private void dgvPlayList_DragLeave(object sender, DragEventArgs e)
        {
            var grid = sender as DataGrid;
            grid.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
        }

        ///////////////////////////////////////////////////////
        // WORKING
        // File Drop handler for DataGrid
        // - Process through each file dropped into DataGrid
        ///////////////////////////////////////////////////////
        private void dgvPlayList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var filePath in files)
                {
                    SongData newSong = getSongTags(filePath);
                    populateDataGrid(newSong);
                }

                RetrieveSongInfo();
            }
        }

        ///////////////////////////////////////////////////////
        // WORKING
        // Tag Handler
        // - Grabs tags from file and returns as SongData ojbect
        ///////////////////////////////////////////////////////
        public SongData getSongTags(string path)
        {
            TagLib.File tagFile = TagLib.File.Create(path);

            uint trackNumber = tagFile.Tag.Track;
            string songTitle = tagFile.Tag.Title;
            string artist = tagFile.Tag.AlbumArtists.FirstOrDefault();
            string albumTitle = tagFile.Tag.Album;
            uint year = tagFile.Tag.Year;
            string genre = tagFile.Tag.Genres.FirstOrDefault();

            // Set Song title to file name, else UNKNOWN
            if (songTitle == null)
                try
                {
                    string editedPath = System.IO.Path.GetFileNameWithoutExtension(path);
                    songTitle = Regex.Replace(editedPath, @"^[\d-]*\s*", "");
                }
                catch
                {
                    songTitle = "Unknown";
                }

            // Set Album name to folder name holding file, else UNKNOWN
            if (albumTitle == null)
                try
                {
                    albumTitle = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path));
                }
                catch
                {

                }

            // Check for Artist in "Contributing Artists" Meta-Data
            if (artist == null || artist == "")
            {
                artist = tagFile.Tag.JoinedPerformers;
            }
            // If still null, set to UNKNOWN
            if (artist == null || artist == "")
                try
                {
                    artist = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(path)));
                }
                catch
                {
                    artist = "Unknown";
                }
            if (genre == null) { genre = "Unknown"; }

            // Year will default to 0 as a UINT if unknown

            // Assign data to new SongData object
            var newSongObject = new SongData { Title = songTitle, Artist = artist, Album = albumTitle, Year = year, Genre = genre, Path = path };
           
            return newSongObject;
        }

        ///////////////////////////////////////////////////////
        // WORKING
        // DataGrid Population Handler
        // - Adds SongData object to DataGrid
        ///////////////////////////////////////////////////////
        public void populateDataGrid(SongData songDataObject)
        {
            var isDup = false;

            // Check if there are duplicates detected in DataGrid by Path
            if (dgvPlayList.Items.Count != 0)
            {
                foreach (SongData row in dgvPlayList.Items)
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
                    this.dgvPlayList.Items.Add(songDataObject);
                }
            }
            else
            {
                this.dgvPlayList.Items.Add(songDataObject);
            }
        }

        ///////////////////////////////////////////////////////
        // WORKING
        // LastFM API XML Parser
        // - (Currently) Returns Album art as URL strings
        ///////////////////////////////////////////////////////
        public async Task<String> GetTrackInfo(string trackName, string artist)
        {
            string art = null;
            string tempArt = null;

            if (String.IsNullOrEmpty(trackName) || String.IsNullOrEmpty(artist))
            {
                throw new Exception("Could not get track info. Track and/or artist fields Empty/null.");
            }
            else
            {
                string requestUrl = GetBaseRequestUrl();
                requestUrl += "&method=track.getInfo&artist=" + System.Web.HttpUtility.UrlEncode(artist.Trim()) + "&track=" + System.Web.HttpUtility.UrlEncode(trackName.Trim());

                string serviceResponse = await GetServiceResponse(requestUrl);

                var xmlResponse = XElement.Parse(serviceResponse);

                // Parse through the returned Xml for the name and match value for each similar artist.
                var getInfo = from trackInfo in xmlResponse.Descendants("track")

                              select new
                              {
                                  trackName = (string)trackInfo.Elements("name").FirstOrDefault() ?? "Unknown",
                                  trackArtist = (string)trackInfo.Elements("album").Elements("artist").FirstOrDefault() ?? "Unknown",
                                  trackAlbum = (string)trackInfo.Elements("album").Elements("title").FirstOrDefault() ?? "Unknown",
                                  albumURL = (string)trackInfo.Elements("album").Elements("image").FirstOrDefault() ?? "Unknown"

                              };

                if (getInfo.Count() > 0)
                {
                    foreach (var trackInfo in getInfo)
                    {
                        try
                        {
                            tempArt = trackInfo.albumURL;
                            art = tempArt.Replace("/64s/", "/300x300/");
                            tbAlbumArtInfo.Text = "";
                        }
                        catch
                        {
                            tbAlbumArtInfo.Text = "Album Art Unavailable";
                            art = "http://icons.iconseeker.com/png/fullsize/3d-cartoon-icons-pack-iii/adobe-help-center.png";
                        }
                    }
                }

                return art;
            }
        }

        ///////////////////////////////////////////////////////
        // WORKING
        // Request XML URL Handler
        // - Pulls XML page data for parsing
        ///////////////////////////////////////////////////////
        private Task<string> GetServiceResponse(string requestUrl)
        {
            return Task.Run(() =>
            {
                string httpResponse = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                request.Timeout = 60000;
                HttpWebResponse response = null;
                StreamReader reader = null;

                try
                {
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        response = (HttpWebResponse)ex.Response;
                    }

                    reader = new StreamReader(response.GetResponseStream());
                    httpResponse = reader.ReadToEnd();
                }
                catch
                {
                    //MessageBox.Show("Script has stopped unexpectedly!");
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (response != null)
                    {
                        response.Close();
                    }
                }

                return httpResponse;
            });
        }

        ///////////////////////////////////////////////////////
        // WORKING
        // Album Art Handler
        // - Requests art URL and displays via. image area
        ///////////////////////////////////////////////////////
        private async void aSyncFileData(string trackName, string artist)
        {
            MainWindow lastFmRequest = new MainWindow();

            try
            {
                // Pull album art from determined Url
                string Url = await lastFmRequest.GetTrackInfo(trackName, artist);
                imgAlbumArt.Source = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri(Url));
            }
            catch
            {
                //MessageBox.Show("Error setting album image!");
            }
        }

        //////////////////////////////////////////////////////
        // WORKING
        // Browse Button Handler
        // - Requests browser explorer, sets folder path
        ///////////////////////////////////////////////////////
        private void OnBtnImport_Click(object sender, RoutedEventArgs e)
        {

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
                Thread.Sleep(1);
                var folder = dlg.FileName;
                Thread.Sleep(1);

                tbMusicDir.Text = folder;
                ProcessDirectory(folder, false);
            }
            else
            {
                MessageBox.Show("There was a problem.");
            }
        }

        //////////////////////////////////////////////////////
        // WORKING
        // Browse folder handler
        // - Imports .mp3 files and populates DataGrid
        ///////////////////////////////////////////////////////
        public void ProcessDirectory(string targetDirectory, Boolean export)
        {
            // Process the list of files found in the directory. (Only grabs .mp3's)
            string[] fileEntries = Directory.GetFiles(targetDirectory, "*.mp3");
            foreach (string fileName in fileEntries)
            {

                SongData newSong = getSongTags(fileName);

                if (export)
                {
                    RetrieveSongInfo();
                    // Add method call to upload to database
                }
                else
                {
                    populateDataGrid(newSong);

                    if (dgvPlayList.Items.Count == 1)
                    {
                        RetrieveSongInfo();
                    }
                }
            }

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                ProcessDirectory(subdirectory, false);
            }
        }

        ////////////////////////////////////////////////////
        //Methods for handling logging in and out of the Web
        ////////////////////////////////////////////////////

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
                btnGetProducts.IsEnabled = true;
            }
        }
    }
}

