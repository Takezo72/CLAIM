using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CLAIM.Models.Shared;
using System.Xml;
using CLAIM.Helpers.XmlGenerators;
using System.Linq;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.Decease
{
    [Serializable]
    public class AskingDeceaseClaimModel : IValidatableObject
    {
        public AskingDeceaseClaimModel()
        {
            InfosAdresse = new AddressModel();

            CopieMandat = new FileUploadModel();
            CessionFuneraire = new FileUploadModel(CopieMandat.ClientId);
            CessionFinanciere = new FileUploadModel(CopieMandat.ClientId);

            Confirmation = new FileUploadModel(CopieMandat.ClientId);
            CertificatNaissance = new FileUploadModel(CopieMandat.ClientId);
            ActeTutelle = new FileUploadModel(CopieMandat.ClientId);
            ActeFiducie = new FileUploadModel(CopieMandat.ClientId);

            Advisor = new AdvisorModel();

        }

        public string PrenomDemandeur { get; set; }
        public string NomDemandeur { get; set; }
        public string Initiale { get; set; }
        public string CourrielDemandeur { get; set; }
        public AddressModel InfosAdresse { get; set; }
        public bool Avocat { get; set; }
        public bool Beneficiaire { get; set; }
        public bool Conseiller { get; set; }
        public bool Fiduciaire { get; set; }
        public bool Liquidateur { get; set; }
        public bool Mandataire { get; set; }
        public bool MaisonFuneraire { get; set; }
        public bool InstitutionFinanciere { get; set; }
        public bool Tuteur { get; set; }
        public bool AutreRole { get; set; }
        public FileUploadModel CopieMandat { get; set; }
        public FileUploadModel CessionFuneraire { get; set; }
        public FileUploadModel CessionFinanciere { get; set; }
        public FileUploadModel Confirmation { get; set; }
        public FileUploadModel CertificatNaissance { get; set; }
        public FileUploadModel ActeTutelle { get; set; }
        public FileUploadModel ActeFiducie { get; set; }
        public AdvisorModel Advisor { get; set; }
        public string PrenomTuteur { get; set; }
        public string NomTuteur { get; set; }
        public string AutreTitre { get; set; }

        public string PreviousStep
        {
            get { return string.Empty; }
            private set { }
        }

        public string NextStep
        {
            get { return "InsuredDeceaseClaim"; }
            private set { }
        }

        public ButtonListModel NavigationButtons
        {
            get
            {
                ConfigurationHelper config = new ConfigurationHelper();
                return new ButtonListModel { NextAction = true, Cancel = true, ReturnUrl = config.IACAReturnUrl };
            }
            private set { }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(PrenomDemandeur))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(PrenomDemandeur) }));
            }

            if (string.IsNullOrWhiteSpace(NomDemandeur))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(NomDemandeur) }));
            }

            result.AddRange(InfosAdresse.Validate(nameof(InfosAdresse), true));

            if (string.IsNullOrWhiteSpace(CourrielDemandeur) || !Helpers.ValidationHelper.IsEmailAdressNumberValid(CourrielDemandeur))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(CourrielDemandeur) }));
            }

            if (Conseiller)
            {
                result.AddRange(Advisor.Validate(nameof(Advisor), true));
            }

            if (Tuteur)
            {
                if (string.IsNullOrWhiteSpace(PrenomTuteur))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { nameof(PrenomTuteur) }));
                }

                if (string.IsNullOrWhiteSpace(NomTuteur))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { nameof(NomTuteur) }));
                }
            }

            if (!Avocat && !Beneficiaire && !Fiduciaire && !Liquidateur &&
                !Mandataire && !MaisonFuneraire && !InstitutionFinanciere && !Tuteur & !AutreRole)
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(Avocat) }));
            }

            if ((string.IsNullOrWhiteSpace(AutreTitre)) && (AutreRole))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(AutreTitre) }));
            }

            if (Mandataire)
            {
                result.AddRange(CopieMandat.Validate(nameof(CopieMandat), !Mandataire));
            }

            if (MaisonFuneraire)
            {
                result.AddRange(CessionFuneraire.Validate(nameof(CessionFuneraire), !MaisonFuneraire));
            }

            if (InstitutionFinanciere)
            {
                result.AddRange(CessionFinanciere.Validate(nameof(CessionFinanciere), !InstitutionFinanciere));
            }

            if (Avocat)
            {
                result.AddRange(Confirmation.Validate(nameof(Confirmation), !Avocat));
            }

            if (Tuteur)
            {
                result.AddRange(CertificatNaissance.Validate(nameof(CertificatNaissance), !Tuteur));
                result.AddRange(ActeTutelle.Validate(nameof(ActeTutelle), !Tuteur));
            }

            if (Fiduciaire)
            {
                result.AddRange(ActeFiducie.Validate(nameof(ActeFiducie), !Fiduciaire));
            }

            return result;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(AskingDeceaseClaimModel).Replace("Model", string.Empty));

            xmlElement.AppendChild(helper.CreateElement(nameof(PrenomDemandeur), PrenomDemandeur));
            xmlElement.AppendChild(helper.CreateElement(nameof(NomDemandeur), NomDemandeur));
            xmlElement.AppendChild(helper.CreateElement(nameof(Initiale), Initiale));
            xmlElement.AppendChild(helper.CreateElement(nameof(InfosAdresse), InfosAdresse.ToXmlElement(helper)));
            xmlElement.AppendChild(helper.CreateElement(nameof(CourrielDemandeur), CourrielDemandeur));

            XmlElement XmlTitres = helper.CreateElement("Titres");
            xmlElement.AppendChild(XmlTitres);
            if (Avocat)
            {
                XmlTitres.AppendChild(helper.CreateElement("Titre", nameof(Avocat)));
            }
            if (Beneficiaire)
            {
                XmlTitres.AppendChild(helper.CreateElement("Titre", nameof(Beneficiaire)));
            }
            if (Conseiller)
            {
                XmlTitres.AppendChild(helper.CreateElement("Titre", nameof(Conseiller)));
            }
            if (Fiduciaire)
            {
                XmlTitres.AppendChild(helper.CreateElement("Titre", nameof(Fiduciaire)));
            }
            if (Liquidateur)
            {
                XmlTitres.AppendChild(helper.CreateElement("Titre", nameof(Liquidateur)));
            }
            if (Mandataire)
            {
                XmlTitres.AppendChild(helper.CreateElement("Titre", nameof(Mandataire)));
            }
            if (MaisonFuneraire)
            {
                XmlTitres.AppendChild(helper.CreateElement("Titre", nameof(MaisonFuneraire)));
            }
            if (InstitutionFinanciere)
            {
                XmlTitres.AppendChild(helper.CreateElement("Titre", nameof(InstitutionFinanciere)));
            }
            if (Tuteur)
            {
                XmlTitres.AppendChild(helper.CreateElement("Titre", nameof(Tuteur)));
            }
            if (AutreRole)
            {
                XmlTitres.AppendChild(helper.CreateElement("Titre", nameof(AutreTitre)));
            }

            if ((Avocat) && (Confirmation.Files.Any()))
            {
                XmlElement XmlAvocat = helper.CreateElement("Avocat");
                xmlElement.AppendChild(XmlAvocat);
                XmlAvocat.AppendChild(helper.CreateElement(nameof(Confirmation), Confirmation.ToXmlElement(helper)));
            }

            if (Conseiller)
            {
                XmlElement XmlConseiller = helper.CreateElement("Conseiller");
                xmlElement.AppendChild(XmlConseiller);
                XmlConseiller.AppendChild(Advisor.ToXmlElement(helper));
            }

            if ((Fiduciaire) && (ActeFiducie.Files.Any()))
            {
                XmlElement XmlFiduciaire = helper.CreateElement("Fiduciaire");
                xmlElement.AppendChild(XmlFiduciaire);
                XmlFiduciaire.AppendChild(ActeFiducie.ToXmlElement(helper));
            }

            if ((Mandataire) && (CopieMandat.Files.Any()))
            {
                XmlElement XmlMandataire = helper.CreateElement("Mandataire");
                xmlElement.AppendChild(XmlMandataire);
                XmlMandataire.AppendChild(CopieMandat.ToXmlElement(helper));
            }

            if ((MaisonFuneraire) && (CessionFuneraire.Files.Any()))
            {
                XmlElement XmlMaisonFuneraire = helper.CreateElement("MaisonFuneraire");
                xmlElement.AppendChild(XmlMaisonFuneraire);
                XmlMaisonFuneraire.AppendChild(CessionFuneraire.ToXmlElement(helper));
            }

            if ((InstitutionFinanciere) && (CessionFinanciere.Files.Any()))
            {
                XmlElement XmlInstitutionFinanciere = helper.CreateElement("InstitutionFinanciere");
                xmlElement.AppendChild(XmlInstitutionFinanciere);
                XmlInstitutionFinanciere.AppendChild(CessionFinanciere.ToXmlElement(helper));
            }

            if (Tuteur)
            {
                XmlElement XmlTuteur = helper.CreateElement("Tuteur");
                xmlElement.AppendChild(XmlTuteur);
                XmlTuteur.AppendChild(helper.CreateElement(nameof(PrenomTuteur), PrenomTuteur));
                XmlTuteur.AppendChild(helper.CreateElement(nameof(NomTuteur), NomTuteur));
                if (CertificatNaissance.Files.Any())
                {
                    XmlElement XmlCertificatNaissance = helper.CreateElement("CertificatNaissance");
                    XmlTuteur.AppendChild(XmlCertificatNaissance);
                    XmlCertificatNaissance.AppendChild(CertificatNaissance.ToXmlElement(helper));
                }
                if (ActeTutelle.Files.Any())
                {
                    XmlElement XmlActeTutelle = helper.CreateElement("ActeTutelle");
                    XmlTuteur.AppendChild(XmlActeTutelle);
                    XmlActeTutelle.AppendChild(ActeTutelle.ToXmlElement(helper));
                }
            }
            if (AutreRole)
            {
                XmlElement XmlAutreTitre = helper.CreateElement("AutreTitre");
                xmlElement.AppendChild(XmlAutreTitre);
                XmlAutreTitre.AppendChild(helper.CreateElement(nameof(AutreTitre), AutreTitre));
            }

            return xmlElement;
        }

    }

    [Serializable]
    public class InsuredAdresse
    {

        public string ChangementAdresse { get; set; }
        public string Numero { get; set; }
        public string Rue { get; set; }
        public string Appartement { get; set; }
        public string Ville { get; set; }
        public string Province { get; set; }
        public string CodePostal { get; set; }
        public string CasePostale { get; set; }
        public string Succursale { get; set; }
        public string RouteRurale { get; set; }
        public string AdresseManuelle { get; set; }

        public string ObtenirAdresse()
        {
            string noapprt = "";

            if (!string.IsNullOrEmpty(Appartement))
            {
                noapprt = Appartement + "-";
            }

            string routeRurale = "";
            if (!string.IsNullOrEmpty(RouteRurale))
            {
                routeRurale = " " + RouteRurale;
            }
            return string.Format("{0}{1}{2}{3}, {4}<br>{5} {6} {7}", CasePostale, noapprt, Numero, routeRurale, Rue, Ville, Province, CodePostal);
        }

    }



}

