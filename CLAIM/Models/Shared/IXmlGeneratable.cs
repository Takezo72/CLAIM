using System.Xml;
using IAFG.IA.VI.VIMWPNP2.Helpers.XmlGenerators;

namespace IAFG.IA.VI.VIMWPNP2.Models.Shared
{
    interface IXmlGeneratable
    {
        void GenerateXml(XmlElement parentElement, XmlHelper helper);
    }
}

