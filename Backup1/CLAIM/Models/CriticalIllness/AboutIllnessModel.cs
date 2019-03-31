using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml;
using CLAIM.Helpers.XmlGenerators;
using CLAIM.Models.Shared;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.CriticalIllness
{
    [Serializable]
    public class AboutIllnessModel : IValidatableObject, INavigation
    {
        public AboutIllnessModel()
        {
            FirstOnsetDate = DateModel.CreatePastDateModel();
            LastMedicalConsultationDate = DateModel.CreatePastDateModel();

            PhysicianInfos = new List<PhysicianModel>();

            PhysicianInfos.Add(new PhysicianModel());

            FamilyMembers = new FamilyMembersModel();
        }

        public string OnsetDescription { get; set; }
        public List<PhysicianModel> PhysicianInfos { get; set; }
        public string Diagnosis { get; set; }

        public string HasFamilyMemberWithSameProblem { get; set; }
        public FamilyMembersModel FamilyMembers { get; set; }


        public DateModel FirstOnsetDate { get; set; }
        public DateModel LastMedicalConsultationDate { get; set; }

        public string PreviousStep
        {
            get { return "AboutInsured"; }
            private set { }
        }

        public string NextStep
        {
            get { return "Summary"; }
            private set { }
        }

        public ButtonListModel NavigationButtons
        {
            get
            {
                ConfigurationHelper config = new ConfigurationHelper();
                return new ButtonListModel { NextAction = true, Cancel = true, PreviousAction = true, ReturnUrl = config.IACAReturnUrl };
            }
            private set { }
        }

        internal void Refine()
        {
            FamilyMembers.Refine(HasFamilyMemberWithSameProblem == "O");
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            result.AddRange(FirstOnsetDate.ValidatePastDate(nameof(FirstOnsetDate), true));
            result.AddRange(LastMedicalConsultationDate.ValidatePastDate(nameof(LastMedicalConsultationDate), true));

            for (var i = 0; i <= PhysicianInfos.Count - 1; i++)
            {
                result.AddRange(PhysicianInfos[i].Validate($"{nameof(PhysicianInfos)}[{i}]"));
            }

            if (string.IsNullOrWhiteSpace(Diagnosis))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(Diagnosis) }));
            }

            if (string.IsNullOrWhiteSpace(OnsetDescription))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(OnsetDescription) }));
            }

            if (string.IsNullOrWhiteSpace(HasFamilyMemberWithSameProblem))
            {
                result.Add(new ValidationResult(string.Empty, new[] { nameof(HasFamilyMemberWithSameProblem) }));
            }

            if (HasFamilyMemberWithSameProblem == "O")
            {
                result.AddRange(FamilyMembers.Validate(nameof(FamilyMembers), true));
            }

            return result;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(AboutIllnessModel).Replace("Model", string.Empty));
            xmlElement.AppendChild(helper.CreateElement(nameof(FirstOnsetDate), helper.TransformerDate(FirstOnsetDate)));
            xmlElement.AppendChild(helper.CreateElement(nameof(OnsetDescription), OnsetDescription));
            xmlElement.AppendChild(helper.CreateElement(nameof(LastMedicalConsultationDate), helper.TransformerDate(LastMedicalConsultationDate)));

            xmlElement.AppendChild(helper.CreateElement(nameof(PhysicianInfos), PhysicianInfos.Select(x => x.ToXmlElement(helper)).ToList()));

            xmlElement.AppendChild(helper.CreateElement(nameof(Diagnosis), Diagnosis));

            xmlElement.AppendChild(helper.CreateElement(nameof(HasFamilyMemberWithSameProblem), (HasFamilyMemberWithSameProblem == "O").ToString()));
            if (HasFamilyMemberWithSameProblem == "O")
            {
                xmlElement.AppendChild(helper.CreateElement(nameof(FamilyMembers), FamilyMembers.ToXmlElement(helper)));
            }

            return xmlElement;
        }
    }
}

