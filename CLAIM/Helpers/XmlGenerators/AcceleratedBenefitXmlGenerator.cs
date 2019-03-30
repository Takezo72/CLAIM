using CLAIM.Models.AcceleratedBenefit;
using NLog;
using System.Globalization;
using CLAIM.Helpers.Configuration;
using System.Xml;

namespace CLAIM.Helpers.XmlGenerators
{
    public class AcceleratedBenefitXmlGenerator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal byte[] Generate(AcceleratedBenefitModel claimData)
        {
            XmlHelper helper = new XmlHelper("RG_PRA_DECL_INIT_V1", CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower());

            helper.AddElement(nameof(AcceleratedBenefitModel).Replace("Model", string.Empty),
                new System.Collections.Generic.List<XmlElement>
                {
                    claimData.InitializeAccelerated.ToXmlElement(helper),
                    claimData.AboutInsured.ToXmlElement(helper),
                    claimData.AboutBenefit.ToXmlElement(helper)
                });


            ConfigurationHelper config = new ConfigurationHelper();

            if (config.IsMockUserIdentity)
            {
                logger.Info(string.Format("MOCK USER ACTIVATED! XML saved to disk."));
                helper.Save("d:\\testAcceleratedBenefit.xml");
            }

            return helper.Extract();
        }
    }
}
