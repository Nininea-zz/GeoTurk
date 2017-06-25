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
                {
                    _db = new GeoTurkDbContext();
                }

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
            var currentUserID = User.Identity.GetUserId<int>();
            if (currentUserID == 0)
            {
                return View(new List<HIT>());
            }

            var workerHits = DB.WorkerHITs.Where(wh => wh.WorkerID == currentUserID).Select(wh => wh.HITID);
            var hits = DB.HITs
                .Where(h => h.PublishDate.HasValue && !workerHits.Contains(h.HITID))
                .OrderByDescending(x => x.PublishDate)
                .ToList();

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
            else
            {
                if (userHit.CompleteDate.HasValue)
                {
                    return RedirectToAction("Hits", "Worker");
                }
            }

            return View(hit);
        }

        public ActionResult AddHITAnswers(int hitID, string userAnswer, List<int> taskChoiseIDs)
        {
            if (string.IsNullOrEmpty(userAnswer) && (taskChoiseIDs == null || taskChoiseIDs.Count == 0))
            {
                return Json(new { success = false, message = " გთხოვთ აირჩიოთ პასუხ(ებ)ი " });
            }

            if (taskChoiseIDs != null)
            {
                taskChoiseIDs.RemoveAll(x => x == 0);
            }

            if (string.IsNullOrEmpty(userAnswer) && (taskChoiseIDs == null || taskChoiseIDs.Count == 0))
            {
                return Json(new { success = false, message = " გთხოვთ აირჩიოთ პასუხ(ებ)ი " });
            }

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
                return Json(new { success = false, message = " პასუხის გაცემამდე გთხოვთ დაიწყოთ ეს დავალება " });
            }

            if (hit.AnswerType == Enums.AnswerType.FreeText || hit.AnswerType == Enums.AnswerType.FileUpload)
            {
                userHit.UserAnswer = userAnswer;
            }
            else
            {
                if (hit.ChoiseType == Enums.ChoiseType.Single)
                {
                    userHit.HITAnswers = new List<HITAnswer>();
                    userHit.HITAnswers.Add(new HITAnswer()
                    {
                        HITID = hitID,
                        WorkerID = userID,
                        TaskChoiseID = taskChoiseIDs.First()
                    });
                }
                else
                {
                    userHit.HITAnswers = new List<HITAnswer>();
                    foreach (var taskChoiseID in taskChoiseIDs)
                    {
                        userHit.HITAnswers.Add(new HITAnswer()
                        {
                            HITID = hitID,
                            WorkerID = userID,
                            TaskChoiseID = taskChoiseID
                        });
                    }
                }
            }
            
            userHit.CompleteDate = DateTime.Now;

            DB.SaveChanges();

            return Json(new { success = true });
        }
    }
}