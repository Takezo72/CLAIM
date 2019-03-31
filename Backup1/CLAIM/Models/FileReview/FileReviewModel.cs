using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CLAIM.Models.Shared;
using CLAIM.Helpers.Configuration;
using CLAIM.Ressources.FormTexts;

namespace CLAIM.Models.FileReview
{
    [Serializable]
    public class FileReviewModel
    {
        public ReviewInsured ReviewInsured { get; set; }
        public ReviewInvalidity ReviewInvalidity { get; set; }
        public ReviewEmployment ReviewEmployment { get; set; }
        public InsuredProceeds ReviewProceeds { get; set; }

        public FileReviewModel()
        {
            Transmis = false;
            ReviewInsured = new ReviewInsured();
            ReviewInvalidity = new ReviewInvalidity();
            ReviewEmployment = new ReviewEmployment();
            ReviewProceeds = new InsuredProceeds();
        }

        public bool Transmis { get; set; }

        internal bool IsTransmitted()
        {
            return Transmis;
        }

        public string PreviousStep
        {
            get { return "BeneficiaryDeceaseClaim"; }
            private set { }
        }

        public string NextStep
        {
            get { return "Confirmation"; }
            private set { }
        }

        public ButtonListModel NavigationButtons
        {
            get
            {

                ConfigurationHelper config = new ConfigurationHelper();
                return new ButtonListModel { Cancel = true, PreviousAction = true, Transmit = true, ReturnUrl = config.IACAReturnUrl };
            }
            private set { }
        }

    }

    [Serializable]
    public class ReviewInvalidity : IValidatableObject
    {
        public string SuiviNouveauMedecin { get; set; }
        public string EvaluationMedecinSpecialise { get; set; }
        public string EtreEvalue { get; set; }
        public string EtreHospitalise { get; set; }
        public PeriodModel PeriodeHospitalise { get; set; }
        public string Etablissement_Hospitalise { get; set; }
        public string Ville_Etablissement_Hospitalise { get; set; }
        public string Raison_Hospitalise { get; set; }
        public string SubirChirurgie { get; set; }
        public string TypeChirurgie { get; set; }
        public DateModel DatePrevue_Chirurgie { get; set; }
        public string ChangerMedication { get; set; }
        public bool NouveauMedicament { get; set; }
        public bool ChangementPosologie { get; set; }
        public bool ArretMedicament { get; set; }
        public bool AutrePosologie { get; set; }
        public List<MedicationModel> NewMedications { get; set; }
        public List<MedicationModel> ChangeMedications { get; set; }
        public List<MedicationModel> StopMedications { get; set; }
        public string AnciennePosologie { get; set; }
        public string NouvellePosologie { get; set; }
        public string Posologie_AutrePrecision { get; set; }
        public string DebuterTherapie { get; set; }
        public bool SuiviTherapie { get; set; }
        public bool DebutTherapie { get; set; }
        public TherapyModel TherapieSuivie { get; set; } = new TherapyModel();
        public TherapyModel TherapieDebutee { get; set; } = new TherapyModel();
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
        public PhysicianModel InfosNouveauMedecin { get; set; }
        public SpecialistPhysicianModel InfosMedecinSpecialiste { get; set; }

        public ReviewInvalidity()
        {
            PeriodeHospitalise = new PeriodModel(DateModel.CreateNeighboringDateModel(), DateModel.CreateNeighboringDateModel());
            PeriodeHospitalise.DeleteRequired = false;

            DatePrevue_Chirurgie = DateModel.CreateNeighboringDateModel();

            NewMedications = new List<MedicationModel> { new MedicationModel() };
            NewMedications[0].Index = 0;

            ChangeMedications = new List<MedicationModel> { new MedicationModel() };
            ChangeMedications[0].Index = 0;

            StopMedications = new List<MedicationModel> { new MedicationModel() };
            StopMedications[0].Index = 0;

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

            InfosNouveauMedecin = new PhysicianModel(true);
            InfosMedecinSpecialiste = new SpecialistPhysicianModel();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            AjouterValidationSuiviNouveauMedecin(result);
            AjouterValidationNouveauMedecin(result);
            AjouterValidationEvaluationMedecinSpecialise(result);
            AjouterValidationMedecinSpecialiste(result);
            AjouterValidationEtreEvalue(result);
            AjouterValidationEtreHospitalise(result);
            AjouterValidationEtablissementHospitalise(result);
            AjouterValidationVilleEtablissementHospitalise(result);
            AjouterValidationPeriodeHospitalise(result);
            AjouterValidationSubirChirurgie(result);
            AjouterValidationDatePrevue_Chirurgie(result);
            AjouterValidationChangerMedication(result);
            AjouterValidationCommentChangerMedication(result);
            AjouterValidationDebuterTherapie(result);
            AjouterValidationStatutTherapie(result);
            AjouterValidationTherapieDebutee(result);
            AjouterValidationTherapieSuivie(result);
            AjouterValidationSymptomes(result);
            AjouterValidationBesoinAide(result);
            AjouterValidationTypeBesoinAide(result);

            return result;
        }

        private void AjouterValidationTherapieDebutee(List<ValidationResult> result)
        {
            if (DebutTherapie)
            {
                result.AddRange(TherapieDebutee.Validate(nameof(TherapieDebutee), true));
            }
        }

        private void AjouterValidationTherapieSuivie(List<ValidationResult> result)
        {
            if (SuiviTherapie)
            {
                result.AddRange(TherapieSuivie.Validate(nameof(TherapieSuivie), true));
            }
        }

        private void AjouterValidationSuiviNouveauMedecin(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(SuiviNouveauMedecin))
            {
                result.Add(new ValidationResult("", new string[] { nameof(SuiviNouveauMedecin) }));
            }
        }

        private void AjouterValidationNouveauMedecin(List<ValidationResult> result)
        {
            if (SuiviNouveauMedecin == "N") result.AddRange(InfosNouveauMedecin.Validate(nameof(InfosNouveauMedecin)));
        }


        private void AjouterValidationEvaluationMedecinSpecialise(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(EvaluationMedecinSpecialise))
            {
                result.Add(new ValidationResult("", new string[] { nameof(EvaluationMedecinSpecialise) }));
            }
        }

        private void AjouterValidationMedecinSpecialiste(List<ValidationResult> result)
        {
            if (EvaluationMedecinSpecialise == "O") result.AddRange(InfosMedecinSpecialiste.Validate(nameof(InfosMedecinSpecialiste)));
        }

        private void AjouterValidationEtreEvalue(List<ValidationResult> result)
        {
            if (EvaluationMedecinSpecialise == "O")
            {
                if (string.IsNullOrEmpty(EtreEvalue))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(EtreEvalue) }));
                }
            }
        }

        private void AjouterValidationEtreHospitalise(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(EtreHospitalise))
            {
                result.Add(new ValidationResult("", new string[] { nameof(EtreHospitalise) }));
            }
        }

        private void AjouterValidationEtablissementHospitalise(List<ValidationResult> result)
        {
            if (EtreHospitalise == "O")
            {
                if (string.IsNullOrEmpty(Etablissement_Hospitalise))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(Etablissement_Hospitalise) }));
                }
            }
        }
        private void AjouterValidationVilleEtablissementHospitalise(List<ValidationResult> result)
        {
            if (EtreHospitalise == "O")
            {
                if (string.IsNullOrEmpty(Ville_Etablissement_Hospitalise))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(Ville_Etablissement_Hospitalise) }));
                }
            }
        }

        private void AjouterValidationPeriodeHospitalise(List<ValidationResult> result)
        {
            if (EtreHospitalise == "O")
            {
                result.AddRange(PeriodeHospitalise.Validate(nameof(PeriodeHospitalise), true));
            }
        }

        private void AjouterValidationSubirChirurgie(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(SubirChirurgie))
            {
                result.Add(new ValidationResult("", new string[] { nameof(SubirChirurgie) }));
            }
        }

        private void AjouterValidationDatePrevue_Chirurgie(List<ValidationResult> result)
        {
            if (SubirChirurgie == "O")
            {
                result.AddRange(DatePrevue_Chirurgie.ValidatePastDate(nameof(DatePrevue_Chirurgie), true));
            }

            if (SubirChirurgie == "A")
            {
                result.AddRange(DatePrevue_Chirurgie.ValidateFutureDate(nameof(DatePrevue_Chirurgie), false));
            }
        }

        private void AjouterValidationTypeChirurgie(List<ValidationResult> result)
        {
            if (SubirChirurgie == "O" || SubirChirurgie == "A")
            {
                if (string.IsNullOrEmpty(TypeChirurgie))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(TypeChirurgie) }));
                }
            }
        }

        private void AjouterValidationChangerMedication(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(ChangerMedication))
            {
                result.Add(new ValidationResult("", new string[] { nameof(ChangerMedication) }));
            }
        }

        private void AjouterValidationCommentChangerMedication(List<ValidationResult> result)
        {
            if (ChangerMedication == "O")
            {
                if (!NouveauMedicament && !ChangementPosologie && !ArretMedicament && !AutrePosologie)
                {
                    result.Add(new ValidationResult("", new string[] { "CommentChangerMedication" }));
                }
            }
        }

        private void AjouterValidationNomMedicament(List<ValidationResult> result)
        {
            if (ChangerMedication == "O")
            {
                if (ChangementPosologie)
                {
                    if (string.IsNullOrEmpty(AnciennePosologie))
                    {
                        result.Add(new ValidationResult("", new string[] { nameof(AnciennePosologie) }));
                    }
                    if (string.IsNullOrEmpty(NouvellePosologie))
                    {
                        result.Add(new ValidationResult("", new string[] { nameof(NouvellePosologie) }));
                    }
                }
            }
        }

        private void AjouterValidationDebuterTherapie(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(DebuterTherapie))
            {
                result.Add(new ValidationResult("", new string[] { nameof(DebuterTherapie) }));
            }
        }

        private void AjouterValidationStatutTherapie(List<ValidationResult> result)
        {
            if (DebuterTherapie == "O")
            {
                if (!(SuiviTherapie || DebutTherapie))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(SuiviTherapie) }));
                }
            }
        }

        private void AjouterValidationSymptomes(List<ValidationResult> result)
        {
            if (!(Symptomes_Depression) && !(Symptomes_Position) && !(Symptomes_Concentration) && !(Symptomes_Douleurs) &&
                !(Symptomes_Fatigue) && !(Symptomes_Negatives) && !(Symptomes_Sommeil) && !(Symptomes_Memoire) &&
                !(Symptomes_Autre))
            {
                result.Add(new ValidationResult("", new string[] { nameof(Symptomes_Depression) }));
            }
        }

        private void AjouterValidationBesoinAide(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(BesoinAide))
            {
                result.Add(new ValidationResult("", new string[] { nameof(BesoinAide) }));
            }
        }

        private void AjouterValidationTypeBesoinAide(List<ValidationResult> result)
        {
            if (BesoinAide == "O")
            {
                if (!(BesoinAide_Transport) && !(BesoinAide_Entretien) && !(BesoinAide_Courses) &&
                    !(BesoinAide_GererArgent) && !(BesoinAide_PreparerRepas) && !(BesoinAide_Escalier) &&
                    !(BesoinAide_Laver) && !(BesoinAide_Autre))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(BesoinAide_Transport) }));
                }
            }
        }


    }

    [Serializable]
    public class ReviewEmployment : IValidatableObject
    {
        public string RetourTravail { get; set; }
        public string TypeRetourTravail { get; set; }
        public PeriodModel PeriodeRetourTravail { get; set; }
        public string RetourTravailConvenu { get; set; }
        public DateModel DateRetourTravailConvenu { get; set; }
        public string TravailRemunere { get; set; }
        public PeriodModel PeriodeTravailRemunere { get; set; }
        public string RetourEtudes { get; set; }
        public string NomProgrammeEtudes { get; set; }
        public string NombreHeuresSemainesEtudes { get; set; }
        public PeriodModel PeriodeProgrammeEtudes { get; set; }
        public string FormationProfessionnelle { get; set; }
        public string NomProgrammeProfessionnel { get; set; }
        public PeriodModel PeriodeProgrammeProfessionnel { get; set; }

        public ReviewEmployment()
        {
            PeriodeRetourTravail = new PeriodModel(DateModel.CreateNeighboringDateModel(), DateModel.CreateNeighboringDateModel());
            PeriodeRetourTravail.DateToRequired = false;
            PeriodeRetourTravail.DeleteRequired = false;

            DateRetourTravailConvenu = DateModel.CreateFutureDateModel();

            PeriodeTravailRemunere = new PeriodModel(DateModel.CreateNeighboringDateModel(), DateModel.CreateNeighboringDateModel());
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

            AjouterValidationRetourTravail(result);
            AjouterValidatioTypeRetourTravail(result);
            AjouterValidationPeriodeRetourTravail(result);

            AjouterValidationRetourTravailConvenu(result);
            AjouterValidationDateRetourTravailConvenu(result);
            AjouterValidationTravailRemunere(result);
            AjouterValidationPeriodeTravailRemunere(result);
            AjouterValidationRetourEtudes(result);
            AjouterValidationNomProgrammeEtudes(result);
            AjouterValidationNombreHeuresSemainesEtudes(result);
            AjouterValidationPeriodeProgrammeEtudes(result);
            AjouterValidationFormationProfessionnelle(result);
            AjouterValidationNomProgrammeProfessionnel(result);
            AjouterValidationPeriodeProgrammeProfessionnel(result);


            return result;
        }

        private void AjouterValidationRetourTravail(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(RetourTravail))
            {
                result.Add(new ValidationResult("", new string[] { nameof(RetourTravail) }));
            }
        }

        private void AjouterValidatioTypeRetourTravail(List<ValidationResult> result)
        {
            if (RetourTravail == "O")
            {
                if (string.IsNullOrEmpty(TypeRetourTravail))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(TypeRetourTravail) }));
                }
            }
        }

        private void AjouterValidationPeriodeRetourTravail(List<ValidationResult> result)
        {
            if (RetourTravail == "O")
            {
                result.AddRange(PeriodeRetourTravail.Validate(nameof(PeriodeRetourTravail), true));
            }
        }

        private void AjouterValidationRetourTravailConvenu(List<ValidationResult> result)
        {
            if (RetourTravail == "N")
            {
                if (string.IsNullOrEmpty(RetourTravailConvenu))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(RetourTravailConvenu) }));
                }
            }
        }

        private void AjouterValidationDateRetourTravailConvenu(List<ValidationResult> result)
        {
            if ((RetourTravail == "N") && (RetourTravailConvenu == "O"))
            {
                result.AddRange(DateRetourTravailConvenu.ValidateFutureDate(nameof(DateRetourTravailConvenu), true));
            }
        }

        private void AjouterValidationTravailRemunere(List<ValidationResult> result)
        {
            if (string.IsNullOrEmpty(TravailRemunere))
            {
                result.Add(new ValidationResult("", new string[] { nameof(TravailRemunere) }));
            }
        }

        private void AjouterValidationPeriodeTravailRemunere(List<ValidationResult> result)
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
            if (RetourEtudes == "O")
            {
                if (string.IsNullOrEmpty(NomProgrammeEtudes))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(NomProgrammeEtudes) }));
                }
            }
        }

        private void AjouterValidationNombreHeuresSemainesEtudes(List<ValidationResult> result)
        {
            if (RetourEtudes == "O")
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

        private void AjouterValidationPeriodeProgrammeEtudes(List<ValidationResult> result)
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
            if (FormationProfessionnelle == "O")
            {
                if (string.IsNullOrEmpty(NomProgrammeProfessionnel))
                {
                    result.Add(new ValidationResult("", new string[] { nameof(NomProgrammeProfessionnel) }));
                }
            }
        }

        private void AjouterValidationPeriodeProgrammeProfessionnel(List<ValidationResult> result)
        {
            if (FormationProfessionnelle == "O")
            {
                result.AddRange(PeriodeProgrammeProfessionnel.Validate(nameof(PeriodeProgrammeProfessionnel), true));
            }
        }

    }

    [Serializable]
    public class ReviewSummary : IValidatableObject
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

