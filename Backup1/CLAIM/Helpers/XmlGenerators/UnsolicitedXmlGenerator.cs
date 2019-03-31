using NLog;
using System.Globalization;
using CLAIM.Models;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Helpers.XmlGenerators
{
    public class UnsolicitedXmlGenerator
    {
        private const string CATEGORY_UNSOLICITED_INFO = "RG009";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal byte[] Generate(UnsolicitedModel model, string urlDepot)
        {
            var helper = new XmlHelper("RG_INV_INFO_NDEM_V1",
                CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower(),
                model.RequestFollowUp.TrackingNumber,
                model.RequestFollowUp.EventId,
                model.RequestFollowUp.AttachmentId);

            helper.AddElement("ClaimType", "ClaimType", model.RequestFollowUp.ClaimType.ToString());

            GenerateXmlRetourTravail(model, helper);
            GenerateXmlChirurgie(model, helper);
            GenerateXmlConsultation(model, helper);

            helper.AddElement("Autre", "Informations", model.AutresInformations);
            helper.AddFiles(model.File, urlDepot, CATEGORY_UNSOLICITED_INFO);

            var config = new ConfigurationHelper();

            if (!config.IsMockUserIdentity) return helper.Extract();

            Logger.Info("MOCK USER ACTIVATED! XML saved to disk.");
            helper.Save(@"d:\testUnsolicited.xml");

            return helper.Extract();
        }

        private static void GenerateXmlRetourTravail(UnsolicitedModel model, XmlHelper helper)
        {
            helper.AddElement("RetourTravail", "RetourAuTravail", model.RetourAuTravail.ToString());
            helper.AddElement("RetourTravail", "DateRetourAuTravail", helper.TransformerDate(model.DateRetourAuTravail));
        }

        private static void GenerateXmlChirurgie(UnsolicitedModel model, XmlHelper helper)
        {
            helper.AddElement("Chirurgie", "Chirurgie", model.Chirurgie.ToString());
            helper.AddElement("Chirurgie", "DateChirurgie", helper.TransformerDate(model.DateChirurgie));
            helper.AddElement("Chirurgie", "TypeChirurgie", model.TypeChirurgie);
        }

        private static void GenerateXmlConsultation(UnsolicitedModel model, XmlHelper helper)
        {
            helper.AddElement("Consultation", "Consultation", model.Consultation.ToString());
            helper.AddElement("Consultation", "DateConsultation", helper.TransformerDate(model.DateConsultation));
            helper.AddElement("Consultation", "Specialite", model.Specialite);
            helper.AddElement("Consultation", "PrenomMedecin", model.PrenomMedecin);
            helper.AddElement("Consultation", "NomMedecin", model.NomMedecin);
            helper.AddElement("Consultation", "Etablissement", model.Etablissement);
            helper.AddElement("Consultation", "Ville", model.Ville);
        }
    }
}
