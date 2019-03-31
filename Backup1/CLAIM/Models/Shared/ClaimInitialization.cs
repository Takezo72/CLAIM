using CLAIM.Helpers.Configuration;
using CLAIM.Helpers.XmlGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class ClaimInitialization : IValidatableObject, INavigation
    {
        public IList<InsurancePolicyModel> InsurancePolicies { get; set; }
        public AdvisorModel Advisor { get; set; }
        public string IsInsured { get; set; }
        public string InsuredFirstName { get; set; }
        public string InsuredLastName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string IsInsurancePolicyOwner { get; set; }
        public string IsAgent { get; set; }
        public string SendInsuranceBenefitsTo { get; set; }

        public ClaimInitialization()
        {
            InsurancePolicies = Enumerable.Repeat<Func<InsurancePolicyModel>>(() => new InsurancePolicyModel(), 10)
                                          .Select(x => x()).ToList();
            Advisor = new AdvisorModel();
        }

        public bool? UserIsInsured
        {
            get
            {
                if (IsInsured == null) return null;
                return IsInsured == "O";
            }
            set
            {
                if (value.HasValue)
                    IsInsured = value.Value ? "O" : "N";
                else
                    IsInsured = null;
            }
        }
        public bool UserIsInsurancePolicyOwner => IsInsurancePolicyOwner == "O";
        public bool UserIsAgent => IsAgent == "O";
        public string PreviousStep => string.Empty;
        public string NextStep => "AboutInsured";

        public ButtonListModel NavigationButtons
        {
            get
            {
                ConfigurationHelper config = new ConfigurationHelper();
                return new ButtonListModel { NextAction = true, Cancel = true, ReturnUrl = config.IACAReturnUrl };
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validations = new List<ValidationResult>();
            validations.AddRange(ValidatePolicies());
            if (string.IsNullOrWhiteSpace(IsInsured))
            {
                validations.Add(new ValidationResult(string.Empty, new[] { nameof(IsInsured) }));
            }
            else
            {
                if (string.IsNullOrWhiteSpace(FirstName) || FirstName.Length > 50)
                    validations.Add(new ValidationResult(string.Empty, new[] { nameof(FirstName) }));
                if (string.IsNullOrWhiteSpace(LastName) || LastName.Length > 50)
                    validations.Add(new ValidationResult(string.Empty, new[] { nameof(LastName) }));
                if (string.IsNullOrWhiteSpace(Email) || Email.Length > 50)
                    validations.Add(new ValidationResult(string.Empty, new[] { nameof(Email) }));
            }
            if (!UserIsInsured ?? false)
            {
                if (string.IsNullOrWhiteSpace(InsuredFirstName) || InsuredFirstName.Length > 50)
                    validations.Add(new ValidationResult(string.Empty, new[] { nameof(InsuredFirstName) }));
                if (string.IsNullOrWhiteSpace(InsuredLastName) || InsuredLastName.Length > 50)
                    validations.Add(new ValidationResult(string.Empty, new[] { nameof(InsuredLastName) }));
            }
            if (string.IsNullOrWhiteSpace(IsInsurancePolicyOwner))
            {
                validations.Add(new ValidationResult(string.Empty, new[] { nameof(IsInsurancePolicyOwner) }));
            }
            if (!UserIsInsurancePolicyOwner)
            {
                if (string.IsNullOrWhiteSpace(IsAgent))
                {
                    validations.Add(new ValidationResult(string.Empty, new[] { nameof(IsAgent) }));
                }
                if (UserIsAgent)
                {
                    validations.AddRange(Advisor.Validate(nameof(Advisor), true));
                    if (string.IsNullOrWhiteSpace(SendInsuranceBenefitsTo))
                        validations.Add(new ValidationResult(string.Empty, new[] { nameof(SendInsuranceBenefitsTo) }));
                }
            }
            return validations;
        }

        private IEnumerable<ValidationResult> ValidatePolicies()
        {
            var validations = new List<ValidationResult>(10);
            foreach (var policy in InsurancePolicies)
            {
                validations.AddRange(policy.Validate(nameof(InsurancePolicies), true));
            }
            return validations;
        }

        internal void Refine()
        {
            foreach (var policy in InsurancePolicies)
            {
                policy.Refine();
            }
            if (UserIsInsured ?? false)
            {
                InsuredFirstName = string.Empty;
                InsuredLastName = string.Empty;
            }
            if (UserIsInsurancePolicyOwner)
            {
                IsAgent = string.Empty;
            }
            if (!UserIsAgent)
            {
                Advisor.Refine();
                SendInsuranceBenefitsTo = string.Empty;
            }
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement("AboutYou");
            xmlElement.AppendChild(helper.CreateElement(nameof(IsInsured), UserIsInsured.ToString()));
            if (!UserIsInsured ?? false)
            {
                xmlElement.AppendChild(helper.CreateElement(nameof(InsuredFirstName), InsuredFirstName));
                xmlElement.AppendChild(helper.CreateElement(nameof(InsuredLastName), InsuredLastName));
            }
            xmlElement.AppendChild(helper.CreateElement(nameof(FirstName), FirstName));
            xmlElement.AppendChild(helper.CreateElement(nameof(LastName), LastName));
            xmlElement.AppendChild(helper.CreateElement(nameof(Email), Email));
            xmlElement.AppendChild(helper.CreateElement(nameof(IsInsurancePolicyOwner), UserIsInsurancePolicyOwner.ToString()));
            if (!UserIsInsurancePolicyOwner)
            {
                xmlElement.AppendChild(helper.CreateElement(nameof(IsAgent), UserIsAgent.ToString()));
                if (UserIsAgent)
                    xmlElement.AppendChild(Advisor.ToXmlElement(helper));
                xmlElement.AppendChild(helper.CreateElement(nameof(SendInsuranceBenefitsTo), SendInsuranceBenefitsTo));
            }
            xmlElement.AppendChild(helper.CreateElement(nameof(InsurancePolicies), InsurancePolicies.Where(x => x.MustBeValidated).Select(x => x.ToXmlElement(helper)).ToList()));
            return xmlElement;
        }
    }
}


