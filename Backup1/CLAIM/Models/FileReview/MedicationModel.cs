using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CLAIM.Models.Shared;

namespace CLAIM.Models.FileReview
{
    [Serializable]
    public class MedicationModel : IValidatableSubModel
    {
        public MedicationModel()
        {

        }

        public int Index { get; set; }
        public string NomMedicament { get; set; }
        public string Posologie { get; set; }
        public string PosologieLabel { get; set; }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            //if (string.IsNullOrWhiteSpace(BeneficiaryName))
            //{
            //    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(BeneficiaryName)}" }));
            //}

            //if (string.IsNullOrWhiteSpace(BeneficiaryFirstName))
            //{
            //    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(BeneficiaryFirstName)}" }));
            //}

            //if (string.IsNullOrWhiteSpace(IdemAdresseBeneficiaire))
            //{
            //    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(IdemAdresseBeneficiaire)}" }));
            //}

            //if (!(string.IsNullOrWhiteSpace(IdemAdresseBeneficiaire)) && (IdemAdresseBeneficiaire == "N"))
            //{
            //    result.AddRange(AdresseBeneficiaire.Validate($"{instanceName}.{nameof(AdresseBeneficiaire)}"));
            //}

            return result;
        }
    }
}

