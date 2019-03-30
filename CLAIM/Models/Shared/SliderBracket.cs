using System;
using System.Resources;

namespace IAFG.IA.VI.VIMWPNP2.Models.Shared
{
    [Serializable]
    public class SliderBracket
    {
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public string BracketNameResource { get; set; }

        public string BracketName()
        {
            if (BracketResourceManager == null)
            {
                return null;
            }
            return BracketResourceManager.GetString(BracketNameResource);
        }

        public ResourceManager BracketResourceManager { get; set; }
    }
}
