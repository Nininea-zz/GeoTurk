using FluentValidation;
using FluentValidation.Attributes;
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
    public class HITValidator : AbstractValidator<HIT>
    {
        public HITValidator()
        {
            RuleFor(hit => hit.ExpireDate).GreaterThan(d => DateTime.Now).WithMessage("ბოლო ვადა არ უნდა იყოს დღევანდელ დღეზე ნაკლები");
            RuleFor(hit => hit.ExpireDate).NotEmpty().WithMessage("შეავსეთ ბოლო ვადა");
            RuleFor(hit => hit.ChoiseType).NotEmpty()
                .When(hit => hit.AnswerType == AnswerType.ChoiseImage || hit.AnswerType == AnswerType.ChoiseText)
                .WithMessage("მიუთითეთ პასუხის ტიპი");
        }

    }

    // Human Intelligence Task - დავალება რომელიც საჭიროებს ადამიანის ინტელექტს
    [Validator(typeof(HITValidator))]
    public class HIT
    {
        // უნიკალური იდენთიფიკატორი
        public int HITID { get; set; }
        //დასახელება
        [DisplayName("დასახელება")]
        [MaxLength(300, ErrorMessage = "დასახელება არ შეიძლება იყოს 300 სიმბოლოზე მეტი")]
        [Required(ErrorMessage = "შეავსეთ დასახელება")]
        public string Title { get; set; }
        //აღწერა
        [DisplayName("აღწერა")]
        [MaxLength(int.MaxValue, ErrorMessage = "დასახელება არ შეიძლება იყოს 2 000 000 სიმბოლოზე მეტი")]
        [Required(ErrorMessage = "შეავსეთ აღწერა")]
        public string Description { get; set; }

        //ხანგრძლივობა წუთებში
        [DisplayName("ხანგრძლივობა წუთებში")]
        [Range(1, 9999.99, ErrorMessage = "ხანგრძლივობა უნდა იყოს 1-დან 9999.99-მდე")]
        [Required(ErrorMessage = "შეავსეთ ხანგრძლივობა")]
        public decimal DurationInMinutes { get; set; } = 1;

        //ინსტრუქცია, დეტალური აღწერა ამოცანისთვის
        [DisplayName("ინსტრუქცია")]
        [Required(ErrorMessage = "შეავსეთ ინსტრუქცია")]
        [MaxLength(int.MaxValue, ErrorMessage = "ინსტრუქცია არ შეიძლება იყოს 2 000 000 სიმბოლოზე მეტი")]
        public string Instuction { get; set; }

        //დაკავშირებული ფაილის მისამართი
        [DisplayName("დაკავშირებული ფაილი")]
        [MaxLength(int.MaxValue, ErrorMessage = "დაკავშრებული ფაილის მისამართი არ შეიძლება იყოს 2 000 000 სიმბოლოზე მეტი")]
        public string RelatedFilePath { get; set; }

        //ვადის გასვლის თარიღი (შემდეგ ამოცანა გახდება არა აქტიური)
        [DisplayName("ბოლო ვადა")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "შეავსეთ ბოლო ვადა")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpireDate { get; set; }

        //კითხვის ტიპი, რომელიც განისაზღვრება დინამიურად
        [DisplayName("კითხვის ტიპი")]
        [Required(ErrorMessage = "შეავსეთ კითხვის ტიპი")]
        public AnswerType AnswerType { get; set; }

        public DateTime? PublishDate { get; set; }

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
        [DisplayName("შემსრულებლების რაოდენობა")]
        [Required(ErrorMessage = "შეავსეთ შემსრულებლების რაოდენობა")]
        [Range(1, 1000000, ErrorMessage = "დავალების შემსრულებლების რაოდენობა უნდა იყოს 1-დან 1000000-მდე")]
        public int WorkersCount { get; set; } = 1;

        // რა თანხა უნდა გადაუხადოს შემკვეთმა თითოეულ შემსრულებელს
        [DefaultValue(0)]
        [DisplayName("ფასი")]
        [Required(ErrorMessage = "შეავსეთ ფასი")]
        [Range(0d, 99999999.99d, ErrorMessage = "დავალების ფასი უნდა იყოს 0-დან 99999999.99-მდე")]
        public decimal Cost { get; set; } = 0;

        [DisplayName("თაგები")]
        [MaxLength(int.MaxValue, ErrorMessage = "თაგები არ შეიძლება იყოს 2 000 000 სიმბოლოზე მეტი")]
        public string Tags { get; set; }

        // შემკვეთი მომხმარებელი
        public int CreatorID { get; set; }
        public virtual User Creator { get; set; }
    }
}