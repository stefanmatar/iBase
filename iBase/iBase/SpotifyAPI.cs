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

            return ListOfTracks;
        }
        public List<Album> SearchAlbums(string albumname)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + albumname + "&offset=0&limit=50&type=album");
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            var converter = new ExpandoObjectConverter();
            dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

            List<Album> ListOfTracks = new List<Album>();

            foreach (var tempalbum in jsonobject.albums.items)
            {
                Album album = new Album();
                album.id = tempalbum.id;
                album.href = tempalbum.href;
                if (tempalbum.images.Count > 1)
                    album.imageurl = tempalbum.images[1].url;
                album.name = tempalbum.name;

                ListOfTracks.Add(album);
            }

            return ListOfTracks;
        }

        public Album GetAlbumFromID(string id)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/albums/" + id);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            var converter = new ExpandoObjectConverter();
            dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

            Album album = new Album();
            album.id = jsonobject.id;
            album.href = jsonobject.href;
            if (jsonobject.images.Count > 1)
                album.imageurl = jsonobject.images[1].url;
            album.name = jsonobject.name;

            return album;
        }

        public Artist GetArtistFromID(string id)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/artists/" + id);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            var converter = new ExpandoObjectConverter();
            dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

            Artist artist = new Artist();
            //artist.albums = tempartist.albums;
            artist.followers_total = jsonobject.followers.total;
            //artist.genres = tempartist.genres.Cast<string>();
            artist.href = jsonobject.href;
            artist.id = jsonobject.id;
            if (jsonobject.images.Count > 1)
                artist.imageurl = jsonobject.images[1].url;
            artist.name = jsonobject.name;
            artist.popularity = jsonobject.popularity;
            artist.type = jsonobject.type;
            return artist;
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
            track.imageurl = jsonobject.album.images[0].url;
            track.track_number = jsonobject.track_number;
            track.type = jsonobject.type;

            List<Artist> artists = new List<Artist>();
            foreach (var temartist in jsonobject.artists)
            {
                artists.Add(GetArtistFromID(temartist.id));
            }
            track.artists = artists;

            return track;
        }
    }
}
