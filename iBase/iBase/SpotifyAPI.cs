using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using System.Linq;
using System.Windows;

namespace iBase
{
    public class SpotifyAPI
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
                artist.followers_total = tempartist.followers.total;
                artist.href = tempartist.href;
                artist.id = tempartist.id;
                artist.name = tempartist.name;
                artist.popularity = tempartist.popularity;
                artist.type = tempartist.type;

                List<string> tempgenres = new List<string>();
                foreach (object x in tempartist.genres)
                    tempgenres.Add("" + x);
                artist.genres = tempgenres;

                if (tempartist.images.Count > 1)
                    artist.imageurl = tempartist.images[1].url;

                if (!iBase.ArtistTables.Any(x => x.Id == artist.id))
                {
                    ArtistTable a = new ArtistTable();
                    a.Id = artist.id;
                    a.Name = artist.name;
                    a.Href = artist.href;
                    if (artist.genres.Count > 0) a.Genre = artist.genres.First();
                    else a.Genre = "N/A";
                    a.FollowersTotal = artist.followers_total;
                    a.Popularity = artist.popularity;
                    a.Type = artist.type;
                    //albums TO DO
                    iBase.ArtistTables.Add(a);
                    iBase.SaveChanges();
                }
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

                if (!iBase.TrackTables.Any(x => x.Id == track.id))
                {
                    TrackTable t = new TrackTable();
                    t.Id = track.id;
                    t.Name = track.name;
                    t.DiscNumber = (int)track.disc_number;
                    t.DurationMS = track.duration_ms;
                    t.Explicity = track.explicity;
                    t.Href = track.href;
                    t.Popularity = track.popularity;
                    t.PreviewURL = track.preview_url;
                    t.TrackNumber = (int)track.track_number;
                    t.Type = track.type;
                    // album + artist TO DO
                    iBase.TrackTables.Add(t);
                    iBase.SaveChanges();
                }

                ListOfTracks.Add(track);
            }


            return ListOfTracks;
        }
        public List<AlbumTable> SearchAlbums(string albumname)
        {
            WebRequest request = WebRequest.Create("https://api.spotify.com/v1/search?q=" + albumname + "&offset=0&limit=50&type=album");
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string json = reader.ReadToEnd();

            var converter = new ExpandoObjectConverter();
            dynamic jsonobject = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);

            List<AlbumTable> ListOfAlbums = new List<AlbumTable>();

            foreach (var album in jsonobject.albums.items)
            {
                ListOfAlbums.Add(GetAlbumFromID(album.id));

                AlbumTable a = new AlbumTable();
                a.Id = album.id;

                if (!iBase.AlbumTables.Any(x => x.Id == a.id))
                {
                    iBase.AlbumTables.Add(a);
                    a.Name = album.name;
                    a.Href = album.href;
                    a.ImageURL = album.ImageURL;
                    iBase.SaveChanges();
                }
            }

            return ListOfAlbums;

        }

        public AlbumTable GetAlbumFromID(string id)
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

            if (!iBase.AlbumTables.Any(x => x.Id == album.id))
            {
                AlbumTable a = new AlbumTable();
                a.Id = album.id;
                a.Name = album.name;
                a.Href = album.href;
                a.ImageURL = album.ImageURL;
                iBase.AlbumTables.Add(a);
                iBase.SaveChanges();
            }
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

            ArtistTable a = new ArtistTable();
            a.Id = artist.id;
            a.Name = artist.name;
            a.Href = artist.href;
            if (artist.genres.Count > 0) a.Genre = artist.genres.First();
            else a.Genre = "No Genre";
            a.FollowersTotal = artist.followers_total;
            a.Popularity = artist.popularity;
            a.Type = artist.type;
            //albums TO DO
            iBase.ArtistTables.Add(a);
            iBase.SaveChanges();

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

            TrackTable t = new TrackTable();
            t.Id = track.id;
            t.Name = track.name;
            t.Album = track.album;
            t.DiscNumber = (int)track.disc_number;
            t.DurationMS = track.duration_ms;
            t.Explicity = track.explicity;
            t.Href = track.href;
            t.Popularity = track.popularity;
            t.ArtistID = track.artists.First().ToString();
            t.PreviewURL = track.preview_url;
            t.TrackNumber = (int)track.track_number;
            t.Type = track.type;
            // album + artist TO DO
            if (!iBase.TrackTables.Any(x => x.Id == t.Id))
            {
                iBase.TrackTables.Add(t);
                iBase.SaveChanges();
            }

            return track;
        }

        public Dictionary<string, string> GetArtistsAlbums(string id)
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
    }
}