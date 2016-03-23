using System;
using System.Collections.Generic;
using System.Net;
using SpotifyAPI.Web; //Base Namespace
using System.IO;
using Newtonsoft.Json;

namespace iBase
{
    class SpotifyAPI
    {
        private static SpotifyWebAPI _spotify;

        public SpotifyAPI()
        {
            _spotify = new SpotifyWebAPI()
            {
                UseAuth = false
            };
        }

        //C# Objects created with http://json2csharp.com/ and parsed with JSON.NET
        //Testoutput: https://api.spotify.com/v1/search?q=love&type=album
        public List<Item_Artist> SearchArtists(string artistname)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + artistname + "&type=artist");
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            Artists_Artist a = JsonConvert.DeserializeObject<Artists_Artist>(json);

            //Returns all artists having the searched name:
            return a.items; //one can get the id via a.items[0|1|2|etc.].external_urls.spotify;
        }

        public List<Item_Track> SearchTracks(string trackname)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + trackname + "&type=track");
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            Tracks_Track a = JsonConvert.DeserializeObject<Tracks_Track>(json);

            return a.items;
        }
        public List<Item_Album> SearchAlbums(string albumname)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + albumname + "&type=album");
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            Albums_Album a = JsonConvert.DeserializeObject<Albums_Album>(json);

            return a.items;
        }
    }
}
