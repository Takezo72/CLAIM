using System;
using CLAIM.Models.Shared;
using System.Collections.Generic;

namespace CLAIM.Models.BankInformation
{
    [Serializable]
    public class CompteBancaire
    {
        public int Identifiant { get; set; }
        public int IdentifiantClient { get; set; }
        public string NomClient { get; set; }
        public string PrenomClient { get; set; }



        public string Description { get; set; } //Description du compte client obtenu dans la liste des comptes bancaires existants du client. 

        public string NomGroupe = "groupeComptes";
        public string CompteSelectionne { get; set; }
        public string NomTitulaires { get; set; }
        public string NumeroInstitution { get; set; }
        public string NumeroTransit { get; set; }
        public string NumeroCompte { get; set; }
        public bool EstPhoto { get; set; }
        public string ClientId { get; set; }
        public IEnumerable<FileModel> PhotoSpecimen { get; set; }
    }
}

