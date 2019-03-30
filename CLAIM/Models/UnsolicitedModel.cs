using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using CLAIM.Models.Shared;

namespace CLAIM.Models
{
    [Serializable]
    public class UnsolicitedModel : IValidatableObject
    {

        public const string SectionConsultation = "SectionConsultation";

        public UnsolicitedModel()
        {
            DateChirurgie = DateModel.CreateNeighboringDateModel();
            DateConsultation = DateModel.CreateNeighboringDateModel();
            DateRetourAuTravail = DateModel.CreateNeighboringDateModel();
            File = new FileUploadModel();
            Transmis = false;
        }

        public bool RetourAuTravail { get; set; }
        public DateModel DateRetourAuTravail { get; set; }

        public bool Chirurgie { get; set; }
        public DateModel DateChirurgie { get; set; }

        [AllowHtml]
        public string TypeChirurgie { get; set; }
        public bool Consultation { get; set; }
        public DateModel DateConsultation { get; set; }

        [AllowHtml]
        public string Specialite { get; set; }

        [AllowHtml]
        public string PrenomMedecin { get; set; }

        [AllowHtml]
        public string NomMedecin { get; set; }

        [AllowHtml]
        public string Etablissement { get; set; }

        [AllowHtml]
        public string Ville { get; set; }

        public bool Transmis { get; set; }

        [StringLength(5000)]
        [AllowHtml]
        public string AutresInformations { get; set; }
        public FileUploadModel File { get; set; }

        public RequestFollowUpModel RequestFollowUp { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            results.AddRange(ValidateRetourAuTravail());
            results.AddRange(ValidateChirurgie());
            results.AddRange(ValidateConsultation());

            return results;
        }

        private IEnumerable<ValidationResult> ValidateRetourAuTravail()
        {
            return RetourAuTravail
                   ? DateRetourAuTravail.Validate(nameof(DateRetourAuTravail), true)
                   : new ValidationResult[] { };
        }

        private IEnumerable<ValidationResult> ValidateChirurgie()
        {

            var results = new List<ValidationResult>();

            if (!Chirurgie) return results;

            results.AddRange(DateChirurgie.Validate(nameof(DateChirurgie), true));
            if (string.IsNullOrWhiteSpace(TypeChirurgie))
                results.Add(new ValidationResult(Ressources.FormTexts.UIUnsolicited.Er03, new List<string> { nameof(TypeChirurgie) }));

            return results;
        }

        private IEnumerable<ValidationResult> ValidateConsultation()
        {
            var results = new List<ValidationResult>();

            if (!Consultation) return results;

            results.AddRange(DateConsultation.Validate(nameof(DateConsultation), true));
            var emptyProperties = new List<string>();
            if (string.IsNullOrWhiteSpace(PrenomMedecin)) emptyProperties.Add(nameof(PrenomMedecin));
            if (string.IsNullOrWhiteSpace(NomMedecin)) emptyProperties.Add(nameof(NomMedecin));
            if (string.IsNullOrWhiteSpace(Etablissement)) emptyProperties.Add(nameof(Etablissement));
            if (string.IsNullOrWhiteSpace(Ville)) emptyProperties.Add(nameof(Ville));
            if (string.IsNullOrWhiteSpace(Specialite)) emptyProperties.Add(nameof(Specialite));

            if (!emptyProperties.Any()) return results;

            emptyProperties.Add(SectionConsultation);
            results.Add(new ValidationResult(Ressources.FormTexts.UIUnsolicited.Er03, emptyProperties));

            return results;
        }
    }
}

