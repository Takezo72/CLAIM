using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using CLAIM.Helpers.XmlGenerators;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class AddressModel : IValidatableSubModel
    {
        public string CanadaPostAPIKey { get; set; }
        public bool PhoneNumberShownAndRequired { get; set; }
        public string Number { get; set; }
        public string Street { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }

        public string FormattedPostalCode
        {
            get
            {
                if (PostalCode == null)
                {
                    return PostalCode;
                }
                string formattedPostalCode = PostalCode.Replace(" ", "").ToUpper();

                if (formattedPostalCode.Length == 6)
                {
                    formattedPostalCode = formattedPostalCode.Insert(3, " ");
                }

                return formattedPostalCode;
            }
        }

        public string PostalBox { get; set; }
        public string Station { get; set; }
        public string RuralRoute { get; set; }
        public string Country { get; set; } //Sert uniquement pour l'aspect technique. Il n'est pas inséré dans le XML.
        public string MainPhoneNumber { get; set; }
        public string MainPhoneExtension { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string SecondaryPhoneExtension { get; set; }
        public string AddressOutsideCanada { get; set; }
        public string PhoneNumberOutsideCanada { get; set; }
        public string Prefix { get; set; }

        public bool IsCanada()
        {
            return string.IsNullOrWhiteSpace(Country) || Country == "Canada";
        }

        public override string ToString()
        {
            if (!IsCanada()) return AddressOutsideCanada;

            var apprt = string.Empty;
            if (!string.IsNullOrWhiteSpace(Apartment))
            {
                apprt = string.Concat(Apartment, "-");
            }

            var ruralRoute = string.Empty;
            if (!string.IsNullOrWhiteSpace(RuralRoute))
            {
                ruralRoute = string.Concat(" ", RuralRoute);
            }

            var postalBox = string.Empty;
            if (!string.IsNullOrWhiteSpace(PostalBox))
            {
                postalBox = string.Format("{0} {1} ", Ressources.FormTexts.UIAddress.POBoxPrint, PostalBox);
            }

            var station = string.Empty;
            if (!string.IsNullOrWhiteSpace(Station))
            {
                station = string.Format("{0} {1} ", Ressources.FormTexts.UIAddress.StationPrint, Station);
            }

            var virgule = string.Empty;
            if ((!string.IsNullOrWhiteSpace(Number)) && (!string.IsNullOrWhiteSpace(Street)))
            {
                virgule = ",";
            }
            return $"{postalBox}{station}{apprt}{Number}{ruralRoute}{virgule} {Street}{Environment.NewLine}{City} {Province} {FormattedPostalCode}";
        }


        public string ToHtmlString()
        {
            return ToString().Replace(Environment.NewLine, "<br />");
        }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            var result = new List<ValidationResult>();

            if (IsCanada())
            {
                if (string.IsNullOrWhiteSpace(Number) || string.IsNullOrWhiteSpace(Street))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(Number)}", $"{instanceName}.{nameof(Street)}" }));
                }

                if (string.IsNullOrWhiteSpace(City))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(City)}" }));
                }

                if (string.IsNullOrWhiteSpace(Province))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(Province)}" }));
                }

                if (string.IsNullOrWhiteSpace(PostalCode) || !Helpers.ValidationHelper.IsPostalCodeValid(PostalCode.ToUpper()))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(PostalCode)}" }));
                }

                if (PhoneNumberShownAndRequired)
                {
                    if (string.IsNullOrWhiteSpace(MainPhoneNumber) || !Helpers.ValidationHelper.IsPhoneNumberValid(MainPhoneNumber))
                    {
                        result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(MainPhoneNumber)}" }));
                    }

                    if (!string.IsNullOrWhiteSpace(SecondaryPhoneNumber) && !Helpers.ValidationHelper.IsPhoneNumberValid(SecondaryPhoneNumber))
                    {
                        result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(SecondaryPhoneNumber)}" }));
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(AddressOutsideCanada))
                {
                    result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(AddressOutsideCanada)}" }));
                }

                if (PhoneNumberShownAndRequired)
                {
                    if (string.IsNullOrWhiteSpace(PhoneNumberOutsideCanada))
                    {
                        result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(PhoneNumberOutsideCanada)}" }));
                    }
                }
            }

            return result;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            var xmlElement = helper.CreateElement(nameof(AddressModel).Replace("Model", string.Empty));
            xmlElement.AppendChild(helper.CreateElement("AddressComplete", ToString()));

            xmlElement.AppendChild(helper.CreateElement(nameof(Country), Country));
            if (IsCanada())
            {
                xmlElement.AppendChild(helper.CreateElement(nameof(MainPhoneNumber), MainPhoneNumber));
                xmlElement.AppendChild(helper.CreateElement(nameof(MainPhoneExtension), MainPhoneExtension));
                xmlElement.AppendChild(helper.CreateElement(nameof(SecondaryPhoneNumber), SecondaryPhoneNumber));
                xmlElement.AppendChild(helper.CreateElement(nameof(SecondaryPhoneExtension), SecondaryPhoneExtension));
            }
            else
            {
                xmlElement.AppendChild(helper.CreateElement(nameof(MainPhoneNumber), PhoneNumberOutsideCanada));
            }

            return xmlElement;
        }

        public AddressModel(bool phoneNumberShownAndRequired = true)
        {
            ConfigurationHelper config = new ConfigurationHelper();

            CanadaPostAPIKey = config.CanadaPostAPIKey;
            PhoneNumberShownAndRequired = phoneNumberShownAndRequired;
        }
    }
}

