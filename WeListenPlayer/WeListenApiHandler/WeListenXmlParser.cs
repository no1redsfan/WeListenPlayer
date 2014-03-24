using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace WeListenPlayer
{
    class WeListenXmlParser
    {
        private AmazonAccesser amazonAccesser = new AmazonAccesser();
        private DuplicateCheck dupCheck = new DuplicateCheck();

        ///////////////////////////////////////////////////////
        // Request XML URL Handler
        // - Pulls XML page data and parses, returns objects
        //
        // - Uses       WeListenXmlParser k = new WeListenXmlParser();
        //              k.GetTrackInfo();
        // - Output     Returns object list of all songs pulled from API
        ///////////////////////////////////////////////////////
        public List<SongData> GetTrackInfo(List<SongData> playlistSongs, bool randomSong)
        {
            // Create master / finalized list
            var pullList = new List<SongData>();

            string baseURL;
            if (randomSong)
            {
                baseURL = "http://welistenmusic.com/api/randomsong/"; // call for a random song
                
            }
            else
            {
                baseURL = "http://welistenmusic.com/api/locations/"; // Base default url
            }

            string location = "3"; // Playlist location (1 default)
            string requestUrl = baseURL + location;

            //string weListenApiKey = "fPOIWBN465IOA4567VUHEPOF8G6I5banspoighao";

            //string sendSongUrl = requestUrl + "&" + weListenApiKey;

            var serviceResponse = new XmlAccesser().GetServiceResponse(requestUrl, "text/xml");

            if (serviceResponse != null)
            {
                var xDoc = XDocument.Parse(serviceResponse.Result);

                XNamespace ns = xDoc.Root.Name.Namespace;

                var playlist = (from list in xDoc.Descendants(ns + "PlayListSong")
                                select new SongData
                                {
                                    ASIN = (string)list.Element(ns + "ASIN"),
                                    Artwork = (string)list.Element(ns + "Artwork"),
                                    Duration = (int)list.Element(ns + "Duration"),
                                    Price = (string)list.Element(ns + "Price"),
                                    Title = (string)list.Element(ns + "Title"),
                                    Artist = (string)list.Element(ns + "Artist"),
                                    Album = (string)list.Element(ns + "Album"),
                                    Year = (int)list.Element(ns + "Year"),
                                    Genre = (string)list.Element(ns + "Genre"),
                                    FilePath = (string)list.Element(ns + "FilePath").Value.Replace("\\\\", "\\"),
                                    PlaylistId = (int)list.Element(ns + "LocationPlayistId"),
                                    Queued = true
                                }).ToList();
                if (playlist != null)
                {
                    foreach (SongData song in playlist)
                    {
                        // Check for duplicate values
                        bool isDup = dupCheck.checkDup(playlistSongs, song);

                        if (!isDup)
                        {
                            // Add AmazonSong to playlist
                            pullList.Add(song);

                            // Add queued parameter
                            RequestQueued que = new RequestQueued();
                            que.LocationId = song.PlaylistId;
                            que.queued = song.Queued;

                            var json = JsonConvert.SerializeObject(que);

                            //string sendToJimmy = sendSongUrl + "&" + que.LocationId + "&" + que.queued;

                            // Initialize new http request, pass sendToJimmy as URL
                        }
                    }
                    return pullList;
                }
                return null;
            }
            return null;
        }
    }
}
