using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoTurk.Models
{
    public class TaskChoise
    {
        public int TaskChoiseID { get; set; }
        public bool IsCorrect { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }

        public int HITID { get; set; }
        public virtual HIT HIT { get; set; }
    }
}