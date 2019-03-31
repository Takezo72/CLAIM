using System.Collections.Generic;
using CLAIM.Models.Decease;
using System.Xml;
using NLog;
using System.Globalization;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Helpers.XmlGenerators
{
    public class DeceaseClaimXmlGenerator
    {
        private const string CATEGORY_COPIE_MANDAT = "RG003";
        private const string CATEGORY_CESSION_FUNERAIRE = "RG003";
        private const string CATEGORY_CESSION_FINANCIERE = "RG003";
        private const string CATEGORY_CONFIRMATION = "RG003";
        private const string CATEGORY_CERTIFICAT_NAISSANCE = "RG003";
        private const string CATEGORY_ACTE_TUTELLE = "RG003";
        private const string CATEGORY_ACTE_FIDUCIE = "RG003";
        private const string CATEGORY_COPIE_PREUVE_DECES = "RG003";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal byte[] Generate(DeceaseModel claimData, string urlDepot)
        {
            var helper = new XmlHelper("RG_DCS_DECL_INIT_V1", CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower());
            helper.AddElement(nameof(DeceaseModel).Replace("Model", string.Empty),
                new List<XmlElement>
                {
                    claimData.AskingDeceaseClaim.ToXmlElement(helper),
                    claimData.InsuredDeceaseClaim.ToXmlElement(helper),
                    claimData.BeneficiaryDeceaseClaim.ToXmlElement(helper)
                });

            helper.AddFiles(claimData.AskingDeceaseClaim.CopieMandat, urlDepot, CATEGORY_COPIE_MANDAT);
            helper.AddFiles(claimData.AskingDeceaseClaim.CessionFuneraire, urlDepot, CATEGORY_CESSION_FUNERAIRE);
            helper.AddFiles(claimData.AskingDeceaseClaim.CessionFinanciere, urlDepot, CATEGORY_CESSION_FINANCIERE);
            helper.AddFiles(claimData.AskingDeceaseClaim.Confirmation, urlDepot, CATEGORY_CONFIRMATION);
            helper.AddFiles(claimData.AskingDeceaseClaim.CertificatNaissance, urlDepot, CATEGORY_CERTIFICAT_NAISSANCE);
            helper.AddFiles(claimData.AskingDeceaseClaim.ActeTutelle, urlDepot, CATEGORY_ACTE_TUTELLE);
            helper.AddFiles(claimData.AskingDeceaseClaim.ActeFiducie, urlDepot, CATEGORY_ACTE_FIDUCIE);
            helper.AddFiles(claimData.BeneficiaryDeceaseClaim.CopiePreuveDeces, urlDepot, CATEGORY_COPIE_PREUVE_DECES);

            var config = new ConfigurationHelper();

            if (config.IsMockUserIdentity)
            {
                logger.Info(string.Format("MOCK USER ACTIVATED! XML saved to disk."));
                helper.Save("d:\\testDeceaseClaim.xml");
            }

            return helper.Extract();
        }

    }
}

