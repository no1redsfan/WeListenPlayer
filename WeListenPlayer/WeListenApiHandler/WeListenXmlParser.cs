using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using Newtonsoft.Json;
using WeListenPlayer.APIClasses;
using WeListenPlayer.FormHandler;
using WeListenPlayer.WeListenApiHandler;

namespace WeListenPlayer.LastFmHandler
{
    class WeListenXmlParser
    {

        ///////////////////////////////////////////////////////
        // Request XML URL Handler
        // - Pulls XML page data and parses, returns objects
        //
        // - Uses       WeListenXmlParser k = new WeListenXmlParser();
        //              k.GetTrackInfo();
        // - Output     Returns object list of all songs pulled from API
        ///////////////////////////////////////////////////////
        public async Task<List<SongData>> GetTrackInfo()
        {

            string baseURL = "http://welistenmusic.com/api/locations/"; // Base default url
            string location = "1"; // Playlist location (1 default)
            string requestUrl = baseURL + location;
            string weListenApiKey = "fPOIWBN465IOA4567VUHEPOF8G6I5banspoighao";

            string sendSongUrl = requestUrl + "&" + weListenApiKey;

            string serviceResponse = await new XmlAccesser().GetServiceResponse(requestUrl);

            var xDoc = XDocument.Parse(serviceResponse);

            XNamespace ns = xDoc.Root.Name.Namespace;
            
            var playlist = (from list in xDoc.Descendants(ns + "PlayListSong")
                        select new SongData
                        {
                            Title = (string)list.Element(ns + "Title"),
                            Artist = (string)list.Element(ns + "Artist"),
                            Album = (string)list.Element(ns + "Album"),
                            Path = (string)list.Element(ns + "FilePath").Value.Replace("\\\\", "\\"),
                            PlaylistId = (int)list.Element(ns + "LocationPlayistId"),
                            Queued  = true
                            
                        }).ToList();

            System.Console.WriteLine("Total songs in request que: " + playlist.Count);
            foreach (var s in playlist)
            {
                // Populate datagrid with each songData s in playlist
                DataGridHandler j = new DataGridHandler();
                j.populateDataGrid(s);

                RequestQueued que = new RequestQueued();
                que.LocationId = s.PlaylistId;
                que.queued = s.Queued;

                var json = JsonConvert.SerializeObject(que);

                string sendToJimmy = sendSongUrl + "&" + que.LocationId + "&" + que.queued;

                // Initialize new http request, pass sendToJimmy as URL

            }

            return playlist;
        }
    }
}
