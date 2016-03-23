using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBase
{
    public class ExternalUrls_Album
    {
        public string spotify { get; set; }
    }

    public class Image_Album
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }

    public class Item_Album
    {
        public string album_type { get; set; }
        public List<string> available_markets { get; set; }
        public ExternalUrls_Album external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image_Album> images { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Albums_Album
    {
        public string href { get; set; }
        public List<Item_Album> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }

    public class RootObject
    {
        public Albums_Album albums { get; set; }
    }

}
