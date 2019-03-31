using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using CLAIM.Helpers.XmlGenerators;
using CLAIM.Ressources.FormTexts;
using System.Web.Mvc;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class FamilyMemberWithSameProblemModel : IValidatableSubModel
    {
        public string FamilyMember { get; set; }
        public YearModel Year { get; set; }
        public string Problem { get; set; }

        public FamilyMemberWithSameProblemModel()
        {
            Year = new YearModel();

        }

        internal void Refine(bool required)
        {
            if (!required)
            {
                Year.Year = "";
                FamilyMember = string.Empty;
                Problem = string.Empty;
            }
        }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(FamilyMember))
            {
                result.Add(new ValidationResult(string.Empty, new[] { string.Format("{0}.{1}", instanceName, nameof(FamilyMember)) }));
            }

            if (string.IsNullOrWhiteSpace(Problem))
            {
                result.Add(new ValidationResult(string.Empty, new[] { string.Format("{0}.{1}", instanceName, nameof(Problem)) }));
            }

            result.AddRange(Year.Validate(string.Format("{0}.{1}", instanceName, nameof(Year)), true));

            return result;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement("FamilyMember");

            xmlElement.AppendChild(helper.CreateElement(nameof(FamilyMember), FamilyMember));

            xmlElement.AppendChild(helper.CreateElement(nameof(Year), Year.ToString()));
            xmlElement.AppendChild(helper.CreateElement(nameof(Problem), Problem));

            return xmlElement;
        }
    }
}

