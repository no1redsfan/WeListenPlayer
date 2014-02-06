using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Amazon.PAAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instantiate Amazon ProductAdvertisingAPI client
            AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();

            // prepare an ItemSearch request
            ItemSearchRequest request = new ItemSearchRequest();
            request.SearchIndex = "MP3Downloads";
            request.Keywords = "Chiodos caves";
            request.RelationshipType = new string[] { "Tracks" };
            request.ResponseGroup = new string[] { "ItemAttributes", "Offers", "RelatedItems" };

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

                Console.WriteLine("=======================================================");
                Console.WriteLine("ASIN: " + item.ASIN);
                Console.WriteLine("Artist: " + item.ItemAttributes.Creator[0].Value);
                Console.WriteLine("Album: " + item.RelatedItems[0].RelatedItem[0].Item.ItemAttributes.Title);
                Console.WriteLine("Song Title: " + item.ItemAttributes.Title);
                Console.WriteLine("Play Length: " + item.ItemAttributes.RunningTime.Value.ToString() + " seconds");
                Console.WriteLine("Price: " + item.Offers.Offer[0].OfferListing[0].Price.FormattedPrice);
                Console.WriteLine("");
                //Console.WriteLine(item.ItemAttributes)
                Console.WriteLine("URL TO ADD MP3 TO WISHLIST");
                Console.WriteLine(item.ItemLinks[3].URL);
                Console.WriteLine("=======================================================");
                Console.WriteLine("");

            }
            Console.WriteLine("done...enter any key to continue>");
            Console.ReadLine();

        }
    }
}
