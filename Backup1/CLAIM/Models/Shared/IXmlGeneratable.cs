using System.Xml;
using CLAIM.Helpers.XmlGenerators;

namespace CLAIM.Models.Shared
{
    interface IXmlGeneratable
    {
        void GenerateXml(XmlElement parentElement, XmlHelper helper);
    }
}

