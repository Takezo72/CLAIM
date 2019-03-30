sing System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using IAFG.IA.VI.VIMWPNP2.Helpers.XmlGenerators;

namespace IAFG.IA.VI.VIMWPNP2.Models.Shared
{
    [Serializable]
    public class PeriodModel : IValidatableSubModel
    {
        public PeriodModel() : this(DateModel.CreateNeighboringDateModel(), DateModel.CreateNeighboringDateModel(), true, true)
        {
        }

        public PeriodModel(DateModel dateTo, DateModel dateFrom) : this(dateTo, dateFrom, true, true)
        {
        }

        public PeriodModel(DateModel dateTo, DateModel dateFrom, bool dateToRequired, bool deleteRequired)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
            DateToRequired = dateToRequired;
            DeleteRequired = deleteRequired;
        }

        public DateModel DateFrom { get; set; }
        public DateModel DateTo { get; set; }
        public bool DateToRequired { get; set; }
        public bool DeleteRequired { get; set; }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            if (DateFrom.YearMax == DateTime.Now.Year)
            {
                result.AddRange(DateFrom.ValidatePastDate($"{instanceName}.{nameof(DateFrom)}", true));
            }
            else
            {
                result.AddRange(DateFrom.Validate($"{instanceName}.{nameof(DateFrom)}", true));
            }

            if (DateTo.YearMax == DateTime.Now.Year)
            {
                result.AddRange(DateTo.ValidatePastDate($"{instanceName}.{nameof(DateTo)}", DateToRequired));
            }
            else
            {
                result.AddRange(DateTo.Validate($"{instanceName}.{nameof(DateTo)}", DateToRequired));
            }

            if ((DateToRequired || !DateTo.IsEmpty()) && (DateTo.ToDate() < DateFrom.ToDate()))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(DateTo)}" }));
            }

            return result;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(PeriodModel).Replace("Model", string.Empty));
            xmlElement.AppendChild(helper.CreateElement(nameof(DateFrom), helper.TransformerDate(DateFrom)));
            xmlElement.AppendChild(helper.CreateElement(nameof(DateTo), helper.TransformerDate(DateTo)));
            return xmlElement;
        }
    }
}

