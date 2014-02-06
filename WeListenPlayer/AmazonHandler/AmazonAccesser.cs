using Amazon.PAAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace WeListenPlayer.AmazonHandler
{
    class AmazonAccesser
    {
        public void InitiateSearchRequest(string keyword)
        {
            // Define new MainWindow object (for reference)
            var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

            // Instantiate Amazon ProductAdvertisingAPI client
            AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();

            // prepare an ItemSearch request
            ItemSearchRequest request = new ItemSearchRequest();
            request.SearchIndex = "MP3Downloads";
            request.RelationshipType = new string[] { "Tracks" };
            request.ResponseGroup = new string[] { "ItemAttributes", "Offers", "RelatedItems" };

            // keyword = search variables
            request.Keywords = keyword;

            ItemSearch itemSearch = new ItemSearch();
            itemSearch.Request = new ItemSearchRequest[] { request };
            itemSearch.AWSAccessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
            itemSearch.AssociateTag = "1330-3170-0573";

            // send the ItemSearch request
            ItemSearchResponse response = amazonClient.ItemSearch(itemSearch);

            // write out the results from the ItemSearch request
            foreach (var item in response.Items[0].Item)
            {

                //// prepare an ItemLookup request
                //ItemLookupRequest lookupRequest = new ItemLookupRequest();
                //lookupRequest.IdType = ItemLookupRequestIdType.ASIN;
                //lookupRequest.ItemId = new string[] { item.ASIN };

                //lookupRequest.SearchIndex = "MusicTracks";
                //lookupRequest.ResponseGroup = new string[] { "Small,RelatedItems" };
                //lookupRequest.RelationshipType = new string[] { "Episode" };

                //ItemLookup itemLookup = new ItemLookup();
                //itemLookup.Request = new ItemLookupRequest[] { lookupRequest };
                //itemLookup.AWSAccessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
                //itemLookup.AssociateTag = "1330-3170-0573";

                //ItemLookupResponse lookupResponse = amazonClient.ItemLookup(itemLookup);

                //var lookupItem = lookupResponse.Items[0].Item;

                string strMsg = "Keywords          : " + keyword + "\r\n" +
                                "ASIN              : " + item.ASIN + "\r\n" +
                                "Artist            : " + item.ItemAttributes.Creator[0].Value + "\r\n" +
                                "Album             : " + item.RelatedItems[0].RelatedItem[0].Item.ItemAttributes.Title + "\r\n" +
                                "Song Title        : " + item.ItemAttributes.Title + "\r\n" +
                                "Play Length       : " + item.ItemAttributes.RunningTime.Value.ToString() + " seconds" + "\r\n" +
                                "Price             : " + item.Offers.Offer[0].OfferListing[0].Price.FormattedPrice;

                MessageBox.Show(strMsg);

            }
        }
    }
}
