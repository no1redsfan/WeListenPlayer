using System;
using System.Configuration;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeListenPlayer.Amazon.PAAPI;

namespace WeListenPlayer
{
    class AmazonAccesser
    {
        // Amazon Keys
        private const string accessKeyId = "AKIAIEUPYITONT562FSA";
        private const string secretKey = "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";

        public async Task<SongData> getAmazonInfo(SongData song)
        {
            var fullRequest = parseInfo(song);
            var newSong = await getData(song, fullRequest);

            return newSong;
        }

        public string parseInfo(SongData song)
        {
            var fullRequest = "";
            var artist = song.Artist;
            var album = song.Album;
            var title = song.Title;

            // Remove anything in parenthesis/brackets and all special characters
            string regEx = @"(?<=\()(.*?)(?=\))|(?<=\[)(.*?)(?=\])|(?<=\{)(.*?)(?=\})|[^\w ]";

            // Handle passed parameters
            if (artist != "UNKNOWN")
            {
                artist = Regex.Replace(artist, regEx, "");
                fullRequest += artist;
            }

            if (album != "UNKNOWN" && album != artist)
            {
                album = Regex.Replace(album, regEx, "");
                fullRequest += " " + album;
            }

            if (title != "UNKNOWN" && title != album)
            {
                title = Regex.Replace(title, regEx, "");
                fullRequest += " " + title;
            }

            return fullRequest;
        }

        public async Task<SongData> getData(SongData newSong, string fullRequest)
        {
            try
            {
                // Instantiate Amazon ProductAdvertisingAPI client
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                binding.MaxReceivedMessageSize = int.MaxValue;
                AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient(
                binding,
                new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

                // add authentication to the ECS client
                amazonClient.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));

                // prepare an ItemSearch request
                ItemSearchRequest request = new ItemSearchRequest();
                request.SearchIndex = "MP3Downloads";
                request.RelationshipType = new string[] { "Tracks" };
                request.ResponseGroup = new string[] { "ItemAttributes", "Images", "Offers", "RelatedItems" };

                request.Keywords = fullRequest;

                ItemSearch itemSearch = new ItemSearch();
                itemSearch.Request = new ItemSearchRequest[] { request };
                itemSearch.AWSAccessKeyId = accessKeyId;
                itemSearch.AssociateTag = "1330-3170-0573";

                // send the ItemSearch request
                ItemSearchResponse response = amazonClient.ItemSearch(itemSearch);

                var item = response.Items[0].Item[0];

                //<ProductTypeName>DOWNLOADABLE_MUSIC_TRACK</ProductTypeName>
                if (response.Items[0].Item[0].ItemAttributes.ProductTypeName == "DOWNLOADABLE_MUSIC_ALBUM")
                {
                    item = response.Items[0].Item[1];
                }

                // if no response to search
                if (item == null)
                {
                    try
                    {
                        // Try new search and remove the album
                        newSong.Album = "UNKNOWN";

                        // Re-iterate over the search method
                        await getData(newSong, fullRequest);
                    }
                    catch
                    {
                        // Removing the album produced no results
                        // Continue forward...
                    }
                }

                // Get year from full Release Date var
                var formatYear = DateTime.Parse(item.ItemAttributes.ReleaseDate).Year;

                newSong.UserID = 1;
                newSong.LocationID = 1;
                newSong.Album = item.RelatedItems[0].RelatedItem[0].Item.ItemAttributes.Title;
                newSong.Artist = item.ItemAttributes.Creator[0].Value;
                newSong.Title = item.ItemAttributes.Title;
                newSong.Year = (int)formatYear;
                newSong.Genre = item.ItemAttributes.Genre;
                newSong.FilePath = "";
                newSong.Duration = (int)item.ItemAttributes.RunningTime.Value;
                newSong.Price = item.Offers.Offer[0].OfferListing[0].Price.FormattedPrice;
                newSong.ASIN = item.ASIN;
                newSong.Artwork = item.LargeImage.URL;

                return newSong;
            }
            catch
            {
                return newSong;
            }
        }
    }
}