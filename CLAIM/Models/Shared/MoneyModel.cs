using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace IAFG.IA.VI.VIMWPNP2.Models.Shared
{
    [Serializable]
    public class MoneyModel : IValidatableSubModel
    {
        public string AmountInput { get; set; }
        public long Min { get; set; }
        public long Max { get; set; }
        public int MaxLength { get; set; }
        public bool AcceptDecimals { get; set; }

        public decimal? GetAmount()
        {
            decimal amount;
            if (AmountInput != null)
            {
                //Essayer dans les deux langues.
                if (!decimal.TryParse(AmountInput.Replace(',', '.'), out amount))
                {
                    if (!decimal.TryParse(AmountInput.Replace('.', ','), out amount))
                    {
                        return null;
                    }
                }

                if (!AcceptDecimals)
                {
                    return (decimal)Math.Floor(amount);
                }

                return (decimal)Math.Round(amount, 2);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var results = new List<ValidationResult>();
            if (isRequired && GetAmount() == null)
            {

                results.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(AmountInput)}" }));
            }
            else
            {
                if (GetAmount() > Max || GetAmount() < Min)
                {
                    results.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(AmountInput)}" }));
                }
            }

            return results;
        }

        public string ToFormattedString()
        {
            if (GetAmount() == null)
            {
                return "";
            }

            return ((float)GetAmount()).ToString("c", CultureInfo.CurrentCulture.NumberFormat);
        }
    }
}

