using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBase
{
    public class ExternalUrls_Artist
    {
        public string spotify { get; set; }
    }

    public class Followers_Artist
    {
        public object href { get; set; }
        public int total { get; set; }
    }

    public class Image_Artist
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }

    public class Item_Artist
    {
        public ExternalUrls_Artist external_urls { get; set; }
        public Followers_Artist followers { get; set; }
        public List<object> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image_Artist> images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Artists_Artist
    {
        public string href { get; set; }
        public List<Item_Artist> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }

    public class RootObject_Artist
    {
        public Artists_Artist artists { get; set; }
    }
}
