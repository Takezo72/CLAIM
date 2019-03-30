using System;
using CLAIM.Models.Shared;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.Decease
{
    [Serializable]
    public class DeceaseModel
    {
        public DeceaseModel()
        {
            //InitializeDeceaseClaim = new InitializeDeceaseClaimModel();
            AskingDeceaseClaim = new AskingDeceaseClaimModel();
            InsuredDeceaseClaim = new InsuredDeceaseClaimModel();
            BeneficiaryDeceaseClaim = new BeneficiaryDeceaseClaimModel();
            Transmis = false;
        }

        //public InitializeDeceaseClaimModel InitializeDeceaseClaim { get; set; }

        public AskingDeceaseClaimModel AskingDeceaseClaim { get; set; }

        public InsuredDeceaseClaimModel InsuredDeceaseClaim { get; set; }

        public BeneficiaryDeceaseClaimModel BeneficiaryDeceaseClaim { get; set; }

        public bool Transmis { get; set; }

        public string PreviousStep
        {
            get { return "BeneficiaryDeceaseClaim"; }
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

