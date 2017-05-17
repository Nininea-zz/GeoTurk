using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoTurk.Models
{
    public class WorkerHIT
    {
        public int WorkerID { get; set; }
        public virtual User Worker { get; set; }

        public int HITID { get; set; }
        public virtual HIT HIT { get; set; }

        public DateTime AssignDate { get; set; }

    }
}