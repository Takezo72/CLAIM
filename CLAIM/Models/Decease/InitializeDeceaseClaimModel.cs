using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CLAIM.Models.Shared;
using System.Xml;
using CLAIM.Helpers.XmlGenerators;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.Decease
{
    [Serializable]
    public class InitializeDeceaseClaimModel : IValidatableObject
    {
        public InitializeDeceaseClaimModel()
        {

        }


        public string PreviousStep
        {
            get { return string.Empty; }
            private set { }
        }

        public string NextStep
        {
            get { return "AskingDeceaseClaim"; }
            private set { }
        }

        public ButtonListModel NavigationButtons
        {
            get
            {
                ConfigurationHelper config = new ConfigurationHelper();
                return new ButtonListModel { NextAction = true, Cancel = true, ReturnUrl = config.IACAReturnUrl };
            }
            private set { }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();



            return result;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(InitializeDeceaseClaimModel).Replace("Model", string.Empty));


            return xmlElement;
        }
    }

}

