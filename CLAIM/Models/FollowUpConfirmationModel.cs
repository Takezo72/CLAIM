using System;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models
{
    [Serializable]
    public class FollowUpConfirmationModel : ConfirmationModel
    {
        public FollowUpConfirmationModel(string trackingNumber)
        {
            ConfigurationHelper helper = new ConfigurationHelper();

            ReturnUrl = string.Format(helper.RequestFollowUpReturnUrl, trackingNumber);
        }
    }
}