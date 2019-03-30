using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using CLAIM.Helpers.XmlGenerators;
using System;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class TherapyModel : IViewModel, IXmlGeneratable, IValidatableSubModel
    {
        public string ViewName
        {
            get { return "~/Views/Shared/_Therapy.cshtml"; }
        }
        public bool Acupuncture { get; set; }
        public bool Chiropratique { get; set; }
        public bool Ergotherapie { get; set; }
        public bool Physiotherapie { get; set; }
        public bool Psychotherapie { get; set; }
        public bool Therapie_Autre { get; set; }
        public string Therapie_AutrePrecision { get; set; }

        public void GenerateXml(XmlElement parentElement, XmlHelper helper)
        {
            XmlElement therapies = helper.CreateElement("Therapies");
            if (Acupuncture)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Acupuncture";
                therapies.AppendChild(therapie);
            }
            if (Chiropratique)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Chiropratie";
                therapies.AppendChild(therapie);
            }
            if (Ergotherapie)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Ergotherapie";
                therapies.AppendChild(therapie);
            }
            if (Physiotherapie)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Physiotherapie";
                therapies.AppendChild(therapie);
            }
            if (Psychotherapie)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Psychotherapie";
                therapies.AppendChild(therapie);
            }
            if (Therapie_Autre)
            {
                XmlElement therapie = helper.CreateElement("Therapie");
                therapie.InnerText = "Autre";
                therapies.AppendChild(therapie);
            }
            parentElement.AppendChild(therapies);
            XmlElement autreTherapie = helper.CreateElement("AutreTherapie");
            autreTherapie.InnerText = Therapie_AutrePrecision;
            parentElement.AppendChild(autreTherapie);
        }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();
            var isEmpty = IsEmpty();

            if (!isRequired && isEmpty)
            {
                return result;
            }

            if (isEmpty)
            {
                result.Add(new ValidationResult(string.Empty, new[] {
                    $"{instanceName}.{nameof(Acupuncture)}"
                }));
            }

            return result;
        }

        internal bool IsEmpty()
        {
            return !(Acupuncture || Chiropratique || Ergotherapie || Physiotherapie || Psychotherapie || Therapie_Autre);
        }
    }
}


