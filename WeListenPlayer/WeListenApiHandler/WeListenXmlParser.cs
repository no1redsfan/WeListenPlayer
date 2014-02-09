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
        // - Pulls XML page data for parsing
        //
        // - Uses       string Url = await new LastFmXmlParser().GetTrackInfo({string:trackName}, {string:artist});
        // - Output     Returns {string:art} (URL to album artwork for SongData object)
        ///////////////////////////////////////////////////////
        public async Task<List<SongData>> GetTrackInfo()
        {
            // Declare new SongData Object
            SongData pulledSong = new SongData();

            // Define new MainWindow object (for reference)
            var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

            string baseURL = "http://welistenmusic.com/api/locations/"; // Base default url
            string location = "1"; // Playlist location (1 default)

            // Set request URL based on above variables (location will change in future)
            string requestUrl = baseURL + location;

            //WebRequest webRequest = WebRequest.Create(requestUrl);

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
