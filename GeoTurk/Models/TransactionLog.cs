using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoTurk.Models
{
    public class TransactionLog
    {
        public int TransactionLogID { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; }

        public int HITID { get; set; }
        public virtual HIT HIT { get; set; }

        public Enums.TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreateDate { get; set; }
    }
}