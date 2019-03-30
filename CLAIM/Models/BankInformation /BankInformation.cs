using System;
using CLAIM.Models.Shared;

namespace CLAIM.Models.BankInformation
{
    [Serializable]
    public class BankInformationModel
    {
        public InformationsClient InfosClient { get; set; }
        public RequestFollowUpModel InfosResquestFollowUp { get; set; }

        public FileUploadModel PhotoSpecimen { get; set; }

        public bool Transmis { get; set; }

        public BankInformationModel()
        {
            PhotoSpecimen = new FileUploadModel();
            ObtenirInfosClient();
        }

        private void ObtenirInfosClient()
        {
            InfosClient = new InformationsClient();
        }

        internal bool IsTransmitted()
        {
            return Transmis;
        }
    }





}

