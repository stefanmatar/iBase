using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBase
{
    public class ExternalUrls_Track
    {
        public string spotify { get; set; }
    }

    public class Image_Track
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }

    public class Album_Track
    {
        public string album_type { get; set; }
        public List<string> available_markets { get; set; }
        public ExternalUrls_Track external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image_Track> images { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class ExternalUrls2_Track
    {
        public string spotify { get; set; }
    }

    public class Artist_Track
    {
        public ExternalUrls2_Track external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class ExternalIds_Track
    {
        public string isrc { get; set; }
    }

    public class ExternalUrls3_Track
    {
        public string spotify { get; set; }
    }

    public class Item_Track
    {
        public Album_Track album { get; set; }
        public List<Artist_Track> artists { get; set; }
        public List<string> available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool @explicit { get; set; }
        public ExternalIds_Track external_ids { get; set; }
        public ExternalUrls3_Track external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string preview_url { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Tracks_Track
    {
        public string href { get; set; }
        public List<Item_Track> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }

    public class RootObject_Track
    {
        public Tracks_Track tracks { get; set; }
    }

}
