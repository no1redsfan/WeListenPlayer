using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeListenPlayer1._1_WPF.LastFmHandler
{
    class LastFmDataAccesser : ObservableCollection<LastFmData>
    {

        ///////////////////////////////////////////////////////
        // LastFM API Credentials
        // - Assigns Credential values Key/Secret to LastFmData Object
        //
        // - Uses       LastFmData LastFmApi = new LastFmDataAccesser().getLastFmCreds();
        // - Output     LastFmApi.Key = xx | LastFmApi.Secret = xx
        ///////////////////////////////////////////////////////
        public LastFmData getLastFmCreds()
        {
            LastFmData creds = new LastFmData();

            creds.Key = LastFmUserCredentials.Default.Key; // Different for other users
            creds.Secret = LastFmUserCredentials.Default.Secret; // Different for other users

            return creds;
        }

        ///////////////////////////////////////////////////////
        // LastFM API Base URL
        // - Gets base url with API Key for XML Parsing (without variables)
        //
        // - Uses       string i = new LastFmDataAccesser().getBaseUrl();
        // - Output     string baseUrl
        ///////////////////////////////////////////////////////
        public string getBaseUrl()
        {
            LastFmData LastFmApi = new LastFmDataAccesser().getLastFmCreds();
            string baseUrl = "http://ws.audioscrobbler.com/2.0/?api_key=" + LastFmApi.Key;
            return baseUrl;
        }
    }
}
