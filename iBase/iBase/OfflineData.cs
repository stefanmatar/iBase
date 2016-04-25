using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBase {
    class OfflineData {

        iBaseDB db = new iBaseDB();

        public IEnumerable<ArtistTable> AllArt {
            get {
                return
                    (from d in db.ArtistTables
                     orderby d.Name
                     select d).ToList();
            }
        }
        public IEnumerable<AlbumTable> AllAlbs {
            get {
                return
                    (from d in db.AlbumTables
                     orderby d.Name
                     select d).ToList();
            }
        }
        public IEnumerable<TrackTable> AllTracks {
            get {
                return
                    (from d in db.TrackTables
                     orderby d.Name
                     select d).ToList();
            }
        }
    }
}
