using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CLAIM.Models.Shared;
using CLAIM.Ressources.FormTexts;

namespace CLAIM.Models
{
    [Serializable]
    public class InsuredClaimModel : IValidatableObject
    {
        public Insured InfosInsured { get; set; }
        public InsuredInvalidity InfosInvalidity { get; set; }
        public InsuredAccident InfosAccident { get; set; }
        public InsuredDisease InfosDisease { get; set; }
        public InsuredEmployment InfosEmployment { get; set; }
        public InsuredProceeds InfosProceeds { get; set; }
        public InsuredSummary InfosSummary { get; set; }
        public bool Transmis { get; set; }

        public InsuredClaimModel()
        {
            Transmis = false;
            InfosInsured = new Insured();
            InfosInvalidity = new InsuredInvalidity();
            InfosEmployment = new InsuredEmployment();
            InfosAccident = new InsuredAccident();
            InfosDisease = new InsuredDisease();
            InfosProceeds = new InsuredProceeds();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            return result;
        }

        internal bool IsTransmitted()
        {
            return Transmis;
        }
    }

    [Serializable]
    public class InsuredAccident : IValidatableObject
    {
        public DateModel DateAccident { get; set; }
        public string NomEtablissementConduit { get; set; }
        public string NomEtablissementTraite { get; set; }
        public string VilleEtablissementConduit { get; set; }
        public string VilleEtablissementTraite { get; set; }
        public string RecitEvenement { get; set; }
        public string AccidentMotorise { get; set; }
        public string Conducteur { get; set; }
        public string HeureAccident { get; set; }
        public string MinuteAccident { get; set; }
        public string LieuAccident { get; set; }
        public string Symptomes { get; set; }
        public SliderModel IntensiteSymptomes { get; set; }
        public string BesoinAide { get; set; }
        public bool BesoinAide_Transport { get; set; }
        public bool BesoinAide_Entretien { get; set; }
        public bool BesoinAide_Courses { get; set; }
        public bool BesoinAide_GererArgent { get; set; }
        public bool BesoinAide_PreparerRepas { get; set; }
        public bool BesoinAide_Escalier { get; set; }
        public bool BesoinAide_Laver { get; set; }
        public bool BesoinAide_Autre { get; set; }
        public string BesoinAide_AutrePrecision { get; set; }
        public string MedecinSpecialiste { get; set; }
        public string Hospitalise { get; set; }
        public string Etablissement_Hospitalise { get; set; }
        public string Ville_Hospitalise { get; set; }
        public PeriodModel PeriodeHospitalise { get; set; }
        public string MedicamentsPris { get; set; }
        public string ListeMedicamentsPris { get; set; }
        public string TherapieSuivie { get; set; }
        public bool Acupuncture { get; set; }
        public bool Chiropratique { get; set; }
        public bool Ergotherapie { get; set; }
        public bool Physiotherapie { get; set; }
        public bool Psychotherapie { get; set; }
        public bool Therapie_Autre { get; set; }
        public string Therapie_AutrePrecision { get; set; }
        public string MaladieSimilaire { get; set; }
        public YearModel MaladieSimilaire_Annee { get; set; }
        public string MaladieSimilaire_Consulte { get; set; }
        public string HasDiagnosisInLastFiveYears { get; set; }
        public List<MedicalConsultationModel> MedicalConsultations { get; set; }
        public SpecialistPhysicianModel InfosMedecinSpecialiste { get; set; }
        public PhysicianModel InfosMedecinMaladieSimilaire { get; set; }
        public List<PhysicianModel> ListeMedecin { get; set; }


        public InsuredAccident()
        {
            MaladieSimilaire_Annee = new YearModel();
            DateAccident = DateModel.CreateLastFiveYearsDateModel();

            PeriodeHospitalise = new PeriodModel(DateModel.CreateLastFiveYearsDateModel(), DateModel.CreateLastFiveYearsDateModel());
            PeriodeHospitalise.DeleteRequired = false;
            ListeMedecin = new List<PhysicianModel> { new PhysicianModel { DisplayFirstConsultationDate = true, IsListItem = true } };

            List<SliderBracket> brackets = new List<SliderBracket>()
            {
                new SliderBracket() { BracketResourceManager = UIAccident.ResourceManager, BracketNameResource = nameof(UIAccident.IU39_3_1), MinValue = 0, MaxValue = 4 },
                new SliderBracket() { BracketResourceManager = UIAccident.ResourceManager, BracketNameResource = nameof(UIAccident.IU39_3_2), MinValue = 5, MaxValue = 7 },
                new SliderBracket() { BracketResourceManager = UIAccident.ResourceManager, BracketNameResource = nameof(UIAccident.IU39_3_3), MinValue = 8, MaxValue = 10 }
            };

            IntensiteSymptomes = new SliderModel()
            {
                Name = nameof(IntensiteSymptomes),
                MaxValue = 10,
                MinValue = 0,
                StepValue = 1,
                Value = "0",
                BracketResourceManager = UIAccident.ResourceManager,
                BracketTitleResource = nameof(UIAccident.IU39_3),
                Brackets = brackets
            };

            InfosMedecinSpecialiste = new SpecialistPhysicianModel(true);
            InfosMedecinMaladieSimilaire = new PhysicianModel();

            MedicalConsultations = new List<MedicalConsultationModel> { new MedicalConsultationModel(true) };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            result.AddRange(IntensiteSymptomes.Validate(nameof(IntensiteSymptomes), true));

            AjouterValidationDateAccident(result, validationContext);
            AjouterValidationEtablissementConduit(result);
            AjouterValidationEtablissementTraite(result);
            AjouterValidationRecitEvenement(result);
            AjouterValidationAccidentMotorise(result);
            AjouterValidationConducteur(result);
            AjouterValidationHeureAccident(result);
            AjouterValidationMinuteAccident(result);
            AjouterValidationBesoinAide(result);
            AjouterValidationListeBesoinAide(result);
            AjouterValidationMedecinSpecialiste(result);
            AjouterValidationDateConsultationSpecialiste(result);
            AjouterValidationHospitalise(result);
            AjouterValidationDate_Hospitalise(result);
            AjouterValidationMedicamentsPris(result);
            AjouterValidationListeMedicamentsPris(result);
            AjouterValidationTherapieSuivie(result);
            AjouterValidationListeTherapieSuivie(result);
            AjouterValidationMaladieSimilaire(result);
            AjouterValidationMaladieSimilaireAnnee(result);
            AjouterValidationMaladieSimilaireConsulte(result);
            AjouterValidation5DernieresAnnees(result);
            AjouterValidationBlocs5DernieresAnnees(result);
            AjouterValidationListeMedecins(result);

            return result;
        }

        private void AjouterValidationListeMedecins(List<ValidationResult> result)
        {
            for (var i = 0; i <= ListeMedecin.Count - 1; i++)
            {
                result.AddRange(ListeMedecin[i].Validate($"{nameof(ListeMedecin)}[{i}]"));
            }
        }

        private void AjouterValidationDateAccident(List<ValidationResult> result, ValidationContext validationContext)
        {
            result.AddRange(DateAccident.ValidatePastDate(nameof(DateAccident), true));
        }

        private void AjouterValidationRecitEvenement(List<ValidationResult> result)
        {
            if ((string.IsNullOrEmpty(RecitEvenement)) && AccidentMotorise == "O")
            {
                result.Add(new ValidationResult("", new string[] { nameof(RecitEvenement) }));
            }
        }

        private void AjouterValidationEtablissementConduit(List<ValidationResult> result)
        {
            if (AccidentMotorise == "O" && (string.IsNullOrEmpty(NomEtablissementConduit)))
            {
                result.Add(new ValidationResult("", new string[] { nameof(NomEtablissementConduit) }));
            }

            if (AccidentMotorise == "O" && (string.IsNullOrEmpty(VilleEtablissementConduit)))
            {
                result.Add(new ValidationResult("", new string[] { nameof(VilleEtablissementConduit) }));
            }
        }

        private void AjouterValidationEtablissementTraite(List<ValidationResult> result)
        {
            if (AccidentMotorise == "O" && !string.IsNullOrEmpty(NomEtablissementTraite) && string.IsNullOrEmpty(VilleEtablissementTraite))
            {
                result.Add(new ValidationResult("", new string[] { nameof(VilleEtablissementTraite) }));
            }
        }

        private void AjouterValidationAccidentMotorise(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(AccidentMotorise))
            {
                result.Add(new ValidationResult("", new string[] { nameof(AccidentMotorise) }));
            }
        }

        private void AjouterValidationConducteur(List<ValidationResult> result)
        {
            if (AccidentMotorise == "O")
            {
                if (string.IsNullOrEmpty(Conducteur))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(Conducteur) }));
                }
            }
        }

        private void AjouterValidationHeureAccident(List<ValidationResult> result)
        {
            if (Conducteur == "O")
            {
                if (string.IsNullOrEmpty(HeureAccident))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(HeureAccident) }));
                }
                else
                {
                    int dateVerification;
                    if (int.TryParse(HeureAccident, out dateVerification))
                    {
                        if (Int32.Parse(HeureAccident) < 0 || Int32.Parse(HeureAccident) > 24)
                        {
                            result.Add(new ValidationResult("", new string[] { nameof(HeureAccident) }));
                        }
                    }
                    else
                    {
                        result.Add(new ValidationResult("", new string[] { nameof(HeureAccident) }));
                    }
                }
            }
        }

        private void AjouterValidationMinuteAccident(List<ValidationResult> result)
        {
            if (Conducteur == "O")
            {
                if (string.IsNullOrEmpty(MinuteAccident))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(MinuteAccident) }));
                }
                else
                {
                    int dateVerification;
                    if (int.TryParse(MinuteAccident, out dateVerification))
                    {
                        if (Int32.Parse(MinuteAccident) < 0 || Int32.Parse(MinuteAccident) > 60)
                        {
                            result.Add(new ValidationResult("", new string[] { nameof(MinuteAccident) }));
                        }
                    }
                    else
                    {
                        result.Add(new ValidationResult("", new string[] { nameof(MinuteAccident) }));
                    }
                }
            }
        }

        private void AjouterValidationBesoinAide(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(BesoinAide))
            {
                result.Add(new ValidationResult("", new string[] { nameof(BesoinAide) }));
            }
        }

        private void AjouterValidationListeBesoinAide(List<ValidationResult> result)
        {
            if (BesoinAide == "O")
            {
                if (!BesoinAide_Transport && !BesoinAide_Entretien && !BesoinAide_Courses && !BesoinAide_GererArgent && !BesoinAide_PreparerRepas &&
                    !BesoinAide_Escalier && !BesoinAide_Laver && !BesoinAide_Autre)
                {
                    result.Add(new ValidationResult("", new string[] { nameof(BesoinAide_Transport) }));
                }
            }
        }

        private void AjouterValidationMedecinSpecialiste(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(MedecinSpecialiste))
            {
                result.Add(new ValidationResult("", new string[] { nameof(MedecinSpecialiste) }));
            }
        }

        private void AjouterValidationDateConsultationSpecialiste(List<ValidationResult> result)
        {
            if (MedecinSpecialiste == "O") result.AddRange(InfosMedecinSpecialiste.Validate(nameof(InfosMedecinSpecialiste)));
        }

        private void AjouterValidationHospitalise(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(Hospitalise))
            {
                result.Add(new ValidationResult("", new string[] { nameof(Hospitalise) }));
            }
        }

        private void AjouterValidationDate_Hospitalise(List<ValidationResult> result)
        {
            if (Hospitalise == "O")
            {
                result.AddRange(PeriodeHospitalise.Validate(nameof(PeriodeHospitalise), true));
            }
        }

        private void AjouterValidationMedicamentsPris(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(MedicamentsPris))
            {
                result.Add(new ValidationResult("", new string[] { nameof(MedicamentsPris) }));
            }
        }

        private void AjouterValidationListeMedicamentsPris(List<ValidationResult> result)
        {
            if (MedicamentsPris == "O")
            {
                if (string.IsNullOrEmpty(ListeMedicamentsPris))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(ListeMedicamentsPris) }));
                }
            }
        }

        private void AjouterValidationTherapieSuivie(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(TherapieSuivie))
            {
                result.Add(new ValidationResult("", new string[] { nameof(TherapieSuivie) }));
            }
        }

        private void AjouterValidationListeTherapieSuivie(List<ValidationResult> result)
        {
            if (TherapieSuivie == "O")
            {
                if (!Acupuncture && !Chiropratique && !Ergotherapie && !Physiotherapie && !Psychotherapie && !Therapie_Autre)
                {
                    result.Add(new ValidationResult("", new string[] { nameof(Acupuncture) }));
                }
            }
        }

        private void AjouterValidationMaladieSimilaire(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(MaladieSimilaire))
            {
                result.Add(new ValidationResult("", new string[] { nameof(MaladieSimilaire) }));
            }
        }

        private void AjouterValidationMaladieSimilaireAnnee(List<ValidationResult> result)
        {
            if (MaladieSimilaire == "O")
            {
                result.AddRange(MaladieSimilaire_Annee.Validate(nameof(MaladieSimilaire_Annee), true));
            }
        }

        private void AjouterValidationMaladieSimilaireConsulte(List<ValidationResult> result)
        {
            if (MaladieSimilaire == "O")
            {
                if (string.IsNullOrEmpty(MaladieSimilaire_Consulte))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(MaladieSimilaire_Consulte) }));
                }
            }
        }

        private void AjouterValidation5DernieresAnnees(List<ValidationResult> result)
        {
            if (string.IsNullOrWhiteSpace(HasDiagnosisInLastFiveYears))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(HasDiagnosisInLastFiveYears) }));
            }
        }

        private void AjouterValidationBlocs5DernieresAnnees(List<ValidationResult> result)
        {
            if (HasDiagnosisInLastFiveYears == "O")
            {
                for (var i = 0; i <= MedicalConsultations.Count - 1; i++)
                {
                    result.AddRange(MedicalConsultations[i].Validate($"{nameof(MedicalConsultations)}[{i}]"));
                }
            }
        }

    }
    [Serializable]
    public class InsuredDisease : IValidatableObject
    {
        public DateModel DatePremierSymptomes { get; set; }
        public bool Symptomes_Depression { get; set; }
        public bool Symptomes_Position { get; set; }
        public bool Symptomes_Concentration { get; set; }
        public bool Symptomes_Douleurs { get; set; }
        public bool Symptomes_Fatigue { get; set; }
        public bool Symptomes_Negatives { get; set; }
        public bool Symptomes_Sommeil { get; set; }
        public bool Symptomes_Memoire { get; set; }
        public bool Symptomes_Autre { get; set; }
        public string Symptomes_AutrePrecision { get; set; }
        public SliderModel IntensiteSymptomes { get; set; }
        public string BesoinAide { get; set; }
        public bool BesoinAide_Transport { get; set; }
        public bool BesoinAide_Entretien { get; set; }
        public bool BesoinAide_Courses { get; set; }
        public bool BesoinAide_GererArgent { get; set; }
        public bool BesoinAide_PreparerRepas { get; set; }
        public bool BesoinAide_Escalier { get; set; }
        public bool BesoinAide_Laver { get; set; }
        public bool BesoinAide_Autre { get; set; }
        public string BesoinAide_AutrePrecision { get; set; }
        public string MedecinSpecialiste { get; set; }
        public string Hospitalise { get; set; }
        public string Etablissement_Hospitalise { get; set; }
        public string Ville_Hospitalise { get; set; }
        public PeriodModel PeriodeHospitalise { get; set; }
        public string MedicamentsPris { get; set; }
        public string ListeMedicamentsPris { get; set; }
        public string TherapieSuivie { get; set; }
        public bool Acupuncture { get; set; }
        public bool Chiropratique { get; set; }
        public bool Ergotherapie { get; set; }
        public bool Physiotherapie { get; set; }
        public bool Psychotherapie { get; set; }
        public bool Therapie_Autre { get; set; }
        public string Therapie_AutrePrecision { get; set; }
        public string MaladieSimilaire { get; set; }
        public YearModel MaladieSimilaire_Annee { get; set; }
        public string MaladieSimilaire_Consulte { get; set; }
        public string HasDiagnosisInLastFiveYears { get; set; }
        public List<MedicalConsultationModel> MedicalConsultations { get; set; }
        public List<PhysicianModel> ListeMedecin { get; set; }
        public SpecialistPhysicianModel InfosMedecinSpecialiste { get; set; }
        public PhysicianModel InfosMedecinMaladieSimilaire { get; set; }

        public InsuredDisease()
        {
            MaladieSimilaire_Annee = new YearModel();

            ListeMedecin = new List<PhysicianModel> { new PhysicianModel { DisplayFirstConsultationDate = true, IsListItem = true } };

            PeriodeHospitalise = new PeriodModel(DateModel.CreateLastFiveYearsDateModel(), DateModel.CreateLastFiveYearsDateModel());
            PeriodeHospitalise.DeleteRequired = false;

            DatePremierSymptomes = DateModel.CreatePastDateModel();

            List<SliderBracket> brackets = new List<SliderBracket>()
            {
                new SliderBracket() { BracketResourceManager = UIAccident.ResourceManager, BracketNameResource = nameof(UIAccident.IU39_3_1), MinValue = 0, MaxValue = 4 },
                new SliderBracket() { BracketResourceManager = UIAccident.ResourceManager, BracketNameResource = nameof(UIAccident.IU39_3_2), MinValue = 5, MaxValue = 7 },
                new SliderBracket() { BracketResourceManager = UIAccident.ResourceManager, BracketNameResource = nameof(UIAccident.IU39_3_3), MinValue = 8, MaxValue = 10 }
            };

            IntensiteSymptomes = new SliderModel()
            {
                Name = nameof(IntensiteSymptomes),
                MaxValue = 10,
                MinValue = 0,
                StepValue = 1,
                Value = "0",
                BracketResourceManager = UIAccident.ResourceManager,
                BracketTitleResource = nameof(UIAccident.IU39_3),
                Brackets = brackets
            };

            InfosMedecinSpecialiste = new SpecialistPhysicianModel(true);
            InfosMedecinMaladieSimilaire = new PhysicianModel();

            MedicalConsultations = new List<MedicalConsultationModel> { new MedicalConsultationModel(true) };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            result.AddRange(IntensiteSymptomes.Validate(nameof(IntensiteSymptomes), true));

            AjouterValidationDatePremierSymptomes(result);
            AjouterValidationListeSymptomes(result);
            AjouterValidationBesoinAide(result);
            AjouterValidationMedecinSpecialiste(result);
            AjouterValidationDateConsultationSpecialiste(result);
            AjouterValidationHospitalise(result);
            AjouterValidationDate_Hospitalise(result);
            AjouterValidationMedicamentsPris(result);
            AjouterValidationListeMedicamentsPris(result);
            AjouterValidationTherapieSuivie(result);
            AjouterValidationListeTherapieSuivie(result);
            AjouterValidationMaladieSimilaire(result);
            AjouterValidationMaladieSimilaireAnnee(result);
            AjouterValidationMaladieSimilaireConsulte(result);
            AjouterValidation5DernieresAnnees(result);
            AjouterValidationBlocs5DernieresAnnees(result);
            AjouterValidationListeMedecins(result);
            return result;
        }

        private void AjouterValidationListeMedecins(List<ValidationResult> result)
        {
            for (var i = 0; i <= ListeMedecin.Count - 1; i++)
            {
                result.AddRange(ListeMedecin[i].Validate($"{nameof(ListeMedecin)}[{i}]"));
            }
        }

        private void AjouterValidationDatePremierSymptomes(List<ValidationResult> result)
        {
            result.AddRange(DatePremierSymptomes.ValidatePastDate(nameof(DatePremierSymptomes), true));
        }

        private void AjouterValidationListeSymptomes(List<ValidationResult> result)
        {
            if (!Symptomes_Depression && !Symptomes_Position && !Symptomes_Concentration && !Symptomes_Douleurs && !Symptomes_Fatigue &&
                !Symptomes_Negatives && !Symptomes_Sommeil && !Symptomes_Memoire && Symptomes_AutrePrecision == null)
            {
                result.Add(new ValidationResult("", new string[] { "Symptomes_Depression" }));
            }
        }

        private void AjouterValidationBesoinAide(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(BesoinAide))
            {
                result.Add(new ValidationResult("", new string[] { "BesoinAide" }));
            }
        }

        private void AjouterValidationListeBesoinAide(List<ValidationResult> result)
        {
            if (BesoinAide == "O")
            {
                if (!BesoinAide_Transport && !BesoinAide_Entretien && !BesoinAide_Courses && !BesoinAide_GererArgent && !BesoinAide_PreparerRepas &&
                    !BesoinAide_Escalier && !BesoinAide_Laver && !BesoinAide_Autre)
                {
                    result.Add(new ValidationResult("", new string[] { "Symptomes_Depression" }));
                }
            }
        }

        private void AjouterValidationMedecinSpecialiste(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(MedecinSpecialiste))
            {
                result.Add(new ValidationResult("", new string[] { "MedecinSpecialiste" }));
            }
        }

        private void AjouterValidationDateConsultationSpecialiste(List<ValidationResult> result)
        {
            if (MedecinSpecialiste == "O") result.AddRange(InfosMedecinSpecialiste.Validate(nameof(InfosMedecinSpecialiste)));
        }

        private void AjouterValidationHospitalise(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(Hospitalise))
            {
                result.Add(new ValidationResult("", new string[] { "Hospitalise" }));
            }
        }

        private void AjouterValidationDate_Hospitalise(List<ValidationResult> result)
        {
            if (Hospitalise == "O")
            {
                result.AddRange(PeriodeHospitalise.Validate(nameof(PeriodeHospitalise), true));
            }
        }

        private void AjouterValidationMedicamentsPris(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(MedicamentsPris))
            {
                result.Add(new ValidationResult("", new string[] { "MedicamentsPris" }));
            }
        }

        private void AjouterValidationListeMedicamentsPris(List<ValidationResult> result)
        {
            if (MedicamentsPris == "O")
            {
                if (string.IsNullOrEmpty(ListeMedicamentsPris))
                {
                    result.Add(new ValidationResult("", new string[] { "ListeMedicamentsPris" }));
                }
            }
        }

        private void AjouterValidationTherapieSuivie(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(TherapieSuivie))
            {
                result.Add(new ValidationResult("", new string[] { "TherapieSuivie" }));
            }
        }

        private void AjouterValidationListeTherapieSuivie(List<ValidationResult> result)
        {
            if (TherapieSuivie == "O")
            {
                if (!Acupuncture && !Chiropratique && !Ergotherapie && !Physiotherapie && !Psychotherapie && !Therapie_Autre)
                {
                    result.Add(new ValidationResult("", new string[] { "Acupuncture" }));
                }
            }
        }

        private void AjouterValidationMaladieSimilaire(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(MaladieSimilaire))
            {
                result.Add(new ValidationResult("", new string[] { "MaladieSimilaire" }));
            }
        }

        private void AjouterValidationMaladieSimilaireAnnee(List<ValidationResult> result)
        {
            if (MaladieSimilaire == "O")
            {
                result.AddRange(MaladieSimilaire_Annee.Validate(nameof(MaladieSimilaire_Annee), true));
            }
        }

        private void AjouterValidationMaladieSimilaireConsulte(List<ValidationResult> result)
        {
            if (MaladieSimilaire == "O")
            {
                if (string.IsNullOrEmpty(MaladieSimilaire_Consulte))
                {
                    result.Add(new ValidationResult("", new string[] { "MaladieSimilaire_Consulte" }));
                }
            }
        }

        private void AjouterValidation5DernieresAnnees(List<ValidationResult> result)
        {
            if (string.IsNullOrWhiteSpace(HasDiagnosisInLastFiveYears))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(HasDiagnosisInLastFiveYears) }));
            }
        }

        private void AjouterValidationBlocs5DernieresAnnees(List<ValidationResult> result)
        {
            if (HasDiagnosisInLastFiveYears == "O")
            {
                for (var i = 0; i <= MedicalConsultations.Count - 1; i++)
                {
                    result.AddRange(MedicalConsultations[i].Validate($"{nameof(MedicalConsultations)}[{i}]"));
                }
            }
        }

    }


    [Serializable]
    public class InsuredInvalidity : IValidatableObject
    {
        public const string CAUSE_INVALIDITE_MALADIE = "M";
        public const string CAUSE_INVALIDITE_ACCIDENT = "A";

        public DateModel DateDebutInvalidite { get; set; }
        public string CauseInvalidite { get; set; }
        public string RetourTravail { get; set; }
        public string RetourTravailConvenu { get; set; }
        public DateModel DateRetourTravailConvenu { get; set; }
        public string TypeRetourTravail { get; set; }
        public PeriodModel PeriodeRetourTravail { get; set; }
        public string TravailRemunere { get; set; }
        public PeriodModel PeriodeTravailRemunere { get; set; }
        public string RetourEtudes { get; set; }
        public string NomProgrammeEtudes { get; set; }
        public string NombreHeuresSemainesEtudes { get; set; }
        public PeriodModel PeriodeProgrammeEtudes { get; set; }
        public string FormationProfessionnelle { get; set; }
        public string NomProgrammeProfessionnel { get; set; }
        public PeriodModel PeriodeProgrammeProfessionnel { get; set; }

        public InsuredInvalidity()
        {

            DateDebutInvalidite = DateModel.CreateLastFiveYearsDateModel();
            DateRetourTravailConvenu = DateModel.CreateFutureDateModel();

            PeriodeRetourTravail = new PeriodModel(DateModel.CreateLastFiveYearsDateModel(), DateModel.CreateNeighboringDateModel());
            PeriodeRetourTravail.DateToRequired = false;
            PeriodeRetourTravail.DeleteRequired = false;

            PeriodeTravailRemunere = new PeriodModel(DateModel.CreateLastFiveYearsDateModel(), DateModel.CreateNeighboringDateModel());
            PeriodeTravailRemunere.DateToRequired = false;
            PeriodeTravailRemunere.DeleteRequired = false;

            PeriodeProgrammeEtudes = new PeriodModel(DateModel.CreateNeighboringDateModel(), DateModel.CreateNeighboringDateModel());
            PeriodeProgrammeEtudes.DateToRequired = false;
            PeriodeProgrammeEtudes.DeleteRequired = false;

            PeriodeProgrammeProfessionnel = new PeriodModel(DateModel.CreateNeighboringDateModel(), DateModel.CreateNeighboringDateModel());
            PeriodeProgrammeProfessionnel.DateToRequired = false;
            PeriodeProgrammeProfessionnel.DeleteRequired = false;

        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            AjouterValidationDateDebutInvalidite(result);
            AjouterValidationCauseInvalidite(result);
            AjouterValidationRetourTravail(result);
            AjouterValidationTypeRetourTravail(result);
            AjouterValidationDateRetourTravail(result);
            AjouterValidationRetourTravailConvenu(result);
            AjouterValidationDateRetourTravailConvenu(result);
            AjouterValidationTravailRemunere(result);
            AjouterValidationDateTravailRemunere(result);
            AjouterValidationRetourEtudes(result);
            AjouterValidationNomProgrammeEtudes(result);
            AjouterValidationNombreHeuresSemainesEtudes(result);
            AjouterValidationDateProgrammeEtudes(result);
            AjouterValidationFormationProfessionnelle(result);
            AjouterValidationNomProgrammeProfessionnel(result);
            AjouterValidationDateProgrammeProfessionnel(result);

            return result;
        }

        private void AjouterValidationDateDebutInvalidite(List<ValidationResult> result)
        {
            result.AddRange(DateDebutInvalidite.ValidatePastDate(nameof(DateDebutInvalidite), true));
        }

        private void AjouterValidationCauseInvalidite(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(CauseInvalidite))
            {
                result.Add(new ValidationResult("", new string[] { nameof(CauseInvalidite) }));
            }
        }

        private void AjouterValidationRetourTravail(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(RetourTravail))
            {
                result.Add(new ValidationResult("", new string[] { nameof(RetourTravail) }));
            }
        }

        private void AjouterValidationTypeRetourTravail(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(RetourTravail) && RetourTravail == "O")
            {
                if (string.IsNullOrEmpty(TypeRetourTravail))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(TypeRetourTravail) }));
                }
            }
        }

        private void AjouterValidationDateRetourTravail(List<ValidationResult> result)
        {
            if (RetourTravail == "O")
            {
                result.AddRange(PeriodeRetourTravail.Validate(nameof(PeriodeRetourTravail), true));
            }
        }

        private void AjouterValidationRetourTravailConvenu(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(RetourTravail) && RetourTravail == "N")
            {
                if (string.IsNullOrEmpty(RetourTravailConvenu))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(RetourTravailConvenu) }));
                }
            }
        }

        private void AjouterValidationDateRetourTravailConvenu(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(RetourTravail) && RetourTravail == "N")
            {
                if (RetourTravailConvenu == "O")
                {
                    result.AddRange(DateRetourTravailConvenu.ValidateFutureDate(nameof(DateRetourTravailConvenu), true));
                }
            }
        }

        private void AjouterValidationTravailRemunere(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(TravailRemunere))
            {
                result.Add(new ValidationResult("", new string[] { nameof(TravailRemunere) }));
            }
        }

        private void AjouterValidationDateTravailRemunere(List<ValidationResult> result)
        {
            if (TravailRemunere == "O")
            {
                result.AddRange(PeriodeTravailRemunere.Validate(nameof(PeriodeTravailRemunere), true));
            }
        }

        private void AjouterValidationRetourEtudes(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(RetourEtudes))
            {
                result.Add(new ValidationResult("", new string[] { nameof(RetourEtudes) }));
            }
        }

        private void AjouterValidationNomProgrammeEtudes(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(RetourEtudes) && RetourEtudes == "O")
            {
                if (string.IsNullOrEmpty(NomProgrammeEtudes))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(NomProgrammeEtudes) }));
                }
            }
        }

        private void AjouterValidationNombreHeuresSemainesEtudes(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(RetourEtudes) && RetourEtudes == "O")
            {
                if (string.IsNullOrEmpty(NombreHeuresSemainesEtudes))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(NombreHeuresSemainesEtudes) }));
                }
                else
                {
                    int nombre;
                    if (!(int.TryParse(NombreHeuresSemainesEtudes, out nombre)))
                    {
                        result.Add(new ValidationResult("", new string[] { nameof(NombreHeuresSemainesEtudes) }));
                    }
                    else if (!(Int32.Parse(NombreHeuresSemainesEtudes) > 0 && Int32.Parse(NombreHeuresSemainesEtudes) < 100))
                    {
                        result.Add(new ValidationResult("", new string[] { nameof(NombreHeuresSemainesEtudes) }));
                    }
                }
            }
        }

        private void AjouterValidationDateProgrammeEtudes(List<ValidationResult> result)
        {
            if (RetourEtudes == "O")
            {
                result.AddRange(PeriodeProgrammeEtudes.Validate(nameof(PeriodeProgrammeEtudes), true));
            }
        }

        private void AjouterValidationFormationProfessionnelle(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(FormationProfessionnelle))
            {
                result.Add(new ValidationResult("", new string[] { nameof(FormationProfessionnelle) }));
            }
        }

        private void AjouterValidationNomProgrammeProfessionnel(List<ValidationResult> result)
        {
            if (!string.IsNullOrEmpty(FormationProfessionnelle) && FormationProfessionnelle == "O")
            {
                if (string.IsNullOrEmpty(NomProgrammeProfessionnel))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(NomProgrammeProfessionnel) }));
                }
            }
        }

        private void AjouterValidationDateProgrammeProfessionnel(List<ValidationResult> result)
        {
            if (FormationProfessionnelle == "O")
            {
                result.AddRange(PeriodeProgrammeProfessionnel.Validate(nameof(PeriodeProgrammeProfessionnel), true));
            }
        }

    }

    [Serializable]
    public class InsuredEmployment : IValidatableObject
    {
        public string StatutEmployment { get; set; }
        public string NomEntreprise { get; set; }
        public AddressModel AdresseEntreprise { get; set; }
        public string EmploiEntreprise { get; set; }
        public DateModel DateFinEmploi { get; set; }
        public DateModel DateFinTravailAutonome { get; set; }
        public string Raison { get; set; }
        public MoneyModel RevenuAnnuelSalarie { get; set; }
        public string TachesSalarie { get; set; }
        public MoneyModel RevenuAnnuelAutonome { get; set; }
        public string TachesAutonome { get; set; }
        public SliderModel PourcentagePhysique { get; set; }
        public SliderModel PourcentagePhysiqueAutonome { get; set; }
        public string EmploiAutonome { get; set; }
        public string DebutPeriodeTravail { get; set; }
        public string FinPeriodeTravail { get; set; }

        public string AutreSpecification { get; set; }

        public InsuredEmployment()
        {
            RevenuAnnuelSalarie = new MoneyModel() { Min = 0, Max = 9999999999, MaxLength = 10, AcceptDecimals = true };
            RevenuAnnuelAutonome = new MoneyModel() { Min = 0, Max = 9999999999, MaxLength = 10, AcceptDecimals = true };

            DateFinEmploi = DateModel.CreateLastFiveYearsDateModel();
            DateFinTravailAutonome = DateModel.CreateLastFiveYearsDateModel();

            AdresseEntreprise = new AddressModel(false);

            PourcentagePhysique = new SliderModel()
            {
                Name = nameof(PourcentagePhysique),
                StepValue = 1,
                MaxValue = 100,
                MinValue = 0,
                Value = "0"
            };

            PourcentagePhysiqueAutonome = new SliderModel()
            {
                Name = nameof(PourcentagePhysiqueAutonome),
                StepValue = 1,
                MaxValue = 100,
                MinValue = 0,
                Value = "0"
            };
        }

        public string ObtenirMois(int NumeroMois)
        {
            string Mois = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(NumeroMois);
            return Mois;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            if (StatutEmployment == "S")
            {
                result.AddRange(PourcentagePhysique.Validate(nameof(PourcentagePhysique), true));
                result.AddRange(AdresseEntreprise.Validate(nameof(AdresseEntreprise), true));
            }

            if (StatutEmployment == "A")
            {
                result.AddRange(PourcentagePhysiqueAutonome.Validate(nameof(PourcentagePhysiqueAutonome), true));
            }

            AjouterValidationNomEntreprise(result);
            AjouterValidationStatutEmployment(result);
            AjouterValidationEmploiEntreprise(result);
            AjouterValidationDateFinEmploiEntr(result);
            AjouterValidationRevenuAnnuelEmploiEntr(result);
            AjouterValidationEmploiAutonome(result);
            AjouterValidationDateFinTravailAutonome(result);
            AjouterValidationRevenuAnnuelEmploiAuto(result);
            AjouterValidationPeriodeTravail(result);
            AjouterValidationAutreSpecification(result);

            return result;
        }

        private void AjouterValidationAutreSpecification(List<ValidationResult> result)
        {
            if (StatutEmployment == "O")
            {
                if (string.IsNullOrEmpty(AutreSpecification))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(AutreSpecification) }));
                }
            }
        }

        private void AjouterValidationNomEntreprise(List<ValidationResult> result)
        {
            if (StatutEmployment == "S")
            {
                if (string.IsNullOrEmpty(NomEntreprise))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(NomEntreprise) }));
                }
            }
        }

        private void AjouterValidationStatutEmployment(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(StatutEmployment))
            {
                result.Add(new ValidationResult("", new string[] { nameof(StatutEmployment) }));
            }
        }

        private void AjouterValidationEmploiEntreprise(List<ValidationResult> result)
        {
            if (StatutEmployment == "S")
            {
                if (string.IsNullOrEmpty(EmploiEntreprise))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(EmploiEntreprise) }));
                }
            }
        }

        private void AjouterValidationDateFinEmploiEntr(List<ValidationResult> result)
        {
            if (StatutEmployment == "S" && EmploiEntreprise == "N")
            {
                result.AddRange(DateFinEmploi.ValidatePastDate(nameof(DateFinEmploi), true));
            }
        }

        private void AjouterValidationRevenuAnnuelEmploiEntr(List<ValidationResult> result)
        {
            if (StatutEmployment == "S")
            {
                result.AddRange(RevenuAnnuelSalarie.Validate(nameof(RevenuAnnuelSalarie), true));
            }
        }

        private void AjouterValidationEmploiAutonome(List<ValidationResult> result)
        {
            if (StatutEmployment == "A")
            {
                if (string.IsNullOrEmpty(EmploiAutonome))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(EmploiAutonome) }));
                }
            }
        }

        private void AjouterValidationDateFinTravailAutonome(List<ValidationResult> result)
        {
            if (StatutEmployment == "A" && EmploiAutonome == "N")
            {
                result.AddRange(DateFinTravailAutonome.ValidatePastDate(nameof(DateFinTravailAutonome), true));
            }
        }

        private void AjouterValidationRevenuAnnuelEmploiAuto(List<ValidationResult> result)
        {
            if (StatutEmployment == "A")
            {
                result.AddRange(RevenuAnnuelAutonome.Validate(nameof(RevenuAnnuelAutonome), true));
            }
        }

        private void AjouterValidationPeriodeTravail(List<ValidationResult> result)
        {
            if (StatutEmployment == "T")
            {
                int debut;
                int fin;
                if ((!(int.TryParse(DebutPeriodeTravail, out debut))) || (!(int.TryParse(FinPeriodeTravail, out fin))))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(DebutPeriodeTravail) }));
                }
            }
        }

    }

    [Serializable]
    public class InsuredSummary : IValidatableObject
    {
        public bool Confirmation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            AjouterValidationConfirmation(result);

            return result;
        }

        private void AjouterValidationConfirmation(List<ValidationResult> result)
        {
            if (!Confirmation)
            {
                result.Add(new ValidationResult("", new string[] { nameof(Confirmation) }));
            }
        }
    }
}

