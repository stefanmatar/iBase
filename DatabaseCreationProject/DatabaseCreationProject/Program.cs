using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace DatabaseCreationProject
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new SpotifyContext())
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("https://ebanking.bankofmaldives.com.mv/xe/").Result;


                if (response.IsSuccessStatusCode)
                {

                    string JSON = response.Content.ReadAsStringAsync().Result;
                    //Deserialize to strongly typed class i.e., RootObject
                    RootObject obj = JsonConvert.DeserializeObject<RootObject>(JSON);

                    //loop through the list and insert into database
                    foreach (Albums_Album currencyItem in obj.albums)
                    {
                        Console.WriteLine(currencyItem.Key + "-" + currencyItem.Value.Code + "-" + currencyItem.Value.Description +
                               "-" + currencyItem.Value.Buy + "-" + currencyItem.Value.Sell);

                    }

                }
            }
        }
    }
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
        public virtual List<object> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public virtual List<Image_Artist> images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Artists_Artist
    {
        public string href { get; set; }
        public virtual List<Item_Artist> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }

    public class RootObject_Artist
    {
        public virtual Artists_Artist artists { get; set; }
    }
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
        public virtual List<string> available_markets { get; set; }
        public virtual ExternalUrls_Track external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public virtual List<Image_Track> images { get; set; }
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
        public virtual ExternalUrls2_Track external_urls { get; set; }
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
        public virtual Album_Track album { get; set; }
        public virtual List<Artist_Track> artists { get; set; }
        public virtual List<string> available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool @explicit { get; set; }
        public virtual ExternalIds_Track external_ids { get; set; }
        public virtual ExternalUrls3_Track external_urls { get; set; }
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
        public virtual List<Item_Track> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }

    public class RootObject_Track
    {
        public virtual Tracks_Track tracks { get; set; }
    }
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
        public virtual List<string> available_markets { get; set; }
        public virtual ExternalUrls_Album external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public virtual List<Image_Album> images { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Albums_Album
    {
        public string href { get; set; }
        public virtual List<Item_Album> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }

    public class RootObject
    {
        public virtual Albums_Album albums { get; set; }
    }
    public class SpotifyContext : DbContext
    {
        public DbSet<ExternalUrls_Artist> ExternalUrls_Artist { get; set; }
        public DbSet<Followers_Artist> Followers_Artist { get; set; }
        public DbSet<Image_Artist> Image_Artist { get; set; }
        public DbSet<Item_Artist> Item_Artist { get; set; }
        public DbSet<Artists_Artist> Artists_Artist { get; set; }
        public DbSet<RootObject_Artist> RootObject_Artist { get; set; }
        public DbSet<ExternalUrls_Track> ExternalUrls_Track { get; set; }
        public DbSet<Image_Track> Image_Track { get; set; }
        public DbSet<Album_Track> Album_Track { get; set; }
        public DbSet<ExternalUrls2_Track> ExternalUrls2_Track { get; set; }
        public DbSet<Artist_Track> Artist_Track { get; set; }
        public DbSet<ExternalIds_Track> ExternalIds_Track { get; set; }
        public DbSet<ExternalUrls3_Track> ExternalUrls3_Track { get; set; }
        public DbSet<Item_Track> Item_Track { get; set; }
        public DbSet<Tracks_Track> Tracks_Track { get; set; }
        public DbSet<RootObject_Track> RootObject_Track { get; set; }
        public DbSet<ExternalUrls_Album> ExternalUrls_Album { get; set; }
        public DbSet<Image_Album> Image_Album { get; set; }
        public DbSet<Item_Album> Item_Album { get; set; }
        public DbSet<Albums_Album> Albums_Album { get; set; }
        public DbSet<RootObject> RootObject { get; set; }
    }
}
