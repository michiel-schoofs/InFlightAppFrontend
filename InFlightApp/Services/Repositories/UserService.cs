using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace InFlightApp.Services.Repositories
{
    public class UserService : IUserService
    {
        private HttpClient client;

        public UserService()
        {
            client = ApiConnection.Client;
        }

        public bool Login(string username, string password)
        {
            string json = JsonConvert.SerializeObject(new { username, password });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync($"{ApiConnection.URL}/Users/crewmember/login", content).Result;

            if (response.StatusCode == HttpStatusCode.BadRequest)
                return false;

            if (response.IsSuccessStatusCode){
                ApiConnection.Token = response.Content.ReadAsStringAsync().Result;
                client = ApiConnection.Client;
            }

            return true;
        }

        public bool AuthenticatePassenger(int seatnumber) {
            HttpResponseMessage response = client.PostAsync($"{ApiConnection.URL}/Users/passengers/login/{seatnumber}", null).Result;

            if (response.StatusCode == HttpStatusCode.BadRequest)
                return false;

            if (response.IsSuccessStatusCode)
                ApiConnection.Token = response.Content.ReadAsStringAsync().Result;

            return true;
        }

        public void StoreCredentials(string username, string password)
        {
            var vault = new PasswordVault();
            PasswordCredential cred = GetCredential();

            if (cred != null)
                vault.Remove(cred);

            var credential = new Windows.Security.Credentials.PasswordCredential("InFlightApp", username, password);
            vault.Add(credential);
        }

        public PasswordCredential GetCredential()
        {
            var vault = new PasswordVault();

            try
            {
                PasswordCredential cred = vault.FindAllByResource("InFlightApp").FirstOrDefault();
                cred.RetrievePassword();
                return cred;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void RemoveCredential(PasswordCredential cred)
        {
            var vault = new PasswordVault();
            vault.Remove(cred);
        }

        public void ChangeSeat(int userId, int seatId)
        {
            string url = $"{ApiConnection.URL}/Users/passengers/{userId}/seat/change/{seatId}";
            client.PutAsync(url, null).Wait();
        }

        public IEnumerable<Passenger> GetPassengers()
        {
            string url = $"{ApiConnection.URL}/Users/passengers";
            string s = client.GetStringAsync(url).Result;
            JArray ar = JArray.Parse(s);
            return ar.Select(p => p.ToObject<Passenger>()).ToList();
        }

        public Persoon GetLoggedIn() {
            string url = $"{ApiConnection.URL}/Users/current";
            string s = client.GetStringAsync(url).Result;
            JObject obj = JObject.Parse(s);
            return obj.ToObject<Passenger>();
        }

        public void ReloadHttpClient() {
            client = ApiConnection.Client;
        }
    }
}
