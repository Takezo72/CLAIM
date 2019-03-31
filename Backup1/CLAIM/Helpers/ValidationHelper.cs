using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CLAIM.Helpers
{
    public class ValidationHelper
    {
        public static bool IsDateValid(string dateToValidate)
        {
            DateTime dateVerification;
            string[] formats = { "dd/MM/yyyy", "d-MM-yyyy", "dd-MM-yyyy", "yyyy/MM/dd", "yyyy-MM-dd" };
            return DateTime.TryParseExact(dateToValidate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVerification);
        }

        public static bool IsDateValid(string dateToValidate, int yearMin, int yearMax)
        {
            DateTime dateVerification;

            string[] formats = { "dd/MM/yyyy", "d-MM-yyyy", "dd-MM-yyyy", "yyyy/MM/dd", "yyyy-MM-dd" };

            if (DateTime.TryParseExact(dateToValidate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVerification))
            {
                return dateVerification.Year >= yearMin && dateVerification.Year <= yearMax;
            }
            else
            {
                return false;
            }
        }

        public static bool IsDateFinSupDebut(string dateDebut, string dateFin)
        {
            string[] formats = { "dd/MM/yyyy", "d/MM/yyyy", "dd-MM-yyyy", "d-MM-yyyy", "yyyy/MM/dd", "yyyy-MM-dd" };
            DateTime dateDebutParse;
            DateTime dateFinParse;
            DateTime.TryParseExact(dateDebut, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateDebutParse);
            DateTime.TryParseExact(dateFin, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFinParse);

            return IsDateFinSupDebut(dateDebutParse, dateFinParse);
        }

        public static bool IsDateFinSupDebut(DateTime dateDebut, DateTime dateFin)
        {
            return (dateFin == default(DateTime)) || (DateTime.Compare(dateDebut, dateFin) <= 0);
        }

        public static bool IsPhoneNumberValid(string phoneNumberToValidate)
        {
            return Regex.IsMatch(phoneNumberToValidate, @"^(\()?([0-9]{3})(\)\s|\)|-)?([0-9]{3})(-)?([0-9]{4})$");
        }

        public static bool IsEmailAdressNumberValid(string emailAdressNumberToValidate)
        {
            return Regex.IsMatch(emailAdressNumberToValidate, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        public static bool IsIntegerValid(string integerToValidate)
        {
            return Regex.IsMatch(integerToValidate, @"^\d+$");
        }

        public static bool IsAmountValid(string amountToValidate)
        {
            return Regex.IsMatch(amountToValidate, @"^(\d+[,.])*\d+$");
        }

        public static bool IsPostalCodeValid(string postalCode)
        {
            return Regex.IsMatch(postalCode, @"^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ][ ]?\d[ABCEGHJKLMNPRSTVWXYZ]\d$");
        }

        public static bool IsYearValid(string year)
        {
            int value;
            if (int.TryParse(year, out value))
            {
                int currentYear = DateTime.Now.Year;
                if (value >= currentYear - 200 && value <= currentYear + 10)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsMonthValid(string month)
        {
            int value;
            if (int.TryParse(month, out value))
            {
                if (value >= 1 && value <= 12)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

