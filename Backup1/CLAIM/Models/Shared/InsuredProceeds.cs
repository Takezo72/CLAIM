using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class InsuredProceeds : IValidatableObject
    {
        public bool PrestationAssuranceEmployeur { get; set; }

        //ClientId for all file upload.
        public string ClientId { get; set; }
        public OtherInsuranceModel AssuranceEmployeur { get; set; }
        public bool PrestationAssuranceIndividuelle { get; set; }
        public OtherInsuranceModel AssuranceIndividuelle { get; set; }
        public bool PrestationAssurancePret { get; set; }
        public OtherInsuranceModel AssurancePret { get; set; }
        public bool PrestationCnesstWsib { get; set; }
        public OtherInsuranceModel AssuranceCnesstWsib { get; set; }
        public bool PrestationIvac { get; set; }
        public OtherInsuranceModel AssuranceIvac { get; set; }
        public bool PrestationAssuranceEmploiRegulier { get; set; }
        public OtherInsuranceModel AssuranceEmploiRegulier { get; set; }
        public bool PrestationAssuranceEmploiMaladie { get; set; }
        public OtherInsuranceModel AssuranceEmploiMaladie { get; set; }
        public bool PrestationRenteInvalidite { get; set; }
        public OtherInsuranceModel AssuranceRenteInvalidite { get; set; }
        public bool PrestationRenteRetraite { get; set; }
        public OtherInsuranceModel AssuranceRenteRetraite { get; set; }
        public bool PrestationSaaq { get; set; }
        public OtherInsuranceModel AssuranceSaaq { get; set; }
        public bool PrestationAutre { get; set; }
        public OtherInsuranceModel AssuranceAutre { get; set; }
        public bool AucunePrestation { get; set; }

        public InsuredProceeds()
        {
            ClientId = Guid.NewGuid().ToString();
        }


        public string PrestationRqap { get; set; }

        internal List<OtherInsuranceModel> ListOtherInsurance()
        {
            return new List<OtherInsuranceModel>() { AssuranceEmployeur, AssuranceIndividuelle, AssurancePret, AssuranceCnesstWsib, AssuranceEmploiMaladie, AssuranceEmploiRegulier, AssuranceIvac, AssuranceSaaq, AssuranceRenteInvalidite, AssuranceRenteRetraite, AssuranceAutre };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            AjouterValidationAutresPrestations(result);
            AjouterValidationAucunePrestation(result);
            AjouterValidationPrestationsRqap(result);

            return result;
        }

        private void AjouterValidationAutresPrestations(List<ValidationResult> result)
        {
            if (!PrestationAssuranceEmployeur && !PrestationAssuranceIndividuelle && !PrestationAssurancePret &&
                !PrestationCnesstWsib && !PrestationIvac && !PrestationAssuranceEmploiRegulier &&
                !PrestationAssuranceEmploiMaladie && !PrestationRenteInvalidite && !PrestationRenteRetraite &&
                !PrestationSaaq && !PrestationAutre && !AucunePrestation)
            {
                result.Add(new ValidationResult("", new string[] { nameof(PrestationAutre) }));
            }
        }

        private void AjouterValidationAucunePrestation(List<ValidationResult> result)
        {
            if (AucunePrestation && (PrestationAssuranceEmployeur || PrestationAssuranceIndividuelle || PrestationAssurancePret ||
                PrestationCnesstWsib || PrestationIvac || PrestationAssuranceEmploiRegulier ||
                PrestationAssuranceEmploiMaladie || PrestationRenteInvalidite || PrestationRenteRetraite ||
                PrestationSaaq || PrestationAutre))
            {
                result.Add(new ValidationResult("", new string[] { nameof(AucunePrestation) }));
            }
        }

        private void AjouterValidationPrestationsRqap(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(PrestationRqap))
            {
                result.Add(new ValidationResult("", new string[] { nameof(PrestationRqap) }));
            }
        }

        internal void CreateProceeds(string typeFormulaire, bool estDeclarantAssure)
        {
            if (PrestationAssuranceEmployeur && AssuranceEmployeur == null)
            {
                AssuranceEmployeur = new OtherInsuranceModel(ClientId)
                {
                    EstDeclarantAssure = estDeclarantAssure,
                    TypeAutreAssurance = OtherInsuranceModel.TYPE_ASSURANCE_EMPLOYEUR,
                    TypeFormulaire = typeFormulaire
                };
            }

            if (PrestationAssuranceIndividuelle && AssuranceIndividuelle == null)
            {
                AssuranceIndividuelle = new OtherInsuranceModel(ClientId)
                {
                    EstDeclarantAssure = estDeclarantAssure,
                    TypeAutreAssurance = OtherInsuranceModel.TYPE_ASSURANCE_INDIVIDUELLE,
                    TypeFormulaire = typeFormulaire
                };
            }

            if (PrestationAssurancePret &&
                AssurancePret == null)
            {
                AssurancePret = new OtherInsuranceModel(ClientId)
                {
                    EstDeclarantAssure = estDeclarantAssure,
                    TypeAutreAssurance = OtherInsuranceModel.TYPE_ASSURANCE_PRET,
                    TypeFormulaire = typeFormulaire
                };
            }

            if (PrestationCnesstWsib &&
                AssuranceCnesstWsib == null)
            {
                AssuranceCnesstWsib = new OtherInsuranceModel(ClientId)
                {
                    EstDeclarantAssure = estDeclarantAssure,
                    TypeAutreAssurance = OtherInsuranceModel.TYPE_ASSURANCE_CNESST,
                    TypeFormulaire = typeFormulaire
                };
            }

            if (PrestationIvac &&
                AssuranceIvac == null)
            {
                AssuranceIvac = new OtherInsuranceModel(ClientId)
                {
                    EstDeclarantAssure = estDeclarantAssure,
                    TypeAutreAssurance = OtherInsuranceModel.TYPE_ASSURANCE_IVAC,
                    TypeFormulaire = typeFormulaire
                };
            }

            if (PrestationAssuranceEmploiRegulier &&
                AssuranceEmploiRegulier == null)
            {
                AssuranceEmploiRegulier = new OtherInsuranceModel(ClientId)
                {
                    EstDeclarantAssure = estDeclarantAssure,
                    TypeAutreAssurance = OtherInsuranceModel.TYPE_ASSURANCE_EMPLOI_REGULIER,
                    TypeFormulaire = typeFormulaire
                };
            }

            if (PrestationAssuranceEmploiMaladie &&
                AssuranceEmploiMaladie == null)
            {
                AssuranceEmploiMaladie = new OtherInsuranceModel(ClientId)
                {
                    EstDeclarantAssure = estDeclarantAssure,
                    TypeAutreAssurance = OtherInsuranceModel.TYPE_ASSURANCE_EMPLOI_MALADIE,
                    TypeFormulaire = typeFormulaire
                };
            }

            if (PrestationRenteInvalidite &&
                AssuranceRenteInvalidite == null)
            {
                AssuranceRenteInvalidite = new OtherInsuranceModel(ClientId)
                {
                    EstDeclarantAssure = estDeclarantAssure,
                    TypeAutreAssurance = OtherInsuranceModel.TYPE_ASSURANCE_RRQ_INVALIDITE,
                    TypeFormulaire = typeFormulaire
                };
            }

            if (PrestationRenteRetraite &&
                AssuranceRenteRetraite == null)
            {
                AssuranceRenteRetraite = new OtherInsuranceModel(ClientId)
                {
                    EstDeclarantAssure = estDeclarantAssure,
                    TypeAutreAssurance = OtherInsuranceModel.TYPE_ASSURANCE_RRQ_RETRAITE,
                    TypeFormulaire = typeFormulaire
                };
            }

            if (PrestationSaaq &&
                AssuranceSaaq == null)
            {
                AssuranceSaaq = new OtherInsuranceModel(ClientId)
                {
                    EstDeclarantAssure = estDeclarantAssure,
                    TypeAutreAssurance = OtherInsuranceModel.TYPE_ASSURANCE_SAAQ,
                    TypeFormulaire = typeFormulaire
                };
            }

            if (PrestationAutre &&
                AssuranceAutre == null)
            {
                AssuranceAutre = new OtherInsuranceModel(ClientId)
                {
                    EstDeclarantAssure = estDeclarantAssure,
                    TypeAutreAssurance = OtherInsuranceModel.TYPE_ASSURANCE_AUTRE,
                    TypeFormulaire = typeFormulaire
                };
            }
        }
    }

}

