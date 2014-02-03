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
    }
}
