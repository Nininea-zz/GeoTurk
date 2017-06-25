using GeoTurk.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GeoTurk.Controllers
{
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

        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Hits()
        {
            var currentUserID = User.Identity.GetUserId<int>(); 
            var hitsList = await DB.WorkerHITs.Where(h => h.WorkerID == currentUserID).Select(wh => wh.HIT).ToListAsync();

            return View(hitsList);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Find()
        {
            var currentUserID = User.Identity.GetUserId<int>();
            if (currentUserID == 0)
            {
                return View(new List<HIT>());
            }

            var workerHits = DB.WorkerHITs.Where(wh => wh.WorkerID == currentUserID).Select(wh => wh.HITID);
            var hits = await DB.HITs
                .Where(h => h.PublishDate.HasValue && !workerHits.Contains(h.HITID))
                .OrderByDescending(x => x.PublishDate)
                .ToListAsync();

            return View(hits);
        }

        [Authorize]
        [HttpGet]
        public ActionResult My()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ViewHit(int hitID)
        {
            var userID = User.Identity.GetUserId<int>();
            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var hit = await DB.HITs.FirstOrDefaultAsync(x => x.HITID == hitID);
            if (hit == null)
            {
                return RedirectToAction("Hits", "Worker");
            }

            var userHit = hit.WorkerHITs.FirstOrDefault(x => x.WorkerID == userID);
            if (userHit != null)
            {
                ViewBag.IsCompleted = userHit.CompleteDate.HasValue;
            }

            return View(hit);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Work(int hitID)
        {
            var userID = User.Identity.GetUserId<int>();
            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var hit = await DB.HITs.FirstOrDefaultAsync(x => x.HITID == hitID);
            if (hit == null)
            {
                return RedirectToAction("Find", "Worker");
            }

            var userHit = await DB.WorkerHITs.FirstOrDefaultAsync(x => x.WorkerID == userID && x.HITID == hitID);
            if (userHit == null)
            {
                userHit = new WorkerHIT
                {
                    HITID = hitID,
                    WorkerID = userID,
                    AssignDate = DateTime.Now,
                    Status = Enums.HITAnswerStatus.None
                };

                hit.WorkerHITs.Add(userHit);
                await DB.SaveChangesAsync();
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

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddHITAnswers(int hitID, string userAnswer, List<int> taskChoiseIDs)
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

            var hit = await DB.HITs.FirstOrDefaultAsync(x => x.HITID == hitID);
            if (hit == null)
            {
                return RedirectToAction("Find", "Worker");
            }

            var userHit = await DB.WorkerHITs.FirstOrDefaultAsync(x => x.WorkerID == userID && x.HITID == hitID);
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

            await DB.SaveChangesAsync();

            return Json(new { success = true, message = " თქვენი პასუხი მიღებულია " });
        }
    }
}   