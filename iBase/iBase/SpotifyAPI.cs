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
        //C# Objects created with http://json2csharp.com/ and parsed with JSON.NET
        //Testoutput: https://api.spotify.com/v1/search?q=love&type=album
        public string SearchArtists(string artistname)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + artistname + "&type=artist");
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            //Artists_Artist a = JsonConvert.DeserializeObject<Artists_Artist>(json);

            //Returns all artists having the searched name:
            //return a.items; //one can get the id via a.items[0|1|2|etc.].external_urls.spotify;

            return json;
        }

        public string SearchTracks(string trackname)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + trackname + "&type=track");
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            //Tracks_Track a = JsonConvert.DeserializeObject<Tracks_Track>(json);
            //return a.items;
            return json;
        }
        public string SearchAlbums(string albumname)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + albumname + "&type=album");
            //getting the response
            WebResponse response = request.GetResponse();
            //checking the status of the response
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            //returning the stream of the request
            Stream dataStream = response.GetResponseStream();
            //reading the dataStream
            StreamReader reader = new StreamReader(dataStream);
            string json = reader.ReadToEnd();

            return json;
        }

        public Artists_Artist SearchArtistViaID(string id)
        {

            return null;
        }
    }
}
