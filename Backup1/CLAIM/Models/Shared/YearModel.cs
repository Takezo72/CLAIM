using CLAIM.Helpers;
using CLAIM.Ressources.FormTexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class YearModel : IValidatableSubModel
    {
        public string Year { get; set; }

        public override string ToString()
        {
            if (ValidationHelper.IsYearValid(Year))
            {
                return Year;
            }

            return "";
        }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();
            if (isRequired && !ValidationHelper.IsYearValid(Year))
            {
                result.Add(new ValidationResult(string.Empty, new[] {
                    $"{instanceName}.{nameof(Year)}"
                }));
            }
            return result;
        }
    }
}
