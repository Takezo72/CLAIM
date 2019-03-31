using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using CLAIM.Helpers.XmlGenerators;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class SpecialistPhysicianModel : PhysicianModel
    {
        public SpecialistPhysicianModel() : base(false)
        {
        }
        public SpecialistPhysicianModel(bool withFirstConsultationDate) : base(withFirstConsultationDate)
        {
        }

        public string Specialty { get; set; }

        public override IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();
            result.AddRange(base.Validate(instanceName, isRequired));

            if (string.IsNullOrWhiteSpace(Specialty))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(Specialty)}" }));
            }

            return result;
        }

        public override XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = base.ToXmlElement(helper);
            xmlElement.AppendChild(helper.CreateElement(nameof(Specialty), Specialty));

            return xmlElement;
        }
    }
}
