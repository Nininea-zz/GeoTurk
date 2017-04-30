using GeoTurk.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoTurk.Controllers
{
    public class RequesterController : Controller
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
            var currentUserID = User.Identity.GetUserId<int>();
            var hitsList = DB.HITs.Where(h => h.CreatorID == currentUserID).ToList();

            return View(hitsList);
        }
    }
}