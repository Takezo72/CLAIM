using CLAIM.Helpers.XmlGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class MonthYearPeriodModel : IValidatableSubModel
    {
        public MonthYearModel DateFrom { get; set; }
        public MonthYearModel DateTo { get; set; }
        public bool DateToRequired { get; set; }
        public bool DeleteRequired { get; set; }


        public MonthYearPeriodModel() : this(true, true)
        {
        }


        public MonthYearPeriodModel(bool dateToRequired, bool deleteRequired)
        {
            DateFrom = new MonthYearModel();
            DateTo = new MonthYearModel();
            DateToRequired = dateToRequired;
            DeleteRequired = deleteRequired;
        }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            result.AddRange(DateFrom.Validate($"{instanceName}.{nameof(DateFrom)}", true));
            result.AddRange(DateTo.Validate($"{instanceName}.{nameof(DateTo)}", DateToRequired));

            if ((DateToRequired || !DateTo.IsEmpty()) && (DateTo.ToDate() < DateFrom.ToDate()))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}" }));
            }

            return result;
        }
        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(MonthYearPeriodModel).Replace("Model", string.Empty));
            xmlElement.AppendChild(helper.CreateElement(nameof(DateFrom), DateFrom.ToXmlElement(helper)));
            xmlElement.AppendChild(helper.CreateElement(nameof(DateTo), DateTo.ToXmlElement(helper)));
            return xmlElement;
        }
    }
}
