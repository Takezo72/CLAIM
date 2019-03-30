using System;

namespace IAFG.IA.VI.VIMWPNP2.Models.Shared
{
    [Serializable]
    public class RequestFollowUpModel
    {
        public string TrackingNumber { get; set; }
        public string EventId { get; set; }
        public string AttachmentId { get; set; }
        public int? ClaimType { get; set; }

        public bool IsCompleted { get; set; }
        public string MessageComplete { get; set; }
    }
}

