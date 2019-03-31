using System;
using System.Collections.Generic;
using System.Linq;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Models.BankInformation
{
    [Serializable]
    public class InformationsClient
    {
        public string Identifiant;
        public string Nom;
        public string Prenom;

        public string CompteSelectionne { get; set; }
        public List<CompteBancaire> Comptes { get; set; }

        public CompteBancaire NouveauCompte { get; set; }

        public InformationsClient()
        {
            //WE.Security.Profile.UserProfile userProfile = Services.UserProfileServices.GetUserProfile();

            //Identifiant = userProfile.AccessCode;
            Identifiant = "Manu";

            //Nom = userProfile.LastName;
            Nom = "Batot";
            //Prenom = userProfile.FirstName;
            Prenom = "Emmanuel";

            NouveauCompte = new CompteBancaire();

            ChargerComptes();
        }

        private void ChargerComptes()
        {
            Comptes = new List<CompteBancaire>();

            //string[] contracts = Services.ClientContractServices.GetClientContracts(Services.UserProfileServices.GetUserProfile().ClientNumber, Services.UserIdentityServices.GetUserIdentity().Context.Company);
            string[] contracts = new string [] { "12345", "98765" };

            ConfigurationHelper config = new ConfigurationHelper();

            foreach (string contract in contracts)
            {
                //CompteBancaire account = Services.BankInformationServices.GetBankAccounts(contract);
                CompteBancaire account = new CompteBancaire {ClientId = "12345", NomClient = "Batot", PrenomClient = "Emmanuel" };


                if (account != null)
                {
                    if (!Comptes.Any(c => c.Description == account.Description))
                    {
                        account.NomClient = Nom;
                        account.PrenomClient = Prenom;
                        account.Identifiant = Comptes.Count + 1;
                        Comptes.Add(account);
                    }

                }
            }
        }

        public string NomComplet()
        {
            return this.Prenom + " " + this.Nom;
        }
    }
}

