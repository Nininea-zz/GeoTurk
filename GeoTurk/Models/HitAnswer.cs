using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoTurk.Models
{
    public class HITAnswer
    {
        // worker hit fk
        public int WorkerID { get; set; }
        public int HITID { get; set; }
        public virtual WorkerHIT WorkerHIT { get; set; }

        // task choise fk
        public int TaskChoiseID { get; set; }
        public virtual TaskChoise TaskChoise { get; set; }
    }
}