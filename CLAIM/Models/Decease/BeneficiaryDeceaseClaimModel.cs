using CLAIM.Helpers.Configuration;
using CLAIM.Helpers.XmlGenerators;
using CLAIM.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml;

namespace CLAIM.Models.Decease
{
    [Serializable]
    public class BeneficiaryDeceaseClaimModel : IValidatableObject
    {
        public BeneficiaryDeceaseClaimModel()
        {
            AdresseMaisonFuneraire = new AddressModel();
            AdresseMaisonFuneraire.Prefix = "AdresseMaisonFuneraire";

            Beneficiaries = new List<BeneficiaryModel> { new BeneficiaryModel() };
            Beneficiaries[0].Index = 0;
            Beneficiaries[0].Prefix = "Beneficiaries[0]";

            CopiePreuveDeces = new FileUploadModel();
        }
        public string InfoFuneraireConnue { get; set; }
        public string NomMaisonFuneraire { get; set; }
        public AddressModel AdresseMaisonFuneraire { get; set; }
        public List<BeneficiaryModel> Beneficiaries { get; set; }
        public bool? BeneficiairesConnus { get; set; }
        public string NumeroContratrenteInd { get; set; }

        public bool Conseiller { get; set; }

        public string prefix { get; set; }

        public string Directives { get; set; }
        public FileUploadModel CopiePreuveDeces { get; set; }

        public string PreviousStep
        {
            get { return "InsuredDeceaseClaim"; }
            private set { }
        }

        public string NextStep
        {
            get { return "Summary"; }
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

            if (string.IsNullOrWhiteSpace(InfoFuneraireConnue))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(InfoFuneraireConnue) }));
            }

            if (InfoFuneraireConnue == "O")
            {
                result.AddRange(AdresseMaisonFuneraire.Validate(nameof(AdresseMaisonFuneraire), true));
            }

            if (!BeneficiairesConnus.HasValue)
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(BeneficiairesConnus) }));
            }

            if (BeneficiairesConnus ?? false)
            {
                for (var i = 0; i < Beneficiaries.Count; i++)
                {
                    if (!Beneficiaries[i].IsDeleted)
                    {
                        result.AddRange(Beneficiaries[i].Validate($"{nameof(Beneficiaries)}[{i}]"));
                    }
                }
            }

            return result;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(BeneficiaryDeceaseClaimModel).Replace("Model", string.Empty));

            xmlElement.AppendChild(helper.CreateElement(nameof(InfoFuneraireConnue), InfoFuneraireConnue));
            xmlElement.AppendChild(helper.CreateElement(nameof(NomMaisonFuneraire), NomMaisonFuneraire));
            xmlElement.AppendChild(helper.CreateElement(nameof(AdresseMaisonFuneraire), AdresseMaisonFuneraire.ToXmlElement(helper)));

            if (BeneficiairesConnus ?? false)
            {
                var xmlBeneficiaries = helper.CreateElement("Beneficiaries");
                xmlElement.AppendChild(xmlBeneficiaries);
                var numeroBeneficiaire = 0;
                foreach (var beneficiary in Beneficiaries)
                {
                    var xmlBeneficiary = helper.CreateElement("Beneficiary");
                    xmlBeneficiaries.AppendChild(xmlBeneficiary);
                    numeroBeneficiaire++;
                    xmlBeneficiary.AppendChild(helper.CreateElement("NumeroBeneficiaire", string.Format("{0}", numeroBeneficiaire)));
                    xmlBeneficiary.AppendChild(helper.CreateElement(nameof(beneficiary.BeneficiaryFirstName), beneficiary.BeneficiaryFirstName));
                    xmlBeneficiary.AppendChild(helper.CreateElement(nameof(beneficiary.BeneficiaryName), beneficiary.BeneficiaryName));
                    xmlBeneficiary.AppendChild(helper.CreateElement(nameof(beneficiary.BirthDate), helper.TransformerDate(beneficiary.BirthDate)));
                    xmlBeneficiary.AppendChild(helper.CreateElement(nameof(beneficiary.IdemAdresseBeneficiaire), beneficiary.IdemAdresseBeneficiaire));
                    if (beneficiary.IdemAdresseBeneficiaire == "N")
                    {
                        xmlBeneficiary.AppendChild(helper.CreateElement(nameof(beneficiary.AdresseBeneficiaire), beneficiary.AdresseBeneficiaire.ToXmlElement(helper)));
                    }
                    if (beneficiary.Transfer == "contrat")
                    {
                        xmlBeneficiary.AppendChild(helper.CreateElement("TransfertRente", "N"));
                        xmlBeneficiary.AppendChild(helper.CreateElement("NumeroContratRenteInd", beneficiary.NumeroContratEpargne));
                    }
                    else if (beneficiary.Transfer == "proposition")
                    {
                        xmlBeneficiary.AppendChild(helper.CreateElement("TransfertRente", "T"));
                        xmlBeneficiary.AppendChild(helper.CreateElement("NumeroPropositionRenteInd", beneficiary.NumeroContratEpargne));
                    }
                    else
                    {
                        xmlBeneficiary.AppendChild(helper.CreateElement("TransfertRente", "X"));
                    }
                }
            }

            xmlElement.AppendChild(helper.CreateElement(nameof(Directives), Directives));
            if (Conseiller)
            {
                if (CopiePreuveDeces.Files.Any())
                {
                    XmlElement XmlPreuveDeces = helper.CreateElement("PreuveDeces");
                    xmlElement.AppendChild(XmlPreuveDeces);
                    XmlPreuveDeces.AppendChild(CopiePreuveDeces.ToXmlElement(helper));
                }
            }

            return xmlElement;
        }
    }
}



