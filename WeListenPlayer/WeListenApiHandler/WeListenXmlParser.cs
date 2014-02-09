using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WeListenPlayer.APIClasses;
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

            // Set request URL based on above variables (location will change in future)
            string requestUrl = baseURL + location;

            string serviceResponse = await new XmlAccesser().GetServiceResponse(requestUrl);

            var xDoc = XDocument.Parse(serviceResponse);

            XNamespace ns = xDoc.Root.Name.Namespace;

            var playlist = (from list in xDoc.Descendants(ns + "PlayListSong")
                        select new SongData
                        {
                            Title = (string)list.Element(ns + "Title"),
                            Artist = (string)list.Element(ns + "Artist"),
                            Album = (string)list.Element(ns + "Album"),
                            Path = (string)list.Element(ns + "FilePath").Value.Replace("\\\\", "\\")
                        }).ToList();

            System.Console.WriteLine("Total songs in request que: " + playlist.Count);

            return playlist;

        }
    }
}
