using GeoTurk.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoTurk.Models
{
    public class HIT
    {
        // უნიკალური იდენთიფიკატორი
        public int HITID { get; set; }
        //დასახელება
        public string Title { get; set; }
        //აღწერა
        public string Description { get; set; }
        //ღანგრძლივობა წუთებში
        public decimal DurationInMinutes { get; set; }
        //ინსტრუქცია, დეტალური აღწერა ამოცანისთვის
        public string Instuction { get; set; }
        //დაკავშირებული ფაილის მისამართი
        public string RelatedFilePath { get; set; }
        //ვადის გასვლის თარიღი (შემდეგ ამოცანა გახდება არა აქტიური)
        public DateTime ExpireDate { get; set; }

        //კითხვის ტიპი, რომელიც განისაზღვრება დინამიურად
        public AnswerType AnswerType { get; set; }
        //პასუხის არჩევის ტიპი, ასევე განისაზღვრება დინამიურად
        public ChoiseType? ChoiseType { get; set; }
        //სავარაუდო პასუხები
        public virtual ICollection<TaskChoise> TaskChoises { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}