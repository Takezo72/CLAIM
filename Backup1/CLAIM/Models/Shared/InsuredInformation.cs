using CLAIM.Helpers.XmlGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public abstract class InsuredInformation : IValidatableObject, INavigation
    {
        public DateModel BirthDate { get; set; }
        public AddressModel Address { get; set; }
        public abstract string PreviousStep { get; }
        public abstract string NextStep { get; }
        public abstract string ControllerName { get; }

        public InsuredInformation()
        {
            BirthDate = DateModel.CreateBirthDateModel();
            Address = new AddressModel();
        }

        public ButtonListModel NavigationButtons
        {
            get
            {
                ConfigurationHelper config = new ConfigurationHelper();
                return new ButtonListModel { NextAction = true, Cancel = true, PreviousAction = true, ReturnUrl = config.IACAReturnUrl };
            }
        }

        internal void Refine()
        {
            if (string.IsNullOrWhiteSpace(Address.SecondaryPhoneNumber))
            {
                Address.SecondaryPhoneExtension = string.Empty;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            var validations = new List<ValidationResult>();
            validations.AddRange(BirthDate.ValidatePastDate(nameof(BirthDate), true));
            validations.AddRange(Address.Validate(nameof(Address)));
            return validations;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement("AboutInsured");
            xmlElement.AppendChild(helper.CreateElement(nameof(BirthDate), helper.TransformerDate(BirthDate)));
            xmlElement.AppendChild(helper.CreateElement(nameof(Address), Address.ToXmlElement(helper)));
            return xmlElement;
        }
    }

    [Serializable]
    public class AcceleratedBenefitInsured : InsuredInformation
    {
        public override string PreviousStep => "InitializeAccelerated";
        public override string NextStep => "AboutBenefit";
        public override string ControllerName => "AcceleratedBenefit";
    }

    [Serializable]
    public class CriticalIllnessInsured : InsuredInformation
    {
        public override string PreviousStep => "InitializeClaim";
        public override string NextStep => "AboutIllness";
        public override string ControllerName => "CriticalIllness";
    }
}
