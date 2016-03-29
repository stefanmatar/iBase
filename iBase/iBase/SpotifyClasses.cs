using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBase
{
    public class Album
    {
        public string id { get; set; }
        public string name { get; set; }
        public string href { get; set; }
        public string imageurl { get; set; }
        public Dictionary<string, string> artists { get; set; }
        public Dictionary<string, string> tracks { get; set; }
    }
    public class Artist
    {
        public long followers_total { get; set; }
        public Dictionary<string, string> albums { get; set; }
        public List<string> genres { get; set; }
        public string imageurl { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public long popularity { get; set; }
        public string type { get; set; }
    }
    public class Track
    {
        public string album { get; set; }
        public Dictionary<string, string> artists { get; set; }
        public long disc_number { get; set; }
        public long duration_ms { get; set; }
        public bool explicity { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public long popularity { get; set; }
        public string imageurl { get; set; }
        public string preview_url { get; set; }
        public long track_number { get; set; }
        public string type { get; set; }
    }
}
