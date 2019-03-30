using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CLAIM.Models.Shared;

namespace CLAIM.Models
{
    [Serializable]
    public class Insured : IValidatableObject
    {
        public string DeclarantAssure { get; set; }
        public string[] Langue { get; set; }
        public string Nom { get; set; }
        public string VotreNom { get; set; }
        public string Prenom { get; set; }
        public string VotrePrenom { get; set; }
        public string PrenomDemandeur { get; set; }
        public string NomDemandeur { get; set; }
        public string Initiale { get; set; }
        public string TelPrincipalDemandeur { get; set; }
        public string TelPrincipalPosteDemandeur { get; set; }
        public string TelSecondaireDemandeur { get; set; }
        public string TelSecondairePosteDemandeur { get; set; }
        public string CourrielDemandeur { get; set; }
        public string ChangementAdresse { get; set; }
        public AddressModel InfosAdresse { get; set; }
        public RequestFollowUpModel InfosResquestFollowUp { get; set; }

        public Insured()
        {
            InfosAdresse = new AddressModel(false);
        }

        public bool EstDeclarantAssure()
        {
            return DeclarantAssure == "O";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            AjouterValidationDeclarantAssure(result);
            AjouterValidationNom(result);
            AjouterValidationPrenom(result);
            AjouterValidationNomDemandeur(result);
            AjouterValidationPrenomDemandeur(result);
            AjouterValidationTelPrincipalDemandeur(result);
            AjouterValidationTelPrincipalPosteDemandeur(result);
            AjouterValidationTelSecondaireDemandeur(result);
            AjouterValidationTelSecondairePosteDemandeur(result);
            AjouterValidationCourrielDemandeur(result);
            AjouterValidationVotreNom(result);
            AjouterValidationVotrePrenom(result);
            AjouterValidationChangementAdresse(result);
            AjouterValidationNouvelleAdresse(result);

            return result;
        }

        private void AjouterValidationDeclarantAssure(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(DeclarantAssure))
            {
                result.Add(new ValidationResult("", new string[] { "DeclarantAssure" }));
            }
        }
        private void AjouterValidationNom(List<ValidationResult> result)
        {
            if (DeclarantAssure == "N")
            {
                if (string.IsNullOrEmpty(Nom) || Nom.Length > 50)
                {
                    result.Add(new ValidationResult("", new string[] { "Nom" }));
                }
            }
        }

        private void AjouterValidationPrenom(List<ValidationResult> result)
        {
            if (DeclarantAssure == "N")
            {
                if (string.IsNullOrEmpty(Prenom) || Prenom.Length > 50)
                {
                    result.Add(new ValidationResult("", new string[] { "Prenom" }));
                }
            }
        }

        private void AjouterValidationNomDemandeur(List<ValidationResult> result)
        {
            if (DeclarantAssure == "N")
            {
                if (string.IsNullOrEmpty(NomDemandeur) || NomDemandeur.Length > 50)
                {
                    result.Add(new ValidationResult("", new string[] { "NomDemandeur" }));
                }
            }
        }

        private void AjouterValidationPrenomDemandeur(List<ValidationResult> result)
        {
            if (DeclarantAssure == "N")
            {
                if (string.IsNullOrEmpty(PrenomDemandeur) || PrenomDemandeur.Length > 50)
                {
                    result.Add(new ValidationResult("", new string[] { "PrenomDemandeur" }));
                }
            }
        }

        private void AjouterValidationTelPrincipalDemandeur(List<ValidationResult> result)
        {
            if (DeclarantAssure == "N")
            {
                if (string.IsNullOrEmpty(TelPrincipalDemandeur) || !Helpers.ValidationHelper.IsPhoneNumberValid(TelPrincipalDemandeur))
                {
                    result.Add(new ValidationResult("", new string[] { "TelPrincipalDemandeur" }));
                }
            }
        }

        private void AjouterValidationTelPrincipalPosteDemandeur(List<ValidationResult> result)
        {
            if (DeclarantAssure == "N")
            {
                if (!(string.IsNullOrEmpty(TelPrincipalPosteDemandeur)) && !Helpers.ValidationHelper.IsIntegerValid(TelPrincipalPosteDemandeur))
                {
                    result.Add(new ValidationResult("", new string[] { "TelPrincipalPosteDemandeur" }));
                }
            }
        }

        private void AjouterValidationTelSecondaireDemandeur(List<ValidationResult> result)
        {
            if (DeclarantAssure == "N")
            {
                if (!(string.IsNullOrEmpty(TelSecondaireDemandeur)) && !Helpers.ValidationHelper.IsPhoneNumberValid(TelSecondaireDemandeur))
                {
                    result.Add(new ValidationResult("", new string[] { "TelSecondaireDemandeur" }));
                }
            }
        }

        private void AjouterValidationTelSecondairePosteDemandeur(List<ValidationResult> result)
        {
            if (DeclarantAssure == "N")
            {
                if (!(string.IsNullOrEmpty(TelSecondairePosteDemandeur)) && !Helpers.ValidationHelper.IsIntegerValid(TelSecondairePosteDemandeur))
                {
                    result.Add(new ValidationResult("", new string[] { "TelSecondairePosteDemandeur" }));
                }
            }
        }

        private void AjouterValidationCourrielDemandeur(List<ValidationResult> result)
        {
            if (DeclarantAssure == "N")
            {
                if (string.IsNullOrEmpty(CourrielDemandeur) || !Helpers.ValidationHelper.IsEmailAdressNumberValid(CourrielDemandeur))
                {
                    result.Add(new ValidationResult("", new string[] { "CourrielDemandeur" }));
                }
            }
        }

        private void AjouterValidationVotreNom(List<ValidationResult> result)
        {
            if (DeclarantAssure == "O")
            {
                if (string.IsNullOrEmpty(VotreNom) || VotreNom.Length > 50)
                {
                    result.Add(new ValidationResult("", new string[] { "VotreNom" }));
                }
            }
        }

        private void AjouterValidationVotrePrenom(List<ValidationResult> result)
        {
            if (DeclarantAssure == "O")
            {
                if (string.IsNullOrEmpty(VotrePrenom) || VotrePrenom.Length > 50)
                {
                    result.Add(new ValidationResult("", new string[] { "VotrePrenom" }));
                }
            }
        }

        private void AjouterValidationChangementAdresse(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(ChangementAdresse))
            {
                result.Add(new ValidationResult("", new string[] { nameof(ChangementAdresse) }));
            }
        }
        private void AjouterValidationNouvelleAdresse(List<ValidationResult> result)
        {
            if (ChangementAdresse == "O")
            {
                result.AddRange(InfosAdresse.Validate(nameof(InfosAdresse), true));
            }
        }

    }
}

