using CLAIM.Models.BankInformation;
using CLAIM.Models.Shared;
using NLog;
using System.Globalization;
using System.Xml;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Helpers.XmlGenerators
{
    public class BankInformationXmlGenerator
    {
        private const string CATEGORY_SPECIMEN = "VI409";
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal byte[] Generate(CompteBancaire compteClient, RequestFollowUpModel infosResquestFollowUp, string urlDepot)
        {
            XmlHelper helper = new XmlHelper("RG_INV_BANK_INFO_V1", CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower(), infosResquestFollowUp.TrackingNumber, infosResquestFollowUp.EventId, infosResquestFollowUp.AttachmentId);

            helper.AddElement("Client", "Prenom", compteClient.PrenomClient);
            helper.AddElement("Client", "Nom", compteClient.NomClient);

            if (compteClient.CompteSelectionne == "autre")
            {
                helper.AddElement("Coordonnees", "CompteChoisi", "autre");
            }
            else
            {
                helper.AddElement("Coordonnees", "CompteChoisi", compteClient.Description);
            }

            helper.AddElement("Coordonnees", "Titulaire", compteClient.NomTitulaires);

            helper.AddElement("Coordonnees", "Photo", compteClient.EstPhoto.ToString());

            if (!compteClient.EstPhoto)
            {
                helper.AddElement("Coordonnees", "Transit", compteClient.NumeroTransit);
                helper.AddElement("Coordonnees", "Institution", compteClient.NumeroInstitution);
                helper.AddElement("Coordonnees", "Compte", compteClient.NumeroCompte);
            }
            else
            {
                XmlElement files = helper.CreateElement("Files");

                foreach (FileModel file in compteClient.PhotoSpecimen)
                {
                    files.AppendChild(file.ToXmlElement(helper));
                }
                helper.AddElement("Coordonnees", files);

                helper.AddFiles(compteClient.PhotoSpecimen, compteClient.ClientId, urlDepot, CATEGORY_SPECIMEN);
            }

            ConfigurationHelper config = new ConfigurationHelper();

            if (config.IsMockUserIdentity)
            {
                logger.Info(string.Format("MOCK USER ACTIVATED! XML saved to disk."));
                helper.Save("d:\\testBankInfo.xml");
            }

            return helper.Extract();
        }
    }
}
