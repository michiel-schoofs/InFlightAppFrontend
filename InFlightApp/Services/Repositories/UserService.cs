using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Storage;

namespace InFlightApp.Services.Repositories
{
    public class UserService : IUserService
    {
        private static Persoon persoon;
        private static bool imageAsked;
        private static StorageFolder sf = ApplicationData.Current.LocalCacheFolder;

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
                ReloadHttpClient();
            }

            return true;
        }

        public bool AuthenticatePassenger(int seatnumber) {
            HttpResponseMessage response = client.PostAsync($"{ApiConnection.URL}/Users/passengers/login/{seatnumber}", null).Result;

            if (response.StatusCode == HttpStatusCode.BadRequest)
                return false;

            if (response.IsSuccessStatusCode){
                ApiConnection.Token = response.Content.ReadAsStringAsync().Result;
                ReloadHttpClient();
            }

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

        public PassengerType? GetpassengerType() {
            if (persoon != null && persoon is Passenger)
                return persoon.Type;

            throw new Exception("Type not set");
        }


        public Passenger GetLoggedIn() {
            try
            {

                if (persoon != null && persoon is Passenger)
                    return (Passenger)persoon;

                string url = $"{ApiConnection.URL}/Users/current";
                string s = client.GetStringAsync(url).Result;
                JObject obj = JObject.Parse(s);

                Passenger p = obj.ToObject<Passenger>();

                int type = obj.Value<int>("type");
                p.Type = (PassengerType)(Enum.GetValues(typeof(PassengerType)).GetValue(type));

                persoon = p;
                return p;
            }catch(Exception ex)
            {
                return null;
            }
        }

        public void LogOut() {
            ApiConnection.Token = null;
            ReloadHttpClient();

            var vault = new PasswordVault();
            PasswordCredential cred = GetCredential();
            persoon = null;
            imageAsked = false;

            if (cred != null) {
                vault.Remove(cred);
            }
        }

        public void ReloadHttpClient() {
            client = ApiConnection.Client;
        }

        public async Task<string> GetImage(){
            if (imageAsked && persoon != null) {
                if (persoon.ImageFile != null)
                    return persoon.ImageFile;

                return "Assets/img/users/defaultuser.png";
            }

            string url = $"{ApiConnection.URL}/Users/current/image";
            string s = client.GetStringAsync(url).Result;
            JObject obj = JObject.Parse(s);

            if (obj != null && obj.Value<int>("id") >= 0)
            {
                string name = $"user.{obj.Value<string>("extension").ToLower()}";
                byte[] bytes = Convert.FromBase64String(obj.Value<string>("data"));

                try
                {
                    StorageFile fi = await sf.GetFileAsync(name);
                    await fi.DeleteAsync();
                }
                catch (Exception) { }

                StorageFile f = await sf.CreateFileAsync(name);

                Stream stream = f.OpenStreamForWriteAsync().Result;
                stream.Write(bytes, 0, bytes.Length);

                if (persoon != null){
                    persoon.ImageFile = f.Path;
                    imageAsked = true;
                }

                return f.Path;
            }

            return "Assets/img/users/defaultuser.png";
        }

        public async Task<bool> HasImage(){
            if (persoon != null && imageAsked)
                return persoon.ImageFile != null;
            
            string url = $"{ApiConnection.URL}/Users/current/image/exist";
            bool exist = bool.Parse(client.GetStringAsync(url).Result);
            return exist;
        }

        public async Task<string> GetImageForPerson(Persoon pers){
            string url = $"{ApiConnection.URL}/Users/{pers.Id}/image";
            string s = client.GetStringAsync(url).Result;
            JObject obj = JObject.Parse(s);
            if (obj != null && obj.Value<int>("id") >= 0)
            {
                string name = $"user-{pers.Id}.{obj.Value<string>("extension").ToLower()}";
                byte[] bytes = Convert.FromBase64String(obj.Value<string>("data"));

                StorageFile f = null;
                try {
                    f = sf.CreateFileAsync(name).AsTask().Result;
                }catch (Exception) {
                    //Bug fix
                    StorageFile fi = sf.GetFileAsync(name).AsTask().Result;
                    fi.DeleteAsync().AsTask().Wait();
                    f = sf.CreateFileAsync(name).AsTask().Result;
                }


                Stream stream = f.OpenStreamForWriteAsync().Result;
                stream.Write(bytes, 0, bytes.Length);

                return f.Path;
            }

            return "Assets/users/defaultuser.png";
        }

        public async Task<bool> HasTravelgroup(){
            if (persoon == null || persoon.Type != PassengerType.Passenger)
                return false;

            string url = $"{ApiConnection.URL}/TravelGroup/exist";
            bool exist = bool.Parse(client.GetStringAsync(url).Result);

            return exist;
        }

        public Seat GetSeatOfLogedIn(){ 
            if (persoon != null && persoon.Type == PassengerType.Passenger) {

                if (((Passenger)persoon).Seat != null)
                    return ((Passenger)persoon).Seat;

                string url = $"{ApiConnection.URL}/Users/passengers/{persoon.Id}/seat";

                string s = client.GetStringAsync(url).Result;
                JObject obj = JObject.Parse(s);

                Seat se = new Seat(){
                    SeatId = obj.Value<int>("seatID"),
                    Type = (SeatType)Enum.Parse(typeof(SeatType), obj.Value<string>("type")),
                    SeatCode = obj.Value<string>("seatCode")
                };

                ((Passenger)persoon).Seat = se;
                return se;
            }

            return null;
        }
    }
}
