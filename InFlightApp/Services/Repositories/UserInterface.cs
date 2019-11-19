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
    public class UserInterface : IUserInterface
    {
        private HttpClient client;

        public UserInterface()
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

        public async Task ChangeSeat(int userId, int seatId)
        {
            string url = $"{ApiConnection.URL}/Users/passengers/${userId}/seat/change/${seatId}";
            await client.PutAsync(url, null);
        }

        public async Task<IEnumerable<Passenger>> GetPassengers()
        {
            string url = $"{ApiConnection.URL}/Users/passengers";
            string s = await client.GetStringAsync(url);
            JArray ar = JArray.Parse(s);

            return ar.Select(passenger =>
            {
                return new Passenger()
                {
                    PassengerId = passenger.Value<int>("passengerId"),
                    FirstName = passenger.Value<string>("firstName"),
                    LastName = passenger.Value<string>("lastName"),
                    Seat = new Seat()
                    {
                        SeatId = passenger.Value<int>("seat.seatID"),
                        Type = (SeatType)Enum.Parse(typeof(SeatType), passenger.Value<string>("seat.type")),
                        SeatCode = passenger.Value<string>("seat.seatCode")
                    }
                };
            }).ToList();
        }

        public void SendNotification(string notification)
        {
            client.PostAsync($"{ApiConnection.URL}/Notification?notification={notification}", null);
        }
    }
}
