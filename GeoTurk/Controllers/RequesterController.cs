using GeoTurk.Helpers;
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
    public class RequesterController : Controller
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

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Hits()
        {
            var currentUserID = User.Identity.GetUserId<int>();
            var hitsList = await DB.HITs.Where(h => h.CreatorID == currentUserID).ToListAsync();

            return View(hitsList);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            var model = new HIT();

            model.AnswerTypesSelectList = Extensions.GetEnumSelectList<Enums.AnswerType>(false);
            model.ChoiseTypesSelectList = Extensions.GetEnumSelectList<Enums.ChoiseType>(false);

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> EditHIT(int hitID)
        {
            var hit = await DB.HITs.SingleOrDefaultAsync(h => h.HITID == hitID);

            if (hit == null)
            {
                return View("Hits");
            }

            return View(hit);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> EditHIT(HIT model)
        {
            model.AnswerTypesSelectList = Extensions.GetEnumSelectList<Enums.AnswerType>(false);
            model.ChoiseTypesSelectList = Extensions.GetEnumSelectList<Enums.ChoiseType>(false);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.HITID != 0)
            {
                // Edit
                var hit = await DB.HITs.SingleOrDefaultAsync(h => h.HITID == model.HITID);
                if (hit == null)
                {
                    return View(model);
                }

                if (model.WorkersCount < hit.WorkerHITs.Count)
                {
                    ModelState.AddModelError("", $"შემსრულებლების რაოდენობა არ შეიძლება იყოს უკვე მომუშავე შემსრულებლების რაოდენობაზე. მინიმალური რაოდენობა: {hit.WorkerHITs.Count}");

                    return View(model);
                }

                hit.Cost = model.Cost;
                hit.Description = model.Description;
                hit.DurationInMinutes = model.DurationInMinutes;
                hit.ExpireDate = model.ExpireDate;
                hit.Instuction = model.Instuction;
                hit.RelatedFilePath = model.RelatedFilePath;
                hit.Tags = model.Tags;
                hit.Title = model.Title;
                hit.WorkersCount = model.WorkersCount;

                await DB.SaveChangesAsync();

                return Json(new { success = true, message = "დავალება განახლდა" });
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> AddHIT(HIT model)
        {
            if (ModelState.IsValid)
            {
                if (model.AnswerType == Enums.AnswerType.ChoiseImage || model.AnswerType == Enums.AnswerType.ChoiseText)
                {
                    return View("AddHITAnswers", model);
                }
                else
                {
                    model.CreatorID = User.Identity.GetUserId<int>();
                    DB.HITs.Add(model);
                    await DB.SaveChangesAsync();

                    return View("HitAddCompleted");
                }
            }

            model.AnswerTypesSelectList = Extensions.GetEnumSelectList<Enums.AnswerType>(false);
            model.ChoiseTypesSelectList = Extensions.GetEnumSelectList<Enums.ChoiseType>(false);

            return View("Add", model);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> SaveHIT(HIT hit, string[] possibleAnswers)
        {
            var taskChoises = new List<TaskChoise>();
            foreach (var answer in possibleAnswers)
            {
                taskChoises.Add(new TaskChoise()
                {
                    Label = answer
                });
            }

            hit.TaskChoises = taskChoises;
            hit.CreatorID = User.Identity.GetUserId<int>();

            DB.HITs.Add(hit);
            await DB.SaveChangesAsync();

            return Json(new { success = true, message = "დავალება შეიქმნა" });
        }

        [HttpGet]
        [Authorize]
        public ActionResult SaveSuccess()
        {
            return View("HitAddCompleted");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> DeleteHIT(int hitID, string caller)
        {
            if (string.IsNullOrEmpty(caller))
            {
                caller = "Hits";
            }

            var hit = await DB.HITs.SingleOrDefaultAsync(h => h.HITID == hitID);
            if (hit != null)
            {
                if (hit.WorkerHITs != null && hit.WorkerHITs.Count > 0)
                {
                    ModelState.AddModelError("", "");
                }
                else
                {
                    DB.HITs.Remove(hit);
                    await DB.SaveChangesAsync();
                }
            }

            return RedirectToAction(caller);
        }

        [HttpGet]
        [Authorize]
        public ActionResult My()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> PublishHIT(int hitID)
        {
            var hit = await DB.HITs.SingleOrDefaultAsync(h => h.HITID == hitID);

            if (hit != null)
            {
                hit.PublishDate = DateTime.Now;
                await DB.SaveChangesAsync();
            }

            return RedirectToAction("Hits", "Requester");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> UnpublishHIT(int hitID)
        {
            var hit = await DB.HITs.SingleOrDefaultAsync(h => h.HITID == hitID);

            if (hit != null)
            {
                hit.PublishDate = null;
                await DB.SaveChangesAsync();
            }

            return RedirectToAction("Hits", "Requester");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> ViewHitAnswers(int hitID)
        {
            var userID = User.Identity.GetUserId<int>();
            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var hit = await DB.HITs.FirstOrDefaultAsync(x => x.HITID == hitID && x.CreatorID == userID);
            if (hit == null)
            {
                return RedirectToAction("Hits", "Requester");
            }

            ViewBag.HITTitle = hit.Title;

            var hitAnswers = hit.WorkerHITs.ToList();

            return View(hitAnswers);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> SetHITAnswerStatus(int hitID, int workerID, Enums.HITAnswerStatus status)
        {
            var workerHit = await DB.WorkerHITs.FirstOrDefaultAsync(x => x.HITID == hitID && x.WorkerID == workerID);
            if (workerHit == null)
            {
                return RedirectToAction("Hits", "Requester");
            }

            if (workerHit.Status == Enums.HITAnswerStatus.None)
            {
                workerHit.Status = status;

                if (status == Enums.HITAnswerStatus.Approved)
                {
                    var requesterID = User.Identity.GetUserId<int>();
                    if (requesterID == 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    var requester = await DB.Users.FirstOrDefaultAsync(x => x.Id == requesterID);
                    if (requester == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    var worker = await DB.Users.FirstOrDefaultAsync(x => x.Id == workerID);
                    if (worker == null)
                    {
                        return RedirectToAction("ViewHitAnswers", "Requester", new { hitID = hitID });
                    }

                    var hitCost = workerHit.HIT.Cost;

                    requester.Balance -= hitCost;
                    worker.Balance += hitCost;

                    DB.TransactionLogs.Add(new TransactionLog() {
                        UserID = requester.Id,
                        HITID = hitID,
                        Type = Enums.TransactionType.HITCost,
                        Amount = -hitCost
                    });
                    DB.TransactionLogs.Add(new TransactionLog()
                    {
                        UserID = worker.Id,
                        HITID = hitID,
                        Type = Enums.TransactionType.HITCost,
                        Amount = hitCost
                    });
                }

                await DB.SaveChangesAsync();
            }

            return RedirectToAction("ViewHitAnswers", "Requester", new { hitID = hitID });
        }
    }
}