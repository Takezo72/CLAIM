using CLAIM.Helpers.XmlGenerators;
using CLAIM.Models.Shared;
using System;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.AcceleratedBenefit
{
    [Serializable]
    public class AcceleratedBenefitModel : INavigation
    {
        public AboutBenefitModel AboutBenefit { get; set; }
        public InsuredInformation AboutInsured { get; set; }
        public ClaimInitialization InitializeAccelerated { get; set; }
        public bool Transmitted { get; set; }

        public bool IsInsured => InitializeAccelerated.UserIsInsured ?? false;
        public string InsuredFirstName => InitializeAccelerated.InsuredFirstName;
        public string PreviousStep => "AboutBenefit";
        public string NextStep => "Confirmation";

        public AcceleratedBenefitModel()
        {
            AboutBenefit = new AboutBenefitModel();
            AboutInsured = new AcceleratedBenefitInsured();
            InitializeAccelerated = new ClaimInitialization();
            Transmitted = false;
        }

        public void Transmit()
        {
            var generator = new AcceleratedBenefitXmlGenerator();
            byte[] form = generator.Generate(this);
            //System.Web.Services.FormFollowUpService.SendForm(form);
            Transmitted = true;
        }

        public AcceleratedBenefitModel Refine()
        {
            InitializeAccelerated.Refine();
            AboutInsured.Refine();
            return this;
        }

        public ButtonListModel NavigationButtons
        {
            get
            {
                ConfigurationHelper config = new ConfigurationHelper();

                if (Transmitted)
                {
                    return new ButtonListModel { ReturnUrl = config.IACAReturnUrl, Print = true };
                }

                return new ButtonListModel { Cancel = true, PreviousAction = true, Transmit = true, ReturnUrl = config.IACAReturnUrl };
            }
        }
    }
}

