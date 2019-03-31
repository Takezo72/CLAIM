using CLAIM.Models.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace CLAIM.Helpers.XmlGenerators
{
    public class XmlHelper
    {
        private XmlDocument Document { get; set; }
        public XmlHelper(string formName, string language, string trackingNumber = "", string eventId = "", string attachmentId = "")
        {
            InitiateDocument();
            GenerateHeader(formName, language, trackingNumber, eventId, attachmentId);
            InitiateBodyGenerateHeader();
        }

        public void AddFiles(FileUploadModel fileUpload, string urlDepot, string codeCategory)
        {
            AddFiles(fileUpload.Files, fileUpload.ClientId, urlDepot, codeCategory);
        }

        public void AddFiles(IEnumerable<FileModel> fileList, string clientId, string urlDepot, string codeCategory)
        {
            XmlNode liste = GetParentNode("ListeDocuments");
            XmlNode items = liste.SelectSingleNode("Items");
            if (items == null)
            {
                items = CreateElement("Items");
                liste.AppendChild(items);
            }

            foreach (FileModel file in fileList.Where(f => string.IsNullOrEmpty(f.ErrorMessage)))
            {
                items.AppendChild(file.ToFileXmlElement(this, clientId, urlDepot, codeCategory));
            }
        }

        private void InitiateBodyGenerateHeader()
        {
            Document.DocumentElement.AppendChild(Document.CreateElement("Donnees"));
        }

        private XmlNode GetParentNode(string parentName)
        {
            var parent = Document.SelectSingleNode($"//Formulaire/Donnees/{parentName}");
            if (parent != null) return parent;

            parent = Document.CreateElement(parentName);
            Document.SelectSingleNode("//Formulaire/Donnees").AppendChild(parent);
            return parent;
        }

        public void AddElement(string parentName, string elementName, string value)
        {
            GetParentNode(parentName).AppendChild(CreateElement(elementName, value));
        }

        public void AddElement(string parentName, XmlElement element)
        {
            GetParentNode(parentName).AppendChild(element);
        }

        public void AddElement(string parentName, IEnumerable<XmlElement> elements)
        {
            var parent = GetParentNode(parentName);
            foreach (var element in elements)
            {
                parent.AppendChild(element);
            }
        }

        public void AppendChildIf(XmlElement element, bool condition, string childName, string childText)
        {
            if (!condition) return;
            element.AppendChild(CreateElement(childName, childText));
        }

        private void GenerateHeader(string formName, string language, string trackingNumber, string eventId, string attachmentId)
        {
            var headerXml = Document.CreateElement("Entete");
            headerXml.AppendChild(CreateElement("Nom", formName));
            headerXml.AppendChild(CreateElement("Date", DateTime.Now.ToString("MM'/'dd'/'yyyy HH:mm:ss")));
            headerXml.AppendChild(CreateElement("Langue", language));

            if (!string.IsNullOrEmpty(trackingNumber))
            {
                headerXml.AppendChild(CreateElement("TrackingNumber", trackingNumber));
                headerXml.AppendChild(CreateElement("EventId", eventId));
                headerXml.AppendChild(CreateElement("AttachmentId", attachmentId));
            }

            Document.DocumentElement.AppendChild(headerXml);
        }

        private void InitiateDocument()
        {
            Document = new XmlDocument();
            Document.AppendChild(Document.CreateXmlDeclaration("1.0", "UTF-8", ""));
            Document.AppendChild(Document.CreateElement("Formulaire"));
        }

        public XmlElement CreateElement(string name)
        {
            return Document.CreateElement(name);
        }

        public XmlElement CreateElement(string name, string innerText)
        {
            var element = Document.CreateElement(name);
            element.InnerText = innerText;
            return element;
        }

        public XmlElement CreateElement(string name, XmlElement childNode)
        {
            var element = Document.CreateElement(name);
            element.AppendChild(childNode);
            return element;
        }

        public XmlElement CreateElement(string name, IEnumerable<XmlElement> childNodes)
        {
            var element = Document.CreateElement(name);
            foreach (var child in childNodes)
            {
                element.AppendChild(child);
            }
            return element;
        }

        public XmlAttribute CreateAttribute(string name)
        {
            return Document.CreateAttribute(name);
        }

        public void Save(string fileName)
        {
            Document.Save(fileName);
        }

        public byte[] Extract()
        {
            return Encoding.UTF8.GetBytes(Document.OuterXml);
        }

        public string TransformerDate(DateModel date)
        {
            return TransformerDate(date.ToString());
        }

        public string TransformerDate(string date)
        {
            var dateTranformee = date;

            DateTime dateATransformer;

            if (DateTime.TryParseExact(date, new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "d-MM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateATransformer))
            {
                dateTranformee = dateATransformer.ToString("MM'/'dd'/'yyyy HH:mm:ss");
            }

            return dateTranformee;
        }
    }
}

