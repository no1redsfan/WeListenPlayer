using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeListenPlayer.APIClasses
{
    public class SongData
    {
        public string Title { set; get; }
        public string Artist { set; get; }
        public string Album { set; get; }
        public uint Year { set; get; }
        public string Genre { set; get; }
        public string Path { set; get; }
        public int PlaylistId { set; get; }
        public bool Played { set; get; }
        public bool Queued { set; get; }
        public string amazonASIN { set; get; }
        public string amazonPrice { set; get; }
        public string amazonLength { set; get; }
        public string amazonArt { set; get; }
        
    }
}
