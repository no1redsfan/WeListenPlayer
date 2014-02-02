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
        public LastFmData getLastFmCreds()
        {
            LastFmData creds = new LastFmData();

            creds.Key = "69473ce24376ae029d0b35211016700c";
            creds.Secret = "305fef5dd50570f5da565ffb863a65ef";

            return creds;
        }
    }
}
