using System;

namespace CLAIM.Models.Shared
{
    [Serializable]
    public class ButtonListModel
    {
        public bool PreviousAction;
        public bool NextAction;
        public bool Transmit;
        public bool Return;
        public bool Cancel;
        public string ReturnUrl;
        public bool Print;
    }

}
