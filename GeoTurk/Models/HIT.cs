using GeoTurk.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoTurk.Models
{
    // Human Intelligence Task - დავალება რომელიც საჭიროებს ადამიანის ინტელექტს
    public class HIT
    {
        // უნიკალური იდენთიფიკატორი
        public int HITID { get; set; }
        //დასახელება
        [DisplayName("დასახელება")]
        public string Title { get; set; }
        //აღწერა
        [DisplayName("აღწერა")]
        public string Description { get; set; }

        //ხანგრძლივობა წუთებში
        [DisplayName("ანგრძლივობა წუთებში")]
        public decimal DurationInMinutes { get; set; } = 1;

        //ინსტრუქცია, დეტალური აღწერა ამოცანისთვის
        [DisplayName("ინსტრუქცია")]
        public string Instuction { get; set; }

        //დაკავშირებული ფაილის მისამართი
        [DisplayName("დაკავშირებული ფაილი")]
        public string RelatedFilePath { get; set; }

        //ვადის გასვლის თარიღი (შემდეგ ამოცანა გახდება არა აქტიური)
        [DisplayName("ბოლო ვადა")]
        public DateTime ExpireDate { get; set; }

        //კითხვის ტიპი, რომელიც განისაზღვრება დინამიურად
        [DisplayName("კითხვის ტიპი")]
        public AnswerType AnswerType { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> AnswerTypesSelectList { get; set; }

        //პასუხის არჩევის ტიპი, ასევე განისაზღვრება დინამიურად
        [DisplayName("პასუხის ტიპი")]
        public ChoiseType? ChoiseType { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> ChoiseTypesSelectList { get; set; }

        //სავარაუდო პასუხები
        public virtual ICollection<TaskChoise> TaskChoises { get; set; }

        // HIT-ის შემსრულებლები
        public virtual ICollection<WorkerHIT> WorkerHITs { get; set; }

        // რამდენ შემსრულებელს შეუძლია დავალების შესრულება
        [DefaultValue(1)]
        [Range(1, 1000000, ErrorMessage = "დავალების შემსრულებლების რაოდენობა უნდა იყოს 1-დან 1000000-მდე")]
        [DisplayName("შემსრულებლების რაოდენობა")]
        public int WorkersCount { get; set; } = 1;

        // რა თანხა უნდა გადაუხადოს შემკვეთმა თითოეულ შემსრულებელს
        [DefaultValue(0)]
        [DisplayName("ფასი")]
        [Range(0d, 99999999.99d, ErrorMessage = "დავალების ფასი უნდა იყოს 0-დან 99999999.99-მდე")]
        public decimal Cost { get; set; } = 0;

        [DisplayName("თაგები")]
        public string Tags { get; set; }

        // შემკვეთი მომხმარებელი
        public int CreatorID { get; set; }
        public virtual User Creator { get; set; }
    }
}