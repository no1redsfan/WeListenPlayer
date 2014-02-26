using Amazon.PAAPI;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace WeListenPlayer.AmazonHandler
{
    class AmazonAccesser
    {
        public string artist {get; set;}
        public string album { get; set; }
        public string title { get; set; }
        public string fullRequest { get; set; }

        MainWindow newMain; // Global Declaration

        // Capture MainWindow Instance and assign to Global Declaration
        public void setMain(MainWindow mainWindow)
        {
            newMain = mainWindow;
        }

        // Collect variables and pass to a New Thread
        public void getAmazonItems(string tempArtist, string tempAlbum, string tempTitle, string fullRequest)
        {
            this.artist = tempArtist;
            this.album = tempAlbum;
            this.title = tempTitle;

            // Remove anything in parenthesis/brackets and all special characters
            string regEx = @"(?<=\()(.*?)(?=\))|(?<=\[)(.*?)(?=\])|(?<=\{)(.*?)(?=\})|[^\w ]";

            // Handle passed parameters
            if (artist != "UNKNOWN")
            {
                artist = Regex.Replace(artist, regEx, "");
                fullRequest += artist;
            }

            if (this.album != "UNKNOWN" && this.album != this.artist)
            {
                album = Regex.Replace(album, regEx, "");
                fullRequest += " " + album;
            }

            if (this.title != "UNKNOWN" && this.title != this.album)
            {
                title = Regex.Replace(title, regEx, "");
                fullRequest += " " + title;
            }

            this.fullRequest = fullRequest;

            Thread thread = new Thread(new ThreadStart(getData));
            thread.IsBackground = true;
            thread.Start();
        }

        // NEW THREAD, GET ALBUM ART
        public void getData()
        {
            try
            {
                // Instantiate Amazon ProductAdvertisingAPI client
                AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();

                // prepare an ItemSearch request
                ItemSearchRequest request = new ItemSearchRequest();
                request.SearchIndex = "MP3Downloads";
                request.RelationshipType = new string[] { "Tracks" };
                request.ResponseGroup = new string[] { "ItemAttributes", "Images", "Offers", "RelatedItems" };

                request.Keywords = this.fullRequest;

                ItemSearch itemSearch = new ItemSearch();
                itemSearch.Request = new ItemSearchRequest[] { request };
                itemSearch.AWSAccessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
                itemSearch.AssociateTag = "1330-3170-0573";

                // send the ItemSearch request
                ItemSearchResponse response = amazonClient.ItemSearch(itemSearch);

                var item = response.Items[0].Item[0];

                //<ProductTypeName>DOWNLOADABLE_MUSIC_TRACK</ProductTypeName>
                if (response.Items[0].Item[0].ItemAttributes.ProductTypeName == "DOWNLOADABLE_MUSIC_ALBUM")
                {
                    item = response.Items[1].Item[1];
                }

                // if no response to search
                if (item == null)
                {
                    try
                    {
                        // Try new search and remove the album
                        this.album = "UNKNOWN";
                        
                        // Re-iterate over the search method
                        getData();
                    }
                    catch
                    {
                        // Removing the album produced no results
                        // Continue forward...
                    }
                }

                // Declare new songData object
                // OBJECT HERE (TEST)

                // Get album artwork
                string imageURL = item.LargeImage.URL;

                // Assign tb values
                try
                {
                    newMain.Dispatcher.BeginInvoke((MethodInvoker)(() => newMain.tbAmazonArtistInfo.Text = item.ItemAttributes.Creator[0].Value));
                    newMain.Dispatcher.BeginInvoke((MethodInvoker)(() => newMain.tbAmazonAlbumInfo.Text = item.RelatedItems[0].RelatedItem[0].Item.ItemAttributes.Title));
                    newMain.Dispatcher.BeginInvoke((MethodInvoker)(() => newMain.tbAmazonTitleInfo.Text = item.ItemAttributes.Title));
                    newMain.Dispatcher.BeginInvoke((MethodInvoker)(() => newMain.tbAmazonYearInfo.Text = item.ItemAttributes.ReleaseDate));
                    newMain.Dispatcher.BeginInvoke((MethodInvoker)(() => newMain.tbAmazonAsinInfo.Text = item.ASIN));
                    newMain.Dispatcher.BeginInvoke((MethodInvoker)(() => newMain.tbAmazonPriceInfo.Text = item.Offers.Offer[0].OfferListing[0].Price.FormattedPrice));
                }
                catch
                {
                    // Could not set label values
                }

                // Declare MainWindow to dispatch arguements, Invoke imgAlbumArt source change
                //newMain.Dispatcher.BeginInvoke((MethodInvoker)(() => newMain.imgAlbumArt.Source = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri(imageURL))));
            }
            catch
            {
                //Assign default album art
                string art = "http://icons.iconseeker.com/png/fullsize/3d-cartoon-icons-pack-iii/adobe-help-center.png";
                //newMain.Dispatcher.BeginInvoke((MethodInvoker)(() => newMain.imgAlbumArt.Source = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri(art))));
            }
        }
    }
}
