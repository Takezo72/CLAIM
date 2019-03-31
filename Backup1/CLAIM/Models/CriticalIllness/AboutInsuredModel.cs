using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using CLAIM.Helpers.XmlGenerators;
using CLAIM.Models.Shared;
using CLAIM.Ressources.FormTexts;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.CriticalIllness
{
    [Serializable]
    public class AboutInsuredModel : IValidatableObject, INavigation
    {
        public AboutInsuredModel()
        {
            BirthDate = DateModel.CreateBirthDateModel();
            Address = new AddressModel();
        }

        public DateModel BirthDate { get; set; }
        public AddressModel Address { get; set; }

        public string PreviousStep
        {
            get { return "InitializeClaim"; }
            private set { }
        }

        public string NextStep
        {
            get { return "AboutIllness"; }
            private set { }
        }

        public ButtonListModel NavigationButtons
        {
            get
            {
                ConfigurationHelper config = new ConfigurationHelper();
                return new ButtonListModel { NextAction = true, Cancel = true, PreviousAction = true, ReturnUrl = config.IACAReturnUrl };
            }
            private set { }
        }

        internal void Refine()
        {
            if (string.IsNullOrWhiteSpace(Address.SecondaryPhoneNumber))
            {
                Address.SecondaryPhoneExtension = string.Empty;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            result.AddRange(BirthDate.ValidatePastDate(nameof(BirthDate), true));

            result.AddRange(Address.Validate(nameof(Address)));

            return result;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(AboutInsuredModel).Replace("Model", string.Empty));
            xmlElement.AppendChild(helper.CreateElement(nameof(BirthDate), helper.TransformerDate(BirthDate)));
            xmlElement.AppendChild(helper.CreateElement(nameof(Address), Address.ToXmlElement(helper)));

            return xmlElement;
        }
    }
}
