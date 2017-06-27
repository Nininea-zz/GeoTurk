using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoTurk.Models
{
    public class RequesterPageViewModel
    {
        public int HitsCount { get; set; }
        public int CompletedHits { get; set; }

        public int LastMonthCompletedHits { get; set; }

        public int WorkersCount { get; set; }

        public decimal Balance { get; set; }

        public List<TransactionLog> LastTransactions { get; set; }
    }
}