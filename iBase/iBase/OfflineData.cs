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
                     select d).ToList();
            }
        }
        public IEnumerable<AlbumTable> AllAlbs {
            get {
                return
                    (from d in db.AlbumTables
                     select d).ToList();
            }
        }
        public IEnumerable<TrackTable> AllTracks {
            get {
                return
                    (from d in db.TrackTables
                     select d).ToList();
            }
        }
    }
}
