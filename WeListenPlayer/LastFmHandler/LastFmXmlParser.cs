using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WeListenPlayer.APIClasses;
using WeListenPlayer.XmlHandler;

namespace WeListenPlayer.LastFmHandler
{
    class LastFmXmlParser
    {

        ///////////////////////////////////////////////////////
        // Request XML URL Handler
        // - Pulls XML page data for parsing
        //
        // - Uses       string Url = await new LastFmXmlParser().GetTrackInfo({string:trackName}, {string:artist});
        // - Output     Returns {string:art} (URL to album artwork for SongData object)
        ///////////////////////////////////////////////////////
        public async Task<SongData> GetTrackInfo()
        {
            // Declare new SongData Object
            SongData pulledSong = new SongData();

            // Define new MainWindow object (for reference)
            var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

            string baseURL = "http://welistenmusic.com/api/locations/"; // Base default url
            string location = "1"; // Playlist location (1 default)

            // Set request URL based on above variables (location will change in future)
            string requestUrl = baseURL + location;

            string serviceResponse = await new XmlAccesser().GetServiceResponse(requestUrl);

            var xmlResponse = XElement.Parse(serviceResponse);

            // Parse through the returned Xml for the name and match value for each similar artist.
            foreach (var PlayListSong in xmlResponse.Descendants("ArrayOfPlayListSong"))
            {
                var getInfo = from trackInfo in xmlResponse.Descendants("PlayListSong")

                              select new
                              {
                                  trackName = (string)trackInfo.Elements("Title").FirstOrDefault() ?? "Unknown",
                                  trackArtist = (string)trackInfo.Elements("Artist").FirstOrDefault() ?? "Unknown",
                                  //trackAlbum = (string)trackInfo.Elements("Album").FirstOrDefault() ?? "Unknown",
                                  trackPath = (string)trackInfo.Elements("FilePath").FirstOrDefault() ?? "Unknown"
                              };

                if (getInfo.Count() > 0)
                {
                    foreach (var trackInfo in getInfo)
                    {
                        try
                        {
                            pulledSong.Title = trackInfo.trackName;
                            pulledSong.Artist = trackInfo.trackArtist;
                            pulledSong.Path = trackInfo.trackPath;

                        }
                        catch
                        {
                            return pulledSong;
                        }
                    }
                }

            } return pulledSong;
        }
    }
}
