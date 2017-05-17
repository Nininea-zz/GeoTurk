using GeoTurk.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoTurk.Controllers
{
    [Authorize]
    public class WorkerController : Controller
    {
        private GeoTurkDbContext _db;
        public GeoTurkDbContext DB
        {
            get
            {
                if (_db == null)
                    _db = new GeoTurkDbContext();

                return _db;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Hits()
        {
            var currentUserID = User.Identity.GetUserId<int>();
            var hitsList = DB.WorkerHITs.Where(h => h.WorkerID == currentUserID).Select(wh => wh.HIT).ToList();

            return View(hitsList);
        }

        public ActionResult Find()
        {
            return View();
        }

        public ActionResult My()
        {
            return View();
        }
    }
}