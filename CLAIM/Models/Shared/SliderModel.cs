using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Web;

namespace IAFG.IA.VI.VIMWPNP2.Models.Shared
{
    [Serializable]
    public class SliderModel : IValidatableSubModel
    {
        public string Name { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int StepValue { get; set; }

        public string BracketTitleResource { get; set; }

        public string BracketTitle()
        {
            if (BracketTitleResource == null)
            {
                return null;
            }

            return BracketResourceManager.GetString(BracketTitleResource);
        }

        public ResourceManager BracketResourceManager { get; set; }


        public IEnumerable<SliderBracket> Brackets { get; set; }


        public string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            if (isRequired && string.IsNullOrEmpty(Value))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.Value" }));
            }
            else
            {
                int val = 0;

                if (!int.TryParse(Value, out val))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.Value" }));
                }
                else
                {
                    if (val < MinValue || val > MaxValue)
                    {
                        result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.Value" }));
                    }
                }
            }

            return result;
        }
    }
}

