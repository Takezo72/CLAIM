using CLAIM.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CLAIM.Helpers.Configuration;
using CLAIM.Helpers.XmlGenerators;
using System.Xml;

namespace CLAIM.Models.AcceleratedBenefit
{
    [Serializable]
    public class AboutBenefitModel : IValidatableObject, INavigation
    {
        public MoneyModel RequestedAmount { get; set; }
        public string Diagnosis { get; set; }
        public PhysicianModel PhysicianInfo { get; set; }

        public string PreviousStep => "AboutInsured";
        public string NextStep => "Summary";

        public AboutBenefitModel()
        {
            RequestedAmount = new MoneyModel()
            {
                Min = 1,
                Max = 100000,
                MaxLength = 6,
                AcceptDecimals = false
            };
            Diagnosis = "";
            PhysicianInfo = new PhysicianModel();
        }

        public ButtonListModel NavigationButtons
        {
            get
            {
                ConfigurationHelper config = new ConfigurationHelper();
                return new ButtonListModel { NextAction = true, Cancel = true, PreviousAction = true, ReturnUrl = config.IACAReturnUrl };
            }
        }

        internal XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement("AboutBenefit");
            xmlElement.AppendChild(helper.CreateElement(nameof(RequestedAmount), RequestedAmount.GetAmount().ToString()));
            xmlElement.AppendChild(helper.CreateElement(nameof(Diagnosis), Diagnosis));
            xmlElement.AppendChild(helper.CreateElement(nameof(PhysicianInfo), PhysicianInfo.ToXmlElement(helper)));
            return xmlElement;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validations = new List<ValidationResult>();

            validations.AddRange(RequestedAmount.Validate(nameof(RequestedAmount), true));

            if (string.IsNullOrWhiteSpace(Diagnosis))
            {
                validations.Add(new ValidationResult(string.Empty, new[] { nameof(Diagnosis) }));
            }

            validations.AddRange(PhysicianInfo.Validate(nameof(PhysicianInfo), true));
            return validations;
        }
    }
}

