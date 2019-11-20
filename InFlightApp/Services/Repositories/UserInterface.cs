using InFlightApp.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Windows.Security.Credentials;

namespace InFlightApp.Services.Repositories
{
    public class UserInterface : IUserInterface{
        private HttpClient client;

        public UserInterface() {
            client = ApiConnection.Client;
        }

        public bool Login(string username, string password){
            string json = JsonConvert.SerializeObject(new { username, password });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync($"{ApiConnection.URL}/Users/crewmember/login", content).Result;

            if (response.StatusCode == HttpStatusCode.BadRequest)
                return false;

            if (response.IsSuccessStatusCode)
                ApiConnection.Token = response.Content.ReadAsStringAsync().Result;

            return true;
        }

        public void StoreCredentials(string username, string password) {
            var vault = new PasswordVault();
            PasswordCredential cred = GetCredential();

            if (cred != null)
                vault.Remove(cred);

            var credential = new Windows.Security.Credentials.PasswordCredential("InFlightApp",username,password);
            vault.Add(credential);
        }

        public PasswordCredential GetCredential() {
            var vault = new PasswordVault();

            try{
                PasswordCredential cred =  vault.FindAllByResource("InFlightApp").FirstOrDefault();
                cred.RetrievePassword();
                return cred;
            } catch (Exception) {
                return null;
            }
        }

        public void  RemoveCredential(PasswordCredential cred) {
            var vault = new PasswordVault();
            vault.Remove(cred);
        }
    }
}
