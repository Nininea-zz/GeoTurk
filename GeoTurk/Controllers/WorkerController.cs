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
            var hits = DB.HITs.Where(h => h.PublishDate.HasValue).OrderByDescending(x => x.PublishDate).ToList();

            return View(hits);
        }

        public ActionResult My()
        {
            return View();
        }

        public ActionResult ViewHit(int hitID)
        {
            var userID = User.Identity.GetUserId<int>();
            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var hit = DB.HITs.FirstOrDefault(x => x.HITID == hitID);
            if (hit == null)
            {
                return RedirectToAction("Hits");
            }

            var userHit = hit.WorkerHITs.FirstOrDefault(x => x.WorkerID == userID);
            if (userHit != null)
            {
                ViewBag.IsCompleted = userHit.CompleteDate.HasValue;
            }

            return View(hit);
        }

        public ActionResult Work(int hitID)
        {
            var userID = User.Identity.GetUserId<int>();
            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var hit = DB.HITs.FirstOrDefault(x => x.HITID == hitID);
            if (hit == null)
            {
                return RedirectToAction("Find", "Worker");
            }

            var userHit = DB.WorkerHITs.FirstOrDefault(x => x.WorkerID == userID && x.HITID == hitID);
            if (userHit == null)
            {
                userHit = new WorkerHIT
                {
                    HITID = hitID,
                    WorkerID = userID,
                    AssignDate = DateTime.Now
                };

                hit.WorkerHITs.Add(userHit);
                DB.SaveChanges();
            }
            
            return View(hit);
        }
    }
}