using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IAFG.IA.VI.VIMWPNP2.Helpers.XmlGenerators;

namespace IAFG.IA.VI.VIMWPNP2.Models.Shared
{
    [Serializable]
    public class OtherInsuranceModel : IValidatableObject
    {
        private const string CATEGORY_CNESST_SAAQ_IVAC = "RG010";
        private const string CATEGORY_RRQ_CPP = "RG038";

        internal const string TYPE_ASSURANCE_EMPLOYEUR = "EMP";
        internal const string TYPE_ASSURANCE_RRQ_RETRAITE = "RRQ";
        internal const string TYPE_ASSURANCE_RRQ_INVALIDITE = "RRI";
        internal const string TYPE_ASSURANCE_PRET = "PRT";
        internal const string TYPE_ASSURANCE_INDIVIDUELLE = "IND";
        internal const string TYPE_ASSURANCE_CNESST = "CST";
        internal const string TYPE_ASSURANCE_SAAQ = "SAA";
        internal const string TYPE_ASSURANCE_IVAC = "IVC";
        internal const string TYPE_ASSURANCE_EMPLOI_MALADIE = "AEM";
        internal const string TYPE_ASSURANCE_EMPLOI_REGULIER = "AER";
        internal const string TYPE_ASSURANCE_AUTRE = "AUT";

        public string TypeFormulaire { get; set; }
        public string TypeAutreAssurance { get; set; }
        public bool EstDeclarantAssure { get; set; }

        public string NomAssureur { get; set; }

        public string AutreAssurance { get; set; }

        public string StatutReclamation { get; set; }
        public string RecoitPrestation { get; set; }
        public MoneyModel MontantPrestation { get; set; }

        public string FrequencePrestation { get; set; }
        public string AutreFrequence { get; set; }

        public DateModel DateFinPrestation { get; set; }

        public string EvalueParUnMedecin { get; set; }

        public DateModel DateEvaluationParUnMedecin { get; set; }


        public string SituationEmploi { get; set; }
        public string AutreSituationEmploi { get; set; }

        public DateModel DateRepriseTravail { get; set; }

        public string ReconnuInapte { get; set; }
        public string TypeAssurancePret { get; set; }

        internal void ToFileXmlElement(XmlHelper helper, string urlDepot)
        {
            string codeCategory = GetCategoryImagerie();

            helper.AddFiles(LettreInapteCNESSTSAAQCopy, urlDepot, codeCategory);
            helper.AddFiles(LettreInapteEmployeurCopy, urlDepot, codeCategory);
            helper.AddFiles(RelevePaiementCopy, urlDepot, codeCategory);
            helper.AddFiles(LettreFinPrestationCopy, urlDepot, codeCategory);
            helper.AddFiles(LettreRepriseEmployeurCopy, urlDepot, codeCategory);
            helper.AddFiles(LettreRepriseCNESSTSAAQCopy, urlDepot, codeCategory);
            helper.AddFiles(LettreRefusCopy, urlDepot, codeCategory);
        }

        private string GetCategoryImagerie()
        {
            string codeCategory = CATEGORY_CNESST_SAAQ_IVAC;

            if (TypeAutreAssurance == TYPE_ASSURANCE_RRQ_RETRAITE || TypeAutreAssurance == TYPE_ASSURANCE_RRQ_INVALIDITE)
            {
                codeCategory = CATEGORY_RRQ_CPP;
            }

            return codeCategory;
        }

        public string AutreTypeAssurancePret { get; set; }

        public string ConditionConsolidee { get; set; }

        public DateModel DateMentionConditionConsolidee { get; set; }

        public string ReorientationNecessaire { get; set; }

        public DateModel DateDecisionReorientation { get; set; }
        public string TypeEmploiReorientation { get; set; }

        public string SituationCnesstWsibSaaq { get; set; }
        public string AutreSituationCnesstWsibSaaq { get; set; }
        public DateModel DateDecisionCnesstWsibSaaq { get; set; }
        public DateModel DateDecisionInapteCnesstWsibSaaq { get; set; }

        public FileUploadModel LettreInapteCNESSTSAAQCopy { get; set; }
        public FileUploadModel LettreInapteEmployeurCopy { get; set; }
        public FileUploadModel RelevePaiementCopy { get; set; }
        public FileUploadModel LettreFinPrestationCopy { get; set; }
        public FileUploadModel LettreRepriseEmployeurCopy { get; set; }
        public FileUploadModel LettreRepriseCNESSTSAAQCopy { get; set; }
        public FileUploadModel LettreRefusCopy { get; set; }

        public OtherInsuranceModel() : this(Guid.NewGuid().ToString())
        {

        }

        public OtherInsuranceModel(string clientId)
        {
            MontantPrestation = new MoneyModel()
            {
                Min = 1,
                Max = 9999999999,
                MaxLength = 10,
                AcceptDecimals = true
            };

            DateFinPrestation = DateModel.CreateLastFiveYearsDateModel();
            DateEvaluationParUnMedecin = DateModel.CreateLastFiveYearsDateModel();

            DateRepriseTravail = DateModel.CreateNeighboringDateModel();
            DateMentionConditionConsolidee = DateModel.CreateLastFiveYearsDateModel();
            DateDecisionReorientation = DateModel.CreateLastFiveYearsDateModel();
            DateDecisionCnesstWsibSaaq = DateModel.CreateLastFiveYearsDateModel();
            DateDecisionInapteCnesstWsibSaaq = DateModel.CreateLastFiveYearsDateModel();

            LettreInapteCNESSTSAAQCopy = new FileUploadModel(clientId);
            LettreInapteEmployeurCopy = new FileUploadModel(clientId);
            RelevePaiementCopy = new FileUploadModel(clientId);
            LettreFinPrestationCopy = new FileUploadModel(clientId);
            LettreRepriseEmployeurCopy = new FileUploadModel(clientId);
            LettreRepriseCNESSTSAAQCopy = new FileUploadModel(clientId);
            LettreRefusCopy = new FileUploadModel(clientId);
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            AjouterValidationRecoitPrestation(result);
            AjouterValidationMontantPrestation(result);
            AjouterValidationFrequencePrestation(result);
            AjouterValidationEvalueParUnMedecin(result);
            AjouterValidationDateEvaluationParUnMedecin(result);
            AjouterValidationDateFinPrestation(result);
            AjouterValidationSituationEmploi(result);
            AjouterValidationDateRepriseTravail(result);
            AjouterValidationReconnuInapte(result);
            AjouterValidationConditionConsolidee(result);
            AjouterValidationDateMentionConditionConsolidee(result);
            AjouterValidationReorientationNecessaire(result);
            AjouterValidationDateDecisionReorientation(result);
            AjouterValidationSituationCnesstWsibSaaq(result);
            AjouterValidationDateDecisionCnesstWsibSaaq(result);
            AjouterValidationDateDecisionInapteCnesstWsibSaaq(result);
            AjouterValidationTypeAutreAssurance(result);

            result.AddRange(LettreInapteCNESSTSAAQCopy.Validate(nameof(LettreInapteCNESSTSAAQCopy), SituationCnesstWsibSaaq == "P"));
            result.AddRange(LettreInapteEmployeurCopy.Validate(nameof(LettreInapteEmployeurCopy), SituationEmploi == "N" || (ReconnuInapte == "O" && (TypeAutreAssurance == TYPE_ASSURANCE_INDIVIDUELLE || TypeAutreAssurance == TYPE_ASSURANCE_PRET))));
            result.AddRange(RelevePaiementCopy.Validate(nameof(RelevePaiementCopy), RecoitPrestation == "O" && (TypeAutreAssurance == TYPE_ASSURANCE_RRQ_INVALIDITE || TypeAutreAssurance == TYPE_ASSURANCE_RRQ_RETRAITE || TypeAutreAssurance == TYPE_ASSURANCE_CNESST || TypeAutreAssurance == TYPE_ASSURANCE_IVAC || TypeAutreAssurance == TYPE_ASSURANCE_SAAQ)));
            result.AddRange(LettreFinPrestationCopy.Validate(nameof(LettreFinPrestationCopy), RecoitPrestation == "N" && TypeAutreAssurance == TYPE_ASSURANCE_RRQ_INVALIDITE));
            result.AddRange(LettreRepriseEmployeurCopy.Validate(nameof(LettreRepriseEmployeurCopy), SituationEmploi == "S" || SituationEmploi == "A"));
            result.AddRange(LettreRepriseCNESSTSAAQCopy.Validate(nameof(LettreRepriseCNESSTSAAQCopy), SituationCnesstWsibSaaq == "R" || SituationCnesstWsibSaaq == "A"));
            result.AddRange(LettreRefusCopy.Validate(nameof(LettreRefusCopy), StatutReclamation == "R" && TypeAutreAssurance == TYPE_ASSURANCE_RRQ_RETRAITE));

            return result;
        }

        private void AjouterValidationTypeAutreAssurance(List<ValidationResult> result)
        {
            if (TypeAutreAssurance == TYPE_ASSURANCE_PRET)
            {
                if (string.IsNullOrEmpty(TypeAssurancePret))
                {
                    result.Add(new ValidationResult("", new string[] { "TypeAssurancePret" }));
                }
            }
        }
        private void AjouterValidationDateDecisionInapteCnesstWsibSaaq(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if ((TypeAutreAssurance == TYPE_ASSURANCE_CNESST || TypeAutreAssurance == TYPE_ASSURANCE_SAAQ))
                {
                    if (!string.IsNullOrEmpty(SituationCnesstWsibSaaq) && SituationCnesstWsibSaaq == "P")
                    {
                        result.AddRange(DateDecisionInapteCnesstWsibSaaq.ValidatePastDate(nameof(DateDecisionInapteCnesstWsibSaaq), true));
                    }
                }
            }
        }

        private void AjouterValidationDateDecisionCnesstWsibSaaq(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if ((TypeAutreAssurance == TYPE_ASSURANCE_CNESST || TypeAutreAssurance == TYPE_ASSURANCE_SAAQ))
                {
                    if (!string.IsNullOrEmpty(SituationCnesstWsibSaaq) && (SituationCnesstWsibSaaq == "R" || SituationCnesstWsibSaaq == "A"))
                    {
                        result.AddRange(DateDecisionCnesstWsibSaaq.ValidatePastDate(nameof(DateDecisionCnesstWsibSaaq), true));
                    }
                }
            }
        }

        private void AjouterValidationSituationCnesstWsibSaaq(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {

                if (TypeAutreAssurance == TYPE_ASSURANCE_CNESST || TypeAutreAssurance == TYPE_ASSURANCE_SAAQ)
                {
                    if (string.IsNullOrEmpty(SituationCnesstWsibSaaq))
                    {
                        result.Add(new ValidationResult("", new string[] { "SituationCnesstWsibSaaq" }));
                    }
                }
            }
        }

        private void AjouterValidationDateDecisionReorientation(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if ((TypeAutreAssurance == TYPE_ASSURANCE_CNESST || TypeAutreAssurance == TYPE_ASSURANCE_SAAQ) && ReorientationNecessaire == "O")
                {
                    if (!string.IsNullOrEmpty(ReorientationNecessaire) && ReorientationNecessaire == "O")
                    {
                        result.AddRange(DateDecisionReorientation.ValidatePastDate(nameof(DateDecisionReorientation), true));
                    }
                }
            }
        }

        private void AjouterValidationReorientationNecessaire(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {

                if (TypeAutreAssurance == TYPE_ASSURANCE_CNESST || TypeAutreAssurance == TYPE_ASSURANCE_SAAQ)
                {
                    if (string.IsNullOrEmpty(ReorientationNecessaire))
                    {
                        result.Add(new ValidationResult("", new string[] { "ReorientationNecessaire" }));
                    }
                }
            }
        }

        private void AjouterValidationDateMentionConditionConsolidee(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {

                if (TypeAutreAssurance == TYPE_ASSURANCE_CNESST || TypeAutreAssurance == TYPE_ASSURANCE_SAAQ)
                {
                    if (!string.IsNullOrEmpty(ConditionConsolidee) && ConditionConsolidee == "O")
                    {
                        result.AddRange(DateMentionConditionConsolidee.ValidatePastDate(nameof(DateMentionConditionConsolidee), true));

                    }
                }
            }

        }

        private void AjouterValidationConditionConsolidee(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if (TypeAutreAssurance == TYPE_ASSURANCE_CNESST || TypeAutreAssurance == TYPE_ASSURANCE_SAAQ)
                {
                    if (string.IsNullOrEmpty(ConditionConsolidee))
                    {
                        result.Add(new ValidationResult("", new string[] { "ConditionConsolidee" }));
                    }
                }
            }
        }

        private void AjouterValidationReconnuInapte(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if (TypeAutreAssurance == TYPE_ASSURANCE_INDIVIDUELLE || TypeAutreAssurance == TYPE_ASSURANCE_PRET)
                {
                    if (string.IsNullOrEmpty(ReconnuInapte))
                    {
                        result.Add(new ValidationResult("", new string[] { "ReconnuInapte" }));
                    }
                }
            }
        }

        private void AjouterValidationDateRepriseTravail(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if (TypeAutreAssurance == TYPE_ASSURANCE_EMPLOYEUR)
                {
                    if (!string.IsNullOrEmpty(SituationEmploi))
                    {
                        if (SituationEmploi == "S" || SituationEmploi == "A")
                        {
                            result.AddRange(DateRepriseTravail.Validate(nameof(DateRepriseTravail), true));
                        }
                    }
                }
            }
        }

        private void AjouterValidationSituationEmploi(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if (TypeAutreAssurance == TYPE_ASSURANCE_EMPLOYEUR)
                {
                    if (string.IsNullOrEmpty(SituationEmploi))
                    {
                        result.Add(new ValidationResult("", new string[] { "SituationEmploi" }));
                    }
                }
            }
        }

        private void AjouterValidationDateFinPrestation(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if (!string.IsNullOrEmpty(RecoitPrestation) && RecoitPrestation == "N")
                {
                    result.AddRange(DateFinPrestation.ValidatePastDate(nameof(DateFinPrestation), true));
                }
            }
        }

        private void AjouterValidationDateEvaluationParUnMedecin(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if (!string.IsNullOrEmpty(RecoitPrestation) && RecoitPrestation == "O")
                {
                    if (!string.IsNullOrEmpty(EvalueParUnMedecin) && EvalueParUnMedecin == "O")
                    {
                        result.AddRange(DateEvaluationParUnMedecin.ValidatePastDate(nameof(DateEvaluationParUnMedecin), true));
                    }
                }
            }
        }

        private void AjouterValidationEvalueParUnMedecin(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if (string.IsNullOrEmpty(EvalueParUnMedecin) && (TypeAutreAssurance != TYPE_ASSURANCE_RRQ_INVALIDITE) && (TypeAutreAssurance != TYPE_ASSURANCE_RRQ_RETRAITE) &&
                   (TypeAutreAssurance != TYPE_ASSURANCE_IVAC) && (TypeAutreAssurance != TYPE_ASSURANCE_EMPLOI_REGULIER) && (TypeAutreAssurance != TYPE_ASSURANCE_EMPLOI_MALADIE))
                {
                    result.Add(new ValidationResult("", new string[] { "EvalueParUnMedecin" }));
                }
            }
        }

        private void AjouterValidationMontantPrestation(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if (!string.IsNullOrEmpty(RecoitPrestation) && RecoitPrestation == "O")
                {
                    result.AddRange(MontantPrestation.Validate(nameof(MontantPrestation), true));
                }
            }
        }

        private void AjouterValidationFrequencePrestation(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if (!string.IsNullOrEmpty(RecoitPrestation) && RecoitPrestation == "O")
                {
                    if (string.IsNullOrEmpty(FrequencePrestation))
                    {
                        result.Add(new ValidationResult("", new string[] { "FrequencePrestation" }));
                    }
                }
            }
        }

        private void AjouterValidationRecoitPrestation(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(StatutReclamation) && StatutReclamation == "A")
            {
                if (string.IsNullOrEmpty(RecoitPrestation))
                {
                    result.Add(new ValidationResult("", new string[] { "RecoitPrestation" }));
                }
            }
        }
    }
}
