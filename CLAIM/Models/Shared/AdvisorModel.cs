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
    public class AdvisorModel : IValidatableSubModel
    {
        public string AdvisorName { get; set; }
        public string AdvisorCode { get; set; }
        public string AgencyName { get; set; }
        public string AgencyNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            if (isRequired)
            {
                if (string.IsNullOrEmpty(AdvisorName))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(AdvisorName)}" }));
                }

                if (string.IsNullOrEmpty(AdvisorCode) || AdvisorCode.Trim().Length != 6)
                {
                    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(AdvisorCode)}" }));
                }

                if (string.IsNullOrEmpty(AgencyName))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(AgencyName)}" }));
                }

                if (string.IsNullOrEmpty(AgencyNumber) || AgencyNumber.Trim().Length != 3)
                {
                    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(AgencyNumber)}" }));
                }
            }

            return result;
        }

        internal void Refine()
        {
            AdvisorName = "";
            AdvisorCode = "";
            AgencyName = "";
            AgencyNumber = "";
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(AdvisorModel).Replace("Model", string.Empty));

            xmlElement.AppendChild(helper.CreateElement(nameof(AdvisorName), AdvisorName));
            xmlElement.AppendChild(helper.CreateElement(nameof(AdvisorCode), AdvisorCode));
            xmlElement.AppendChild(helper.CreateElement(nameof(AgencyName), AgencyName));
            xmlElement.AppendChild(helper.CreateElement(nameof(AgencyNumber), AgencyNumber));

            return xmlElement;
        }
    }
}


