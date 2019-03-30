using CLAIM.Models.Shared;
using CLAIM.Models.Decease;
using System;
using System.Globalization;
using System.Text;
using CLAIM.Ressources.FormTexts;


namespace CLAIM.Helpers
{
    public class FormatHelper
    {
        public static string FormatDate(DateModel dateToFormat)
        {
            return FormatDate(dateToFormat.ToString());
        }

        public static string FormatDate(string dateToFormat)
        {
            string dateFormated = dateToFormat;

            DateTime dateReceived;
            string[] formats = new[] { "dd/MM/yyyy", "dd-MM-yyyy", "d-MM-yyyy", "yyyy/MM/dd", "yyyy-MM-dd" };

            if (DateTime.TryParseExact(dateToFormat, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateReceived))
            {
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToUpper() == "EN")
                {
                    dateFormated = dateReceived.ToString("MMMM dd, yyyy");
                }
                else
                {
                    dateFormated = dateReceived.ToString("dd MMMM yyyy");
                }

            }

            return dateFormated;
        }

        public static string FormatPhysician(SpecialistPhysician model)
        {
            var sb = new StringBuilder();

            sb.Append($"{model.FirstName} {model.LastName}");
            sb.Append($"<br />{model.HealthcareFacility}, {model.City}");
            if (model.DisplayFirstConsultationDate)
            {
                sb.Append($"<br />{UIPhysician.FirstConsultationDate}: {FormatDate(model.FirstConsultationDate)}");
            }

            return sb.ToString();
        }

        public static string FormatPhysician(Specialist model)
        {
            var sb = new StringBuilder();

            sb.Append($"{model.FirstName} {model.LastName}");
            sb.Append($"<br />{model.Specialty}");
            sb.Append($"<br />{model.HealthcareFacility}, {model.City}");
            if (model.DisplayFirstConsultationDate)
            {
                sb.Append($"<br />{UIPhysician.FirstConsultationDate}: {FormatDate(model.FirstConsultationDate)}");
            }

            return sb.ToString();
        }

        public static string FormatBeneficiary(BeneficiaryModel model)
        {
            var sb = new StringBuilder();

            if (model.AdresseBeneficiaire.IsCanada())
            {
                sb.Append($"{model.BeneficiaryFirstName} {model.BeneficiaryName}");
                sb.Append($"<br />{model.AdresseBeneficiaire.Number} {model.AdresseBeneficiaire.Street} {model.AdresseBeneficiaire.Apartment}");
                sb.Append($"<br />{model.AdresseBeneficiaire.City} {model.AdresseBeneficiaire.Province}");
                sb.Append($"<br />{model.AdresseBeneficiaire.FormattedPostalCode}");
                if (!(string.IsNullOrWhiteSpace(model.AdresseBeneficiaire.PostalBox)) || !(string.IsNullOrWhiteSpace(model.AdresseBeneficiaire.Station)) ||
                    !(string.IsNullOrWhiteSpace(model.AdresseBeneficiaire.RuralRoute)))
                {
                    sb.Append($"<br />{model.AdresseBeneficiaire.PostalBox} {model.AdresseBeneficiaire.Station} {model.AdresseBeneficiaire.RuralRoute}");
                }
                sb.Append($"<br />{model.AdresseBeneficiaire.Country}");
                sb.Append($"<br />{model.AdresseBeneficiaire.MainPhoneNumber} {model.AdresseBeneficiaire.MainPhoneExtension}");
                sb.Append($"<br />{model.AdresseBeneficiaire.SecondaryPhoneNumber} {model.AdresseBeneficiaire.SecondaryPhoneExtension}");
            }
            else
            {
                sb.Append($"{model.BeneficiaryFirstName} {model.BeneficiaryName}");
                sb.Append($"<br />{model.AdresseBeneficiaire.AddressOutsideCanada}");
                sb.Append($"<br />{model.AdresseBeneficiaire.PhoneNumberOutsideCanada}");
            }
            return sb.ToString();
        }
    }
}

