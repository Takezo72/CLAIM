using CLAIM.Helpers.XmlGenerators;
using System;
using System.Collections.Generic;
using System.Xml;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using System.IO;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class FileModel : IValidatableSubModel
    {
        private const string DEFAULT_CATEGORY = "RG009";
        public string Id { get; set; }
        public string FileName { get; set; }
        public string ErrorMessage { get; set; }

        public string Category { get; set; }

        public FileModel()
        {
            Id = "";
            FileName = "";
            ErrorMessage = "";
        }

        public FileModel(JObject newFile)
        {
            FileName = (string)newFile.Property(nameof(FileName)).Value;
            ErrorMessage = (string)newFile.Property(nameof(ErrorMessage)).Value;
            Id = (string)newFile.Property(nameof(Id)).Value;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            XmlElement xmlElement = helper.CreateElement(nameof(FileName), FileName);

            return helper.CreateElement("File", xmlElement);
        }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            if (isRequired && string.IsNullOrEmpty(Id))
            {
                result.Add(new ValidationResult(string.Empty, new[] { $"{instanceName}.{nameof(FileName)}" }));
            }

            return result;
        }

        internal XmlNode ToFileXmlElement(XmlHelper helper, string clientId, string urlDepot, string codeCategory)
        {
            XmlNode item = helper.CreateElement("Item");

            XmlElement type = helper.CreateElement("Type");
            type.InnerText = Path.GetExtension(FileName).Replace(".", "");

            XmlElement fileName = helper.CreateElement("file_name");
            fileName.InnerText = FileName;

            XmlElement url = helper.CreateElement("url");
            url.InnerText = string.Format(urlDepot, clientId, Id);

            XmlElement transfertResult = helper.CreateElement("transfert_result");
            transfertResult.InnerText = "True";

            XmlElement category = helper.CreateElement("categorie");

            if (string.IsNullOrEmpty(codeCategory))
            {
                category.InnerText = DEFAULT_CATEGORY;
            }
            else
            {
                category.InnerText = codeCategory;
            }

            item.AppendChild(type);
            item.AppendChild(fileName);
            item.AppendChild(url);
            item.AppendChild(transfertResult);
            item.AppendChild(category);

            return item;
        }
    }
}
