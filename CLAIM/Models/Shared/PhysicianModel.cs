using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using IAFG.IA.VI.VIMWPNP2.Helpers.XmlGenerators;

namespace IAFG.IA.VI.VIMWPNP2.Models.Shared
{
    [Serializable]
    public class PhysicianModel : IValidatableSubModel
    {
        public PhysicianModel() : this(false)
        {
        }
        public PhysicianModel(bool withFirstConsultationDate)
        {
            DisplayFirstConsultationDate = withFirstConsultationDate;

            FirstConsultationDate = DateModel.CreatePastDateModel();
        }

        public bool IsListItem { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HealthcareFacility { get; set; }
        public string City { get; set; }
        public bool DisplayFirstConsultationDate { get; set; }
        public DateModel FirstConsultationDate { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(FirstName)}" }));
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(LastName)}" }));
            }

            if (string.IsNullOrWhiteSpace(HealthcareFacility))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(HealthcareFacility)}" }));
            }

            if (string.IsNullOrWhiteSpace(City))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(City)}" }));
            }

            if (DisplayFirstConsultationDate)
            {
                result.AddRange(FirstConsultationDate.ValidatePastDate($"{instanceName}.{nameof(FirstConsultationDate)}", true));
            }

            return result;
        }

        public virtual XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(PhysicianModel).Replace("Model", string.Empty));
            xmlElement.AppendChild(helper.CreateElement(nameof(FirstName), FirstName));
            xmlElement.AppendChild(helper.CreateElement(nameof(LastName), LastName));
            xmlElement.AppendChild(helper.CreateElement(nameof(HealthcareFacility), HealthcareFacility));
            xmlElement.AppendChild(helper.CreateElement(nameof(City), City));

            if (DisplayFirstConsultationDate)
            {
                xmlElement.AppendChild(helper.CreateElement(nameof(FirstConsultationDate), helper.TransformerDate(FirstConsultationDate)));
            }

            return xmlElement;
        }
    }
}

