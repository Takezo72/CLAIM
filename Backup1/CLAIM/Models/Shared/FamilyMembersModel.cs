using CLAIM.Helpers.XmlGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CLAIM.Helpers.Configuration;
using System.Xml;

namespace CLAIM.Models.Shared
{

    [Serializable]
    public class FamilyMembersModel : IValidatableSubModel
    {
        public List<FamilyMemberWithSameProblemModel> FamilyMembers { get; set; }

        public FamilyMembersModel()
        {
            List<FamilyMemberWithSameProblemModel> list = new List<FamilyMemberWithSameProblemModel>();

            list.Add(new FamilyMemberWithSameProblemModel());


            FamilyMembers = list;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            XmlElement xmlElement = helper.CreateElement(nameof(FamilyMembers));

            foreach (FamilyMemberWithSameProblemModel member in FamilyMembers)
            {
                xmlElement.AppendChild(member.ToXmlElement(helper));
            }

            return xmlElement;
        }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            ConfigurationHelper config = new ConfigurationHelper();

            for (int index = 0; index < FamilyMembers.Count(); index++)
            {
                result.AddRange(FamilyMembers[index].Validate(string.Format("{0}.{1}[{2}]", instanceName, nameof(FamilyMembers), index), true));
            }

            return result;
        }

        internal void Refine(bool required)
        {
            foreach (FamilyMemberWithSameProblemModel member in FamilyMembers)
            {
                member.Refine(required);
            }
        }
    }
}

