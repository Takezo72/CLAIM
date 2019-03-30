using CLAIM.Models.FileReview;
using CLAIM.Models.Shared;
using System.Xml;
using NLog;
using System.Globalization;
using System.Linq;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Helpers.XmlGenerators
{
    public class FileReviewXmlGenerator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal byte[] Generate(FileReviewModel claimData, string urlDepot)
        {
            XmlHelper helper = new XmlHelper("RG_INV_REVISION_V1", CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower(), claimData.ReviewInsured.InfosResquestFollowUp.TrackingNumber, claimData.ReviewInsured.InfosResquestFollowUp.EventId, claimData.ReviewInsured.InfosResquestFollowUp.AttachmentId);

            GenerateXmlInsuredInfos(claimData, helper);
            GenerateXmlInvalidityInfos(claimData, helper);

            GenerateXmlEmploymentInfos(claimData, helper);

            GenerateXmlProceedsInfos(claimData, helper);

            foreach (OtherInsuranceModel otherInsurance in claimData.ReviewProceeds.ListOtherInsurance().Where(o => o != null))
            {
                otherInsurance.ToFileXmlElement(helper, urlDepot);
            }

            ConfigurationHelper config = new ConfigurationHelper();

            if (config.IsMockUserIdentity)
            {
                logger.Info(string.Format("MOCK USER ACTIVATED! XML saved to disk."));
                helper.Save("d:\\testFileReview.xml");
            }

            return helper.Extract();
        }

        private void GenerateXmlEmploymentInfos(FileReviewModel claimData, XmlHelper helper)
        {
            helper.AddElement("Emploi", "EstRetourneTravail", (claimData.ReviewEmployment.RetourTravail == "O").ToString());
            helper.AddElement("Emploi", "TypeRetourTravail", claimData.ReviewEmployment.TypeRetourTravail);
            helper.AddElement("Emploi", "DateDebutRetourTravail", helper.TransformerDate(claimData.ReviewEmployment.PeriodeRetourTravail.DateFrom));
            helper.AddElement("Emploi", "DateFinRetourTravail", helper.TransformerDate(claimData.ReviewEmployment.PeriodeRetourTravail.DateTo));
            helper.AddElement("Emploi", "EstRetourTravailConvenu", (claimData.ReviewEmployment.RetourTravailConvenu == "O").ToString());
            helper.AddElement("Emploi", "DateRetourTravailConvenu", helper.TransformerDate(claimData.ReviewEmployment.DateRetourTravailConvenu));

            helper.AddElement("Emploi", "EstTravailRemunere", (claimData.ReviewEmployment.TravailRemunere == "O").ToString());
            helper.AddElement("Emploi", "DateDebutTravailRemunere", helper.TransformerDate(claimData.ReviewEmployment.PeriodeTravailRemunere.DateFrom));
            helper.AddElement("Emploi", "DateFinTravailRemunere", helper.TransformerDate(claimData.ReviewEmployment.PeriodeTravailRemunere.DateTo));

            helper.AddElement("Emploi", "EstRetourEtudes", (claimData.ReviewEmployment.RetourEtudes == "O").ToString());
            helper.AddElement("Emploi", "NomProgrammeRetourEtudes", claimData.ReviewEmployment.NomProgrammeEtudes);
            helper.AddElement("Emploi", "NombreHeuresSemainesRetourEtudes", claimData.ReviewEmployment.NombreHeuresSemainesEtudes);
            helper.AddElement("Emploi", "DateDebutRetourEtudes", helper.TransformerDate(claimData.ReviewEmployment.PeriodeProgrammeEtudes.DateFrom));
            helper.AddElement("Emploi", "DateFinRetourEtudes", helper.TransformerDate(claimData.ReviewEmployment.PeriodeProgrammeEtudes.DateTo));

            helper.AddElement("Emploi", "EstFormationProfessionnelle", (claimData.ReviewEmployment.FormationProfessionnelle == "O").ToString());
            helper.AddElement("Emploi", "NomProgrammeFormation", claimData.ReviewEmployment.NomProgrammeProfessionnel);
            helper.AddElement("Emploi", "DateDebutFormation", helper.TransformerDate(claimData.ReviewEmployment.PeriodeProgrammeProfessionnel.DateFrom));
            helper.AddElement("Emploi", "DateFinFormation", helper.TransformerDate(claimData.ReviewEmployment.PeriodeProgrammeProfessionnel.DateTo));

        }

        private void GenerateXmlProceedsInfos(FileReviewModel claimData, XmlHelper helper)
        {
            //Autres prestations
            helper.AddElement("Prestations", "RecoitPrestRQAP", (claimData.ReviewProceeds.PrestationRqap == "O").ToString());
            helper.AddElement("Prestations", "RecoitPrestEmployeur", claimData.ReviewProceeds.PrestationAssuranceEmployeur.ToString());
            helper.AddElement("Prestations", "RecoitPrestAssInd", claimData.ReviewProceeds.PrestationAssuranceIndividuelle.ToString());
            helper.AddElement("Prestations", "RecoitPrestAssPret", claimData.ReviewProceeds.PrestationAssurancePret.ToString());
            helper.AddElement("Prestations", "RecoitPrestCnesst", claimData.ReviewProceeds.PrestationCnesstWsib.ToString());
            helper.AddElement("Prestations", "RecoitPrestIvac", claimData.ReviewProceeds.PrestationIvac.ToString());
            helper.AddElement("Prestations", "RecoitPrestAssEmpReg", claimData.ReviewProceeds.PrestationAssuranceEmploiRegulier.ToString());
            helper.AddElement("Prestations", "RecoitPrestAssEmpMal", claimData.ReviewProceeds.PrestationAssuranceEmploiMaladie.ToString());
            helper.AddElement("Prestations", "RecoitPrestRenteRetraite", claimData.ReviewProceeds.PrestationRenteRetraite.ToString());
            helper.AddElement("Prestations", "RecoitPrestRenteInv", claimData.ReviewProceeds.PrestationRenteInvalidite.ToString());
            helper.AddElement("Prestations", "RecoitPrestSaaq", claimData.ReviewProceeds.PrestationSaaq.ToString());
            helper.AddElement("Prestations", "RecoitPrestAutre", claimData.ReviewProceeds.PrestationAutre.ToString());
            helper.AddElement("Prestations", "RecoitAucunePrest", claimData.ReviewProceeds.AucunePrestation.ToString());


            if (claimData.ReviewProceeds.PrestationAssuranceEmployeur)
            {
                GenerateXmlOtherProceeds(claimData.ReviewProceeds.AssuranceEmployeur, "Employeur", helper);
            }

            if (claimData.ReviewProceeds.PrestationAssuranceIndividuelle)
            {
                GenerateXmlOtherProceeds(claimData.ReviewProceeds.AssuranceIndividuelle, "Individuelle", helper);
            }

            if (claimData.ReviewProceeds.PrestationAssurancePret)
            {
                GenerateXmlOtherProceeds(claimData.ReviewProceeds.AssurancePret, "Pret", helper);
            }

            if (claimData.ReviewProceeds.PrestationCnesstWsib)
            {
                GenerateXmlOtherProceeds(claimData.ReviewProceeds.AssuranceCnesstWsib, "CNESST", helper);
            }

            if (claimData.ReviewProceeds.PrestationIvac)
            {
                GenerateXmlOtherProceeds(claimData.ReviewProceeds.AssuranceIvac, "IVAC", helper);
            }

            if (claimData.ReviewProceeds.PrestationAssuranceEmploiRegulier)
            {
                GenerateXmlOtherProceeds(claimData.ReviewProceeds.AssuranceEmploiRegulier, "AssuranceEmploiReg", helper);
            }

            if (claimData.ReviewProceeds.PrestationAssuranceEmploiMaladie)
            {
                GenerateXmlOtherProceeds(claimData.ReviewProceeds.AssuranceEmploiMaladie, "AssuranceEmploiMal", helper);
            }

            if (claimData.ReviewProceeds.PrestationRenteInvalidite)
            {
                GenerateXmlOtherProceeds(claimData.ReviewProceeds.AssuranceRenteInvalidite, "RenteInvalidite", helper);
            }

            if (claimData.ReviewProceeds.PrestationRenteRetraite)
            {
                GenerateXmlOtherProceeds(claimData.ReviewProceeds.AssuranceRenteRetraite, "RenteRetraite", helper);
            }

            if (claimData.ReviewProceeds.PrestationSaaq)
            {
                GenerateXmlOtherProceeds(claimData.ReviewProceeds.AssuranceSaaq, "SAAQ", helper);
            }

            if (claimData.ReviewProceeds.PrestationAutre)
            {
                GenerateXmlOtherProceeds(claimData.ReviewProceeds.AssuranceAutre, "Autre", helper);
            }

        }

        private void GenerateXmlInvalidityInfos(FileReviewModel claimData, XmlHelper helper)
        {

            helper.AddElement("Invalidite", "SuiviParNouveauMedecin", (claimData.ReviewInvalidity.SuiviNouveauMedecin == "O").ToString());
            XmlElement nouveauMedecin = helper.CreateElement("NouveauMedecin");
            nouveauMedecin.AppendChild(claimData.ReviewInvalidity.InfosNouveauMedecin.ToXmlElement(helper));
            helper.AddElement("Invalidite", nouveauMedecin);

            helper.AddElement("Invalidite", "EstConsulteMedecinSpecialiste", (claimData.ReviewInvalidity.EvaluationMedecinSpecialise == "O").ToString());
            XmlElement medecinSpecialiste = helper.CreateElement("MedecinSpecialiste");
            medecinSpecialiste.AppendChild(claimData.ReviewInvalidity.InfosMedecinSpecialiste.ToXmlElement(helper));
            helper.AddElement("Invalidite", medecinSpecialiste);

            XmlElement statut = helper.CreateElement("Statut");
            statut.InnerText = claimData.ReviewInvalidity.EtreEvalue;
            medecinSpecialiste.AppendChild(statut);

            helper.AddElement("Invalidite", "EstHospitalise", (claimData.ReviewInvalidity.EtreHospitalise == "O").ToString());
            helper.AddElement("Invalidite", "EtablissementHospitalise", claimData.ReviewInvalidity.Etablissement_Hospitalise);
            helper.AddElement("Invalidite", "VilleEtablissementHospitalise", claimData.ReviewInvalidity.Ville_Etablissement_Hospitalise);
            helper.AddElement("Invalidite", "DateDebutHospitalisation", helper.TransformerDate(claimData.ReviewInvalidity.PeriodeHospitalise.DateFrom));
            helper.AddElement("Invalidite", "DateFinHospitalisation", helper.TransformerDate(claimData.ReviewInvalidity.PeriodeHospitalise.DateTo));
            helper.AddElement("Invalidite", "RaisonHospitalise", claimData.ReviewInvalidity.Raison_Hospitalise);

            helper.AddElement("Invalidite", "SubitChirurgie", claimData.ReviewInvalidity.SubirChirurgie);
            helper.AddElement("Invalidite", "DateChirurgie", helper.TransformerDate(claimData.ReviewInvalidity.DatePrevue_Chirurgie));
            helper.AddElement("Invalidite", "TypeChirurgie", claimData.ReviewInvalidity.TypeChirurgie);

            helper.AddElement("Invalidite", "ChangeMedication", (claimData.ReviewInvalidity.ChangerMedication == "O").ToString());

            if (claimData.ReviewInvalidity.NouveauMedicament)
            {
                helper.AddElement("Invalidite", "NouveauMedicament", claimData.ReviewInvalidity.NouveauMedicament.ToString());
                XmlElement nouvellesPosologies = helper.CreateElement("NouvellesPosologies");
                helper.AddElement("Invalidite", nouvellesPosologies);
                foreach (var newMedication in claimData.ReviewInvalidity.NewMedications)
                {
                    XmlElement nouvellePosologie = helper.CreateElement("NouvellePosologie");
                    nouvellesPosologies.AppendChild(nouvellePosologie);
                    helper.AppendChildIf(nouvellePosologie, claimData.ReviewInvalidity.NouveauMedicament, "NomMedicament", newMedication.NomMedicament);
                    helper.AppendChildIf(nouvellePosologie, claimData.ReviewInvalidity.NouveauMedicament, "Posologie", newMedication.Posologie);
                }
            }

            if (claimData.ReviewInvalidity.ChangementPosologie)
            {
                helper.AddElement("Invalidite", "ChangementPosologie", claimData.ReviewInvalidity.ChangementPosologie.ToString());
                XmlElement changementsPosologies = helper.CreateElement("ChangementsPosologies");
                helper.AddElement("Invalidite", changementsPosologies);
                foreach (var changeMedication in claimData.ReviewInvalidity.ChangeMedications)
                {
                    XmlElement changementPosologie = helper.CreateElement("ChangementPosologie");
                    changementsPosologies.AppendChild(changementPosologie);
                    helper.AppendChildIf(changementPosologie, claimData.ReviewInvalidity.ChangementPosologie, "NomMedicament", changeMedication.NomMedicament);
                    helper.AppendChildIf(changementPosologie, claimData.ReviewInvalidity.ChangementPosologie, "Posologie", changeMedication.Posologie);
                }
            }

            if (claimData.ReviewInvalidity.ArretMedicament)
            {
                helper.AddElement("Invalidite", "ArretMedicament", claimData.ReviewInvalidity.ArretMedicament.ToString());
                XmlElement arretsPosologies = helper.CreateElement("ArretsPosologies");
                helper.AddElement("Invalidite", arretsPosologies);
                foreach (var stopMedication in claimData.ReviewInvalidity.StopMedications)
                {
                    XmlElement arretPosologie = helper.CreateElement("ArretPosologie");
                    arretsPosologies.AppendChild(arretPosologie);
                    helper.AppendChildIf(arretPosologie, claimData.ReviewInvalidity.ArretMedicament, "NomMedicament", stopMedication.NomMedicament);
                }
            }

            if (claimData.ReviewInvalidity.AutrePosologie)
            {
                helper.AddElement("Invalidite", "AutrePosologie", claimData.ReviewInvalidity.AutrePosologie.ToString());
                XmlElement autrePosologie = helper.CreateElement("AutrePosologie");
                helper.AddElement("Invalidite", autrePosologie);
                helper.AppendChildIf(autrePosologie, claimData.ReviewInvalidity.AutrePosologie, "AutrePosologiePrecision", claimData.ReviewInvalidity.Posologie_AutrePrecision);
            }

            helper.AddElement("Invalidite", "StatutTherapie", claimData.ReviewInvalidity.DebuterTherapie);

            if (claimData.ReviewInvalidity.SuiviTherapie)
            {
                helper.AddElement("Invalidite", "SuiviTherapie", claimData.ReviewInvalidity.SuiviTherapie.ToString());
                XmlElement therapieSuivie = helper.CreateElement(nameof(claimData.ReviewInvalidity.TherapieSuivie));
                claimData.ReviewInvalidity.TherapieSuivie.GenerateXml(therapieSuivie, helper);
                helper.AddElement("Invalidite", therapieSuivie);
            }

            if (claimData.ReviewInvalidity.DebutTherapie)
            {
                helper.AddElement("Invalidite", "DebutTherapie", claimData.ReviewInvalidity.DebutTherapie.ToString());
                XmlElement therapieDebutee = helper.CreateElement(nameof(claimData.ReviewInvalidity.TherapieDebutee));
                claimData.ReviewInvalidity.TherapieDebutee.GenerateXml(therapieDebutee, helper);
                helper.AddElement("Invalidite", therapieDebutee);
            }


            XmlElement symptomes = helper.CreateElement("Symptomes");
            if (claimData.ReviewInvalidity.Symptomes_Depression)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Depression";
                symptomes.AppendChild(symptome);
            }
            if (claimData.ReviewInvalidity.Symptomes_Position)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Position";
                symptomes.AppendChild(symptome);
            }
            if (claimData.ReviewInvalidity.Symptomes_Concentration)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Concentration";
                symptomes.AppendChild(symptome);
            }
            if (claimData.ReviewInvalidity.Symptomes_Douleurs)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Douleurs";
                symptomes.AppendChild(symptome);
            }
            if (claimData.ReviewInvalidity.Symptomes_Fatigue)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Fatigue";
                symptomes.AppendChild(symptome);
            }
            if (claimData.ReviewInvalidity.Symptomes_Negatives)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Negatives";
                symptomes.AppendChild(symptome);
            }
            if (claimData.ReviewInvalidity.Symptomes_Sommeil)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Sommeil";
                symptomes.AppendChild(symptome);
            }
            if (claimData.ReviewInvalidity.Symptomes_Memoire)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Memoire";
                symptomes.AppendChild(symptome);
            }
            if (claimData.ReviewInvalidity.Symptomes_Autre)
            {
                XmlElement symptome = helper.CreateElement("Symptome");
                symptome.InnerText = "Autre";
                symptomes.AppendChild(symptome);
            }
            helper.AddElement("Invalidite", symptomes);
            helper.AddElement("Invalidite", "AutreSymptomes", claimData.ReviewInvalidity.Symptomes_AutrePrecision);
            helper.AddElement("Invalidite", "IntensiteSymptomes", claimData.ReviewInvalidity.IntensiteSymptomes.Value);

            helper.AddElement("Invalidite", "BesoinAide", (claimData.ReviewInvalidity.BesoinAide == "O").ToString());
            XmlElement activites = helper.CreateElement("Activites");
            if (claimData.ReviewInvalidity.BesoinAide_Transport)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Transport";
                activites.AppendChild(activite);
            }
            if (claimData.ReviewInvalidity.BesoinAide_Entretien)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Entretien";
                activites.AppendChild(activite);
            }
            if (claimData.ReviewInvalidity.BesoinAide_Courses)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Courses";
                activites.AppendChild(activite);
            }
            if (claimData.ReviewInvalidity.BesoinAide_GererArgent)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "GererArgent";
                activites.AppendChild(activite);
            }
            if (claimData.ReviewInvalidity.BesoinAide_PreparerRepas)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "PreparerRepas";
                activites.AppendChild(activite);
            }
            if (claimData.ReviewInvalidity.BesoinAide_Escalier)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Escalier";
                activites.AppendChild(activite);
            }
            if (claimData.ReviewInvalidity.BesoinAide_Laver)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Laver";
                activites.AppendChild(activite);
            }
            if (claimData.ReviewInvalidity.BesoinAide_Autre)
            {
                XmlElement activite = helper.CreateElement("Activite");
                activite.InnerText = "Autre";
                activites.AppendChild(activite);
            }
            helper.AddElement("Invalidite", activites);
            helper.AddElement("Invalidite", "AutreActivite", claimData.ReviewInvalidity.BesoinAide_AutrePrecision);

        }

        private void GenerateXmlInsuredInfos(FileReviewModel claimData, XmlHelper helper)
        {
            helper.AddElement("Assure", "EstDeclarantAssure", (claimData.ReviewInsured.DeclarantAssure == "O").ToString());
            if (!claimData.ReviewInsured.EstDeclarantAssure())
            {
                helper.AddElement("Assure", "Prenom", claimData.ReviewInsured.Prenom);
                helper.AddElement("Assure", "Nom", claimData.ReviewInsured.Nom);
            }
            else
            {
                helper.AddElement("Assure", "Prenom", claimData.ReviewInsured.VotrePrenom);
                helper.AddElement("Assure", "Nom", claimData.ReviewInsured.VotreNom);
            }

            helper.AddElement("Assure", "EstChangementAdresse", (claimData.ReviewInsured.ChangementAdresse == "O").ToString());
            helper.AddElement("Assure", helper.CreateElement(nameof(claimData.ReviewInsured.InfosAdresse), claimData.ReviewInsured.InfosAdresse.ToXmlElement(helper)));

            if (!claimData.ReviewInsured.EstDeclarantAssure())
            {
                helper.AddElement("Demandeur", "Prenom", claimData.ReviewInsured.PrenomDemandeur);
                helper.AddElement("Demandeur", "Nom", claimData.ReviewInsured.NomDemandeur);
                helper.AddElement("Demandeur", "TelephonePrincipal", claimData.ReviewInsured.TelPrincipalDemandeur);
                helper.AddElement("Demandeur", "PostePrincipal", claimData.ReviewInsured.TelPrincipalPosteDemandeur);
                helper.AddElement("Demandeur", "TelephoneSecondaire", claimData.ReviewInsured.TelSecondaireDemandeur);
                helper.AddElement("Demandeur", "PosteSecondaire", claimData.ReviewInsured.TelSecondairePosteDemandeur);
                helper.AddElement("Demandeur", "Courriel", claimData.ReviewInsured.CourrielDemandeur);
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

            XmlElement dateDecisionCnesst = helper.CreateElement("DateDecisionCnesst");
            dateDecisionCnesst.InnerText = helper.TransformerDate(prestationInfos.DateDecisionCnesstWsibSaaq);
            prestation.AppendChild(dateDecisionCnesst);

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
