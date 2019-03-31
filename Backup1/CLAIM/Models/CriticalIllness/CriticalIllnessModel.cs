using System;
using CLAIM.Models.Shared;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.CriticalIllness
{
    [Serializable]
    public class CriticalIllnessModel : INavigation
    {
        public CriticalIllnessModel()
        {
            InitializeClaim = new ClaimInitialization();
            AboutInsured = new CriticalIllnessInsured();
            AboutIllness = new AboutIllnessModel();
        }

        public ClaimInitialization InitializeClaim { get; set; }

        public InsuredInformation AboutInsured { get; set; }

        public AboutIllnessModel AboutIllness { get; set; }

        internal CriticalIllnessModel Refine()
        {
            InitializeClaim.Refine();
            AboutInsured.Refine();
            AboutIllness.Refine();

            return this;
        }

        public bool Transmis { get; set; }

        public string PreviousStep
        {
            get { return "AboutIllness"; }
            private set { }
        }

        public string NextStep
        {
            get { return "Confirmation"; }
            private set { }
        }

        public ButtonListModel NavigationButtons
        {
            get
            {

                ConfigurationHelper config = new ConfigurationHelper();

                if (Transmis)
                {
                    return new ButtonListModel { ReturnUrl = config.IACAReturnUrl, Print = true };
                }

                return new ButtonListModel { Cancel = true, PreviousAction = true, Transmit = true, ReturnUrl = config.IACAReturnUrl };
            }
            private set { }
        }

        public ButtonListModel ConfirmationButtons
        {
            get { return new ButtonListModel { Return = true, ReturnUrl = "http://ia.ca/faire-une-reclamation" }; }
            private set { }
        }
    }
}

