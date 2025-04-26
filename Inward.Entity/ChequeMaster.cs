using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCM.Entity
{
    public class ChequeMaster
    {
        public long ChequeMasterID { get; set; }
        public string chequeno { get; set; }
        public string fromcheque { get; set; }
        public string tocheque { get; set; }
        public string remarks { get; set; }
        public string chequeprefix { get; set; }
        public DataTable dt { get; set; }
        public List<chequenolist> chequenolist { get; set; }
    }

    public class chequenolist 
    {
        public string chequeno { get; set; }
    }
}
