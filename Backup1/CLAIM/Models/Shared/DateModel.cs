using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CLAIM.Helpers;
using CLAIM.Ressources.FormTexts;
using System.Linq;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class DateModel : IValidatableSubModel
    {
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        public int YearMin { get; set; }
        public int YearMax { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Day) && string.IsNullOrEmpty(Year))
            {
                return "";
            }
            return $"{Day}-{Month}-{Year}";
        }

        public DateModel()
        {
            YearMin = DateTime.Now.Year - 100;
            YearMax = DateTime.Now.Year + 2;
            ResetErrorMessage();
        }

        public void ResetErrorMessage()
        {
            ErrorMessage = ErrorMessages.Invalid_DateFormat;
        }

        public DateTime ToDate()
        {
            DateTime dateResult;

            string[] formats = { "d-MM-yyyy", "dd-MM-yyyy" };

            DateTime.TryParseExact(ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult);

            return dateResult;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Day) && string.IsNullOrEmpty(Year);
        }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            if (!isRequired && IsEmpty())
            {
                return result;
            }

            if ((isRequired && IsEmpty()) || !ValidationHelper.IsDateValid(ToString(), YearMin, YearMax))
            {
                result.Add(InvalidResult(instanceName));
            }

            return result;
        }

        public IEnumerable<ValidationResult> ValidatePastDate(string instanceName, bool isRequired = false)
        {
            var validations = Validate(instanceName, isRequired).ToList();
            if (!validations.Any() && ToString() != "")
            {
                if (ToDate() > DateTime.Now)
                {
                    validations.Add(InvalidResult(instanceName));
                    ErrorMessage = ErrorMessages.Future_Date;
                }
                else
                {
                    ResetErrorMessage();
                }
            }
            return validations;
        }

        public IEnumerable<ValidationResult> ValidateFutureDate(string instanceName, bool isRequired = false)
        {
            var validations = Validate(instanceName, isRequired).ToList();
            if (!validations.Any() && ToString() != "")
            {
                if (ToDate() < DateTime.Now)
                {
                    validations.Add(InvalidResult(instanceName));
                    ErrorMessage = ErrorMessages.Past_Date;
                }
                else
                {
                    ResetErrorMessage();
                }
            }
            return validations;
        }

        private ValidationResult InvalidResult(string instanceName)
        {
            return new ValidationResult(string.Empty, new[]
            {
                $"{instanceName}.{nameof(Day)}",
                $"{instanceName}.{nameof(Month)}",
                $"{instanceName}.{nameof(Year)}"
            });
        }


        public static DateModel CreateBirthDateModel()
        {
            return new DateModel()
            {
                YearMax = DateTime.Now.Year,
                YearMin = DateTime.Now.Year - 150
            };
        }

        public static DateModel CreateFutureDateModel()
        {
            return new DateModel()
            {
                YearMax = DateTime.Now.Year + 3,
                YearMin = DateTime.Now.Year
            };
        }


        public static DateModel CreateLastFiveYearsDateModel()
        {
            return new DateModel()
            {
                YearMax = DateTime.Now.Year,
                YearMin = DateTime.Now.Year - 6
            };
        }

        public static DateModel CreateNeighboringDateModel()
        {
            return new DateModel()
            {
                YearMax = DateTime.Now.Year + 2,
                YearMin = DateTime.Now.Year - 6
            };
        }

        public static DateModel CreatePastDateModel()
        {
            return new DateModel()
            {
                YearMax = DateTime.Now.Year,
                YearMin = DateTime.Now.Year - 100
            };
        }
    }
}

