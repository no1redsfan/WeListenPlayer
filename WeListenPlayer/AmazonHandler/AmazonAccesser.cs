using Amazon.PAAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace WeListenPlayer.AmazonHandler
{
    class AmazonAccesser
    {
        public void InitiateSearchRequest(string artist, string album, string title)
        {
            // Define new MainWindow object (for reference)
            var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

            // Handle passed parameters
            string itemString = "";

            if (artist != "UNKNOWN")
            {
                itemString += artist + " ";
            }

            if (album != "UNKNOWN")
            {
                itemString += album + " ";
            }

            if (title != "UNKNOWN")
            {
                itemString += title;
            }

            // Instantiate Amazon ProductAdvertisingAPI client
            AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();

            // prepare an ItemSearch request
            ItemSearchRequest request = new ItemSearchRequest();
            request.SearchIndex = "MP3Downloads";
            request.RelationshipType = new string[] { "Tracks" };
            request.ResponseGroup = new string[] { "ItemAttributes", "Images", "Offers", "RelatedItems" };

            // keyword = search variables
            request.Keywords = itemString;

            ItemSearch itemSearch = new ItemSearch();
            itemSearch.Request = new ItemSearchRequest[] { request };
            itemSearch.AWSAccessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
            itemSearch.AssociateTag = "1330-3170-0573";

            // send the ItemSearch request
            ItemSearchResponse response = amazonClient.ItemSearch(itemSearch);

            // Get first (most related) item from search results
            var item = response.Items[0].Item[0];

            // Declare new songData object
            // OBJECT HERE (TEST)

            // Get album artwork
            string imageURL = item.MediumImage.URL;
            mainWindow.imgAlbumArt.Source = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri(imageURL));


            //// write out the results from the ItemSearch request
            //foreach (var item in response.Items[0].Item)
            {

            //    //// prepare an ItemLookup request
            //    //ItemLookupRequest lookupRequest = new ItemLookupRequest();
            //    //lookupRequest.IdType = ItemLookupRequestIdType.ASIN;
            //    //lookupRequest.ItemId = new string[] { item.ASIN };

            //    //lookupRequest.SearchIndex = "MusicTracks";
            //    //lookupRequest.ResponseGroup = new string[] { "Small,RelatedItems" };
            //    //lookupRequest.RelationshipType = new string[] { "Episode" };

            //    //ItemLookup itemLookup = new ItemLookup();
            //    //itemLookup.Request = new ItemLookupRequest[] { lookupRequest };
            //    //itemLookup.AWSAccessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
            //    //itemLookup.AssociateTag = "1330-3170-0573";

            //    //ItemLookupResponse lookupResponse = amazonClient.ItemLookup(itemLookup);

            //    string strMsg = "Keywords          : " + keyword + "\r\n" +
            //                    "ASIN              : " + item.ASIN + "\r\n" +
            //                    "Artist            : " + item.ItemAttributes.Creator[0].Value + "\r\n" +
            //                    "Album             : " + item.RelatedItems[0].RelatedItem[0].Item.ItemAttributes.Title + "\r\n" +
            //                    "Song Title        : " + item.ItemAttributes.Title + "\r\n" +
            //                    "Play Length       : " + item.ItemAttributes.RunningTime.Value.ToString() + " seconds" + "\r\n" +
            //                    "Price             : " + item.Offers.Offer[0].OfferListing[0].Price.FormattedPrice + "\r\n" + 
            //                    "Album Art         : " + item.MediumImage.URL;

            //    MessageBox.Show(strMsg);

            }
        }
    }
}
