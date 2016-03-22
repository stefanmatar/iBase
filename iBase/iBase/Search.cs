using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web; //Base Namespace
using SpotifyAPI.Web.Auth; //All Authentication-related classes
using SpotifyAPI.Web.Enums; //Enums
using SpotifyAPI.Web.Models; //Models for the JSON-responses

namespace iBase
{
    class Search
    {
        private static SpotifyWebAPI _spotify;
        public SearchItem SearchSomething(string searchterm, SearchType type)
        {
            SearchItem item = _spotify.SearchItems(searchterm, type);
            //Returns a SearchItem which contains the properties Paging<FullArtist> Artists,Paging<FullTrack> Tracks, Paging<SimpleAlbum> Albums, Paging<SimplePlaylist> Playlists. They are filled based on your search-type.

            return item; //To get the amount: item.Albums.Total! NOTE: if album is searched, then item.Tracks = null, item.Artists = null
        }
    }
}
