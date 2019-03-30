using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAFG.IA.VI.VIMWPNP2.Models
{
    [Serializable]
    public class ConfirmationModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string MessageReception { get; set; }
        public bool Succes { get; set; }
        public string ReturnUrl { get; set; }

        public int StepNumber { get; set; }
    }
}
