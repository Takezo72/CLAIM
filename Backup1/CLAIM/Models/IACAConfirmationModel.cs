using CLAIM.Helpers.Configuration;
using System;

namespace CLAIM.Models
{
    [Serializable]
    public class IACAConfirmationModel : ConfirmationModel
    {
        public IACAConfirmationModel()
        {
            ConfigurationHelper helper = new ConfigurationHelper();

            ReturnUrl = string.Empty; //string.Format(helper.IACAReturnUrl);
        }
    }
}