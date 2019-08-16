using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rp3.Test.Mvc.Models
{
    public class TransactionEditModel
    {
        [Display(Prompt = "Id", Name = "Id")]
        public int TransactionId { get; set; }

        [Display(Prompt = "Tipo de transacción", Name = "Tipo")]
        public short TransactionTypeId { get; set; }

        [Display(Prompt = "Categoría", Name = "Categoría")]
        public int CategoryId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Prompt = "Fecha de la transacción", Name = "Fecha")]
        [CustomDateRange(ErrorMessage ="La fecha debe ser no menor a 30 días de la fecha actual.")]
        public DateTime RegisterDate { get; set; }

        [Required]
        [Display(Prompt = "Descripción", Name = "Descripción")]
        public string ShortDescription { get; set; }

        [Display(Prompt = "Nota", Name = "Nota")]
        public string Notes { get; set; }

        [DataType(DataType.Currency)]
        [Display(Prompt = "Valor", Name = "Valor")]
        [Range(0.01,1000000,ErrorMessage ="El monto debe ser mayor a 0")]
        public decimal Amount { get; set; }

        public SelectList CategorySelectList { get; set; }
        public SelectList TransactionTypeSelectList { get; set; }
    }


    /// <summary>
    /// Atributo personalizado que permite comparar una fecha dentro del rango de la fecha actual y 30 días previos
    /// y permite su uso en validación client-side
    /// </summary>
    public class CustomDateRangeAttribute : RangeAttribute, IClientValidatable
    {

        public CustomDateRangeAttribute()
          : base(typeof(DateTime), DateTime.Now.AddDays(-30).ToShortDateString(), DateTime.Now.ToShortDateString()) {
            
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var modelClientValidationRule = new ModelClientValidationRule
            {
                ValidationType = "customdaterange",
                ErrorMessage = ErrorMessage
            };

            modelClientValidationRule.ValidationParameters.Add("min", DateTime.Now.AddDays(-30).ToShortDateString());
            return new List<ModelClientValidationRule> { modelClientValidationRule };
        }

        public override bool IsValid(object value)
        {
            return true;
        }
    }
}