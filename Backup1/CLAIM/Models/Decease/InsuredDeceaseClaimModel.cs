using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CLAIM.Models.Shared;
using CLAIM.Helpers;
using CLAIM.Ressources.FormTexts;
using System.Xml;
using CLAIM.Helpers.XmlGenerators;
using System.Linq;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.Decease
{
    [Serializable]
    public class InsuredDeceaseClaimModel : IValidatableObject
    {
        private const string ETAT_CIVIL_UNION_DE_FAIT = "F";
        private const string ETAT_CIVIL_DIVORCE = "D";
        private const string ETAT_CIVIL_SEPARE = "S";

        private const string TYPE_SEPARATION_FAIT = "F";
        private const string TYPE_SEPARATION_LEGALE = "L";

        private const string CAUSE_DECES_MALADIE = "O";
        private const string CAUSE_DECES_AUTRE = "N";

        private const string PAYS_DECES_AUTRE = "O";


        public InsuredDeceaseClaimModel()
        {
            InsurancePolicies = Enumerable.Repeat<Func<InsurancePolicyModel>>(() => new InsurancePolicyModel(), 4)
                                          .Select(x => x())
                                          .ToList();

            AdresseAssure = new AddressModel();
            AdresseAssure.PhoneNumberShownAndRequired = false;

            DateNaissanceAssure = DateModel.CreateBirthDateModel();
            DateDecesAssure = DateModel.CreatePastDateModel();
            DateUnionFait = DateModel.CreatePastDateModel();
            DateJugementDivorce = DateModel.CreatePastDateModel();
            DateSeparationFait = DateModel.CreatePastDateModel();
            DateSeparationLegale = DateModel.CreatePastDateModel();

            AnneePremiersSymptomes = new YearModel();
            AnneePremiereConsultation = new YearModel();
            MedicalConsultations1 = new List<MedicalConsultationModel> { new MedicalConsultationModel(true) };
            MedicalConsultations2 = new List<MedicalConsultationModel> { new MedicalConsultationModel(true) };
        }

        public string PrenomAssure { get; set; }
        public string NomAssure { get; set; }
        public string InitialeAssure { get; set; }
        public IEnumerable<InsurancePolicyModel> InsurancePolicies { get; set; }
        public AddressModel AdresseAssure { get; set; }
        public DateModel DateNaissanceAssure { get; set; }
        public DateModel DateDecesAssure { get; set; }
        public string EtatCivilAssure { get; set; }
        public DateModel DateUnionFait { get; set; }
        public DateModel DateJugementDivorce { get; set; }
        public string TypeSeparation { get; set; }
        public DateModel DateSeparationFait { get; set; }
        public DateModel DateSeparationLegale { get; set; }
        public string CauseDeces { get; set; }
        public string PaysDeces { get; set; }
        public string AutrePaysDeces { get; set; }
        public bool Conseiller { get; set; }
        public bool Beneficiaire { get; set; }
        public string CauseDecesMaladie { get; set; }
        public string PreciserMaladieCauseDeces { get; set; }
        public string RdAnneePremiersSymptomes { get; set; }
        public YearModel AnneePremiersSymptomes { get; set; }
        public string RdAnneePremiereConsultation { get; set; }
        public YearModel AnneePremiereConsultation { get; set; }
        public string HasDiagnosisInLastFiveYears1 { get; set; }
        public string HasDiagnosisInLastFiveYears2 { get; set; }
        public List<MedicalConsultationModel> MedicalConsultations1 { get; set; }
        public List<MedicalConsultationModel> MedicalConsultations2 { get; set; }
        public string PreviousStep
        {
            get { return "AskingDeceaseClaim"; }
            private set { }
        }

        public string NextStep
        {
            get { return "BeneficiaryDeceaseClaim"; }
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(PrenomAssure))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(PrenomAssure) }));
            }

            if (string.IsNullOrWhiteSpace(NomAssure))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(NomAssure) }));
            }

            result.AddRange(AdresseAssure.Validate(nameof(AdresseAssure)));
            result.AddRange(DateNaissanceAssure.ValidatePastDate(nameof(DateNaissanceAssure), true));
            result.AddRange(DateDecesAssure.ValidatePastDate(nameof(DateDecesAssure), true));

            if (ValidationHelper.IsDateValid(DateNaissanceAssure.ToString()) && ValidationHelper.IsDateValid(DateDecesAssure.ToString()))
            {
                if (DateDecesAssure.ToDate() < DateNaissanceAssure.ToDate())
                {
                    DateDecesAssure.ErrorMessage = ErrorMessages.Date_Deces_Naissance;//Resources.DeathClaim.ER35;
                    result.Add(new ValidationResult(string.Empty, new[] { $"{nameof(DateDecesAssure)}.{nameof(DateDecesAssure.Day)}", $"{nameof(DateDecesAssure)}.{nameof(DateDecesAssure.Month)}", $"{nameof(DateDecesAssure)}.{nameof(DateDecesAssure.Year)}" }));
                }

            }
            else
            {
                DateDecesAssure.ResetErrorMessage();
            }

            if (string.IsNullOrWhiteSpace(EtatCivilAssure))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(EtatCivilAssure) }));
            }

            if (EtatCivilAssure == ETAT_CIVIL_UNION_DE_FAIT)
            {
                result.AddRange(DateUnionFait.ValidatePastDate(nameof(DateUnionFait), false));
            }

            if (EtatCivilAssure == ETAT_CIVIL_DIVORCE)
            {
                result.AddRange(DateJugementDivorce.ValidatePastDate(nameof(DateJugementDivorce), false));
            }

            if (EtatCivilAssure == ETAT_CIVIL_SEPARE && (string.IsNullOrWhiteSpace(TypeSeparation)))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(TypeSeparation) }));
            }

            if (EtatCivilAssure == ETAT_CIVIL_SEPARE && TypeSeparation == TYPE_SEPARATION_FAIT)
            {
                result.AddRange(DateSeparationFait.ValidatePastDate(nameof(DateSeparationFait), false));
            }

            if (EtatCivilAssure == ETAT_CIVIL_SEPARE && TypeSeparation == TYPE_SEPARATION_LEGALE)
            {
                result.AddRange(DateSeparationLegale.ValidatePastDate(nameof(DateSeparationLegale), false));
            }

            if (string.IsNullOrWhiteSpace(CauseDecesMaladie))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(CauseDecesMaladie) }));
            }

            if (string.IsNullOrWhiteSpace(PaysDeces))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(PaysDeces) }));
            }

            if (PaysDeces == PAYS_DECES_AUTRE && string.IsNullOrWhiteSpace(AutrePaysDeces))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(AutrePaysDeces) }));
            }

            if (CauseDecesMaladie == CAUSE_DECES_AUTRE)
            {
                if (string.IsNullOrWhiteSpace(CauseDeces))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { nameof(CauseDeces) }));
                }

                if (Conseiller)
                {
                    if (string.IsNullOrWhiteSpace(HasDiagnosisInLastFiveYears2))
                    {
                        result.Add(new ValidationResult(string.Empty, new[] { nameof(HasDiagnosisInLastFiveYears2) }));
                    }

                    if (HasDiagnosisInLastFiveYears2 == "O")
                    {
                        for (var i = 0; i <= MedicalConsultations2.Count - 1; i++)
                        {
                            result.AddRange(MedicalConsultations2[i].Validate($"{nameof(MedicalConsultations2)}[{i}]"));
                        }
                    }
                }
            }

            if (CauseDecesMaladie == CAUSE_DECES_MALADIE)
            {
                if (string.IsNullOrWhiteSpace(PreciserMaladieCauseDeces))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { nameof(PreciserMaladieCauseDeces) }));
                }

                if (Conseiller)
                {
                    if (string.IsNullOrWhiteSpace(RdAnneePremiersSymptomes))
                    {
                        result.Add(new ValidationResult(string.Empty, new[] { nameof(RdAnneePremiersSymptomes) }));
                    }

                    if (RdAnneePremiersSymptomes == "O")
                    {
                        result.AddRange(AnneePremiersSymptomes.Validate(nameof(AnneePremiersSymptomes), true));
                    }

                    if (string.IsNullOrWhiteSpace(RdAnneePremiereConsultation))
                    {
                        result.Add(new ValidationResult(string.Empty, new[] { nameof(RdAnneePremiereConsultation) }));
                    }

                    if (RdAnneePremiereConsultation == "O" && string.IsNullOrWhiteSpace(AnneePremiereConsultation.Year))
                    {
                        result.AddRange(AnneePremiereConsultation.Validate(nameof(AnneePremiereConsultation), true));
                    }

                    if (string.IsNullOrWhiteSpace(HasDiagnosisInLastFiveYears2))
                    {
                        result.Add(new ValidationResult(string.Empty, new[] { nameof(HasDiagnosisInLastFiveYears2) }));
                    }

                    if (HasDiagnosisInLastFiveYears2 == "O")
                    {
                        for (var i = 0; i <= MedicalConsultations2.Count - 1; i++)
                        {
                            result.AddRange(MedicalConsultations2[i].Validate($"{nameof(MedicalConsultations2)}[{i}]"));
                        }
                    }
                }
            }

            return result;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(InsuredDeceaseClaimModel).Replace("Model", string.Empty));

            xmlElement.AppendChild(helper.CreateElement(nameof(PrenomAssure), PrenomAssure));
            xmlElement.AppendChild(helper.CreateElement(nameof(NomAssure), NomAssure));
            xmlElement.AppendChild(helper.CreateElement(nameof(InitialeAssure), InitialeAssure));
            xmlElement.AppendChild(helper.CreateElement(nameof(InsurancePolicies), InsurancePolicies.Where(x => x.MustBeValidated).Select(x => x.ToXmlElement(helper)).ToList()));

            xmlElement.AppendChild(helper.CreateElement(nameof(AdresseAssure), AdresseAssure.ToXmlElement(helper)));
            xmlElement.AppendChild(helper.CreateElement(nameof(DateNaissanceAssure), helper.TransformerDate(DateNaissanceAssure)));
            xmlElement.AppendChild(helper.CreateElement(nameof(DateDecesAssure), helper.TransformerDate(DateDecesAssure)));

            xmlElement.AppendChild(helper.CreateElement(nameof(EtatCivilAssure), EtatCivilAssure));
            if (EtatCivilAssure == ETAT_CIVIL_UNION_DE_FAIT)
            {
                xmlElement.AppendChild(helper.CreateElement(nameof(DateUnionFait), helper.TransformerDate(DateUnionFait)));
            }
            if (EtatCivilAssure == ETAT_CIVIL_DIVORCE)
            {
                xmlElement.AppendChild(helper.CreateElement(nameof(DateJugementDivorce), helper.TransformerDate(DateJugementDivorce)));
            }
            if (EtatCivilAssure == ETAT_CIVIL_SEPARE)
            {
                xmlElement.AppendChild(helper.CreateElement(nameof(TypeSeparation), TypeSeparation));
                if (TypeSeparation == TYPE_SEPARATION_FAIT)
                {
                    xmlElement.AppendChild(helper.CreateElement(nameof(DateSeparationFait), helper.TransformerDate(DateSeparationFait)));
                }
                if (TypeSeparation == TYPE_SEPARATION_LEGALE)
                {
                    xmlElement.AppendChild(helper.CreateElement(nameof(DateSeparationLegale), helper.TransformerDate(DateSeparationLegale)));
                }
            }

            xmlElement.AppendChild(helper.CreateElement(nameof(CauseDecesMaladie), CauseDecesMaladie));

            if (CauseDecesMaladie == CAUSE_DECES_AUTRE)
            {
                xmlElement.AppendChild(helper.CreateElement(nameof(CauseDeces), CauseDeces));
                if (Conseiller)
                {
                    xmlElement.AppendChild(helper.CreateElement(nameof(HasDiagnosisInLastFiveYears2), HasDiagnosisInLastFiveYears2));
                    if (HasDiagnosisInLastFiveYears2 == "O")
                    {
                        XmlElement XmlConsultations = helper.CreateElement("MedicalConsultations");
                        xmlElement.AppendChild(XmlConsultations);
                        foreach (var medicalconsultation in MedicalConsultations2)
                        {
                            XmlElement XmlConsultation = helper.CreateElement("Consultation");
                            XmlConsultations.AppendChild(XmlConsultation);
                            XmlConsultation.AppendChild(helper.CreateElement(nameof(medicalconsultation.Reason), medicalconsultation.Reason));
                            XmlConsultation.AppendChild(helper.CreateElement(nameof(medicalconsultation.Year), medicalconsultation.Year.ToString()));
                            XmlConsultation.AppendChild(helper.CreateElement(nameof(medicalconsultation.PhysicianInfos), medicalconsultation.PhysicianInfos.ToXmlElement(helper)));
                        }
                    }
                }
            }

            if (CauseDecesMaladie == CAUSE_DECES_MALADIE)
            {
                {
                    xmlElement.AppendChild(helper.CreateElement(nameof(PreciserMaladieCauseDeces), PreciserMaladieCauseDeces));

                    if (Conseiller)
                    {
                        xmlElement.AppendChild(helper.CreateElement(nameof(RdAnneePremiersSymptomes), RdAnneePremiersSymptomes));
                        if (RdAnneePremiersSymptomes == "O")
                        {
                            xmlElement.AppendChild(helper.CreateElement(nameof(AnneePremiersSymptomes), AnneePremiersSymptomes.Year));
                        }
                        xmlElement.AppendChild(helper.CreateElement(nameof(RdAnneePremiereConsultation), RdAnneePremiereConsultation));
                        if (RdAnneePremiereConsultation == "O")
                        {
                            xmlElement.AppendChild(helper.CreateElement(nameof(AnneePremiereConsultation), AnneePremiereConsultation.Year));
                        }

                        xmlElement.AppendChild(helper.CreateElement(nameof(HasDiagnosisInLastFiveYears2), HasDiagnosisInLastFiveYears2));
                        if (HasDiagnosisInLastFiveYears2 == "O")
                        {
                            XmlElement XmlConsultations = helper.CreateElement("MedicalConsultations");
                            xmlElement.AppendChild(XmlConsultations);
                            foreach (var medicalconsultation in MedicalConsultations2)
                            {
                                XmlElement XmlConsultation = helper.CreateElement("Consultation");
                                XmlConsultations.AppendChild(XmlConsultation);
                                XmlConsultation.AppendChild(helper.CreateElement(nameof(medicalconsultation.Reason), medicalconsultation.Reason));
                                XmlConsultation.AppendChild(helper.CreateElement(nameof(medicalconsultation.Year), medicalconsultation.Year.ToString()));
                                XmlConsultation.AppendChild(helper.CreateElement(nameof(medicalconsultation.PhysicianInfos), medicalconsultation.PhysicianInfos.ToXmlElement(helper)));
                            }
                        }
                    }
                }
            }

            xmlElement.AppendChild(helper.CreateElement(nameof(PaysDeces), PaysDeces));
            if (PaysDeces == PAYS_DECES_AUTRE)
            {
                xmlElement.AppendChild(helper.CreateElement(nameof(AutrePaysDeces), AutrePaysDeces));
            }

            return xmlElement;
        }
    }
}

