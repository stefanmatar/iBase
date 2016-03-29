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
        iBaseDB iBase = new iBaseDB();
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

                ArtistTable t = new ArtistTable();
                t.Id = artist.id;
                t.Name = artist.name;
                t.Href = artist.href;
                t.FollowersTotal = artist.followers_total;
                t.Popularity = artist.popularity;
                t.Type = artist.type;
                t.Genre = "";
                iBase.ArtistTables.Add(t);

                ListOfArtists.Add(artist);

            }
            iBase.SaveChanges();
            return ListOfArtists;
        }

        public List<Track> SearchTracks(string trackname)
        {
            try
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
            catch (Exception)
            {
                return null;
            }
        }
        public List<AlbumTable> SearchAlbums(string albumname)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + albumname + "&offset=0&limit=50&type=album");
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                string json = reader.ReadToEnd();

                var converter = new ExpandoObjectConverter();
                dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

                List<AlbumTable> ListOfAlbums = new List<AlbumTable>();

                foreach (var tempalbum in jsonobject.albums.items)
                {
                    ListOfAlbums.Add(GetAlbumFromID(tempalbum.id));
                }

                return ListOfAlbums;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public AlbumTable GetAlbumFromID(string id)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.spotify.com/v1/albums/" + id);
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                string json = reader.ReadToEnd();

                var converter = new ExpandoObjectConverter();
                dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

                AlbumTable album = new AlbumTable();
                album.id = jsonobject.id;
                album.href = jsonobject.href;
                if (jsonobject.images.Count > 1)
                    album.imageurl = jsonobject.images[1].url;
                album.name = jsonobject.name;

                Dictionary<string, string> artists = new Dictionary<string, string>();
                foreach (var tempartist in jsonobject.artists)
                {
                    artists[tempartist.name] = tempartist.id;
                }
                album.artists = artists;

                Dictionary<string, string> tracks = new Dictionary<string, string>();
                foreach (var temptrack in jsonobject.tracks.items)
                {
                    tracks[temptrack.name] = temptrack.id;
                }
                album.tracks = tracks;

                return album;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public Artist GetArtistFromID(string id)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.spotify.com/v1/artists/" + id);
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                string json = reader.ReadToEnd();

                var converter = new ExpandoObjectConverter();
                dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

                Artist artist = new Artist();
                artist.followers_total = jsonobject.followers.total;
                artist.href = jsonobject.href;
                artist.id = jsonobject.id;
                artist.name = jsonobject.name;
                artist.popularity = jsonobject.popularity;
                artist.type = jsonobject.type;

                foreach (var tempgenre in jsonobject.genres)
                {
                    artist.genres.Add(tempgenre);
                }

                artist.albums = GetArtistsAlbums(jsonobject.id);
                if (jsonobject.images.Count > 1)
                    artist.imageurl = jsonobject.images[1].url;

                ArtistTable t = new ArtistTable();
                t.Id = artist.id;
                t.Name = artist.name;
                t.Href = artist.href;
                t.FollowersTotal = artist.followers_total;
                t.Popularity = artist.popularity;
                t.Type = artist.type;
                if (artist.genres.Count > 0) t.Genre = artist.genres[0];
                else t.Genre = "No Genre";
                iBase.ArtistTables.Add(t);
                iBase.SaveChanges();

                return artist;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Track GetTrackFromID(string id)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.spotify.com/v1/tracks/" + id);
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                string json = reader.ReadToEnd();

                var converter = new ExpandoObjectConverter();
                dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

                Track track = new Track();
                track.album = jsonobject.album.name;
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

                Dictionary<string, string> artists = new Dictionary<string, string>();
                foreach (var tempartist in jsonobject.artists)
                {
                    artists.Add(tempartist.name, tempartist.id);
                }
                track.artists = artists;

                return track;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Dictionary<string, string> GetArtistsAlbums(string id)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.spotify.com/v1/artists/" + id + "/albums");
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                string json = reader.ReadToEnd();

                var converter = new ExpandoObjectConverter();
                dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

                Dictionary<string, string> albums = new Dictionary<string, string>();
                foreach (var temptrack in jsonobject.items)
                {
                    albums.Add(temptrack.name, temptrack.id);
                }

                return albums;

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}