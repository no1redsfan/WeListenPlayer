using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WeListenPlayer1._1_WPF.XmlHandler;

namespace WeListenPlayer1._1_WPF.LastFmHandler
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
        public async Task<String> GetTrackInfo(string trackName, string artist)
        {

            // Define new MainWindow object (for reference)
            var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

            string art = null;
            string tempArt = null;

            if (String.IsNullOrEmpty(trackName) || String.IsNullOrEmpty(artist))
            {
                throw new Exception("Could not get track info. Track and/or artist fields Empty/null.");
            }
            else
            {
                string requestUrl = new LastFmDataAccesser().getBaseUrl();
                requestUrl += "&method=track.getInfo&artist=" + System.Web.HttpUtility.UrlEncode(artist.Trim()) + "&track=" + System.Web.HttpUtility.UrlEncode(trackName.Trim());

                string serviceResponse = await new XmlAccesser().GetServiceResponse(requestUrl);

                var xmlResponse = XElement.Parse(serviceResponse);

                // Parse through the returned Xml for the name and match value for each similar artist.
                var getInfo = from trackInfo in xmlResponse.Descendants("track")

                              select new
                              {
                                  trackName = (string)trackInfo.Elements("name").FirstOrDefault() ?? "Unknown",
                                  trackArtist = (string)trackInfo.Elements("album").Elements("artist").FirstOrDefault() ?? "Unknown",
                                  trackAlbum = (string)trackInfo.Elements("album").Elements("title").FirstOrDefault() ?? "Unknown",
                                  albumURL = (string)trackInfo.Elements("album").Elements("image").FirstOrDefault() ?? "Unknown"

                              };

                if (getInfo.Count() > 0)
                {
                    foreach (var trackInfo in getInfo)
                    {
                        try
                        {
                            tempArt = trackInfo.albumURL;
                            art = tempArt.Replace("/64s/", "/300x300/");

                            mainWindow.tbAlbumArtInfo.Text = "";
                        }
                        catch
                        {
                            mainWindow.tbAlbumArtInfo.Text = "Album Art Unavailable";
                            art = "http://icons.iconseeker.com/png/fullsize/3d-cartoon-icons-pack-iii/adobe-help-center.png";
                        }
                    }
                }

                return art;
            }
        }
    }
}
