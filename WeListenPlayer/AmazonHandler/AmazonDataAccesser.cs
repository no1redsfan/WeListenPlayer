using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeListenPlayer.AmazonHandler
{
    class AmazonDataAccesser : ObservableCollection<AmazonData>
    {

        ///////////////////////////////////////////////////////
        // Amazon API Credentials
        // - Assigns Credential values Key/Secret to LastFmData Object
        //
        // - Uses       AmazonData AmazonApi = new AmazonDataAccesser().getAmazonCreds();
        // - Output     AmazonApi.Key = xx | AmazonApi.Secret = xx
        ///////////////////////////////////////////////////////
        public AmazonData getAmazonCreds()
        {
            AmazonData creds = new AmazonData();

            creds.Key = AmazonUserCredentials.Default.Key; // Different for other users
            creds.Secret = AmazonUserCredentials.Default.Secret; // Different for other users

            return creds;
        }
    }
}
