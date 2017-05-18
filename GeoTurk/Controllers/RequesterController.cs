using GeoTurk.Enums;
using GeoTurk.Helpers;
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
            return View();
        }

        public ActionResult Hits()
        {
            var currentUserID = User.Identity.GetUserId<int>();
            var hitsList = DB.HITs.Where(h => h.CreatorID == currentUserID).ToList();

            return View(hitsList);
        }

        public ActionResult Add()
        {
            var model = new HIT();
            model.AnswerTypesSelectList = Extensions.GetEnumSelectList<AnswerType>(false);
            model.ChoiseTypesSelectList = Extensions.GetEnumSelectList<ChoiseType>(false);

            return View(model);
        }

        public ActionResult EditHIT(int hitID)
        {
            var hit = DB.HITs.SingleOrDefault(h => h.HITID == hitID);
            if (hit == null)
                return View("Hits");

            return View(hit);
        }
        [HttpPost]
        public ActionResult EditHIT(HIT model)
        {
            model.AnswerTypesSelectList = Extensions.GetEnumSelectList<AnswerType>(false);
            model.ChoiseTypesSelectList = Extensions.GetEnumSelectList<ChoiseType>(false);

            if (!ModelState.IsValid)
                return View(model);

            if (model.HITID != 0)
            {
                // Edit
                var hit = DB.HITs.SingleOrDefault(h => h.HITID == model.HITID);
                if (hit == null)
                    return View(model);

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

                DB.SaveChanges();

                return Json(new { success = true, message = "დავალება განახლდა" });
            }

            return View();
        }

        public ActionResult AddHIT(HIT model)
        {
            if (ModelState.IsValid)
            {
                if (model.AnswerType == AnswerType.ChoiseImage || model.AnswerType == AnswerType.ChoiseText)
                    return View("AddHITAnswers", model);
                else
                {
                    model.CreatorID = User.Identity.GetUserId<int>();
                    DB.HITs.Add(model);
                    DB.SaveChanges();

                    return View("HitAddCompleted");
                }
            }

            model.AnswerTypesSelectList = Extensions.GetEnumSelectList<AnswerType>(false);
            model.ChoiseTypesSelectList = Extensions.GetEnumSelectList<ChoiseType>(false);

            return View("Add", model);
        }

        public ActionResult SaveHIT(HIT hit, string[] possibleAnswers)
        {
            var taskChoises = new List<TaskChoise>();
            foreach (var answer in possibleAnswers)
                taskChoises.Add(new TaskChoise()
                {
                    Label = answer
                });

            hit.TaskChoises = taskChoises;
            hit.CreatorID = User.Identity.GetUserId<int>();

            DB.HITs.Add(hit);
            DB.SaveChanges();

            return Json(new { success = true, message = "დავალება შეიქმნა" });
        }

        public ActionResult SaveSuccess()
        {
            return View("HitAddCompleted");
        }

        public ActionResult DeleteHIT(int hitID, string caller)
        {
            if (string.IsNullOrEmpty(caller))
                caller = "Hits";

            var hit = DB.HITs.SingleOrDefault(h => h.HITID == hitID);
            if (hit != null)
            {
                if (hit.WorkerHITs != null && hit.WorkerHITs.Count > 0)
                    ModelState.AddModelError("", "");
                else
                {
                    DB.HITs.Remove(hit);
                    DB.SaveChanges();
                }
            }

            return RedirectToAction(caller);
        }

        public ActionResult My()
        {
            return View();
        }

        public ActionResult PublishHIT(int hitID)
        {
            var hit = DB.HITs.SingleOrDefault(h => h.HITID == hitID);

            if (hit != null)
            {
                hit.PublishDate = DateTime.Now;
                DB.SaveChanges();
            }

            return RedirectToAction("Hits");
        }

        public ActionResult UnpublishHIT(int hitID)
        {
            var hit = DB.HITs.SingleOrDefault(h => h.HITID == hitID);

            if (hit != null)
            {
                hit.PublishDate = null;
                DB.SaveChanges();
            }

            return RedirectToAction("Hits");
        }
    }
}