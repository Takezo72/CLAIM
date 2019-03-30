using CLAIM.Helpers.XmlGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class FileUploadModel : IValidatableSubModel
    {
        public int NbFilesMax { get; set; }
        public bool FilesInError { get; set; }

        public IEnumerable<FileModel> Files { get; set; }

        public string ClientId { get; set; }

        public FileUploadModel() : this(Guid.NewGuid().ToString())
        {

        }

        public FileUploadModel(string id)
        {
            FileUploadConfigurationHelper config = new FileUploadConfigurationHelper();

            NbFilesMax = config.MaxNumberFile;
            FilesInError = false;

            Files = new List<FileModel>();

            ClientId = id;
        }

        public XmlElement ToXmlElement(XmlHelper helper)
        {
            XmlElement xmlElement = helper.CreateElement(nameof(Files));

            foreach (FileModel file in Files)
            {
                xmlElement.AppendChild(file.ToXmlElement(helper));
            }

            return xmlElement;
        }

        public IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            FileUploadConfigurationHelper config = new FileUploadConfigurationHelper();

            if (config.EnableFileUpload)
            {
                if (isRequired && !Files.Any())
                {
                    //Files are never required for the moment. Until the file upload is stable.
                    //result.Add(new ValidationResult("", new[] { $"{instanceName}.MissingFile" }));
                }

                if (FilesInError)
                {
                    result.Add(new ValidationResult("", new[] { $"{instanceName}.FilesInError" }));
                }
            }

            return result;
        }
    }

}

