using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml;
using IAFG.IA.VI.VIMWPNP2.Helpers.XmlGenerators;

namespace IAFG.IA.VI.VIMWPNP2.Models.Shared
{
    [Serializable]
    public class MedicalConsultationModel : IValidatableSubModel
    {
        public MedicalConsultationModel() : this(false)
        {
        }

        public MedicalConsultationModel(bool withOnlyYear)
        {
            if (withOnlyYear)
            {
                Year = new YearModel();
            }
            else
            {
                Periods = new List<MonthYearPeriodModel> { new MonthYearPeriodModel() };
            }
            PhysicianInfos = new PhysicianModel();
            DeceaseCase = withOnlyYear;

        }

        public string Reason { get; set; }
        public List<MonthYearPeriodModel> Periods { get; set; }
        public PhysicianModel PhysicianInfos { get; set; }
        public bool DeceaseCase { get; set; }
        public YearModel Year { get; set; }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Reason))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(Reason)}" }));
            }

            if (DeceaseCase)
            {
                result.AddRange(Year.Validate($"{instanceName}.{nameof(Year)}", true));
            }
            else
            {
                for (var i = 0; i <= Periods.Count - 1; i++)
                {
                    result.AddRange(Periods[i].Validate($"{instanceName}.{nameof(Periods)}[{i}]"));
                }

            }

            result.AddRange(PhysicianInfos.Validate($"{instanceName}.{nameof(PhysicianInfos)}"));

            return result;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(MedicalConsultationModel).Replace("Model", string.Empty));
            xmlElement.AppendChild(helper.CreateElement(nameof(Reason), Reason));
            xmlElement.AppendChild(helper.CreateElement(nameof(Periods), Periods.Select(x => x.ToXmlElement(helper)).ToList()));
            xmlElement.AppendChild(helper.CreateElement(nameof(PhysicianInfos), PhysicianInfos.ToXmlElement(helper)));
            return xmlElement;
        }
    }
}


