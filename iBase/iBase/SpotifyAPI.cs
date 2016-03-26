using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace iBase
{
    class SpotifyAPI
    {
        public List<Artist> SearchArtists(string artistname)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + artistname + "&type=artist");
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            var converter = new ExpandoObjectConverter();
            dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

            List<Artist> ListOfArtists = new List<Artist>();

            foreach (var tempartist in jsonobject.artists.items)
            {
                Artist artist = new Artist();
                //artist.albums = tempartist.albums;
                artist.followers_total = tempartist.followers.total;
                //artist.genres = tempartist.genres.Cast<string>();
                artist.href = tempartist.href;
                artist.id = tempartist.id;
                if (tempartist.images.Count > 1)
                    artist.imageurl = tempartist.images[1].url;
                artist.name = tempartist.name;
                artist.popularity = tempartist.popularity;
                artist.type = tempartist.type;

                ListOfArtists.Add(artist);
            }
            return ListOfArtists;
        }

        public List<Track> SearchTracks(string trackname)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + trackname + "&offset=0&limit=50&type=track");
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            var converter = new ExpandoObjectConverter();
            dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

            List<Track> ListOfTracks = new List<Track>();

            foreach (var temptrack in jsonobject.tracks.items)
            {
                Track track = new Track();
                track.id = temptrack.id;
                track.disc_number = temptrack.disc_number;
                track.duration_ms = temptrack.duration_ms;
                track.explicity = temptrack.@explicit;
                track.href = temptrack.href;
                track.name = temptrack.name;
                track.popularity = temptrack.popularity;
                track.preview_url = temptrack.preview_url;
                track.track_number = temptrack.track_number;
                track.type = temptrack.type;

                ListOfTracks.Add(track);
            }

            //Returns all artists having the searched name:
            return ListOfTracks; //one can get the id via a.items[0|1|2|etc.].external_urls.spotify;
        }
        public string SearchAlbums(string albumname)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + albumname + "&offset=0&limit=100&type=album");
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

        public Artist GetArtistFromID(string id)
        {
            return null;
        }

        public Track GetTrackFromID(string id)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/tracks/" + id);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            var converter = new ExpandoObjectConverter();
            dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

            Track track = new Track();
            track.id = jsonobject.id;
            track.disc_number = jsonobject.disc_number;
            track.duration_ms = jsonobject.duration_ms;
            track.explicity = jsonobject.@explicit;
            track.href = jsonobject.href;
            track.name = jsonobject.name;
            track.popularity = jsonobject.popularity;
            track.preview_url = jsonobject.preview_url;
            track.imageurl= jsonobject.album.images[0].url;
            track.track_number = jsonobject.track_number;
            track.type = jsonobject.type;

            return track;
        }
    }
}
