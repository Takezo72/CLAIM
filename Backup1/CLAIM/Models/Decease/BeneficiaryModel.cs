using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CLAIM.Models.Shared;

namespace CLAIM.Models.Decease
{
    [Serializable]
    public class BeneficiaryModel : IValidatableSubModel
    {
        public BeneficiaryModel()
        {
            AdresseBeneficiaire = new AddressModel();
            BirthDate = DateModel.CreateBirthDateModel();
        }

        public int Index { get; set; }
        public bool IsDeleted { get; set; } = false;

        public string BeneficiaryName { get; set; }
        public string BeneficiaryFirstName { get; set; }
        public DateModel BirthDate { get; set; }
        public string IdemAdresseBeneficiaire { get; set; }
        public AddressModel AdresseBeneficiaire { get; set; }
        public string Transfer { get; set; }
        public string NumeroContratEpargne { get; set; }
        public string Prefix { get; set; }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(BeneficiaryName))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(BeneficiaryName)}" }));
            }

            if (string.IsNullOrWhiteSpace(BeneficiaryFirstName))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(BeneficiaryFirstName)}" }));
            }

            if (string.IsNullOrWhiteSpace(IdemAdresseBeneficiaire))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(IdemAdresseBeneficiaire)}" }));
            }

            if (!(string.IsNullOrWhiteSpace(IdemAdresseBeneficiaire)) && (IdemAdresseBeneficiaire == "N"))
            {
                result.AddRange(AdresseBeneficiaire.Validate($"{instanceName}.{nameof(AdresseBeneficiaire)}"));
            }

            if ((Transfer == "contrat" || Transfer == "proposition")
                && string.IsNullOrWhiteSpace(NumeroContratEpargne))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(NumeroContratEpargne)}" }));
            }

            return result;
        }
    }
}
