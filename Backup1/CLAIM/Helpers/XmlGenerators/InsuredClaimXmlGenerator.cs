using CLAIM.Models;
using CLAIM.Models.Shared;
using System.Xml;
using NLog;
using System.Globalization;
using System.Linq;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Helpers.XmlGenerators
{
    public class InsuredClaimXmlGenerator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal byte[] Generate(InsuredClaimModel claimData, string urlDepot)
        {
            XmlHelper helper = new XmlHelper("RG_INV_DECL_SUIT_V1", CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower(), claimData.InfosInsured.InfosResquestFollowUp.TrackingNumber, claimData.InfosInsured.InfosResquestFollowUp.EventId, claimData.InfosInsured.InfosResquestFollowUp.AttachmentId);

            GenerateXmlInsuredInfos(claimData, helper);
            GenerateXmlInvalidityInfos(claimData, helper);
            if (claimData.InfosInvalidity.CauseInvalidite == InsuredInvalidity.CAUSE_INVALIDITE_ACCIDENT)
            {
                GenerateXmlAccidentInfos(claimData, helper);
            }

            if (claimData.InfosInvalidity.CauseInvalidite == InsuredInvalidity.CAUSE_INVALIDITE_MALADIE)
            {
                GenerateXmlDiseaseInfos(claimData, helper);
            }

            GenerateXmlEmploymentInfos(claimData, helper);

            GenerateXmlProceedsInfos(claimData, helper);

            foreach (OtherInsuranceModel otherInsurance in claimData.InfosProceeds.ListOtherInsurance().Where(o => o != null))
            {
                otherInsurance.ToFileXmlElement(helper, urlDepot);
            }

            ConfigurationHelper config = new ConfigurationHelper();

            if (config.IsMockUserIdentity)
            {
                logger.Info(string.Format("MOCK USER ACTIVATED! XML saved to disk."));
                helper.Save("d:\\testInsuredClaim.xml");
            }

            return helper.Extract();
        }

        private void GenerateXmlEmploymentInfos(InsuredClaimModel claimData, XmlHelper helper)
        {
            helper.AddElement("Emploi", "Statut", claimData.InfosEmployment.StatutEmployment);
            helper.AddElement("Emploi", "NomEntreprise", claimData.InfosEmployment.NomEntreprise);

            helper.AddElement("Emploi", "ToujoursEmploye", (claimData.InfosEmployment.EmploiEntreprise == "O").ToString());

            helper.AddElement("Emploi", "RaisonFinEmploi", claimData.InfosEmployment.Raison);
            if (claimData.InfosEmployment.StatutEmployment == "S")
            {
                helper.AddElement("Emploi", helper.CreateElement(nameof(claimData.InfosEmployment.AdresseEntreprise), claimData.InfosEmployment.AdresseEntreprise.ToXmlElement(helper)));
                helper.AddElement("Emploi", "DateFinEmploi", helper.TransformerDate(claimData.InfosEmployment.DateFinEmploi));
                helper.AddElement("Emploi", "RevenuAnnuelBrut", claimData.InfosEmployment.RevenuAnnuelSalarie.GetAmount().ToString());
                helper.AddElement("Emploi", "Taches", claimData.InfosEmployment.TachesSalarie);
                helper.AddElement("Emploi", "PourcentagePhysique", claimData.InfosEmployment.PourcentagePhysique.Value);
            }
            else
            {
                helper.AddElement("Emploi", "DateFinEmploi", helper.TransformerDate(claimData.InfosEmployment.DateFinTravailAutonome));
                helper.AddElement("Emploi", "RevenuAnnuelBrut", claimData.InfosEmployment.RevenuAnnuelAutonome.GetAmount().ToString());
                helper.AddElement("Emploi", "Taches", claimData.InfosEmployment.TachesAutonome);
                helper.AddElement("Emploi", "PourcentagePhysique", claimData.InfosEmployment.PourcentagePhysiqueAutonome.Value);
            }

            if (claimData.InfosEmployment.StatutEmployment == "O")
            {
                helper.AddElement("Emploi", "AutreSpecification", claimData.InfosEmployment.AutreSpecification);
            }

            helper.AddElement("Emploi", "DebutPeriodeTravail", claimData.InfosEmployment.DebutPeriodeTravail);
            helper.AddElement("Emploi", "FinPeriodeTravail", claimData.InfosEmployment.FinPeriodeTravail);
        }

        private void GenerateXmlDiseaseInfos(InsuredClaimModel claimData, XmlHelper helper)
        {
            helper.AddElement("Maladie", "DatePremiersSymptomes", helper.TransformerDate(claimData.InfosDisease.DatePremierSymptomes));

            XmlElement symptomes = helper.CreateElement("Symptomes");

            if (claimData.InfosDisease.Symptomes_Depression)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Depression";
                symptomes.AppendChild(symptome);
            }

            if (claimData.InfosDisease.Symptomes_Position)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Position";
                symptomes.AppendChild(symptome);
            }

            if (claimData.InfosDisease.Symptomes_Concentration)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Concentration";
                symptomes.AppendChild(symptome);
            }

            if (claimData.InfosDisease.Symptomes_Douleurs)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Douleurs";
                symptomes.AppendChild(symptome);
            }

            if (claimData.InfosDisease.Symptomes_Fatigue)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Fatigue";
                symptomes.AppendChild(symptome);
            }

            if (claimData.InfosDisease.Symptomes_Negatives)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Negatives";
                symptomes.AppendChild(symptome);
            }

            if (claimData.InfosDisease.Symptomes_Sommeil)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Sommeil";
                symptomes.AppendChild(symptome);
            }

            if (claimData.InfosDisease.Symptomes_Memoire)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Memoire";
                symptomes.AppendChild(symptome);
            }

            if (claimData.InfosDisease.Symptomes_Autre)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Autre";
                symptomes.AppendChild(symptome);
            }

            helper.AddElement("Maladie", symptomes);
            helper.AddElement("Maladie", "AutreSymptomes", claimData.InfosDisease.Symptomes_AutrePrecision);
            helper.AddElement("Maladie", "IntensiteSymptomes", claimData.InfosDisease.IntensiteSymptomes.Value);

            helper.AddElement("Maladie", "BesoinAide", (claimData.InfosDisease.BesoinAide == "O").ToString());

            XmlElement activites = helper.CreateElement("Activites");

            if (claimData.InfosDisease.BesoinAide_Transport)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Transport";
                activites.AppendChild(activite);
            }

            if (claimData.InfosDisease.BesoinAide_Entretien)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Entretien";
                activites.AppendChild(activite);
            }

            if (claimData.InfosDisease.BesoinAide_Courses)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Courses";
                activites.AppendChild(activite);
            }

            if (claimData.InfosDisease.BesoinAide_GererArgent)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "GererArgent";
                activites.AppendChild(activite);
            }

            if (claimData.InfosDisease.BesoinAide_PreparerRepas)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "PreparerRepas";
                activites.AppendChild(activite);
            }

            if (claimData.InfosDisease.BesoinAide_Escalier)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Escalier";
                activites.AppendChild(activite);
            }

            if (claimData.InfosDisease.BesoinAide_Laver)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Laver";
                activites.AppendChild(activite);
            }

            if (claimData.InfosDisease.BesoinAide_Autre)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Autre";
                activites.AppendChild(activite);
            }

            helper.AddElement("Maladie", activites);
            helper.AddElement("Maladie", "AutreActivite", claimData.InfosDisease.BesoinAide_AutrePrecision);

            XmlElement medecins = helper.CreateElement("Medecins");

            foreach (var physician in claimData.InfosDisease.ListeMedecin)
            {
                medecins.AppendChild(physician.ToXmlElement(helper));
            }

            helper.AddElement("Maladie", medecins);

            helper.AddElement("Maladie", "EstConsulteMedecinSpecialiste", (claimData.InfosDisease.MedecinSpecialiste == "O").ToString());

            XmlElement medecinSpecialiste = helper.CreateElement("MedecinSpecialiste");
            medecinSpecialiste.AppendChild(claimData.InfosDisease.InfosMedecinSpecialiste.ToXmlElement(helper));
            helper.AddElement("Maladie", medecinSpecialiste);

            helper.AddElement("Maladie", "EstHospitalise", (claimData.InfosDisease.Hospitalise == "O").ToString());
            helper.AddElement("Maladie", "EtablissementHospitalise", claimData.InfosDisease.Etablissement_Hospitalise);
            helper.AddElement("Maladie", "VilleHospitalise", claimData.InfosDisease.Ville_Hospitalise);
            helper.AddElement("Maladie", "DateDebutHospitalisation", helper.TransformerDate(claimData.InfosDisease.PeriodeHospitalise.DateFrom));
            helper.AddElement("Maladie", "DateFinHospitalisation", helper.TransformerDate(claimData.InfosDisease.PeriodeHospitalise.DateTo));

            helper.AddElement("Maladie", "PrendMedicament", (claimData.InfosDisease.MedicamentsPris == "O").ToString());
            helper.AddElement("Maladie", "Medicaments", claimData.InfosDisease.ListeMedicamentsPris);

            helper.AddElement("Maladie", "SuitTherapie", (claimData.InfosDisease.TherapieSuivie == "O").ToString());

            XmlElement therapies = helper.CreateElement("Therapies");

            if (claimData.InfosDisease.Acupuncture)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Acupuncture";
                therapies.AppendChild(therapie);
            }

            if (claimData.InfosDisease.Chiropratique)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Chiropratie";
                therapies.AppendChild(therapie);
            }

            if (claimData.InfosDisease.Ergotherapie)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Ergotherapie";
                therapies.AppendChild(therapie);
            }

            if (claimData.InfosDisease.Physiotherapie)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Physiotherapie";
                therapies.AppendChild(therapie);
            }

            if (claimData.InfosDisease.Psychotherapie)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Psychotherapie";
                therapies.AppendChild(therapie);
            }

            if (claimData.InfosDisease.Therapie_Autre)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Autre";
                therapies.AppendChild(therapie);
            }

            helper.AddElement("Maladie", therapies);
            helper.AddElement("Maladie", "AutreTherapie", claimData.InfosDisease.Therapie_AutrePrecision);

            helper.AddElement("Maladie", "DejaSouffertMaladieSimilaire", (claimData.InfosDisease.MaladieSimilaire == "O").ToString());
            helper.AddElement("Maladie", "AnneeMaladieSimilaire", claimData.InfosDisease.MaladieSimilaire_Annee.ToString());

            helper.AddElement("Maladie", "AConsulterMaladieSimilaire", (claimData.InfosDisease.MaladieSimilaire_Consulte == "O").ToString());

            XmlElement medecinMaladieSimilaire = helper.CreateElement("MedecinMaladieSimilaire");
            medecinMaladieSimilaire.AppendChild(claimData.InfosDisease.InfosMedecinMaladieSimilaire.ToXmlElement(helper));
            helper.AddElement("Maladie", medecinMaladieSimilaire);


            helper.AddElement("Maladie", "ConsulteMedecin5DernieresAnnees", (claimData.InfosDisease.HasDiagnosisInLastFiveYears == "O").ToString());

            if (claimData.InfosDisease.HasDiagnosisInLastFiveYears == "O")
            {
                XmlElement consultations = helper.CreateElement("Consultations");

                foreach (MedicalConsultationModel Consultation in claimData.InfosDisease.MedicalConsultations)
                {
                    XmlElement consultation = helper.CreateElement("Consultation");

                    XmlElement raison = helper.CreateElement("Raison");
                    raison.InnerText = Consultation.Reason;

                    consultation.AppendChild(raison);

                    XmlElement year = helper.CreateElement("Year");
                    year.InnerText = Consultation.Year.ToString();

                    consultation.AppendChild(year);

                    consultation.AppendChild(Consultation.PhysicianInfos.ToXmlElement(helper));

                    consultations.AppendChild(consultation);
                }

                helper.AddElement("Maladie", consultations);
            }
        }

        private void GenerateXmlAccidentInfos(InsuredClaimModel claimData, XmlHelper helper)
        {
            helper.AddElement("Accident", "DateAccident", helper.TransformerDate(claimData.InfosAccident.DateAccident));

            helper.AddElement("Accident", "EstMotorise", (claimData.InfosAccident.AccidentMotorise == "O").ToString());
            helper.AddElement("Accident", "EstConducteur", (claimData.InfosAccident.Conducteur == "O").ToString());

            if (claimData.InfosAccident.HeureAccident != null && claimData.InfosAccident.MinuteAccident != null)
            {
                helper.AddElement("Accident", "HeureAccident", string.Format("{0}:{1}", claimData.InfosAccident.HeureAccident.PadLeft(2, '0'), claimData.InfosAccident.MinuteAccident.PadLeft(2, '0')));
            }
            else
            {
                helper.AddElement("Accident", "HeureAccident", "");
            }

            helper.AddElement("Accident", "LieuAccident", claimData.InfosAccident.LieuAccident);
            helper.AddElement("Accident", "NomEtablissementConduit", helper.TransformerDate(claimData.InfosAccident.NomEtablissementConduit));
            helper.AddElement("Accident", "VilleEtablissementConduit", helper.TransformerDate(claimData.InfosAccident.VilleEtablissementConduit));
            helper.AddElement("Accident", "NomEtablissementTraite", helper.TransformerDate(claimData.InfosAccident.NomEtablissementTraite));
            helper.AddElement("Accident", "VilleEtablissementTraite", helper.TransformerDate(claimData.InfosAccident.VilleEtablissementTraite));
            helper.AddElement("Accident", "Recit", claimData.InfosAccident.RecitEvenement);

            helper.AddElement("Accident", "Symptomes", claimData.InfosAccident.Symptomes);
            helper.AddElement("Accident", "IntensiteSymptomes", claimData.InfosAccident.IntensiteSymptomes.Value);


            helper.AddElement("Accident", "BesoinAide", (claimData.InfosAccident.BesoinAide == "O").ToString());

            XmlElement activites = helper.CreateElement("Activites");

            if (claimData.InfosAccident.BesoinAide_Transport)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Transport";
                activites.AppendChild(activite);
            }

            if (claimData.InfosAccident.BesoinAide_Entretien)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Entretien";
                activites.AppendChild(activite);
            }

            if (claimData.InfosAccident.BesoinAide_Courses)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Courses";
                activites.AppendChild(activite);
            }

            if (claimData.InfosAccident.BesoinAide_GererArgent)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "GererArgent";
                activites.AppendChild(activite);
            }

            if (claimData.InfosAccident.BesoinAide_PreparerRepas)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "PreparerRepas";
                activites.AppendChild(activite);
            }

            if (claimData.InfosAccident.BesoinAide_Escalier)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Escalier";
                activites.AppendChild(activite);
            }

            if (claimData.InfosAccident.BesoinAide_Laver)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Laver";
                activites.AppendChild(activite);
            }

            if (claimData.InfosAccident.BesoinAide_Autre)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Autre";
                activites.AppendChild(activite);
            }

            helper.AddElement("Accident", activites);
            helper.AddElement("Accident", "AutreActivite", claimData.InfosAccident.BesoinAide_AutrePrecision);

            XmlElement medecins = helper.CreateElement("Medecins");
            foreach (var physician in claimData.InfosAccident.ListeMedecin)
            {
                medecins.AppendChild(physician.ToXmlElement(helper));
            }
            helper.AddElement("Accident", medecins);

            helper.AddElement("Accident", "EstConsulteMedecinSpecialiste", (claimData.InfosAccident.MedecinSpecialiste == "O").ToString());

            XmlElement medecinSpecialiste = helper.CreateElement("MedecinSpecialiste");
            medecinSpecialiste.AppendChild(claimData.InfosAccident.InfosMedecinSpecialiste.ToXmlElement(helper));
            helper.AddElement("Accident", medecinSpecialiste);

            helper.AddElement("Accident", "EstHospitalise", (claimData.InfosAccident.Hospitalise == "O").ToString());
            helper.AddElement("Accident", "EtablissementHospitalise", claimData.InfosAccident.Etablissement_Hospitalise);
            helper.AddElement("Accident", "VilleHospitalise", claimData.InfosAccident.Etablissement_Hospitalise);
            helper.AddElement("Accident", "DateDebutHospitalisation", helper.TransformerDate(claimData.InfosAccident.PeriodeHospitalise.DateFrom));
            helper.AddElement("Accident", "DateFinHospitalisation", helper.TransformerDate(claimData.InfosAccident.PeriodeHospitalise.DateTo));

            helper.AddElement("Accident", "PrendMedicament", (claimData.InfosAccident.MedicamentsPris == "O").ToString());
            helper.AddElement("Accident", "Medicaments", claimData.InfosAccident.ListeMedicamentsPris);

            helper.AddElement("Accident", "SuitTherapie", (claimData.InfosAccident.TherapieSuivie == "O").ToString());

            XmlElement therapies = helper.CreateElement("Therapies");

            if (claimData.InfosAccident.Acupuncture)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Acupuncture";
                therapies.AppendChild(therapie);
            }

            if (claimData.InfosAccident.Chiropratique)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Chiropratie";
                therapies.AppendChild(therapie);
            }

            if (claimData.InfosAccident.Ergotherapie)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Ergotherapie";
                therapies.AppendChild(therapie);
            }

            if (claimData.InfosAccident.Physiotherapie)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Physiotherapie";
                therapies.AppendChild(therapie);
            }

            if (claimData.InfosAccident.Psychotherapie)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Psychotherapie";
                therapies.AppendChild(therapie);
            }

            if (claimData.InfosAccident.Therapie_Autre)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Autre";
                therapies.AppendChild(therapie);
            }

            helper.AddElement("Accident", therapies);
            helper.AddElement("Accident", "AutreTherapie", claimData.InfosAccident.Therapie_AutrePrecision);

            helper.AddElement("Accident", "DejaSouffertMaladieSimilaire", (claimData.InfosAccident.MaladieSimilaire == "O").ToString());
            helper.AddElement("Accident", "AnneeMaladieSimilaire", claimData.InfosAccident.MaladieSimilaire_Annee.ToString());

            helper.AddElement("Accident", "AConsulterMaladieSimilaire", (claimData.InfosAccident.MaladieSimilaire_Consulte == "O").ToString());

            XmlElement medecinMaladieSimilaire = helper.CreateElement("MedecinMaladieSimilaire");
            medecinMaladieSimilaire.AppendChild(claimData.InfosAccident.InfosMedecinMaladieSimilaire.ToXmlElement(helper));
            helper.AddElement("Accident", medecinMaladieSimilaire);

            helper.AddElement("Accident", "ConsulteMedecin5DernieresAnnees", (claimData.InfosAccident.HasDiagnosisInLastFiveYears == "O").ToString());

            if (claimData.InfosAccident.HasDiagnosisInLastFiveYears == "O")
            {
                XmlElement consultations = helper.CreateElement("Consultations");

                foreach (MedicalConsultationModel Consultation in claimData.InfosAccident.MedicalConsultations)
                {
                    XmlElement consultation = helper.CreateElement("Consultation");

                    XmlElement raison = helper.CreateElement("Raison");
                    raison.InnerText = Consultation.Reason;

                    consultation.AppendChild(raison);

                    XmlElement year = helper.CreateElement("Year");
                    year.InnerText = Consultation.Year.ToString();

                    consultation.AppendChild(year);

                    consultation.AppendChild(Consultation.PhysicianInfos.ToXmlElement(helper));

                    consultations.AppendChild(consultation);
                }

                helper.AddElement("Accident", consultations);
            }

        }

        private void GenerateXmlProceedsInfos(InsuredClaimModel claimData, XmlHelper helper)
        {
            //Autres prestations
            helper.AddElement("Prestations", "RecoitPrestRQAP", (claimData.InfosProceeds.PrestationRqap == "O").ToString());
            helper.AddElement("Prestations", "RecoitPrestEmployeur", claimData.InfosProceeds.PrestationAssuranceEmployeur.ToString());
            helper.AddElement("Prestations", "RecoitPrestAssInd", claimData.InfosProceeds.PrestationAssuranceIndividuelle.ToString());
            helper.AddElement("Prestations", "RecoitPrestAssPret", claimData.InfosProceeds.PrestationAssurancePret.ToString());
            helper.AddElement("Prestations", "RecoitPrestCnesst", claimData.InfosProceeds.PrestationCnesstWsib.ToString());
            helper.AddElement("Prestations", "RecoitPrestIvac", claimData.InfosProceeds.PrestationIvac.ToString());
            helper.AddElement("Prestations", "RecoitPrestAssEmpReg", claimData.InfosProceeds.PrestationAssuranceEmploiRegulier.ToString());
            helper.AddElement("Prestations", "RecoitPrestAssEmpMal", claimData.InfosProceeds.PrestationAssuranceEmploiMaladie.ToString());
            helper.AddElement("Prestations", "RecoitPrestRenteRetraite", claimData.InfosProceeds.PrestationRenteRetraite.ToString());
            helper.AddElement("Prestations", "RecoitPrestRenteInv", claimData.InfosProceeds.PrestationRenteInvalidite.ToString());
            helper.AddElement("Prestations", "RecoitPrestSaaq", claimData.InfosProceeds.PrestationSaaq.ToString());
            helper.AddElement("Prestations", "RecoitPrestAutre", claimData.InfosProceeds.PrestationAutre.ToString());


            if (claimData.InfosProceeds.PrestationAssuranceEmployeur)
            {
                GenerateXmlOtherProceeds(claimData.InfosProceeds.AssuranceEmployeur, "Employeur", helper);
            }

            if (claimData.InfosProceeds.PrestationAssuranceIndividuelle)
            {
                GenerateXmlOtherProceeds(claimData.InfosProceeds.AssuranceIndividuelle, "Individuelle", helper);
            }

            if (claimData.InfosProceeds.PrestationAssurancePret)
            {
                GenerateXmlOtherProceeds(claimData.InfosProceeds.AssurancePret, "Pret", helper);
            }

            if (claimData.InfosProceeds.PrestationCnesstWsib)
            {
                GenerateXmlOtherProceeds(claimData.InfosProceeds.AssuranceCnesstWsib, "CNESST", helper);
            }

            if (claimData.InfosProceeds.PrestationIvac)
            {
                GenerateXmlOtherProceeds(claimData.InfosProceeds.AssuranceIvac, "IVAC", helper);
            }

            if (claimData.InfosProceeds.PrestationAssuranceEmploiRegulier)
            {
                GenerateXmlOtherProceeds(claimData.InfosProceeds.AssuranceEmploiRegulier, "AssuranceEmploiReg", helper);
            }

            if (claimData.InfosProceeds.PrestationAssuranceEmploiMaladie)
            {
                GenerateXmlOtherProceeds(claimData.InfosProceeds.AssuranceEmploiMaladie, "AssuranceEmploiMal", helper);
            }

            if (claimData.InfosProceeds.PrestationRenteInvalidite)
            {
                GenerateXmlOtherProceeds(claimData.InfosProceeds.AssuranceRenteInvalidite, "RenteInvalidite", helper);
            }

            if (claimData.InfosProceeds.PrestationRenteRetraite)
            {
                GenerateXmlOtherProceeds(claimData.InfosProceeds.AssuranceRenteRetraite, "RenteRetraite", helper);
            }

            if (claimData.InfosProceeds.PrestationSaaq)
            {
                GenerateXmlOtherProceeds(claimData.InfosProceeds.AssuranceSaaq, "SAAQ", helper);
            }

            if (claimData.InfosProceeds.PrestationAutre)
            {
                GenerateXmlOtherProceeds(claimData.InfosProceeds.AssuranceAutre, "Autre", helper);
            }

        }

        private void GenerateXmlInvalidityInfos(InsuredClaimModel claimData, XmlHelper helper)
        {
            helper.AddElement("Invalidite", "DateDebutInvalidite", helper.TransformerDate(claimData.InfosInvalidity.DateDebutInvalidite));
            helper.AddElement("Invalidite", "CauseInvalidite", claimData.InfosInvalidity.CauseInvalidite);
            helper.AddElement("Invalidite", "EstRetourneTravail", (claimData.InfosInvalidity.RetourTravail == "O").ToString());
            helper.AddElement("Invalidite", "TypeRetourTravail", claimData.InfosInvalidity.TypeRetourTravail);
            helper.AddElement("Invalidite", "DateDebutRetourTravail", helper.TransformerDate(claimData.InfosInvalidity.PeriodeRetourTravail.DateFrom));
            helper.AddElement("Invalidite", "DateFinRetourTravail", helper.TransformerDate(claimData.InfosInvalidity.PeriodeRetourTravail.DateTo));
            helper.AddElement("Invalidite", "EstRetourTravailConvenu", (claimData.InfosInvalidity.RetourTravailConvenu == "O").ToString());
            helper.AddElement("Invalidite", "DateRetourTravailConvenu", helper.TransformerDate(claimData.InfosInvalidity.DateRetourTravailConvenu));

            helper.AddElement("Invalidite", "EstTravailRemunere", (claimData.InfosInvalidity.TravailRemunere == "O").ToString());
            helper.AddElement("Invalidite", "DateDebutTravailRemunere", helper.TransformerDate(claimData.InfosInvalidity.PeriodeTravailRemunere.DateFrom));
            helper.AddElement("Invalidite", "DateFinTravailRemunere", helper.TransformerDate(claimData.InfosInvalidity.PeriodeTravailRemunere.DateTo));

            helper.AddElement("Invalidite", "EstRetourEtudes", (claimData.InfosInvalidity.RetourEtudes == "O").ToString());
            helper.AddElement("Invalidite", "NomProgrammeRetourEtudes", claimData.InfosInvalidity.NomProgrammeEtudes);
            helper.AddElement("Invalidite", "NombreHeuresSemainesRetourEtudes", claimData.InfosInvalidity.NombreHeuresSemainesEtudes);
            helper.AddElement("Invalidite", "DateDebutRetourEtudes", helper.TransformerDate(claimData.InfosInvalidity.PeriodeProgrammeEtudes.DateFrom));
            helper.AddElement("Invalidite", "DateFinRetourEtudes", helper.TransformerDate(claimData.InfosInvalidity.PeriodeProgrammeEtudes.DateTo));

            helper.AddElement("Invalidite", "EstFormationProfessionnelle", (claimData.InfosInvalidity.FormationProfessionnelle == "O").ToString());
            helper.AddElement("Invalidite", "NomProgrammeFormation", claimData.InfosInvalidity.NomProgrammeProfessionnel);
            helper.AddElement("Invalidite", "DateDebutFormation", helper.TransformerDate(claimData.InfosInvalidity.PeriodeProgrammeProfessionnel.DateFrom));
            helper.AddElement("Invalidite", "DateFinFormation", helper.TransformerDate(claimData.InfosInvalidity.PeriodeProgrammeProfessionnel.DateTo));
        }

        private void GenerateXmlInsuredInfos(InsuredClaimModel claimData, XmlHelper helper)
        {
            helper.AddElement("Assure", "EstDeclarantAssure", (claimData.InfosInsured.DeclarantAssure == "O").ToString());

            helper.AddElement("Assure", "EstChangementAdresse", (claimData.InfosInsured.ChangementAdresse == "O").ToString());

            if (claimData.InfosInsured.ChangementAdresse == "O")
            {

                helper.AddElement("Assure", helper.CreateElement(nameof(claimData.InfosInsured.InfosAdresse), claimData.InfosInsured.InfosAdresse.ToXmlElement(helper)));
            }

            if (!claimData.InfosInsured.EstDeclarantAssure())
            {

                helper.AddElement("Assure", "Prenom", claimData.InfosInsured.Prenom);
                helper.AddElement("Assure", "Nom", claimData.InfosInsured.Nom);

                helper.AddElement("Demandeur", "Prenom", claimData.InfosInsured.PrenomDemandeur);
                helper.AddElement("Demandeur", "Nom", claimData.InfosInsured.NomDemandeur);
                helper.AddElement("Demandeur", "TelephonePrincipal", claimData.InfosInsured.TelPrincipalDemandeur);
                helper.AddElement("Demandeur", "PostePrincipal", claimData.InfosInsured.TelPrincipalPosteDemandeur);
                helper.AddElement("Demandeur", "TelephoneSecondaire", claimData.InfosInsured.TelSecondaireDemandeur);
                helper.AddElement("Demandeur", "PosteSecondaire", claimData.InfosInsured.TelSecondairePosteDemandeur);
                helper.AddElement("Demandeur", "Courriel", claimData.InfosInsured.CourrielDemandeur);
            }
            else
            {

                helper.AddElement("Assure", "Prenom", claimData.InfosInsured.VotrePrenom);
                helper.AddElement("Assure", "Nom", claimData.InfosInsured.VotreNom);
            }
        }

        private void GenerateXmlOtherProceeds(OtherInsuranceModel prestationInfos, string nom, XmlHelper helper)
        {
            XmlElement prestation = helper.CreateElement("Prestation");
            XmlAttribute nomPrestation = helper.CreateAttribute("Nom");
            nomPrestation.Value = nom;

            prestation.Attributes.Append(nomPrestation);

            XmlElement autreAssurance = helper.CreateElement("AutreAssurance");
            autreAssurance.InnerText = prestationInfos.AutreAssurance;
            prestation.AppendChild(autreAssurance);

            XmlElement nomAssureur = helper.CreateElement("NomAssureur");
            nomAssureur.InnerText = prestationInfos.NomAssureur;
            prestation.AppendChild(nomAssureur);

            XmlElement typePret = helper.CreateElement("TypePret");
            typePret.InnerText = prestationInfos.TypeAssurancePret;
            prestation.AppendChild(typePret);

            XmlElement typePretAutre = helper.CreateElement("TypePretAutre");
            typePretAutre.InnerText = prestationInfos.AutreTypeAssurancePret;
            prestation.AppendChild(typePretAutre);

            XmlElement statutReclamation = helper.CreateElement("StatutReclamation");
            statutReclamation.InnerText = prestationInfos.StatutReclamation;
            prestation.AppendChild(statutReclamation);

            if (prestationInfos.LettreRefusCopy.Files.Any())
            {
                XmlElement copieDecisionRefus = helper.CreateElement("CopieDecisionRefus");
                copieDecisionRefus.AppendChild(prestationInfos.LettreRefusCopy.ToXmlElement(helper));
                prestation.AppendChild(copieDecisionRefus);
            }

            //Acceptée
            XmlElement recoitPrestation = helper.CreateElement("RecoitPrestation");
            recoitPrestation.InnerText = (prestationInfos.RecoitPrestation == "O").ToString();
            prestation.AppendChild(recoitPrestation);

            XmlElement montantPrestation = helper.CreateElement("MontantPrestation");
            montantPrestation.InnerText = prestationInfos.MontantPrestation.GetAmount().ToString();
            prestation.AppendChild(montantPrestation);

            XmlElement frequencePrestation = helper.CreateElement("FrequencePrestation");
            frequencePrestation.InnerText = prestationInfos.FrequencePrestation;
            prestation.AppendChild(frequencePrestation);

            XmlElement frequencePrestationAutre = helper.CreateElement("FrequencePrestationAutre");
            frequencePrestationAutre.InnerText = prestationInfos.AutreFrequence;
            prestation.AppendChild(frequencePrestationAutre);

            if (prestationInfos.RelevePaiementCopy.Files.Any())
            {
                XmlElement copieRelevePaiement = helper.CreateElement("CopieRelevePaiement");
                copieRelevePaiement.AppendChild(prestationInfos.RelevePaiementCopy.ToXmlElement(helper));
                prestation.AppendChild(copieRelevePaiement);
            }

            XmlElement dateFinPrestation = helper.CreateElement("DateFinPrestation");
            dateFinPrestation.InnerText = helper.TransformerDate(prestationInfos.DateFinPrestation);
            prestation.AppendChild(dateFinPrestation);

            if (prestationInfos.LettreFinPrestationCopy.Files.Any())
            {
                XmlElement copieFinPrestation = helper.CreateElement("CopieFinPrestation");
                copieFinPrestation.AppendChild(prestationInfos.LettreFinPrestationCopy.ToXmlElement(helper));
                prestation.AppendChild(copieFinPrestation);
            }

            XmlElement evalueMedecinRecommande = helper.CreateElement("EstEvalueMedecinRecommande");
            evalueMedecinRecommande.InnerText = (prestationInfos.EvalueParUnMedecin == "O").ToString();
            prestation.AppendChild(evalueMedecinRecommande);

            XmlElement dateEvaluationMedecin = helper.CreateElement("DateEvaluationMedecinRecommande");
            dateEvaluationMedecin.InnerText = helper.TransformerDate(prestationInfos.DateEvaluationParUnMedecin);
            prestation.AppendChild(dateEvaluationMedecin);

            XmlElement conditionConsolidee = helper.CreateElement("EstConditionConsolidee");
            conditionConsolidee.InnerText = (prestationInfos.ConditionConsolidee == "O").ToString();
            prestation.AppendChild(conditionConsolidee);

            XmlElement dateMentionConditionCosolidee = helper.CreateElement("DateConditionConsolidee");
            dateMentionConditionCosolidee.InnerText = helper.TransformerDate(prestationInfos.DateMentionConditionConsolidee);
            prestation.AppendChild(dateMentionConditionCosolidee);

            XmlElement reorientation = helper.CreateElement("EstReorientation");
            reorientation.InnerText = (prestationInfos.ReorientationNecessaire == "O").ToString();
            prestation.AppendChild(reorientation);

            XmlElement dateReorientation = helper.CreateElement("DateReorientation");
            dateReorientation.InnerText = helper.TransformerDate(prestationInfos.DateDecisionReorientation);
            prestation.AppendChild(dateReorientation);

            XmlElement typeEmploiReorientation = helper.CreateElement("TypeEmploiReorientation");
            typeEmploiReorientation.InnerText = prestationInfos.TypeEmploiReorientation;
            prestation.AppendChild(typeEmploiReorientation);

            XmlElement situationEmploi = helper.CreateElement("SituationEmploi");
            situationEmploi.InnerText = prestationInfos.SituationEmploi;
            prestation.AppendChild(situationEmploi);

            XmlElement situationEmploiAutre = helper.CreateElement("SituationEmploiAutre");
            situationEmploiAutre.InnerText = prestationInfos.AutreSituationEmploi;
            prestation.AppendChild(situationEmploiAutre);

            XmlElement situationCnesst = helper.CreateElement("SituationCnesst");
            situationCnesst.InnerText = prestationInfos.SituationCnesstWsibSaaq;
            prestation.AppendChild(situationCnesst);

            if (prestationInfos.LettreRepriseCNESSTSAAQCopy.Files.Any())
            {
                XmlElement copieDecisionReprise = helper.CreateElement("CopieDecisionReprise");
                copieDecisionReprise.AppendChild(prestationInfos.LettreRepriseCNESSTSAAQCopy.ToXmlElement(helper));
                prestation.AppendChild(copieDecisionReprise);
            }

            if (prestationInfos.LettreRepriseEmployeurCopy.Files.Any())
            {
                XmlElement copieDecisionReprise = helper.CreateElement("CopieDecisionReprise");
                copieDecisionReprise.AppendChild(prestationInfos.LettreRepriseEmployeurCopy.ToXmlElement(helper));
                prestation.AppendChild(copieDecisionReprise);
            }

            XmlElement situationCnesstAutre = helper.CreateElement("SituationCnesstAutre");
            situationCnesstAutre.InnerText = prestationInfos.AutreSituationCnesstWsibSaaq;
            prestation.AppendChild(situationCnesstAutre);

            XmlElement dateRepriseTravail = helper.CreateElement("DateRepriseTravail");
            dateRepriseTravail.InnerText = helper.TransformerDate(prestationInfos.DateRepriseTravail);
            prestation.AppendChild(dateRepriseTravail);

            if (prestationInfos.SituationCnesstWsibSaaq == "P")
            {
                XmlElement dateDecisionCnesst = helper.CreateElement("DateDecisionCnesst");
                dateDecisionCnesst.InnerText = helper.TransformerDate(prestationInfos.DateDecisionInapteCnesstWsibSaaq);
                prestation.AppendChild(dateDecisionCnesst);
            }
            else
            {
                XmlElement dateDecisionCnesst = helper.CreateElement("DateDecisionCnesst");
                dateDecisionCnesst.InnerText = helper.TransformerDate(prestationInfos.DateDecisionCnesstWsibSaaq);
                prestation.AppendChild(dateDecisionCnesst);
            }

            XmlElement inapte = helper.CreateElement("EstInapte");
            inapte.InnerText = (prestationInfos.ReconnuInapte == "O").ToString();
            prestation.AppendChild(inapte);


            if (prestationInfos.LettreInapteCNESSTSAAQCopy.Files.Any())
            {
                XmlElement copieDecisionInapte = helper.CreateElement("CopieDecisionInapte");
                copieDecisionInapte.AppendChild(prestationInfos.LettreInapteCNESSTSAAQCopy.ToXmlElement(helper));
                prestation.AppendChild(copieDecisionInapte);
            }

            if (prestationInfos.LettreInapteEmployeurCopy.Files.Any())
            {
                XmlElement copieDecisionInapte = helper.CreateElement("CopieDecisionInapte");
                copieDecisionInapte.AppendChild(prestationInfos.LettreInapteEmployeurCopy.ToXmlElement(helper));
                prestation.AppendChild(copieDecisionInapte);
            }

            helper.AddElement("Prestations", prestation);
        }

    }
}
