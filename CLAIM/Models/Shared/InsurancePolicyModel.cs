using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using CLAIM.Helpers.XmlGenerators;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class InsurancePolicyModel : IValidatableSubModel
    {
        public string Number { get; set; }

        public bool MustBeValidated { get; set; }

        public string FormatedNumber()
        {
            if (!MustBeValidated || string.IsNullOrWhiteSpace(Number)) return string.Empty;
            return Number.Trim();
        }

        internal void Refine()
        {
            MustBeValidated = !string.IsNullOrWhiteSpace(Number);
        }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            if (!MustBeValidated || string.IsNullOrWhiteSpace(Number)) return result;

            long numeric;
            if (Number.Length != 10 || !long.TryParse(Number, out numeric))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(Number)}" }));
            }

            return result;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            return helper.CreateElement(nameof(InsurancePolicyModel).Replace("Model", string.Empty), FormatedNumber());
        }

    }
}
