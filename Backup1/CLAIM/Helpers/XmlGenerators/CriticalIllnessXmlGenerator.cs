using System.Collections.Generic;
using NLog;
using System.Globalization;
using System.Xml;
using CLAIM.Models.CriticalIllness;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Helpers.XmlGenerators
{
    public class CriticalIllnessXmlGenerator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal byte[] Generate(CriticalIllnessModel claimData)
        {
            var helper = new XmlHelper("RG_MGR_DECL_INIT_V1", CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower());
            helper.AddElement(nameof(CriticalIllnessModel).Replace("Model", string.Empty),
                new List<XmlElement>
                {
                    claimData.InitializeClaim.ToXmlElement(helper),
                    claimData.AboutInsured.ToXmlElement(helper),
                    claimData.AboutIllness.ToXmlElement(helper)
                });

            var config = new ConfigurationHelper();
            if (config.IsMockUserIdentity)
            {
                logger.Info("MOCK USER ACTIVATED! XML saved to disk.");
                helper.Save("d:\\testCriticalIllness.xml");
            }

            return helper.Extract();
        }
    }
}
