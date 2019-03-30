using CLAIM.Helpers;
using CLAIM.Helpers.XmlGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Xml;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class MonthYearModel : IValidatableSubModel
    {
        public string Month { get; set; }
        public string Year { get; set; }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            if (isRequired && (!ValidationHelper.IsMonthValid(Month) || !ValidationHelper.IsYearValid(Year)))
            {
                result.Add(new ValidationResult(string.Empty, new[] {
                    $"{instanceName}.{nameof(Month)}",
                    $"{instanceName}.{nameof(Year)}"
                }));
            }

            return result;
        }

        internal DateTime ToDate()
        {
            DateTime dateResult;

            string[] formats = { "MM-yyyy", "M-yyyy" };

            DateTime.TryParseExact(string.Format("{0}-{1}", Month, Year), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult);

            return dateResult;
        }

        internal bool IsEmpty()
        {
            return string.IsNullOrEmpty(Month) && string.IsNullOrEmpty(Year);
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(MonthYearModel).Replace("Model", string.Empty));
            xmlElement.AppendChild(helper.CreateElement(nameof(Month), Month));
            xmlElement.AppendChild(helper.CreateElement(nameof(Year), Year));
            return xmlElement;
        }
    }
}

